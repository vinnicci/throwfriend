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
                itemSelect.ItemSelectionNode = this;
            }
        }
    }

    public AnimationPlayer Anim {get; private set;}
    Label description;
    Color RED = new Color(0.6f, 0.3f, 0.3f);


    public override void _Ready()
    {
        base._Ready();
        Anim = (AnimationPlayer)GetNode("Anim");
        incompatibleItems = new List<String>();
    }


    public void ShowItemSelection(bool visibility) {
        if(Anim.IsPlaying() == false) {
            if(visibility && Visible == false) {
                Anim.Play("show");
            }
            else if(visibility == false && Visible) {
                Anim.Play("hide");
            }
        }
        SetIncompatibleItems();
    }


    List<String> incompatibleItems = new List<String>();


    public void AddIncompatibleItem(List<String> incompatibilityList) {
        foreach(String itemName in incompatibilityList) {
            if(incompatibleItems.Contains(itemName) == false) {
                incompatibleItems.Add(itemName);
            }
        }
    }


    public void SetIncompatibleItems() {
        foreach(ItemSelect itemselect in GetNode("GridContainer").GetChildren()) {
            if(incompatibleItems.Contains(itemselect.Name)) {
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
