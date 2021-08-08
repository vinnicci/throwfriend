using Godot;
using System;

//  World Layout Rules:
//  Start level must use cell ID 13
//  Secret levels must have 3 walls (IDs: 7, 11, 13, 14)
//  Checkpoints max entrance of 3 (no ID 0)
//  A level must be adjacent to one secret room only

public class WorldLayout : Node2D
{
    TileMap worldTileMap1;
    TileMap worldTileMap2;
    TileMap worldTileMap3;
    Main mainNode;
    // cells dict format
    // cells = {
    //     pos : {
    //         type: "lvl type"
    //         id: "tilemap id"
    //         set: "tileset"
    //         scn: "filename" 
    //     }
    // }
    Godot.Collections.Dictionary cells = new Godot.Collections.Dictionary();
    Godot.Collections.Dictionary marks = new Godot.Collections.Dictionary();


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
        (Godot.Collections.Dictionary)((Resource)mainNode.Saver.Get("world_save_file")).Get("WorldCells");
        if(dict.Count != 0) {
            QueueFree();
            return;
        }
        InitTileMap(worldTileMap1, 1);
        InitTileMap(worldTileMap2, 2);
        InitTileMap(worldTileMap3, 3);
        ((Resource)mainNode.Saver.Get("world_save_file")).Set("WorldCells", cells);
        QueueFree();
    }


    void InitTileMap(TileMap worldTileMap, int setNum) {
        foreach(Node2D node in GetChildren()) {
            if(node is WorldMarker) {
                Vector2 key = worldTileMap.WorldToMap(worldTileMap.ToLocal(node.GlobalPosition));
                if(marks.Contains(key) == false) { 
                    marks.Add(key, ((WorldMarker)node).levelType);
                }
            }
        }
        foreach(Vector2 pos in worldTileMap.GetUsedCells()) {
            Godot.Collections.Dictionary posDict = new Godot.Collections.Dictionary();
            WorldMarker.LevelType lvlType = GetLevelType(pos);
            if(lvlType == WorldMarker.LevelType.Start) {
                ((Resource)mainNode.Saver.Get("player_save_file")).Set("CurrentCell", pos);
            }
            posDict.Add("type", GetLevelType(pos));
            posDict.Add("id", worldTileMap.GetCellv(pos));
            posDict.Add("set", setNum);
            posDict.Add("scn", "");
            cells.Add(pos, posDict);
        }
    }


    WorldMarker.LevelType GetLevelType(Vector2 pos) {
        if(marks.Contains(pos)) {
            return (WorldMarker.LevelType)marks[pos];
        }
        return default;
    }


}
