extends Node2D


export var detection_range: int = 500
export var seek_dist: int = 150
export var too_far_dist: int = 250
export var too_close_dist: int = 75
export var flee_ray_len: int = 75

onready var ray: RayCast2D = $RayCast2D
onready var detection_area: Area2D = $DetectionRange
onready var tick: Timer = $Tick
onready var flee_rays_parent: Node2D = $FleeRays
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
		flee_ray.cast_to = Vector2(flee_ray_len, 0)
		flee_ray.rotation_degrees = i * 45
		var pos = flee_ray.get_node("Pos")
		pos.position = Vector2(flee_ray_len, 0)
		flee_rays_dict[flee_ray] = pos
	btree = get_node("BTREE")
	#init local blackboard
	bb["detection_range_sq"] = detection_range * detection_range
	bb["seek_dist"] = seek_dist
	bb["seek_dist_sq"] = seek_dist * seek_dist
	bb["too_far_dist"] = too_far_dist
	bb["too_far_dist_sq"] = too_far_dist * too_far_dist
	bb["too_close_dist"] = too_close_dist
	bb["too_close_dist_sq"] = too_close_dist * too_close_dist
	bb["enemy_dist"] = -1 #represents path distance
	bb["enemy_range_sq"] = -1 #straight line distance, disregarding walls, always a squared value
	bb["enemy"] = null
	bb["patrol_point"] = null


var level_node: Node2D
var parent_node: RigidBody2D
var weapon_node: Node2D
var player_node: RigidBody2D


func init_properties(new_lvl: Node2D, new_parent: RigidBody2D, patrol_pts: Array = []):
	level_node = new_lvl
	if is_ent_valid(level_node.get("PlayerNode")):
		player_node = level_node.get("PlayerNode")
	parent_node = new_parent
	if is_instance_valid(parent_node.WeaponNode):
		weapon_node = parent_node.WeaponNode
	bb["patrol_points"] = []
	var lvl_patrol_pts: Node2D = level_node.get_node("PatrolPoints")
	for patrol_pt in patrol_pts:
		bb["patrol_points"].append(lvl_patrol_pts.get_child(patrol_pt))
	bb["allies"] = []


func _physics_process(_delta: float) -> void:
	if is_ent_valid(parent_node) == false:
		is_moving = false
		set_process(false)
		set_physics_process(false)
		return
	if is_moving:
		var e = bb["enemy"]
		var t = bb["target"]
		if is_instance_valid(weapon_node):
			if is_ent_valid(e):
				weapon_node.look_at(e.global_position)
			else:
				weapon_node.look_at(t)
		parent_node.Velocity = (separate_from_allies(t - parent_node.global_position)).normalized()
	elif is_moving == false:
		parent_node.Velocity = (separate_from_allies()).normalized()


func separate_from_allies(velocity: Vector2 = Vector2.ZERO) -> Vector2:
	for ally in bb["allies"]:
		if parent_node.global_position == ally.global_position:
			velocity += Vector2(1,0).rotated(deg2rad(rand_range(0,360)))
		else:
			velocity += parent_node.global_position - ally.global_position
	return velocity


func is_ent_valid(ent: Node2D):
	return is_instance_valid(ent) && ent.get("Health") != null && ent.Health > 0


func get_new_path(target):
	if is_ent_valid(target) == false:
		return
	path_points.clear()
	path_points = level_node.call("GetPath", target.global_position, parent_node.global_position)
	path_points.pop_back()
	if path_points.size() != 0:
		bb["target"] = path_points.back()


func _on_FriendlyRange_body_entered(body: Node):
	if body == parent_node:
		return
	if body is RigidBody2D && body.has_node("AI"):
		bb["allies"].append(body)


func _on_FriendlyRange_body_exited(body: Node):
	if body is RigidBody2D && bb["allies"].has(body):
		bb["allies"].erase(body)


func _on_DetectionRange_body_entered(body: Node):
	if body == player_node:
		tick.start()


func _on_DetectionRange_body_exited(body: Node):
	if body == player_node && bb["enemy"] != player_node:
		tick.stop()


func engage_enemy(enemy: RigidBody2D):
	if is_ent_valid(enemy) == false || is_ent_valid(bb["enemy"]) || is_ent_valid(parent_node) == false:
		return;
	parent_node.call("PlaySoundEffect", "Alert")
	bb["enemy"] = enemy
	var arr: Array = level_node.get("PlayerEngaging")
	if arr.has(parent_node.name) == false:
		arr.append(parent_node.name)
	_on_Tick_timeout()
	tick.start()


var turret_types: Array = [
	"res://scenes/enemies/all rounder/ais/TurretAllRounderAI.tscn",
	"res://scenes/enemies/shooter/ais/TurretShooterAI.tscn"
]


#periodic distance calculator for enemy/master
func _on_Tick_timeout():
	var e = bb["enemy"]
	if is_ent_valid(e) == false && is_ent_valid(player_node):
		GlobalGD.get_ray_update(self, ray, player_node)
	elif is_ent_valid(e):
		#range distance - used by turrets
		level_node.call("GetDist", e.global_position, parent_node.global_position, self, "enemy_range_sq", false)
		ray.look_at(e.global_position)
		if turret_types.has(filename):
			return
		#path distance
		level_node.call("GetDist", e.global_position, parent_node.global_position, self, "enemy_dist", true)


func update_dist(key: String, dist: int):
	bb[key] = dist


#btree tasks


onready var timer_resume: Timer = $Resume
var paused: bool = false
signal resume


#time based wait task using coroutine
func task_timed_stop(task):
	if paused:
		return
	elif paused == false && timer_resume.is_stopped():
		paused = true;
		timer_resume.start(task.get_param(0))
	yield(self, "resume")
	task.succeed()


