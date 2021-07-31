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
        //hp
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
        Item item = player.Item1;
        if(IsInstanceValid(item)) {
            playerSaveFile.Set("PlayerItem1", item.Filename);
        }
        else {
            playerSaveFile.Set("PlayerItem1", "");
        }
        //player item 2
        item = player.Item2;
        if(IsInstanceValid(item)) {
            playerSaveFile.Set("PlayerItem2", item.Filename);
        }
        else {
            playerSaveFile.Set("PlayerItem2", "");
        }
        //weap item 1
        item = player.WeaponNode.Item1;
        if(IsInstanceValid(item)) {
            playerSaveFile.Set("WeapItem1", item.Filename);
        }
        else {
            playerSaveFile.Set("WeapItem1", "");
        }
        //weap item 2
        item = player.WeaponNode.Item2;
        if(IsInstanceValid(item)) {
            playerSaveFile.Set("WeapItem2", item.Filename);
        }
        else {
            playerSaveFile.Set("WeapItem2", "");
        }
        player.UpdateStatsDisp();
        //call save function in Main Singleton
        saver.Call("save_player_data");
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
        PackedScene itemPack;
        Item item;
        if((String)playerSaveFile.Get("PlayerItem1") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)playerSaveFile.Get("PlayerItem1"));
            item = (Item)itemPack.Instance();
            player.ItemSlot1Node.AddChild(item);
            player.ActivateItem(1);
        }
        //player item 2
        if((String)playerSaveFile.Get("PlayerItem2") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)playerSaveFile.Get("PlayerItem2"));
            item = (Item)itemPack.Instance();
            player.ItemSlot2Node.AddChild(item);
            player.ActivateItem(2);
        }
        //weap item 1
        if((String)playerSaveFile.Get("WeapItem1") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)playerSaveFile.Get("WeapItem1"));
            item = (Item)itemPack.Instance();
            player.WeaponNode.ItemSlot1Node.AddChild(item);
            player.WeaponNode.ActivateItem(1);
        }
        //weap item 2
        if((String)playerSaveFile.Get("WeapItem2") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)playerSaveFile.Get("WeapItem2"));
            item = (Item)itemPack.Instance();
            player.WeaponNode.ItemSlot2Node.AddChild(item);
            player.WeaponNode.ActivateItem(2);
        }
        player.UpdateUpgrade();
        player.UpdateSlotsIcon();
        player.UpdateStatsDisp();
    }


    void InitLevelData() {
        if(IsInstanceValid((Resource)saver.Get("level_save_file")) == false) {
            saver.Set("level_save_file", ResourceLoader.Load(Global.SAVE_DIR + "level_save.tres"));
        }
        Resource levelSaveFile = (Resource)saver.Get("level_save_file");
        String[] levelDataArr = {
            "Collectables"
        };
        if(VerifySaveFile(levelSaveFile, levelDataArr) == false) {
            return;
        }
        //init save file dict
        foreach(Node2D node in currentLevel.GetChildren()) {
            //collectables
            if(node is BaseCollectable) {
                InitCollectables(levelSaveFile, (BaseCollectable)node,
                (Godot.Collections.Dictionary)levelSaveFile.Get("Collectables"));
            }
        }
        saver.Call("save_level_data");
    }


    void InitCollectables(Resource levelSaveFile, BaseCollectable collectable, Godot.Collections.Dictionary dict) {
        //init collectables data
        if(dict.Contains(collectable.GetPath().ToString()) == false) {
            dict.Add(collectable.GetPath().ToString(), true);
        }
        else if((bool)dict[collectable.GetPath().ToString()] == false) {
            collectable.QueueFree();
        }
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(collectable.GetPath().ToString());
        collectable.Connect("body_entered", this, nameof(OnCollectableCollected), arr);
    }


    void OnCollectableCollected(Godot.Object body, String collectablePath) {
        Resource levelSaveFile = (Resource)saver.Get("level_save_file");
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)levelSaveFile.Get("Collectables");
        dict[collectablePath.ToString()] = false;
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
        base._ExitTree();
        QueueFree();
    }


}
