using Godot;
using System;

public class Blackboard : Node2D
{
    [Export] Godot.Collections.Array<NodePath> agents = new Godot.Collections.Array<NodePath>();
    public Godot.Collections.Dictionary blackboard = new Godot.Collections.Dictionary();


    public void Init() {
        foreach(NodePath agentPath in agents) {
            Enemy agentNode = (Enemy)GetNodeOrNull(agentPath);
            if(IsInstanceValid(agentNode) == true && agentNode.HasNode("AI") == true) {
                AssignAgent(agentNode);
            }
        }
    }


    public void AssignAgent(Enemy agent) {
        Node2D ai = (Node2D)agent.GetNode("AI");
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)GetNode("AI").Get("blackboards");
        dict.Add(Name, this);
    }


}
