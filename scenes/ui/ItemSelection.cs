using Godot;
using System;
using System.Collections.Generic;

public class ItemSelection : Panel
{
    Loadout loadoutNode;
    public Loadout LoadoutNode {
        get {
            return loadoutNode;
        }
        set{
            loadoutNode = value;
            description = (Label)GetNode("Description");
            foreach(ItemSelect itemSelect in GetNode("GridContainer").GetChildren()) {
                itemSelect.DescriptionNode = description;
                itemSelect.LoadoutNode = LoadoutNode;
            }
        }
    }

    AnimationPlayer anim;
    Label description;
    Color RED = new Color(0.6f, 0.3f, 0.3f);


    public override void _Ready()
    {
        base._Ready();
        anim = (AnimationPlayer)GetNode("Anim");
    }


    public void ShowItemSelection(bool visibility) {
        if(anim.IsPlaying() == false) {
            if(visibility && Visible == false) {
                anim.Play("show");
            }
            else if(visibility == false && Visible) {
                anim.Play("hide");
            }
        }
    }


    public void SetIncompatibleItems(List<String> incompatibilityList) {
        foreach(ItemSelect itemselect in GetNode("GridContainer").GetChildren()) {
            if(incompatibilityList.Contains(itemselect.Name)) {
                ColorRect color = (ColorRect)itemselect.GetNode("ColorRect");
                TextureButton button = (TextureButton)itemselect.GetNode("TextureButton");
                color.Modulate = RED;
                button.Disabled = true;
                button.Modulate = new Color(1,1,1,0.5f);
            }
        }
    }


    void OnWeaponItemSelectionHide() {
        Visible = false;
    }


    void OnPlayerItemSelectionHide() {
        Visible = false;
    }


}
