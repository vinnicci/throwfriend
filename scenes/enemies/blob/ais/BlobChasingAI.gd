extends "res://scenes/enemies/AI.gd"


func _try_interrupt() -> bool:
	if is_ent_valid(bb["enemy"]) == true:
		return true
	return false