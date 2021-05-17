extends "res://scenes/enemies/AI.gd"


func _try_interrupt_flee(_task) -> bool:
	var output = _try_interrupt_flee(_task)
	return output || is_act_ready("charge")


func _try_interrupt_seek(task) -> bool:
	var output = _try_interrupt_seek(task)
	return (output ||
	(is_act_ready("spawn_blob") && bb[task.get_param(0) + "_dist"] <= bb["seek_dist"]))


func _try_interrupt_patrol(_task) -> bool:
	var output = _try_interrupt_patrol(_task)
	return output || is_ent_valid(bb["enemy"])