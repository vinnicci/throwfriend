extends "res://scenes/enemies/AI.gd"


func _try_interrupt_patrol() -> bool:
    if is_ent_valid(bb["enemy"]) == true:
        return true
    return false


func _try_interrupt_flee() -> bool:
    if is_act_ready("melee_attack") || is_act_ready("throw_blob") || is_act_ready("shoot"):
        return true
    return false