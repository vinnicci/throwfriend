extends "res://scenes/enemies/AI.gd"


func _try_interrupt_patrol(_task) -> bool:
    var output = ._try_interrupt_patrol(_task)
    return output || is_ent_valid(bb["enemy"])


func _try_interrupt_seek(task) -> bool:
    var output = ._try_interrupt_seek(task)
    return (output ||
    (is_act_ready("throw_blob") || is_act_ready("shoot") && bb[task.get_param(0) + "_dist"] <= bb["seek_dist"]))


func _try_interrupt_flee(_task) -> bool:
    var output = ._try_interrupt_flee(_task)
    return (output || is_act_ready("melee_attack") || is_act_ready("throw_blob") || is_act_ready("shoot"))
