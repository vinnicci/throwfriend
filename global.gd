extends Node


func _physics_process(_delta: float) -> void:
	if ray_queue.size() > 0:
		var arr: Array = ray_queue.pop_back()
		ray_update(arr[0], arr[1], arr[2])


var ray_queue: Array = []


func ray_update(requester, ray, player_node) -> void:
	ray.look_at(player_node.global_position)
	ray.force_raycast_update()
	if ray.get_collider() == player_node:
		requester.engage_enemy(player_node)


func get_ray_update(requester, ray, player_node) -> void:
	var arr: Array = []
	arr.append(requester) #0
	arr.append(ray) #1
	arr.append(player_node) #2
	ray_queue.push_front(arr)
