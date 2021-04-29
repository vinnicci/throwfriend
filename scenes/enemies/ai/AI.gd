extends Node2D


export var detection_range: int = 600
export var seek_dist: int = 300
export var enemy_too_far_dist: int = 500
export var enemy_too_close_dist: int = 150

onready var ray: RayCast2D = $RayCast2D
onready var detection_area: Area2D = $DetectionRange
onready var tick: Timer = $Tick
onready var flee_rays_parent: Node2D = $FleeRays
var action_timer: Timer
var flee_rays_dict: Dictionary
var btree: Node

var is_moving: bool = false
var bb: Dictionary #blackboard
var path_points: Array


func _ready() -> void:
	ray.cast_to = Vector2(detection_range, 0)
	var circle: CircleShape2D = CircleShape2D.new()
	circle.radius = detection_range
	detection_area.get_node("CollisionShape2D").shape = circle
	for i in range(8):
		var flee_ray: RayCast2D = flee_rays_parent.get_node("FleeRay" + i as String)
		flee_ray.rotation_degrees = i * 45
		flee_rays_dict[flee_ray] = flee_ray.get_node("Pos")
	btree = get_node("BTREE")
	bb["seek_dist"] = seek_dist
	bb["enemy_too_far_dist"] = enemy_too_far_dist
	bb["enemy_too_close_dist"] = enemy_too_close_dist
	bb["enemy"] = null


var level_node: Node2D
var parent_node: RigidBody2D
var player_node: RigidBody2D


func init_properties(new_lvl: Node2D, new_parent: RigidBody2D):
	level_node = new_lvl
	player_node = level_node.PlayerNode
	parent_node = new_parent
	action_timer = parent_node.get_node("ActionTimer")


func _physics_process(_delta: float) -> void:
	if is_ent_valid(parent_node) == false:
		is_moving = false
		btree.enable = false
		print("ayy dead")
		return
	if is_moving == true:
		_go_to()
	elif is_moving == false:
		parent_node.Velocity = Vector2.ZERO


func is_ent_valid(ent: Node2D):
	var output: bool = is_instance_valid(ent) == true
	if output == true && ent.get("IsDead") != null:
		output = output && ent.IsDead == false
	return output


func _go_to():
	parent_node.Velocity = (bb["target"] - parent_node.global_position).clamped(1)


func _get_seek_point(target):
	if is_ent_valid(target) == false:
		return
	path_points.pop_back()
	if path_points.size() == 0:
		_get_new_path(target)
	bb["target"] = path_points.back()


func _get_new_path(target):
	if is_ent_valid(target) == false:
		return
	path_points.clear()
	path_points = level_node.call("GetPath", target.global_position, parent_node.global_position)
	path_points.pop_back()


func _get_flee_point():
	if is_ent_valid(bb["enemy"]) == false:
		return
	var flee_routes: Dictionary = {}
	for flee_ray in flee_rays_dict:
		if is_instance_valid(flee_ray.get_collider()) == true:
			continue
		var pos = flee_rays_dict[flee_ray].global_position
		var flee_points: int = bb["enemy"].global_position.distance_squared_to(pos) as int
		flee_routes[flee_points] = pos
	if flee_routes.keys().size() != 0:
		bb["target"] = flee_routes[flee_routes.keys().max()]
	else:
		bb["target"] = -bb["enemy"].global_position


func _on_DetectionRange_body_entered(body: Node):
	if body is RigidBody2D && body == player_node:
		tick.start()


func _on_DetectionRange_body_exited(body: Node):
	if body is RigidBody2D && body == player_node && bb["enemy"] != player_node:
		tick.stop()


#periodic distance calculator for enemy/master
func _on_Tick_timeout():
	if is_ent_valid(bb["enemy"]) == false && is_ent_valid(player_node) == true:
		ray.look_at(player_node.global_position)
		ray.force_raycast_update()
		if ray.get_collider() == player_node:
			bb["enemy"] = player_node
			bb["enemy_dist"] = level_node.call("GetDist", bb["enemy"].global_position, parent_node.global_position)
	elif is_ent_valid(bb["enemy"]) == true:
		bb["enemy_dist"] = level_node.call("GetDist", bb["enemy"].global_position, parent_node.global_position)


#btree tasks


onready var timer_resume: Timer = $Resume
var paused: bool = false
signal resume


#time based wait task using coroutine
func task_timed_stop(task):
	if paused == true:
		return
	elif paused == false && timer_resume.is_stopped() == true:
		paused = true;
		timer_resume.start(task.get_param(0))
	yield(self, "resume")
	task.succeed()


func _on_Resume_timeout():
	paused = false
	emit_signal("resume")


func task_is_ent_valid(task):
	if is_ent_valid(bb[task.get_param(0)]) == true:
		task.succeed()
	else:
		task.failed()


#param 0: flee or seek
#param 1: if seek -> target to track
func task_get_target_point(task):
	if task.get_param(0) == "flee":
		_get_flee_point()
	elif task.get_param(0) == "seek":
		_get_seek_point(bb[task.get_param(1)])
	task.succeed() 


#param 0: target distance(must exist in blackboard)
#param 1: distance required
func task_is_target_close(task):
	if bb[task.get_param(0)] <= bb[task.get_param(1)]:
		task.succeed()
	else:
		task.failed()


const ORIGIN_DIST: = 62500
const TARGET_DIST: = 2500


#param 0: seek or flee
#param 1: if seek -> target to track
func task_go_to_target(task):
	is_moving = true
	if task.get_param(0) == "seek":
		if parent_node.global_position.distance_squared_to(bb["target"]) <= TARGET_DIST:
			_get_seek_point(bb[task.get_param(1)])
		if bb[task.get_param(1)].global_position.distance_squared_to(path_points.front()) > ORIGIN_DIST:
			_get_new_path(bb[task.get_param(1)])
		if bb[task.get_param(1) + "_dist"] <= bb["seek_dist"] || is_ent_valid(bb["enemy"]) == false:
			is_moving = false
			path_points.clear()
			task.succeed()
			return
	elif task.get_param(0) == "flee":
		if parent_node.global_position.distance_squared_to(bb["target"]) <= TARGET_DIST:
			_get_flee_point()
		if bb["enemy_dist"] > bb["enemy_too_far_dist"] || is_ent_valid(bb["enemy"]) == false:
			is_moving = false
			task.succeed()
			return


#param 0: enemy action name
func task_act(task):
	parent_node.call("DoAction", task.get_param(0))
	task.succeed()


func task_is_act_ready(task):
	if action_timer.is_stopped() == true:
		task.succeed()
	else:
		task.failed()

