extends Node2D


export var master_seek_dist: int = 300
export var detection_range: int = 600
export var enemy_seek_dist: int = 250
export var enemy_too_far_dist: int = 600

var level_node: Node2D
var player_node: RigidBody2D
var parent_node: RigidBody2D
var enemy_ent: RigidBody2D
var master_ent: RigidBody2D

onready var ray: RayCast2D = $RayCast2D
onready var detection_area: Area2D = $DetectionRange
onready var tick: Timer = $Tick
onready var look: RayCast2D = $RayCast2D2

enum States {STOP, ENEMY_SEEK, MASTER_SEEK, FLEE}
var move_state: int = States.STOP


func _ready() -> void:
	ray.cast_to = Vector2(detection_range, 0)
	var circle: CircleShape2D = CircleShape2D.new()
	circle.radius = detection_range
	detection_area.get_node("CollisionShape2D").shape = circle


func init_properties(new_lvl: Node2D, new_parent: RigidBody2D):
	level_node = new_lvl
	player_node = level_node.PlayerNode
	parent_node = new_parent


func _physics_process(_delta: float) -> void:
	if is_instance_valid(parent_node) == false:
		return
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
	look.look_at(next_point)


func _flee():
	pass


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
	else:
		# _calc_enemy_dist(level_node.call("GetPath", enemy_ent.global_position, parent_node.global_position))
		enemy_ent_dist = level_node.call("GetDist", enemy_ent.global_position, parent_node.global_position)


# func _calc_enemy_dist(arr: Array) -> void:
# 	if arr.size() == 0:
# 		return
# 	var dist: int = arr.pop_back().distance_to(arr.back())
# 	while arr.size() > 1:
# 		dist += arr.pop_back().distance_to(arr.back()) as int
# 	enemy_ent_dist = dist


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
	return


func task_seek_enemy(task):
	if enemy_ent_dist > enemy_seek_dist:
		move_state = States.ENEMY_SEEK
	else:
		move_state = States.STOP
		task.succeed()




	
