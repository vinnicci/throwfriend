using System;
using Godot;

public interface IQuest
{
    Main MainNode {get; set;}
    String QuestID {get; set;}
    void CheckQuest();
    void UpdateQuest();
}