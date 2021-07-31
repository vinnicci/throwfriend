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
         UpdateHealth();
      }
   }

   Tween tween;


   public override void _Ready()
   {
      base._Ready();
      tween = (Tween)GetNode("Tween");
   }


   public void UpdateHealth() {
      if(parentNode.IsDead) {
         QueueFree();
      }
      tween.InterpolateProperty(this, "value", Value, parentNode.Health, 0.5f,
      Tween.TransitionType.Linear, Tween.EaseType.InOut);
      tween.Start();
   }


}
