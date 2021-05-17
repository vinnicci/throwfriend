extends "res://scenes/enemies/AI.gd"


func _try_interrupt_flee(_task):
	return ._try_interrupt_flee(_task) || is_act_ready("shoot")


func _try_interrupt_patrol(_task) -> bool:
	return ._try_interrupt_patrol(_task) || is_ent_valid(bb["enemy"])

