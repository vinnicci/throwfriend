extends "res://scenes/enemies/AI.gd"


func _try_interrupt_patrol(_task) -> bool:
	if bb.keys().has("seek_dist_sq") == false:
		bb["seek_dist_sq"] = bb["seek_dist"] * bb["seek_dist"]
	return (._try_interrupt_patrol(_task) ||
	(is_ent_valid(bb["enemy"]) &&
	parent_node.global_position.distance_squared_to(bb["enemy"].global_position) <= bb["seek_dist_sq"]))
