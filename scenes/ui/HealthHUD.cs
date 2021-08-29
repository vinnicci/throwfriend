using Godot;
using System;

public class HealthHUD : TextureProgress
{
   Entity parentNode;
   public Entity ParentNode {
      get {
         return parentNode;
      }
      set {
         parentNode = value;
         MaxValue = parentNode.Health;
         Value = MaxValue;
         UpdateHealth(true);
      }
   }

   Tween tween;


   public override void _Ready()
   {
      base._Ready();
      tween = (Tween)GetNode("Tween");
   }


   public void UpdateHealth(bool updatedBaseHP = false) {
      if(parentNode.Health <= 0 && updatedBaseHP == false) {
         Visible = false;
      }
      tween.InterpolateProperty(this, "value", Value, parentNode.Health, 0.5f,
      Tween.TransitionType.Linear, Tween.EaseType.InOut);
      tween.Start();
   }


}
