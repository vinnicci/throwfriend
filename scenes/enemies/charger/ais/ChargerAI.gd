extends "res://scenes/enemies/AI.gd"


func _try_interrupt_flee(_task) -> bool:
	return ._try_interrupt_flee(_task) || is_act_ready("charge")


func _try_interrupt_seek(task) -> bool:
	return (._try_interrupt_seek(task) ||
	(is_act_ready("spawn_blob") && bb[task.get_param(0) + "_dist"] <= bb["seek_dist"]))


func _try_interrupt_patrol(_task) -> bool:
	return ._try_interrupt_patrol(_task) || is_ent_valid(bb["enemy"])