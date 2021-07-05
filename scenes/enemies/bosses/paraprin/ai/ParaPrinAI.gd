extends "res://scenes/enemies/AI.gd"


var weapon1: Node2D
var weapon2: Node2D


func _ready() -> void:
	pass # Replace with function body.


func init_properties(new_lvl: Node2D, new_parent: RigidBody2D, patrol_pts: Array = []):
	.init_properties(new_lvl, new_parent, patrol_pts)
	#weapon1 = parent_node.get_node("Sprite/Skirt/Torso/Skeleton2D/Spine/Spine2/Spine3/EnemyWeapon")


#param 0: target
func task_aim_weapon_1(task):
	weapon1.look_at(bb[task.get_param(0)].global_position)
	parent_node.Velocity = Vector2(1,0).rotated(weapon1.global_rotation)
	parent_node.call("AdjustSprites")
	task.succeed()


#param 0: target
func task_aim_weapon_2(task):
	weapon2.look_at(bb[task.get_param(0)].global_position)
	parent_node.Velocity = Vector2(1,0).rotated(weapon2.global_rotation)
	parent_node.call("AdjustSprites")
	task.succeed()
