using Godot;
using System;

public class HealthHUD : TextureProgress
{
   private Entity parentNode;
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

   private Tween tween;


   public override void _Ready()
   {
      base._Ready();
      tween = (Tween)GetNode("Tween");
   }


   public void UpdateHealth() {
      if(parentNode.IsDead == true) {
         QueueFree();
      }
      tween.InterpolateProperty(this, "value", Value, parentNode.Health, 0.5f,
      Tween.TransitionType.Linear, Tween.EaseType.InOut);
      tween.Start();
   }


}
