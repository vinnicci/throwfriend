extends "res://scenes/enemies/AI.gd"


func _try_interrupt_flee(_task):
	var output = _try_interrupt_flee(_task)
	return output || is_act_ready("shoot")


func _try_interrupt_patrol(_task) -> bool:
	var output = _try_interrupt_patrol(_task)
	return output || is_ent_valid(bb["enemy"])

