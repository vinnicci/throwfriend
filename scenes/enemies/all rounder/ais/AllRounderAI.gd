extends "res://scenes/enemies/AI.gd"


func _try_interrupt_patrol(_task) -> bool:
	return ._try_interrupt_patrol(_task) || is_ent_valid(bb["enemy"])


func _try_interrupt_seek(task) -> bool:
	return (._try_interrupt_seek(task) ||
	((is_act_ready("throw_blob") || 
	is_act_ready("shoot") || 
	is_act_ready("shoot_back") ||
	is_act_ready("throw_flyblob") || 
	is_act_ready("tele_attack") ||
	is_act_ready("super_scatter") ||
	is_act_ready("triple_shot")) &&
	bb[task.get_param(0) + "_dist"] <= bb["seek_dist"]))


func _try_interrupt_flee(_task) -> bool:
	return (._try_interrupt_flee(_task) ||
	is_act_ready("melee_attack") ||
	is_act_ready("melee_attack_back") ||
	is_act_ready("shoot") ||
	is_act_ready("shoot_back") ||
	is_act_ready("throw_blob") ||
	is_act_ready("throw_flyblob") ||
	is_act_ready("tele_attack") ||
	is_act_ready("super_scatter") ||
	is_act_ready("triple_shot"))
