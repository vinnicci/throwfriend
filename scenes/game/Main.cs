using Godot;
using System;

public class Main : Node
{
    public MainMenu MainMenuNode {get; set;}
    public Node GameNode {get; set;}
    public AnimationPlayer FadeAnim {get; set;}
    public Node Saver {get; set;}

    ColorRect screenColor;
    Level currentLevel;
    

    public override void _Ready()
    {
        base._Ready();
        GD.Randomize();
        FadeAnim = (AnimationPlayer)GetNode("CanvasLayer/Anim");
        screenColor = (ColorRect)GetNode("CanvasLayer/ColorRect");
        Saver = GetNode("Saver");
    }


    const String STARTING_SCENE = "res://test scenes/TestLevel1.tscn";


    public void NewGame() {
        PackedScene playerPack = (PackedScene)ResourceLoader.Load(Global.PLAYER_SCN);
        GoToLevel(STARTING_SCENE, "SavePoint", (Player)playerPack.Instance(), false);
        Saver.Call("new_player_save_file");
        Saver.Call("new_level_save_file");
        Saver.Call("new_world_save_file");
    }


    bool VerifyDir() {
        String[] filesArr = {
            Global.SAVE_DIR + "player_save.tres",
            Global.SAVE_DIR + "level_save.tres",
            Global.SAVE_DIR + "world_save.tres"
        };
        Directory dir = new Directory();
        foreach(String file in filesArr) {
            if(dir.FileExists(file) == false) {
                GD.PrintErr("Cannot locate save file in the directory.");
                return false;
            }
        }
        return true;
    }


    public void LoadGame() {
        if(VerifyDir() == false) {
            return;
        }
        PackedScene playerPack = (PackedScene)ResourceLoader.Load("res://scenes/player/Player.tscn");
        Player player = (Player)playerPack.Instance();
        Saver.Call("load_player_data");
        Saver.Call("load_level_data");
        Saver.Call("load_world_data");
        Resource playerSaveFile = (Resource)Saver.Get("player_save_file");
        String[] saveDataArr = {
            "Level",
            "MaxHP",
            "SnarkDmgMult",
            "AvailableUpgrades",
            "PlayerItem1",
            "PlayerItem2",
            "WeapItem1",
            "WeapItem2"
        };
        if(VerifySaveFile(playerSaveFile, saveDataArr) == false) {
            return;
        }
        GoToLevel((String)playerSaveFile.Get("Level"), "SavePoint", player, true);
    }


    bool VerifySaveFile(Resource saveFile, String[] saveDataArr) {
        foreach(String data in saveDataArr) {
            if(saveFile.Get(data) == null) {
                GD.PrintErr(saveFile.ResourceName + ": Invalid save file.");
                return false;
            }
        }
        return true;
    }


    public void SavePlayerData(String levelFileName = "") {
        Resource playerSaveFile = (Resource)Saver.Get("player_save_file");
        Player player = (Player)currentLevel.GetNode("Player");
        //current level
        if(levelFileName == "") {
            levelFileName = (String)playerSaveFile.Get("Level");
        }
        playerSaveFile.Set("Level", levelFileName);
        //replenish and then save hp
        player.ChangeEntityBaseStats(player.health, -1);
        playerSaveFile.Set("MaxHP", player.Health);
        //snark dmg mult
        playerSaveFile.Set("SnarkDmgMult", player.SnarkDmgMult);
        //available upgrades
        playerSaveFile.Set("AvailableUpgrades", player.AvailableUpgrade);
        //items
        player.RefreshItems();
        player.WeaponNode.RefreshItems();
        //player item 1
        SavePlayerItems(player.Item1, playerSaveFile, "PlayerItem", 1);
        //player item 2
        SavePlayerItems(player.Item2, playerSaveFile, "PlayerItem", 2);
        //weap item 1
        SavePlayerItems(player.WeaponNode.Item1, playerSaveFile, "WeapItem", 1);
        //weap item 2
        SavePlayerItems(player.WeaponNode.Item2, playerSaveFile, "WeapItem", 2);
        player.UpdateStatsDisp();
        Saver.Call("save_player_data");
    }


    void SavePlayerItems(Item item, Resource playerSaveFile, String itemType, int slotNum) {
        String slot = itemType + slotNum;
        if(IsInstanceValid(item)) {
            playerSaveFile.Set(slot, item.Filename);
        }
        else {
            playerSaveFile.Set(slot, "");
        }
    }


    void LoadPlayerData(Player player) {
        Resource playerSaveFile = (Resource)Saver.Get("player_save_file");
        //hp
        player.ChangeEntityBaseStats((int)playerSaveFile.Get("MaxHP"), -1);
        //snark dmg mult
        player.SnarkDmgMult = (int)playerSaveFile.Get("SnarkDmgMult");
        //available upgrade
        player.AvailableUpgrade = (int)playerSaveFile.Get("AvailableUpgrades");
        //items
        player.RefreshItems();
        player.WeaponNode.RefreshItems();
        //player item 1
        LoadPlayerItems(playerSaveFile, player, "PlayerItem", 1);
        //player item 2
        LoadPlayerItems(playerSaveFile, player, "PlayerItem", 2);
        //weap item 1
        LoadPlayerItems(playerSaveFile, player, "WeapItem", 1);
        //weap item 2
        LoadPlayerItems(playerSaveFile, player, "WeapItem", 2);
        player.UpdateUpgrade();
        player.UpdateSlotsIcon();
        player.UpdateStatsDisp();
    }


