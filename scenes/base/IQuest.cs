using System;
using Godot;

public interface IQuest
{
    Main MainNode {get; set;}
    String QuestID {get; set;}
    String QuestKey {get; set;}
    void CheckQuest();
    void UpdateQuest();
}