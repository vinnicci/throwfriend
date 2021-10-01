using Godot;
using System;

public class WorldLayout : Node2D
{
    TileMap worldTileMap1;
    TileMap worldTileMap2;
    TileMap worldTileMap3;
    Main mainNode;


    public override void _Ready()
    {
        base._Ready();
        mainNode = (Main)GetNode("/root/Main");
        worldTileMap1 = (TileMap)GetNode("WorldTileMap1");
        worldTileMap2 = (TileMap)GetNode("WorldTileMap2");
        worldTileMap3 = (TileMap)GetNode("WorldTileMap3");
    }


    public void GenerateLayout() {
        Godot.Collections.Dictionary marks = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary cells = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("WorldCells");
        if(dict.Count != 0) {
            return;
        }
        InitTileMap(worldTileMap1, 1, marks, cells);
        InitTileMap(worldTileMap2, 2, marks, cells);
        InitTileMap(worldTileMap3, 3, marks, cells);
        mainNode.WorldSaveFile.Set("WorldCells", cells);
    }
    

    // cells dict format
    // cells = {
    //     pos : {
    //         type: "lvl type"
    //         id: "tilemap id"
    //         set: "tileset"
    //         scn: "filename"
    //         name: "lvl name"
    //         hpMult: "enemy hp multiplier"
    //         speedMult: "enemy speed multiplier"
    //     }
    // }


    void InitTileMap(TileMap worldTileMap, int setNum,
    Godot.Collections.Dictionary marks, Godot.Collections.Dictionary cells) {
        foreach(Node2D node in GetChildren()) {
            if(node is WorldMarker) {
                Vector2 key = worldTileMap.WorldToMap(worldTileMap.ToLocal(node.GlobalPosition));
                if(marks.Contains(key) == false) {
                    marks.Add(key, node);
                }
            }
        }
        foreach(Vector2 pos in worldTileMap.GetUsedCells()) {
            Godot.Collections.Dictionary posDict = new Godot.Collections.Dictionary();
            WorldMarker marker = GetLevelMarker(pos, marks);
            if(marker.levelName == "Town Entrance") {
                mainNode.PlayerSaveFile.Set("CurrentCell", pos);
            }
            posDict.Add("name", marker.levelName);
            posDict.Add("scn", SetLevelFile(marker.levelFiles));
            posDict.Add("hpMult", SetDifficulty(setNum, 0));
            posDict.Add("speedMult", SetDifficulty(setNum, 1));
            cells.Add(pos, posDict);
        }
    }


    String SetLevelFile(Godot.Collections.Array<String> arr) {
        String lvl = "";
        Godot.Collections.Array usedLvls =
        (Godot.Collections.Array)mainNode.WorldSaveFile.Get("LevelsUsed");
        do {
            arr.Shuffle();
            lvl = (String)arr[0];
            arr.RemoveAt(0);
        } while(usedLvls.Contains(lvl));
        usedLvls.Add(lvl);
        return lvl;
    }


    const float HP_2 = 3f;
    const float SPEED_2 = 1.15f;
    const float HP_3 = 6f;
    const float SPEED_3 = 1.3f;


    float SetDifficulty(int setNum, int type) {
        float rOut = 0;
        switch(setNum) {
            case 1: return 1f;
            case 2: rOut = type == 0 ? HP_2 : SPEED_2; break;
            case 3: rOut = type == 0 ? HP_3 : SPEED_3; break;
        }
        return rOut;
    }


    WorldMarker GetLevelMarker(Vector2 pos, Godot.Collections.Dictionary marks) {
        if(marks.Contains(pos)) {
            return (WorldMarker)marks[pos];
        }
        return default;
    }


}
