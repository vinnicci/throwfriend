using Godot;
using System;

//  World Layout Rules:
//  Secret levels must have THREE walls (IDs: 7, 11, 13, 14)
//  Checkpoints max entrance of THREE (no ID 0)
//  A level containing secret must only have ONE wall (3 entrances) (IDs: 1, 2, 4, 8)

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
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("WorldCells");
        if(dict.Count != 0) {
            return;
        }
        InitTileMap(worldTileMap1, 1);
        InitTileMap(worldTileMap2, 2);
        InitTileMap(worldTileMap3, 3);
        mainNode.WorldSaveFile.Set("WorldCells", cells);
    }


    Godot.Collections.Dictionary marks = new Godot.Collections.Dictionary();
    Godot.Collections.Dictionary cells = new Godot.Collections.Dictionary();
    // cells dict format
    // cells = {
    //     pos : {
    //         type: "lvl type"
    //         id: "tilemap id"
    //         set: "tileset"
    //         scn: "filename"
    //         hpMult: "enemy hp multiplier"
    //         speedMult: "enemy speed multiplier"
    //     }
    // }


    void InitTileMap(TileMap worldTileMap, int setNum) {
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
            WorldMarker marker = GetLevelMarker(pos);
            WorldMarker.LevelType type;
            if(IsInstanceValid(marker) == false) {
                type = WorldMarker.LevelType.None;
            }
            else {
                type = marker.levelType;
            }
            if(type == WorldMarker.LevelType.Start) {
                mainNode.PlayerSaveFile.Set("CurrentCell", pos);
            }
            posDict.Add("type", type);
            posDict.Add("id", worldTileMap.GetCellv(pos));
            posDict.Add("set", setNum);
            if(type == WorldMarker.LevelType.Misc) {
                posDict.Add("scn", marker.miscLevelFile);
            }
            else {
                posDict.Add("scn", "");
            }
            posDict.Add("hpMult", SetDifficulty(setNum, 0));
            posDict.Add("speedMult", SetDifficulty(setNum, 1));
            cells.Add(pos, posDict);
        }
    }


    float SetDifficulty(int setNum, int type) {
        float rOut = 0;
        switch(setNum) {
            case 1: return 1f;
            case 2: rOut = type == 0 ? 2f : 1.25f; break;
            case 3: rOut = type == 0 ? 4f : 1.5f; break;
        }
        return rOut;
    }


    WorldMarker GetLevelMarker(Vector2 pos) {
        if(marks.Contains(pos)) {
            return (WorldMarker)marks[pos];
        }
        return default;
    }


}
