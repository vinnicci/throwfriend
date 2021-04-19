extends Node2D


export var detection_range: int = 600
export var enemy_seek_dist: int = 300
export var enemy_too_far_dist: int = 500
export var enemy_too_close_dist: int = 150
export var master_seek_dist: int = 300

var level_node: Node2D
var player_node: RigidBody2D
var parent_node: RigidBody2D
var enemy_ent: RigidBody2D
var master_ent: RigidBody2D

onready var ray: RayCast2D = $RayCast2D
onready var detection_area: Area2D = $DetectionRange
onready var tick: Timer = $Tick
onready var flee_tick: Timer = $FleeTick
onready var flee_rays_parent: Node2D = $FleeRays
var flee_rays_dict: Dictionary
var dist_dict: Dictionary

enum States {STOP, ENEMY_SEEK, MASTER_SEEK, FLEE}
var move_state: int = States.STOP


func _ready() -> void:
	ray.cast_to = Vector2(detection_range, 0)
	var circle: CircleShape2D = CircleShape2D.new()
	circle.radius = detection_range
	detection_area.get_node("CollisionShape2D").shape = circle
	for i in range(8):
		var flee_ray: RayCast2D = flee_rays_parent.get_node("FleeRay" + i as String)
		flee_ray.rotation_degrees = i * 45
		flee_rays_dict[flee_ray] = flee_ray.get_node("Pos")
	dist_dict["enemy_seek"] = enemy_seek_dist
	dist_dict["enemy_too_far"] = enemy_too_far_dist
	dist_dict["enemy_too_close"] = enemy_too_close_dist
	dist_dict["master_seek"] = master_seek_dist


func init_properties(new_lvl: Node2D, new_parent: RigidBody2D):
	level_node = new_lvl
	player_node = level_node.PlayerNode
	parent_node = new_parent


func _physics_process(_delta: float) -> void:
	if is_instance_valid(parent_node) == false:
		return
	if move_state == States.FLEE && flee_rays_dict.keys()[0].enabled == false:
		print("flee on")
		for flee_ray in flee_rays_dict.keys():
			flee_ray.enabled = true
	elif move_state != States.FLEE && flee_rays_dict.keys()[0].enabled == true:
		print("flee off")
		for flee_ray in flee_rays_dict.keys():
			flee_ray.enabled = false
	match move_state:
		States.STOP:
			parent_node.Velocity = Vector2(0,0)
		States.ENEMY_SEEK:
			_seek(enemy_ent)
		States.MASTER_SEEK:
			_seek(master_ent)
		States.FLEE:
			_flee()


var path_points: Array
var next_point = null
var master_ent_dist: int
var enemy_ent_dist: int


func _seek(target_ent: RigidBody2D):
	if is_instance_valid(target_ent) == false:
		return
	if next_point != null && parent_node.global_position.distance_squared_to(next_point) > 5625:
		parent_node.Velocity = next_point - parent_node.global_position
		return
	path_points.pop_back()
	if path_points.size() == 0 || target_ent.global_position.distance_squared_to(path_points.front()) > 62500:
		next_point = null
		path_points = level_node.call("GetPath", target_ent.global_position, parent_node.global_position)
	next_point = path_points.back()


var next_flee_point = null


func _flee():
	parent_node.Velocity = (next_flee_point - parent_node.global_position).clamped(1)


func _on_DetectionRange_body_entered(body: Node):
	if body is RigidBody2D && body == player_node:
		tick.start()


func _on_DetectionRange_body_exited(body: Node):
	if body is RigidBody2D && body == player_node && enemy_ent != player_node:
		tick.stop()


func _on_Tick_timeout():
	if is_instance_valid(enemy_ent) == false:
		ray.look_at(player_node.global_position)
		ray.force_raycast_update()
		if ray.get_collider() == player_node:
			enemy_ent = player_node
			enemy_ent_dist = level_node.call("GetDist", enemy_ent.global_position, parent_node.global_position)
	else:
		enemy_ent_dist = level_node.call("GetDist", enemy_ent.global_position, parent_node.global_position)


func _on_FleeTick_timeout():
	var flee_routes: Dictionary = {}
	for flee_ray in flee_rays_dict:
		#skip ray that is colliding with walls or a physics object
		if is_instance_valid(flee_ray.get_collider()) == true:
			continue
		var pos = flee_rays_dict[flee_ray].global_position
		var flee_points: int = enemy_ent.global_position.distance_squared_to(pos) as int
		flee_routes[flee_points] = pos
	if flee_routes.keys().size() != 0:
		next_flee_point = flee_routes[flee_routes.keys().max()]
	else:
		next_flee_point = -enemy_ent.global_position


#btree tasks


func task_idle(task):
	move_state = States.STOP
	path_points = []
	next_point = null
	task.succeed()


func task_is_enemy_valid(task):
	if is_instance_valid(enemy_ent) == true:
		task.succeed()
	else:
		task.failed()


func task_is_enemy_close(task):
	if enemy_ent_dist == -1:
		enemy_ent_dist = level_node.call("GetDist", enemy_ent.global_position, parent_node.global_position)
	if enemy_ent_dist <= dist_dict[task.get_param(0)]:
		task.succeed()
	else:
		task.failed()


func task_seek_enemy(task):
	move_state = States.ENEMY_SEEK
	task.succeed()


func task_flee(task):
	if flee_tick.is_stopped() == true:
		flee_tick.start()
		_on_FleeTick_timeout()
	move_state = States.FLEE
	task.succeed()


	
