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


    public void SaveData(Godot.Collections.Dictionary dict) {
        Directory dir = new Directory();
        if(dir.FileExists(Global.SAVE_DIR) == false) {
            dir.MakeDirRecursive(Global.SAVE_DIR);
        }
        saver.Call("save_data", dict);
    }


    const String STARTING_SCENE = "res://test scenes/TestLevel1.tscn";


    public void NewGame() {
        PackedScene playerPack = (PackedScene)ResourceLoader.Load(Global.PLAYER_SCN);
        GoToLevel(STARTING_SCENE, "SavePoint", (Player)playerPack.Instance());
    }


    bool VerifyDir() {
        String[] filesArr = {
            Global.SAVE_DIR + "save.tres",
        };
        Directory dir = new Directory();
        foreach(String file in filesArr) {
            if(dir.FileExists(Global.SAVE_DIR + "save.tres") == false) {
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
        Resource saveFile = ResourceLoader.Load(Global.SAVE_DIR + "save.tres");
        if(VerifySaveFile(saveFile) == false) {
            return;
        }
        GoToLevel((String)saveFile.Get("Level"), "SavePoint", player, saveFile);
    }


    bool VerifySaveFile(Resource saveFile) {
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
        foreach(String data in saveDataArr) {
            if(saveFile.Get(data) == null) {
                GD.PrintErr("Invalid save file.");
                return false;
            }
        }
        return true;
    }


    void LoadData(Player player, Resource saveFile) {
        //hp
        player.ChangeEntityBaseStats((int)saveFile.Get("MaxHP"), -1);
        //snark dmg mult
        player.SnarkDmgMult = (int)saveFile.Get("SnarkDmgMult");
        //available upgrade
        player.AvailableUpgrade = (int)saveFile.Get("AvailableUpgrades");
        player.UpdateUpgrade();
        player.UpdateStatsDisp();
    }


    void LoadItems(Player player, Resource saveFile) {
        //player item 1
        PackedScene itemPack;
        Item item;
        player.RefreshItems();
        player.WeaponNode.RefreshItems();
        if((String)saveFile.Get("PlayerItem1") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)saveFile.Get("PlayerItem1"));
            item = (Item)itemPack.Instance();
            player.ItemSlot1Node.AddChild(item);
            player.RefreshItems();
            player.ActivateItem(1);
        }
        //player item 2
        if((String)saveFile.Get("PlayerItem2") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)saveFile.Get("PlayerItem2"));
            item = (Item)itemPack.Instance();
            player.ItemSlot2Node.AddChild(item);
            player.RefreshItems();
            player.ActivateItem(2);
        }
        //weap item 1
        if((String)saveFile.Get("WeapItem1") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)saveFile.Get("WeapItem1"));
            item = (Item)itemPack.Instance();
            player.WeaponNode.ItemSlot1Node.AddChild(item);
            player.WeaponNode.RefreshItems();
            player.WeaponNode.ActivateItem(1);
        }
        //weap item 2
        if((String)saveFile.Get("WeapItem2") != "") {
            itemPack = (PackedScene)ResourceLoader.Load((String)saveFile.Get("WeapItem2"));
            item = (Item)itemPack.Instance();
            player.WeaponNode.ItemSlot2Node.AddChild(item);
            player.WeaponNode.RefreshItems();
            player.WeaponNode.ActivateItem(2);
        }
        player.UpdateSlotsIcon();
    }


    public async void GoToMainMenu() {
        FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        currentLevel.QueueFree();
        MainMenuNode.Show();
    }


    public async void GoToLevel(String fileName, String nodePos, Player player, Resource saveFile = null) {
        FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        if(IsInstanceValid(currentLevel) == true) {
            Player currPlayer = (Player)currentLevel.GetNodeOrNull("Player");
            if(IsInstanceValid(currPlayer) == true && currPlayer == player) {
                currentLevel.RemoveChild(player);
            }
        }
        CallDeferred(nameof(GoToLevelDef), fileName, nodePos, player, saveFile);
    }


    void GoToLevelDef(String fileName, String nodePos, Player player, Resource saveFile = null) {
        if(IsInstanceValid(currentLevel) == true) {
            currentLevel.Free();
        }
        PackedScene lvlPack = (PackedScene)ResourceLoader.Load(fileName); 
        currentLevel = (Level)lvlPack.Instance();
        currentLevel.AddChild(player);
        GameNode.AddChild(currentLevel);
        var spawnPos = currentLevel.GetNode(nodePos);
        player.GlobalPosition = ((Node2D)spawnPos).GlobalPosition;
        if(saveFile != null) {
            LoadItems(player, saveFile);
            LoadData(player, saveFile);
        }
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
