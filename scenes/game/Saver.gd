extends Node


const SAVE_DIR: String = "user://save/"
var player_save_file
var level_save_file


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


func new_level_save_file():
    level_save_file = load("res://scenes/game/LevelSaveFile.gd").new()
    save_level_data()


func save_level_data():
    if ResourceSaver.save(SAVE_DIR + "level_save.tres", level_save_file) != OK:
        printerr("Level save data unsuccessful.")