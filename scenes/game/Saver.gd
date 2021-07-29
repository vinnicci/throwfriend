extends Node


const SAVE_DIR: String = "user://save/"


func save_data(dict: Dictionary):
    #save data
    var save_file = load("res://scenes/game/SaveFile.gd").new()
    save_file.Level = dict["Level"]
    save_file.MaxHP = dict["MaxHP"]
    save_file.SnarkDmgMult = dict["SnarkDmgMult"]
    save_file.AvailableUpgrades = dict["AvailableUpgrades"]
    save_file.PlayerItem1 = dict["PlayerItem1"]
    save_file.PlayerItem2 = dict["PlayerItem2"]
    save_file.WeapItem1 = dict["WeapItem1"]
    save_file.WeapItem2 = dict["WeapItem2"]
    if ResourceSaver.save(SAVE_DIR + "save.tres", save_file) != OK:
        printerr("Save data unsuccessful.")

