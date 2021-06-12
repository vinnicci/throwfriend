using Godot;

[Tool]
public class TransformConstraintTool : Node2D
{
    Godot.Collections.Array<NodePath> targets;
    ///<summary>Fill array with NodePaths</summary>
    [Export] Godot.Collections.Array<NodePath> Targets {
        get {
            return targets;
        }
        set {
            targets = value;
            SetBoundTargets();
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
        Targets = Targets;
        Enabled = Enabled;
    }


    Godot.Collections.Array bound_targets;


    void SetBoundTargets() {
        if(Targets == null || Targets.Count == 0) {
            return;
        }
        bound_targets = new Godot.Collections.Array();
        foreach(NodePath target in Targets) {
            Node2D node = GetNodeOrNull<Node2D>(target);
            if(node == null) {
                continue;
            }
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
        if(ScaleRateX != 0) {
            t.x *= Scale.x * ScaleRateX;
        }
        if(ScaleRateY != 0) {
            t.y *= Scale.y * ScaleRateY;
        }
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
            Vector2 diff = Transform.origin - tOrigin.origin;
            diff.x *= PositionRateX;
            diff.y *= PositionRateY;
            node.Translate(diff);
        }
        tOrigin = Transform;
    }


}
