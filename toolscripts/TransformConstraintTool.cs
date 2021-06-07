using Godot;

[Tool]
public class TransformConstraintTool : Node2D
{
    Godot.Collections.Array targets;
    ///<summary>Fill array with NodePaths</summary>
    [Export] Godot.Collections.Array Targets {
        get {
            return targets;
        }
        set {
            targets = value;
            if(targets == null) {
                enabled = false;
            }
            if(enabled == true) {
                SetBoundTargets();
            }
        }
    }
    bool enabled;
    [Export] bool Enabled {
        get {
            return enabled;
        }
        set {
            enabled = value;
            SetProcess(enabled);
            if(enabled == true) {
                SetBoundTargets();
                tOrigin = Transform;
            }
        }
    }
    [Export] float ScaleRateX = 1f;
    [Export] float ScaleRateY = 1f;
    [Export] float RotationRate = 1f;
    [Export] float PositionRateX = 1f;
    [Export] float PositionRateY = 1f;


    public override void _Ready()
    {
        base._Ready();
        Enabled = Enabled;
    }


    Godot.Collections.Array bound_targets;


    void SetBoundTargets() {
        if(targets.Count == 0 || targets == null) {
            return;
        }
        bound_targets = new Godot.Collections.Array();
        foreach(NodePath target in targets) {
            if(target == null) {
                continue;
            }
            Node2D node = GetNodeOrNull<Node2D>(target);
            bound_targets.Add(node);
        }
    }


    Transform2D tOrigin;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(Transform == tOrigin) {
            return;
        }
        Transform2D t = Transform2D.Identity;
        //scale
        t.x *= Scale.x * ScaleRateX;
        t.y *= Scale.y * ScaleRateY;
        //rotation
        t = t.Rotated(Rotation * RotationRate);
        foreach(Node2D node in bound_targets) {
            if(IsInstanceValid(node) == false) {
                continue;
            }
            t.origin = node.Transform.origin;
            //apply scale and rotation
            node.Transform = t;

            //position
            Vector2 diff = GlobalPosition - tOrigin.origin;
            diff.x *= PositionRateX;
            diff.y *= PositionRateY;
            node.Translate(diff);
        }
        tOrigin = Transform;
    }


}