func _on_Resume_timeout():
	paused = false
	emit_signal("resume")


#check if dead


#param 0: bb name
func task_is_ent_valid(task):
	if is_ent_valid(bb[task.get_param(0)]):
		task.succeed()
	else:
		task.failed()


#get points


#param 0: target
func task_get_seek_point(task):
	get_seek_point(bb[task.get_param(0)])
	task.succeed()


func get_seek_point(target):
	if is_ent_valid(target) == false:
		return
	path_points.pop_back()
	if path_points.size() == 0:
		get_new_path(target)
	bb["target"] = path_points.back()


func task_get_flee_point(task):
	get_flee_point()
	task.succeed()


func get_flee_point():
	var e = bb["enemy"]
	if is_ent_valid(e) == false:
		return
	var flee_routes: Dictionary = {}
	for flee_ray in flee_rays_dict:
		if is_instance_valid(flee_ray.get_collider()):
			continue
		var pos = flee_rays_dict[flee_ray].global_position
		var flee_points: int = e.global_position.distance_squared_to(pos) as int
		flee_routes[flee_points] = pos
	if flee_routes.keys().size() != 0:
		bb["target"] = flee_routes[flee_routes.keys().max()]
	else:
		bb["target"] = -e.global_position


func task_get_patrol_point(task):
	get_patrol_point()
	task.succeed()


func get_patrol_point():
	if bb["patrol_points"].size() == 0:
		return
	var next_target = bb["patrol_points"].pop_back()
	bb["patrol_point"] = next_target
	get_new_path(next_target)
	bb["patrol_points"].push_front(next_target)


#dist check


#param 0: target
#param 1: distance required
func task_is_target_close(task):
	var dist: int = get_target_dist(task.get_param(0))
	if dist <= 0:
		task.failed()
	elif dist <= bb[task.get_param(1)]:
		task.succeed()
	else:
		task.failed()


func get_target_dist(target_bb_name: String) -> int:
	if bb.keys().has(target_bb_name + "_dist"):
		return bb[target_bb_name + "_dist"]
	return -1


#param 0: target
#param 1: distance required
func task_is_target_in_range(task):
	if(bb[task.get_param(0) + "_range_sq"] <= 0):
		task.failed()
	elif bb[task.get_param(0) + "_range_sq"] <= bb[task.get_param(1) + "_sq"]:
		task.succeed()
	else:
		task.failed()


#move actions


export var target_dist_margin_sq: int = 1250
const ORIGIN_DIST: = 30000


#_try_interrupt overridable func - use custom conditions to stop seeking
#param 0: target
#param 1: distance needed
func task_seek(task):
	is_moving = true
	var t = bb[task.get_param(0)]
	if parent_node.global_position.distance_squared_to(bb["target"]) <= target_dist_margin_sq:
		get_seek_point(t)
	if t.global_position.distance_squared_to(path_points.front()) > ORIGIN_DIST:
		get_new_path(t)
	if _try_interrupt_seek(task):
		is_moving = false
		path_points.clear()
		task.succeed()


func _try_interrupt_seek(task) -> bool:
	return bb[task.get_param(0) + "_dist"] <= bb[task.get_param(1)] || is_ent_valid(bb["enemy"]) == false


#_try_interrupt overridable func - use custom conditions to stop fleeing
#inverted return value is also used as precondition
#param 0: distance needed
func task_flee(task):
	is_moving = true
	if (parent_node.global_position.distance_squared_to(bb["target"]) <= target_dist_margin_sq ||
	parent_node.global_position.distance_squared_to(bb["target"]) > ORIGIN_DIST):
		get_flee_point()
	if _try_interrupt_flee(task):
		is_moving = false
		task.succeed()


#precondition for fleeing
#param 0: distance needed
func task_should_flee(task):
	if(_try_interrupt_flee(task) == false):
		task.succeed()
	else:
		task.failed()


func _try_interrupt_flee(task) -> bool:
	return bb["enemy_dist"] > bb[task.get_param(0)] || is_ent_valid(bb["enemy"]) == false


#_try_interrupt overridable func - use custom conditions to stop patrolling
func task_patrol(task):
	if bb["patrol_points"].size() == 0:
		task.succeed()
		return
	is_moving = true
	if _try_interrupt_patrol(task):
		is_moving = false
		bb["patrol_point"] = null
		task.succeed()
	elif parent_node.global_position.distance_squared_to(bb["target"]) <= target_dist_margin_sq:
		get_seek_point(bb["patrol_point"])


func _try_interrupt_patrol(_task) -> bool:
	return (parent_node.global_position.distance_squared_to(bb["patrol_point"].global_position) <= 
	target_dist_margin_sq)


#ai actions


#param 0: target
func task_aim_weapon(task):
	if ray.get_collider() == bb[task.get_param(0)]:
		weapon_node.look_at(bb[task.get_param(0)].global_position)
		parent_node.Velocity = Vector2(1,0).rotated(weapon_node.global_rotation)
		parent_node.call("AdjustSprites")
		task.succeed()
	else:
		task.failed()


#param 0: enemy action name
func task_act(task):
	var action = task.get_param(0)
	parent_node.call("DoAction", action)
	var dict: Dictionary = parent_node.ActDict[action]
	if dict["IsActive"] == false:
		task.succeed()


#param 0: enemy action name
func task_is_act_ready(task):
	if is_act_ready(task.get_param(0)):
		task.succeed()
	else:
		task.failed()


func is_act_ready(act_name: String):
	return parent_node.IsActReady(act_name)
