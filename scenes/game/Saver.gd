extends Node


const SAVE_DIR: String = "user://save/"
var player_save_file
var level_save_file
var world_save_file


func new_player_save_file():
	var dir: Directory = Directory.new()
	if(dir.file_exists(SAVE_DIR) == false):
		if dir.make_dir_recursive(SAVE_DIR) != OK:
			printerr("Save directory error.")
	if ResourceSaver.save(SAVE_DIR + "player_save.tres", load("res://scenes/game/PlayerSaveFile.gd").new()) != OK:
		printerr("Player save data unsuccessful.")


func save_player_data():
	if ResourceSaver.save(SAVE_DIR + "player_save.tres", player_save_file) != OK:
		printerr("Player save data unsuccessful.")


func load_player_data():
	player_save_file = ResourceLoader.load(SAVE_DIR + "player_save.tres", "", true)


func new_level_save_file():
	if ResourceSaver.save(SAVE_DIR + "level_save.tres", load("res://scenes/game/LevelSaveFile.gd").new()) != OK:
		printerr("Level save data unsuccessful.")


func save_level_data():
	if ResourceSaver.save(SAVE_DIR + "level_save.tres", level_save_file) != OK:
		printerr("Level save data unsuccessful.")


func load_level_data():
	level_save_file = ResourceLoader.load(SAVE_DIR + "level_save.tres", "", true)


func new_world_save_file():
	if ResourceSaver.save(SAVE_DIR + "world_save.tres", load("res://scenes/game/WorldSaveFile.gd").new()) != OK:
		printerr("World save data unsuccessful.")


func save_world_data():
	if ResourceSaver.save(SAVE_DIR + "world_save.tres", world_save_file) != OK:
		printerr("World save data unsuccessful.")


func load_world_data():
	world_save_file = ResourceLoader.load(SAVE_DIR + "world_save.tres", "", true)

