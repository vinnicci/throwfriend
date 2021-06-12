extends "res://scenes/enemies/AI.gd"


func _try_interrupt_flee(_task) -> bool:
	return ._try_interrupt_flee(_task) || is_act_ready("spawn_blob_charger")