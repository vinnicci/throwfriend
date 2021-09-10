using Godot;
using System;

public class KillSwitch : Trigger
{
    public override bool OnSwitchedOn() {
        if(base.OnSwitchedOn() == false) {
            return false;
        }
        KillAllEnemies();
        return true;
    }


    void KillAllEnemies() {
        foreach(Node2D node in LevelNode.GetNode("Enemies").GetChildren()) {
            if(node is Enemy) {
                ((Enemy)node).OnSwitchedOn();
            }
        }
        foreach(Node2D node in LevelNode.GetNode("Objects").GetChildren()) {
            if(node is RandomEnemySpawner) {
                ((RandomEnemySpawner)node).OnSwitchedOn();
            }
        }
    }


}
