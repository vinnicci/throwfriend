using Godot;
using System;

public class Main : Node
{
    public MainMenu MainMenuNode {get; set;}
    public Node GameNode {get; set;}
    public AnimationPlayer FadeAnim {get; set;}

    ColorRect screenColor;
    Level currentLevel;
    Node saver;


    public override void _Ready()
    {
        base._Ready();
        GD.Randomize();
        FadeAnim = (AnimationPlayer)GetNode("CanvasLayer/Anim");
        screenColor = (ColorRect)GetNode("CanvasLayer/ColorRect");
        saver = GetNode("Saver");
    }


    const String STARTING_SCENE = "res://test scenes/TestLevel1.tscn";


    public void NewGame() {
        PackedScene playerPack = (PackedScene)ResourceLoader.Load(Global.PLAYER_SCN);
        GoToLevel(STARTING_SCENE, "SavePoint", (Player)playerPack.Instance(), false);
        saver.Call("new_player_save_file");
        saver.Call("new_level_save_file");
    }


    bool VerifyDir() {
        String[] filesArr = {
            Global.SAVE_DIR + "player_save.tres",
            Global.SAVE_DIR + "level_save.tres"
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
        if(IsInstanceValid((Resource)saver.Get("player_save_file")) == false) {
            saver.Set("player_save_file", ResourceLoader.Load(Global.SAVE_DIR + "player_save.tres"));
        }
        Resource playerSaveFile = (Resource)saver.Get("player_save_file");
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
        Resource playerSaveFile = (Resource)saver.Get("player_save_file");
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
        saver.Call("save_player_data");
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
        Resource playerSaveFile = (Resource)saver.Get("player_save_file");
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
            player.ItemSlot1Node.AddChild(item);
            player.ActivateItem(slotNum);
        }
    }


    void InitLevelData() {
        if(IsInstanceValid((Resource)saver.Get("level_save_file")) == false) {
            saver.Set("level_save_file", ResourceLoader.Load(Global.SAVE_DIR + "level_save.tres"));
        }
        Resource levelSaveFile = (Resource)saver.Get("level_save_file");
        String[] levelDataArr = {
            "Collectables",
            "Triggers"
        };
        if(VerifySaveFile(levelSaveFile, levelDataArr) == false) {
            return;
        }
        //init save file dict
        foreach(Node2D node in currentLevel.GetChildren()) {
            //collectables
            if(node is Collectable) {
                InitLevelObject(levelSaveFile, node, "Collectables");
            }
            //triggers
            else if(node is Trigger) {
                InitLevelObject(levelSaveFile, node, "Triggers");
            }
        }
        saver.Call("save_level_data");
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
        GD.Print(((ILevelObject)levelObj).SwitchSignal);
        levelObj.Connect(((ILevelObject)levelObj).SwitchSignal, this, nameof(OnLevelObjectSwitched), arr);
    }


    void OnLevelObjectSwitched(String path, String type) {
        Resource levelSaveFile = (Resource)saver.Get("level_save_file");
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)levelSaveFile.Get(type);
        dict[path.ToString()] = false;
        saver.Call("save_level_data");
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
            saver.Call("save_level_data");
        }
        if(player.IsConnected(nameof(Entity.Died), this, "OnPlayerDied") == false) {
            player.Connect(nameof(Entity.Died), this, "OnPlayerDied");
        }
        CallDeferred(nameof(GoToLevelDef), fileName, nodePos, player, loadPlayerData);
    }


    void GoToLevelDef(String fileName, String nodePos, Player player, bool loadPlayerData) {
        if(IsInstanceValid(currentLevel)) {
            currentLevel.Free();
        }
        PackedScene lvlPack = (PackedScene)ResourceLoader.Load(fileName); 
        currentLevel = (Level)lvlPack.Instance();
        currentLevel.AddChild(player);
        GameNode.AddChild(currentLevel);
        var spawnPos = currentLevel.GetNode(nodePos);
        player.GlobalPosition = ((Node2D)spawnPos).GlobalPosition;
        if(loadPlayerData) {
            LoadPlayerData(player);
        }
        InitLevelData();
    }


    void OnPlayerDied() {
        SavePlayerData();
    }


    void OnFadeAnimFinished(String animName) {
        if(animName == "fade_in") {
            FadeAnim.Play("fade_out");
        }
    }


    public override void _ExitTree()
    {
        if(IsInstanceValid(currentLevel)) {
            SavePlayerData();
        }
        base._ExitTree();
        QueueFree();
    }


}
