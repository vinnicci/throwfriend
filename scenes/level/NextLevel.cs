using Godot;
using System;

public class NextLevel : Area2D, ILevelObject
{
    String nextLevel;
    bool proceeding = false;
    Player player;

    public Main MainNode {get; set;}
    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}

    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}


    public override void _Ready() {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
        InitLevelObject();
    }


    public void InitLevelObject() {
        SwitchedOnSignal = nameof(SwitchedOn);
        SwitchedOffSignal = nameof(SwitchedOff);
    }


    public void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnTriggeredAllBoundTriggers));
    }


    public Level LevelNode {get; set;}


    void OnNextLevelBodyEntered(Godot.Object body) {
        if(body is Player) {
            player = (Player)body;
            proceeding = true;
            if(player.WeaponNode.CurrentState != Weapon.States.HELD) {
                player.WarnPlayer("YOU MUST CARRY SNARK TO PROCEED");
            }
            else if(LevelNode.PlayerEngaging > 0) {
                player.WarnPlayer("CAN'T PROCEED WHILE ENEMIES ARE TRACKING YOU");
            }
            EmitSignal(nameof(SwitchedOn));
        }
    }


    void OnNextLevelBodyExited(Godot.Object body) {
        if(body is Player) {
            proceeding = false;
        }
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(proceeding && player.WeaponNode.CurrentState == Weapon.States.HELD && LevelNode.PlayerEngaging == 0) {
            MainNode.GoToLevel(GetRandomLevel(), GetEntrance(), (Player)player, false);
            SetProcess(false);
        }
    }


    String GetRandomLevel() {
        Vector2 oldCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        ShiftCurrentCell();
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("NextLevels");
        Godot.Collections.Dictionary posDict =
        (Godot.Collections.Dictionary)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("WorldCells"))[currentCell];
        String lvl = "";
        String key = oldCell.ToString() + GetPath().ToString();
        if(dict.Contains(key) && (String)dict[key] != "") {
            return (String)dict[key];
        }
        if((WorldMarker.LevelType)posDict["type"] == WorldMarker.LevelType.Misc ||
        (WorldMarker.LevelType)posDict["type"] == WorldMarker.LevelType.Start) {
            lvl = (String)posDict["scn"];
        }
        else {
            Godot.Collections.Array arr =
            GetFromSet((int)posDict["id"], (int)posDict["set"], (WorldMarker.LevelType)posDict["type"]);
            Godot.Collections.Array usedLvls =
            (Godot.Collections.Array)MainNode.WorldSaveFile.Get("LevelsUsed");
            do {
                arr.Shuffle();
                lvl = (String)arr[0];
                arr.RemoveAt(0);
            }
            while(usedLvls.Contains(lvl));
            usedLvls.Add(lvl);
        }
        LinkToLevel(lvl, oldCell.ToString());
        return lvl;
    }


    Godot.Collections.Array GetFromSet(int id, int setNum, WorldMarker.LevelType type) {
        String idStr = "ID" + id.ToString().PadLeft(2, '0');
        String setNumStr = "_SET" + setNum.ToString();
        String typeStr = "";
        switch(type) {
            case WorldMarker.LevelType.None: break;
            case WorldMarker.LevelType.Checkpoint: typeStr = "_CHECKPT"; break;
            case WorldMarker.LevelType.Secret: typeStr = "_SECRET"; break;
            case WorldMarker.LevelType.SecretN: typeStr = "_SECRET_N"; break;
            case WorldMarker.LevelType.SecretW: typeStr = "_SECRET_W"; break;
            case WorldMarker.LevelType.SecretE: typeStr = "_SECRET_E"; break;
            case WorldMarker.LevelType.SecretS: typeStr = "_SECRET_S"; break;
        }
        switch(idStr + setNumStr + typeStr) {
            //set 1 none
            case nameof(Global.ID00_SET1): return new Godot.Collections.Array(Global.ID00_SET1);
            case nameof(Global.ID01_SET1): return new Godot.Collections.Array(Global.ID01_SET1);
            case nameof(Global.ID02_SET1): return new Godot.Collections.Array(Global.ID02_SET1);
            case nameof(Global.ID03_SET1): return new Godot.Collections.Array(Global.ID03_SET1);
            case nameof(Global.ID04_SET1): return new Godot.Collections.Array(Global.ID04_SET1);
            case nameof(Global.ID05_SET1): return new Godot.Collections.Array(Global.ID05_SET1);
            case nameof(Global.ID06_SET1): return new Godot.Collections.Array(Global.ID06_SET1);
            case nameof(Global.ID07_SET1): return new Godot.Collections.Array(Global.ID07_SET1);
            case nameof(Global.ID08_SET1): return new Godot.Collections.Array(Global.ID08_SET1);
            case nameof(Global.ID09_SET1): return new Godot.Collections.Array(Global.ID09_SET1);
            case nameof(Global.ID10_SET1): return new Godot.Collections.Array(Global.ID10_SET1);
            case nameof(Global.ID11_SET1): return new Godot.Collections.Array(Global.ID11_SET1);
            case nameof(Global.ID12_SET1): return new Godot.Collections.Array(Global.ID12_SET1);
            case nameof(Global.ID13_SET1): return new Godot.Collections.Array(Global.ID13_SET1);
            case nameof(Global.ID14_SET1): return new Godot.Collections.Array(Global.ID14_SET1);
            //set 2 none
            case nameof(Global.ID00_SET2): return new Godot.Collections.Array(Global.ID00_SET2);
            case nameof(Global.ID01_SET2): return new Godot.Collections.Array(Global.ID01_SET2);
            case nameof(Global.ID02_SET2): return new Godot.Collections.Array(Global.ID02_SET2);
            case nameof(Global.ID03_SET2): return new Godot.Collections.Array(Global.ID03_SET2);
            case nameof(Global.ID04_SET2): return new Godot.Collections.Array(Global.ID04_SET2);
            case nameof(Global.ID05_SET2): return new Godot.Collections.Array(Global.ID05_SET2);
            case nameof(Global.ID06_SET2): return new Godot.Collections.Array(Global.ID06_SET2);
            case nameof(Global.ID07_SET2): return new Godot.Collections.Array(Global.ID07_SET2);
            case nameof(Global.ID08_SET2): return new Godot.Collections.Array(Global.ID08_SET2);
            case nameof(Global.ID09_SET2): return new Godot.Collections.Array(Global.ID09_SET2);
            case nameof(Global.ID10_SET2): return new Godot.Collections.Array(Global.ID10_SET2);
            case nameof(Global.ID11_SET2): return new Godot.Collections.Array(Global.ID11_SET2);
            case nameof(Global.ID12_SET2): return new Godot.Collections.Array(Global.ID12_SET2);
            case nameof(Global.ID13_SET2): return new Godot.Collections.Array(Global.ID13_SET2);
            case nameof(Global.ID14_SET2): return new Godot.Collections.Array(Global.ID14_SET2);
            //set 3 none
            case nameof(Global.ID00_SET3): return new Godot.Collections.Array(Global.ID00_SET3);
            case nameof(Global.ID01_SET3): return new Godot.Collections.Array(Global.ID01_SET3);
            case nameof(Global.ID02_SET3): return new Godot.Collections.Array(Global.ID02_SET3);
            case nameof(Global.ID03_SET3): return new Godot.Collections.Array(Global.ID03_SET3);
            case nameof(Global.ID04_SET3): return new Godot.Collections.Array(Global.ID04_SET3);
            case nameof(Global.ID05_SET3): return new Godot.Collections.Array(Global.ID05_SET3);
            case nameof(Global.ID06_SET3): return new Godot.Collections.Array(Global.ID06_SET3);
            case nameof(Global.ID07_SET3): return new Godot.Collections.Array(Global.ID07_SET3);
            case nameof(Global.ID08_SET3): return new Godot.Collections.Array(Global.ID08_SET3);
            case nameof(Global.ID09_SET3): return new Godot.Collections.Array(Global.ID09_SET3);
            case nameof(Global.ID10_SET3): return new Godot.Collections.Array(Global.ID10_SET3);
            case nameof(Global.ID11_SET3): return new Godot.Collections.Array(Global.ID11_SET3);
            case nameof(Global.ID12_SET3): return new Godot.Collections.Array(Global.ID12_SET3);
            case nameof(Global.ID13_SET3): return new Godot.Collections.Array(Global.ID13_SET3);
            case nameof(Global.ID14_SET3): return new Godot.Collections.Array(Global.ID14_SET3);
            //set 1 checkpt
            case nameof(Global.ID01_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID01_SET1_CHECKPT);
            case nameof(Global.ID02_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID02_SET1_CHECKPT);
            case nameof(Global.ID03_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID03_SET1_CHECKPT);
            case nameof(Global.ID04_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID04_SET1_CHECKPT);
            case nameof(Global.ID05_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID05_SET1_CHECKPT);
            case nameof(Global.ID06_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID06_SET1_CHECKPT);
            case nameof(Global.ID07_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID07_SET1_CHECKPT);
            case nameof(Global.ID08_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID08_SET1_CHECKPT);
            case nameof(Global.ID09_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID09_SET1_CHECKPT);
            case nameof(Global.ID10_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID10_SET1_CHECKPT);
            case nameof(Global.ID11_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID11_SET1_CHECKPT);
            case nameof(Global.ID12_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID12_SET1_CHECKPT);
            case nameof(Global.ID13_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID13_SET1_CHECKPT);
            case nameof(Global.ID14_SET1_CHECKPT): return new Godot.Collections.Array(Global.ID14_SET1_CHECKPT);
            //set 2 checkpt
            case nameof(Global.ID01_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID01_SET2_CHECKPT);
            case nameof(Global.ID02_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID02_SET2_CHECKPT);
            case nameof(Global.ID03_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID03_SET2_CHECKPT);
            case nameof(Global.ID04_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID04_SET2_CHECKPT);
            case nameof(Global.ID05_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID05_SET2_CHECKPT);
            case nameof(Global.ID06_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID06_SET2_CHECKPT);
            case nameof(Global.ID07_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID07_SET2_CHECKPT);
            case nameof(Global.ID08_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID08_SET2_CHECKPT);
            case nameof(Global.ID09_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID09_SET2_CHECKPT);
            case nameof(Global.ID10_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID10_SET2_CHECKPT);
            case nameof(Global.ID11_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID11_SET2_CHECKPT);
            case nameof(Global.ID12_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID12_SET2_CHECKPT);
            case nameof(Global.ID13_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID13_SET2_CHECKPT);
            case nameof(Global.ID14_SET2_CHECKPT): return new Godot.Collections.Array(Global.ID14_SET2_CHECKPT);
            //set 3 checkpt
            case nameof(Global.ID01_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID01_SET3_CHECKPT);
            case nameof(Global.ID02_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID02_SET3_CHECKPT);
            case nameof(Global.ID03_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID03_SET3_CHECKPT);
            case nameof(Global.ID04_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID04_SET3_CHECKPT);
            case nameof(Global.ID05_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID05_SET3_CHECKPT);
            case nameof(Global.ID06_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID06_SET3_CHECKPT);
            case nameof(Global.ID07_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID07_SET3_CHECKPT);
            case nameof(Global.ID08_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID08_SET3_CHECKPT);
            case nameof(Global.ID09_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID09_SET3_CHECKPT);
            case nameof(Global.ID10_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID10_SET3_CHECKPT);
            case nameof(Global.ID11_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID11_SET3_CHECKPT);
            case nameof(Global.ID12_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID12_SET3_CHECKPT);
            case nameof(Global.ID13_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID13_SET3_CHECKPT);
            case nameof(Global.ID14_SET3_CHECKPT): return new Godot.Collections.Array(Global.ID14_SET3_CHECKPT);
            //set 1 secret
            case nameof(Global.ID07_SET1_SECRET): return new Godot.Collections.Array(Global.ID07_SET1_SECRET);
            case nameof(Global.ID11_SET1_SECRET): return new Godot.Collections.Array(Global.ID11_SET1_SECRET);
            case nameof(Global.ID13_SET1_SECRET): return new Godot.Collections.Array(Global.ID13_SET1_SECRET);
            case nameof(Global.ID14_SET1_SECRET): return new Godot.Collections.Array(Global.ID14_SET1_SECRET);
            //set 2 secret
            case nameof(Global.ID07_SET2_SECRET): return new Godot.Collections.Array(Global.ID07_SET2_SECRET);
            case nameof(Global.ID11_SET2_SECRET): return new Godot.Collections.Array(Global.ID11_SET2_SECRET);
            case nameof(Global.ID13_SET2_SECRET): return new Godot.Collections.Array(Global.ID13_SET2_SECRET);
            case nameof(Global.ID14_SET2_SECRET): return new Godot.Collections.Array(Global.ID14_SET2_SECRET);
            //set 3 secret
            case nameof(Global.ID07_SET3_SECRET): return new Godot.Collections.Array(Global.ID07_SET3_SECRET);
            case nameof(Global.ID11_SET3_SECRET): return new Godot.Collections.Array(Global.ID11_SET3_SECRET);
            case nameof(Global.ID13_SET3_SECRET): return new Godot.Collections.Array(Global.ID13_SET3_SECRET);
            case nameof(Global.ID14_SET3_SECRET): return new Godot.Collections.Array(Global.ID14_SET3_SECRET);
            //set 1 secret n
            case nameof(Global.ID01_SET1_SECRET_N): return new Godot.Collections.Array(Global.ID01_SET1_SECRET_N);
            case nameof(Global.ID02_SET1_SECRET_N): return new Godot.Collections.Array(Global.ID02_SET1_SECRET_N);
            case nameof(Global.ID04_SET1_SECRET_N): return new Godot.Collections.Array(Global.ID04_SET1_SECRET_N);
            case nameof(Global.ID08_SET1_SECRET_N): return new Godot.Collections.Array(Global.ID08_SET1_SECRET_N);
            //set 2 secret n
            case nameof(Global.ID01_SET2_SECRET_N): return new Godot.Collections.Array(Global.ID01_SET2_SECRET_N);
            case nameof(Global.ID02_SET2_SECRET_N): return new Godot.Collections.Array(Global.ID02_SET2_SECRET_N);
            case nameof(Global.ID04_SET2_SECRET_N): return new Godot.Collections.Array(Global.ID04_SET2_SECRET_N);
            case nameof(Global.ID08_SET2_SECRET_N): return new Godot.Collections.Array(Global.ID08_SET2_SECRET_N);
            //set 3 secret n
            case nameof(Global.ID01_SET3_SECRET_N): return new Godot.Collections.Array(Global.ID01_SET3_SECRET_N);
            case nameof(Global.ID02_SET3_SECRET_N): return new Godot.Collections.Array(Global.ID02_SET3_SECRET_N);
            case nameof(Global.ID04_SET3_SECRET_N): return new Godot.Collections.Array(Global.ID04_SET3_SECRET_N);
            case nameof(Global.ID08_SET3_SECRET_N): return new Godot.Collections.Array(Global.ID08_SET3_SECRET_N);
            //set 1 secret e
            case nameof(Global.ID01_SET1_SECRET_E): return new Godot.Collections.Array(Global.ID01_SET1_SECRET_E);
            case nameof(Global.ID02_SET1_SECRET_E): return new Godot.Collections.Array(Global.ID02_SET1_SECRET_E);
            case nameof(Global.ID04_SET1_SECRET_E): return new Godot.Collections.Array(Global.ID04_SET1_SECRET_E);
            case nameof(Global.ID08_SET1_SECRET_E): return new Godot.Collections.Array(Global.ID08_SET1_SECRET_E);
            //set 2 secret e
            case nameof(Global.ID01_SET2_SECRET_E): return new Godot.Collections.Array(Global.ID01_SET2_SECRET_E);
            case nameof(Global.ID02_SET2_SECRET_E): return new Godot.Collections.Array(Global.ID02_SET2_SECRET_E);
            case nameof(Global.ID04_SET2_SECRET_E): return new Godot.Collections.Array(Global.ID04_SET2_SECRET_E);
            case nameof(Global.ID08_SET2_SECRET_E): return new Godot.Collections.Array(Global.ID08_SET2_SECRET_E);
            //set 3 secret e
            case nameof(Global.ID01_SET3_SECRET_E): return new Godot.Collections.Array(Global.ID01_SET3_SECRET_E);
            case nameof(Global.ID02_SET3_SECRET_E): return new Godot.Collections.Array(Global.ID02_SET3_SECRET_E);
            case nameof(Global.ID04_SET3_SECRET_E): return new Godot.Collections.Array(Global.ID04_SET3_SECRET_E);
            case nameof(Global.ID08_SET3_SECRET_E): return new Godot.Collections.Array(Global.ID08_SET3_SECRET_E);
            //set 1 secret w
            case nameof(Global.ID01_SET1_SECRET_W): return new Godot.Collections.Array(Global.ID01_SET1_SECRET_W);
            case nameof(Global.ID02_SET1_SECRET_W): return new Godot.Collections.Array(Global.ID02_SET1_SECRET_W);
            case nameof(Global.ID04_SET1_SECRET_W): return new Godot.Collections.Array(Global.ID04_SET1_SECRET_W);
            case nameof(Global.ID08_SET1_SECRET_W): return new Godot.Collections.Array(Global.ID08_SET1_SECRET_W);
            //set 2 secret w
            case nameof(Global.ID01_SET2_SECRET_W): return new Godot.Collections.Array(Global.ID01_SET2_SECRET_W);
            case nameof(Global.ID02_SET2_SECRET_W): return new Godot.Collections.Array(Global.ID02_SET2_SECRET_W);
            case nameof(Global.ID04_SET2_SECRET_W): return new Godot.Collections.Array(Global.ID04_SET2_SECRET_W);
            case nameof(Global.ID08_SET2_SECRET_W): return new Godot.Collections.Array(Global.ID08_SET2_SECRET_W);
            //set 3 secret w
            case nameof(Global.ID01_SET3_SECRET_W): return new Godot.Collections.Array(Global.ID01_SET3_SECRET_W);
            case nameof(Global.ID02_SET3_SECRET_W): return new Godot.Collections.Array(Global.ID02_SET3_SECRET_W);
            case nameof(Global.ID04_SET3_SECRET_W): return new Godot.Collections.Array(Global.ID04_SET3_SECRET_W);
            case nameof(Global.ID08_SET3_SECRET_W): return new Godot.Collections.Array(Global.ID08_SET3_SECRET_W);
            //set 1 secret s
            case nameof(Global.ID01_SET1_SECRET_S): return new Godot.Collections.Array(Global.ID01_SET1_SECRET_S);
            case nameof(Global.ID02_SET1_SECRET_S): return new Godot.Collections.Array(Global.ID02_SET1_SECRET_S);
            case nameof(Global.ID04_SET1_SECRET_S): return new Godot.Collections.Array(Global.ID04_SET1_SECRET_S);
            case nameof(Global.ID08_SET1_SECRET_S): return new Godot.Collections.Array(Global.ID08_SET1_SECRET_S);
            //set 2 secret s
            case nameof(Global.ID01_SET2_SECRET_S): return new Godot.Collections.Array(Global.ID01_SET2_SECRET_S);
            case nameof(Global.ID02_SET2_SECRET_S): return new Godot.Collections.Array(Global.ID02_SET2_SECRET_S);
            case nameof(Global.ID04_SET2_SECRET_S): return new Godot.Collections.Array(Global.ID04_SET2_SECRET_S);
            case nameof(Global.ID08_SET2_SECRET_S): return new Godot.Collections.Array(Global.ID08_SET2_SECRET_S);
            //set 3 secret s
            case nameof(Global.ID01_SET3_SECRET_S): return new Godot.Collections.Array(Global.ID01_SET3_SECRET_S);
            case nameof(Global.ID02_SET3_SECRET_S): return new Godot.Collections.Array(Global.ID02_SET3_SECRET_S);
            case nameof(Global.ID04_SET3_SECRET_S): return new Godot.Collections.Array(Global.ID04_SET3_SECRET_S);
            case nameof(Global.ID08_SET3_SECRET_S): return new Godot.Collections.Array(Global.ID08_SET3_SECRET_S);
        }
        return default;
    }


    public void LinkToLevel(String lvl, String cell) {
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("NextLevels");
        String key = cell + GetPath().ToString();
        dict[key] = lvl;
    }


    String GetEntrance() {
        String ent = "";
        switch(Name) {
            case "N": ent = "Objects/S/SpawnPos"; break;
            case "E": ent = "Objects/W/SpawnPos"; break;
            case "W": ent = "Objects/E/SpawnPos"; break;
            case "S": ent = "Objects/N/SpawnPos"; break;
        }
        return ent;
    }


    void ShiftCurrentCell() {
        Vector2 to = Vector2.Zero;
        switch(Name) {
            case "N": to = Vector2.Up; break;
            case "E": to = Vector2.Right; break;
            case "W": to = Vector2.Left; break;
            case "S": to = Vector2.Down; break;
        }
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        MainNode.PlayerSaveFile.Set("CurrentCell", currentCell + to);
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();



    public void OnSwitchedOn() {
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("NextLevels");
        String key = MainNode.PlayerSaveFile.Get("CurrentCell").ToString() + GetPath().ToString();
        nextLevel = (String)dict[key];
    }


    public void OnSwitchedOff() {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnSwitchedOff));
    }


}
