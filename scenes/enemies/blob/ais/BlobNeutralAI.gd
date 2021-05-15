extends "res://scenes/enemies/AI.gd"


func _try_interrupt() -> bool:
	if bb.keys().has("see_dist_sq") == false:
		bb["seek_dist_sq"] = bb["seek_dist"] * bb["seek_dist"]
	if (is_ent_valid(bb["enemy"]) == true &&
	parent_node.global_position.distance_squared_to(bb["enemy"].global_position) <= bb["seek_dist_sq"]):
		return true
	return false
