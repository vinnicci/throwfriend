extends "res://scenes/enemies/AI.gd"


func _try_interrupt_flee():
	if is_act_ready("shoot") == true:
		return true
	return false


func _try_interrupt_patrol() -> bool:
	if is_ent_valid(bb["enemy"]) == true:
		return true
	return false

