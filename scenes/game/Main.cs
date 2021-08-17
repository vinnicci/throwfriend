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
    public Resource PlayerSaveFile {get; private set;}
    public Resource LevelSaveFile {get; private set;}
    public Resource WorldSaveFile {get; private set;}
    

    public override void _Ready()
    {
        base._Ready();
        GD.Randomize();
        FadeAnim = (AnimationPlayer)GetNode("CanvasLayer/Anim");
        screenColor = (ColorRect)GetNode("CanvasLayer/ColorRect");
        Saver = GetNode("Saver");
    }


    const String STARTING_SCENE = "res://levels/misc/Tutorial.tscn";


    public void NewGame() {
        PackedScene playerPack = (PackedScene)ResourceLoader.Load(Global.PLAYER_SCN);
        Saver.Call("new_player_save_file");
        Saver.Call("new_level_save_file");
        Saver.Call("new_world_save_file");
        PlayerSaveFile = (Resource)Saver.Get("player_save_file");
        LevelSaveFile = (Resource)Saver.Get("level_save_file");
        WorldSaveFile = (Resource)Saver.Get("world_save_file");
        ((WorldLayout)GameNode.GetNode("WorldLayout")).GenerateLayout();
        GoToLevel(STARTING_SCENE, "Objects/SavePoint/Pos", (Player)playerPack.Instance(), false);
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
            MainMenuNode.Show();
            return;
        }
        PackedScene playerPack = (PackedScene)ResourceLoader.Load("res://scenes/player/Player.tscn");
        Player player = (Player)playerPack.Instance();
        Saver.Call("load_player_data");
        Saver.Call("load_level_data");
        Saver.Call("load_world_data");
        PlayerSaveFile = (Resource)Saver.Get("player_save_file");
        LevelSaveFile = (Resource)Saver.Get("level_save_file");
        WorldSaveFile = (Resource)Saver.Get("world_save_file");
        String[] saveDataArr = {
            "SaveCell",
            "CurrentCell",
            "MaxHP",
            "SnarkDmgMult",
            "AvailableUpgrades",
            "PlayerItem1",
            "PlayerItem2",
            "WeapItem1",
            "WeapItem2"
        };
        if(VerifySaveFile(PlayerSaveFile, saveDataArr) == false) {
            return;
        }
        GoToLevel(LoadLvl(), "Objects/SavePoint/Pos", player, true);
    }


    String LoadLvl() {
        Vector2 cell = (Vector2)PlayerSaveFile.Get("SaveCell");
        PlayerSaveFile.Set("CurrentCell", cell);
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)WorldSaveFile.Get("WorldCells");
        return (String)((Godot.Collections.Dictionary)dict[cell])["scn"];
    }


    bool VerifySaveFile(Resource saveFile, String[] saveDataArr) {
        foreach(String data in saveDataArr) {
            if(saveFile.Get(data) == null) {
                GD.PrintErr("Error loading a save file.");
                return false;
            }
        }
        return true;
    }


    public void SavePlayerData(bool newCell = false) {
        Player player = (Player)currentLevel.GetNode("Player");
        //current level
        if(newCell) {
            PlayerSaveFile.Set("SaveCell", PlayerSaveFile.Get("CurrentCell"));
        }
        //replenish and then save hp
        player.ChangeEntityBaseStats(player.health, -1);
        PlayerSaveFile.Set("MaxHP", player.Health);
        //snark dmg mult
        PlayerSaveFile.Set("SnarkDmgMult", player.SnarkDmgMult);
        //available upgrades
        PlayerSaveFile.Set("AvailableUpgrades", player.AvailableUpgrade);
        //items
        player.RefreshItems();
        player.WeaponNode.RefreshItems();
        //player item 1
        SavePlayerItems(player.Item1, "PlayerItem", 1);
        //player item 2
        SavePlayerItems(player.Item2, "PlayerItem", 2);
        //weap item 1
        SavePlayerItems(player.WeaponNode.Item1, "WeapItem", 1);
        //weap item 2
        SavePlayerItems(player.WeaponNode.Item2, "WeapItem", 2);
        player.UpdateStatsDisp();
        Saver.Call("save_player_data");
    }


    void SavePlayerItems(Item item, String itemType, int slotNum) {
        String slot = itemType + slotNum;
        if(IsInstanceValid(item)) {
            PlayerSaveFile.Set(slot, item.Filename);
        }
        else {
            PlayerSaveFile.Set(slot, "");
        }
    }


    void LoadPlayerData(Player player) {
        //hp
        player.ChangeEntityBaseStats((int)PlayerSaveFile.Get("MaxHP"), -1);
        //snark dmg mult
        player.SnarkDmgMult = (int)PlayerSaveFile.Get("SnarkDmgMult");
        //available upgrade
        player.AvailableUpgrade = (int)PlayerSaveFile.Get("AvailableUpgrades");
        //items
        player.RefreshItems();
        player.WeaponNode.RefreshItems();
        //player item 1
        LoadPlayerItems(player, "PlayerItem", 1);
        //player item 2
        LoadPlayerItems(player, "PlayerItem", 2);
        //weap item 1
        LoadPlayerItems(player, "WeapItem", 1);
        //weap item 2
        LoadPlayerItems(player, "WeapItem", 2);
        player.UpdateUpgrade();
        player.UpdateSlotsIcon();
        player.UpdateStatsDisp();
    }


    void LoadPlayerItems(Player player, String itemType, int slotNum) {
        String slot = itemType + slotNum;
        if((String)PlayerSaveFile.Get(slot) != "") {
            PackedScene itemPack = (PackedScene)ResourceLoader.Load((String)PlayerSaveFile.Get(slot));
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
        String[] levelDataArr = {
            "Collectables",
            "Tiles",
            "Triggers",
            "NextLevels",
            "Enemies",
        };
        if(VerifySaveFile(LevelSaveFile, levelDataArr) == false) {
            return;
        }
        String[] worldDataArr = {
            "NextLevels",
            "EnemySpawns",
            "WorldCells",
            "Quests",
        };
        if(VerifySaveFile(WorldSaveFile, worldDataArr) == false) {
            return;
        }
        //init save file dict
        //lvl objects
        foreach(Node2D node in currentLevel.GetNode<Node2D>("Objects").GetChildren()) {
            if(node is Collectable) {
                InitLevelObject(node, "Collectables");
            }
            else if(node is Walls || node is Floors || node is Props) {
                InitLevelObject(node, "Tiles");
            } 
            else if(node is Trigger) {
                InitLevelObject(node, "Triggers");
            }
            else if(node is NextLevel) {
                InitLevelObject(node, "NextLevels");
                //enlist to world data
                Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)WorldSaveFile.Get("NextLevels");
                String key = PlayerSaveFile.Get("CurrentCell").ToString() + node.GetPath().ToString();
                if(dict.Contains(key) == false) {
                    dict.Add(key, "");
                }
            }
            if(node is IQuest) {
                //add object quest ID to world save file
                Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)WorldSaveFile.Get("Quests");
                String key = ((IQuest)node).QuestID;
                if(dict.Contains(key) == false) {
                    dict.Add(key, new Godot.Collections.Array());
                }
                ((IQuest)node).CheckQuest();
            }
        }
        foreach(Enemy enemy in currentLevel.GetNode<Node2D>("Enemies").GetChildren()) {
            InitLevelObject(enemy, "Enemies");
            if(enemy is IQuest) {
                //add enemy quest ID to world save file
                Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)WorldSaveFile.Get("Quests");
                String key = ((IQuest)enemy).QuestID;
                if(dict.Contains(key) == false) {
                    dict.Add(key, new Godot.Collections.Array());
                }
            }
        }
    }


    public void InitLevelObject(Node2D levelObj, String objType) {
        if(((ILevelObject)levelObj).Persist == false) {
            return;
        }
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)LevelSaveFile.Get(objType);
        String key = PlayerSaveFile.Get("CurrentCell").ToString() + levelObj.GetPath().ToString();
        if(dict.Contains(key) == false) {
            dict.Add(key, true);
        }
        else if((bool)dict[key] == false) {
            ((ILevelObject)levelObj).OnSwitchedOn();
        }
        Godot.Collections.Array arr = new Godot.Collections.Array();
        //pass path
        arr.Add(key);
        //pass type
        arr.Add(objType);
        levelObj.Connect(((ILevelObject)levelObj).SwitchedOnSignal, this, nameof(OnLevelObjectSwitched), arr);
    }


    void OnLevelObjectSwitched(String path, String type) {
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)LevelSaveFile.Get(type);
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
        player.IsStopped = true;
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
        AddLvlNameToWorld(fileName);
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
        ApplyEnemyMult(currentLevel);
        currentLevel.AddChild(player);
        GameNode.AddChild(currentLevel);
        Node2D spawnPos = currentLevel.GetNodeOrNull<Node2D>(nodePos);
        player.GlobalPosition = ((Node2D)spawnPos).GlobalPosition;
        if(loadPlayerData) {
            LoadPlayerData(player);
        }
        InitLevelData();
        //link entrance to current level
        Node2D entrance = spawnPos.GetParentOrNull<Node2D>();
        if(entrance is NextLevel) {
            ((NextLevel)entrance).LinkToLevel(prevLvlPack, PlayerSaveFile.Get("CurrentCell").ToString());
        }
        player.IsStopped = false;
        Saver.Call("save_level_data");
        Saver.Call("save_world_data");
    }


    void ApplyEnemyMult(Level lvl) {
        Vector2 currentCell = (Vector2)PlayerSaveFile.Get("CurrentCell");
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)((Godot.Collections.Dictionary)WorldSaveFile.Get("WorldCells"))[currentCell];
        if(lvl.overrideableEnemyMults) {
            lvl.enemyHealthMult = (float)dict["hpMult"];
            lvl.enemySpeedMult = (float)dict["speedMult"];
        }
    }


    void AddLvlNameToWorld(String fileName) {
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)WorldSaveFile.Get("WorldCells");
        Vector2 currentCell = (Vector2)PlayerSaveFile.Get("CurrentCell");
        if((String)((Godot.Collections.Dictionary)(dict[currentCell]))["scn"] == "") {
            ((Godot.Collections.Dictionary)dict[currentCell])["scn"] = fileName;
        }
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
