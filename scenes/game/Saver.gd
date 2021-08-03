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
    player_save_file = load("res://scenes/game/PlayerSaveFile.gd").new()
    save_player_data()


func save_player_data():
    if ResourceSaver.save(SAVE_DIR + "player_save.tres", player_save_file) != OK:
        printerr("Player save data unsuccessful.")


func load_player_data():
    if load(SAVE_DIR + "player_save.tres") == null:
        printerr("Player load data unsuccessful.")
    else:
        player_save_file = load(SAVE_DIR + "player_save.tres")


func new_level_save_file():
    level_save_file = load("res://scenes/game/LevelSaveFile.gd").new()
    save_level_data()


func save_level_data():
    if ResourceSaver.save(SAVE_DIR + "level_save.tres", level_save_file) != OK:
        printerr("Level save data unsuccessful.")


func load_level_data():
    if load(SAVE_DIR + "level_save.tres") == null:
        printerr("Level load data unsuccessful.")
    else:
        level_save_file = load(SAVE_DIR + "level_save.tres")


func new_world_save_file():
    world_save_file = load("res://scenes/game/WorldSaveFile.gd").new()
    save_world_data()


func save_world_data():
    if ResourceSaver.save(SAVE_DIR + "world_save.tres", world_save_file) != OK:
        printerr("World save data unsuccessful.")


func load_world_data():
    if load(SAVE_DIR + "world_save.tres") == null:
        printerr("World load data unsuccessful.")
    else:
        world_save_file = load(SAVE_DIR + "world_save.tres")