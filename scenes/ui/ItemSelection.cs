using Godot;
using System;
using System.Collections.Generic;

public class ItemSelection : Panel
{
    private Loadout loadoutNode;
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

    private AnimationPlayer anim;
    private Label description;
    private Color RED = new Color(0.6f, 0.3f, 0.3f);


    public override void _Ready()
    {
        base._Ready();
        anim = (AnimationPlayer)GetNode("Anim");
    }


    public void ShowItemSelection(bool visibility) {
        if(anim.IsPlaying() == false) {
            if(visibility == true && Visible == false) {
                anim.Play("show");
            }
            else if(visibility == false && Visible == true) {
                anim.Play("hide");
            }
        }
    }


    public void SetIncompatibleItems(List<String> incompatibilityList) {
        foreach(ItemSelect itemselect in GetNode("GridContainer").GetChildren()) {
            if(incompatibilityList.Contains(itemselect.Name) == true) {
                ColorRect color = (ColorRect)itemselect.GetNode("ColorRect");
                TextureButton button = (TextureButton)itemselect.GetNode("TextureButton");
                color.Modulate = RED;
                button.Disabled = true;
                button.Modulate = new Color(1,1,1,0.5f);
            }
        }
    }


    private void OnWeaponItemSelectionHide() {
        Visible = false;
    }


    private void OnPlayerItemSelectionHide() {
        Visible = false;
    }


}