    void LoadPlayerItems(Resource playerSaveFile, Player player, String itemType, int slotNum) {
        String slot = itemType + slotNum;
        if((String)playerSaveFile.Get(slot) != "") {
            PackedScene itemPack = (PackedScene)ResourceLoader.Load((String)playerSaveFile.Get(slot));
            Item item = (Item)itemPack.Instance();
            if(itemType == "PlayerItem") {
                ((Node2D)player.Get("ItemSlot" + slotNum + "Node")).AddChild(item);
                player.ActivateItem(slotNum);
            }
            else if(itemType == "WeapItem") {
                ((Node2D)player.WeaponNode.Get("ItemSlot" + slotNum + "Node")).AddChild(item);
                player.WeaponNode.ActivateItem(slotNum);
            }
        }
    }


    void InitLevelData() {
        Resource levelSaveFile = (Resource)Saver.Get("level_save_file");
        String[] levelDataArr = {
            "Collectables",
            "Triggers",
            "Walls",
            "NextLevels"
        };
        if(VerifySaveFile(levelSaveFile, levelDataArr) == false) {
            return;
        }
        Resource worldSaveFile = (Resource)Saver.Get("world_save_file");
        String[] worldDataArr = {
            "NextLevels",
            "EnemySpawns",
            "LevelPool"
        };
        if(VerifySaveFile(worldSaveFile, worldDataArr) == false) {
            return;
        }
        //init save file dict
        foreach(Node2D node in currentLevel.GetChildren()) {
            if(node is Collectable) {
                InitLevelObject(levelSaveFile, node, levelDataArr[0]);
            }
            else if(node is Trigger) {
                InitLevelObject(levelSaveFile, node, levelDataArr[1]);
            }
            //secret walls and stuff
            else if(node is Wall) {
                InitLevelObject(levelSaveFile, node, levelDataArr[2]);
            }
            else if(node is NextLevel) {
                InitLevelObject(levelSaveFile, node, levelDataArr[3]);
                //enlist to world data
                Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)worldSaveFile.Get("NextLevels");
                if(dict.Contains(node.GetPath().ToString()) == false) {
                    dict.Add(node.GetPath().ToString(), "");
                }
            }
        }
    }


    void InitLevelObject(Resource levelSaveFile, Node2D levelObj, String objType) {
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)levelSaveFile.Get(objType);
        if(dict.Contains(levelObj.GetPath().ToString()) == false) {
            dict.Add(levelObj.GetPath().ToString(), true);
        }
        else if((bool)dict[levelObj.GetPath().ToString()] == false) {
            ((ILevelObject)levelObj).Switch();
        }
        Godot.Collections.Array arr = new Godot.Collections.Array();
        //pass path
        arr.Add(levelObj.GetPath().ToString());
        //pass type
        arr.Add(objType);
        levelObj.Connect(((ILevelObject)levelObj).SwitchSignal, this, nameof(OnLevelObjectSwitched), arr);
    }


    void OnLevelObjectSwitched(String path, String type) {
        Resource levelSaveFile = (Resource)Saver.Get("level_save_file");
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)levelSaveFile.Get(type);
        dict[path.ToString()] = false;
        Saver.Call("save_level_data");
    }


    public async void GoToMainMenu() {
        FadeAnim.Play("fade_in");
        SavePlayerData();
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        currentLevel.QueueFree();
        MainMenuNode.Show();
    }


    public async void GoToLevel(String fileName, String nodePos, Player player, bool loadPlayerData) {
        FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        if(IsInstanceValid(currentLevel)) {
            Player currPlayer = (Player)currentLevel.GetNodeOrNull("Player");
            if(IsInstanceValid(currPlayer) && currPlayer == player) {
                currentLevel.RemoveChild(player);
            }
            Saver.Call("save_level_data");
        }
        if(player.IsConnected(nameof(Entity.Died), this, "OnPlayerDied") == false) {
            player.Connect(nameof(Entity.Died), this, "OnPlayerDied");
        }
        CallDeferred(nameof(GoToLevelDef), fileName, nodePos, player, loadPlayerData);
    }


    void GoToLevelDef(String fileName, String nodePos, Player player, bool loadPlayerData) {
        String prevLvlPack = "";
        if(IsInstanceValid(currentLevel)) {
            prevLvlPack = currentLevel.Filename;
            currentLevel.Free();
        }
        PackedScene lvlPack = (PackedScene)ResourceLoader.Load(fileName); 
        currentLevel = (Level)lvlPack.Instance();
        currentLevel.AddChild(player);
        GameNode.AddChild(currentLevel);
        Node2D spawnPos = currentLevel.GetNodeOrNull<Node2D>(nodePos);
        player.GlobalPosition = ((Node2D)spawnPos).GlobalPosition;
        if(loadPlayerData) {
            LoadPlayerData(player);
        }
        InitLevelData();
        //link entrance to current level within world data
        Node2D entrance = spawnPos.GetParentOrNull<Node2D>();
        if(entrance is NextLevel) {
            ((NextLevel)entrance).LinkToLevel(prevLvlPack);
        }
        Saver.Call("save_level_data");
        Saver.Call("save_world_data");
    }


    void OnPlayerDied() {
        SavePlayerData();
    }


    void OnFadeAnimFinished(String animName) {
        if(animName == "fade_in") {
            FadeAnim.Play("fade_out");
        }
    }


}
