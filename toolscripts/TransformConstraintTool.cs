using Godot;
using Godot.Collections;

[Tool]
class TransformConstraintTool : Node2D
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
            if(enabled) {
                tOrigin = Transform;
            }
        }
    }
    bool scaleXClamped;
    bool scaleYClamped;
    float scaleXClampMin;
    float scaleYClampMin;
    float scaleXClampMax;
    float scaleYClampMax;
    float scaleRateX;
    float scaleRateY;
    bool positionXClamped;
    bool positionYClamped;
    float positionXClampMin;
    float positionYClampMin;
    float positionXClampMax;
    float positionYClampMax;
    float positionRateX;
    float positionRateY;
    float rotationRate;


    public override void _Ready()
    {
        base._Ready();
        Targets = Targets;
        Enabled = Enabled;
    }


    public override Array _GetPropertyList()
    {
        Godot.Collections.Array properties = new Godot.Collections.Array();
        Godot.Collections.Dictionary dictionary;

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/RateX");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/RateY");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/ClampedX");
        dictionary.Add("type", Variant.Type.Bool);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/ClampXMin");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/ClampXMax");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/ClampedY");
        dictionary.Add("type", Variant.Type.Bool);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/ClampYMin");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Position/ClampYMax");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/RateX");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/RateY");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/ClampedX");
        dictionary.Add("type", Variant.Type.Bool);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/ClampXMin");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/ClampXMax");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/ClampedY");
        dictionary.Add("type", Variant.Type.Bool);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/ClampYMin");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Scale/ClampYMax");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        dictionary = new Godot.Collections.Dictionary();
        dictionary.Add("name", "Rotation/Rate");
        dictionary.Add("type", Variant.Type.Real);
        dictionary.Add("usage", PropertyUsageFlags.Default);
        properties.Add(dictionary);

        return properties;
    }


    public override bool _Set(string property, object value)
    {
        switch(property) {
            case "Position/RateX": positionRateX = (float)value; break;
            case "Position/RateY": positionRateY = (float)value; break;
            case "Position/ClampedX": positionXClamped = (bool)value; break;
            case "Position/ClampedY": positionYClamped = (bool)value; break;
            case "Position/ClampXMin": positionXClampMin = (float)value; break;
            case "Position/ClampXMax": positionXClampMax = (float)value; break;
            case "Position/ClampYMin": positionYClampMin = (float)value; break;
            case "Position/ClampYMax": positionYClampMax = (float)value; break;
            case "Scale/RateX": {
                if((float)value == 0) {
                    GD.Print("Scale rate of 0 not allowed.");
                    value = 1f;
                }
                scaleRateX = (float)value;
            } break;
            case "Scale/RateY": {
                if((float)value == 0) {
                    GD.Print("Scale rate of 0 not allowed.");
                    value = 1f;
                }
                scaleRateY = (float)value;
            } break;
            case "Scale/ClampedX": scaleXClamped = (bool)value; break;
            case "Scale/ClampedY": scaleYClamped = (bool)value; break;
            case "Scale/ClampXMin": {
                if((float)value == 0) {
                    GD.Print("Scale clamp of 0 not allowed.");
                    value = -1f;
                }
                scaleXClampMin = (float)value;
            } break;
            case "Scale/ClampXMax": {
                if((float)value == 0) {
                    GD.Print("Scale clamp of 0 not allowed.");
                    value = 1f;
                }
                scaleXClampMax = (float)value;
            } break;
            case "Scale/ClampYMin": {
                if((float)value == 0) {
                    GD.Print("Scale clamp of 0 not allowed.");
                    value = -1f;
                }
                scaleYClampMin = (float)value;
            } break;
            case "Scale/ClampYMax": {
                if((float)value == 0) {
                    GD.Print("Scale clamp of 0 not allowed.");
                    value = 1f;
                }
                scaleYClampMax = (float)value;
            } break;
            case "Rotation/Rate": {
                rotationRate = (float)value;
            }
            return true;
        }
        return false;
    }


    public override object _Get(string property)
    {
        switch(property) {
            case "Position/RateX": return positionRateX;
            case "Position/RateY": return positionRateY;
            case "Position/ClampedX": return positionXClamped;
            case "Position/ClampedY": return positionYClamped;
            case "Position/ClampXMin": return positionXClampMin;
            case "Position/ClampXMax": return positionXClampMax;
            case "Position/ClampYMin": return positionYClampMin;
            case "Position/ClampYMax": return positionYClampMax;
            case "Scale/RateX": return scaleRateX;
            case "Scale/RateY": return scaleRateY;
            case "Scale/ClampedX": return scaleXClamped;
            case "Scale/ClampedY": return scaleYClamped;
            case "Scale/ClampXMin": return scaleXClampMin;
            case "Scale/ClampXMax": return scaleXClampMax;
            case "Scale/ClampYMin": return scaleYClampMin;
            case "Scale/ClampYMax": return scaleYClampMax;
            case "Rotation/Rate": return rotationRate;
            default: return default;
        }
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
        if(Transform == tOrigin || bound_targets == null || bound_targets.Count == 0) {
            return;
        }
        if(scaleXClamped) {
            float x = Mathf.Clamp(Scale.x, scaleXClampMin, scaleXClampMax);
            Vector2 v = new Vector2(x,Scale.y);
            Scale = v;
        }
        if(scaleYClamped) {
            float y = Mathf.Clamp(Scale.y, scaleYClampMin, scaleYClampMax);
            Vector2 v = new Vector2(Scale.x,y);
            Scale = v;
        }
        if(positionXClamped) {
            float x = Mathf.Clamp(Position.x, positionXClampMin, positionXClampMax);
            Vector2 v = new Vector2(x,Position.y);
            Position = v;
        }
        if(positionYClamped) {
            float y = Mathf.Clamp(Position.y, positionYClampMin, positionYClampMax);
            Vector2 v = new Vector2(Position.x,y);
            Position = v;
        }
        Transform2D t = Transform2D.Identity;
        //scale
        t.x *= Scale.x * scaleRateX;
        t.y *= Scale.y * scaleRateY;
        //rotation
        t = t.Rotated(Rotation * rotationRate);
        foreach(Node2D node in bound_targets) {
            if(IsInstanceValid(node) == false) {
                continue;
            }
            t.origin = node.Transform.origin;
            //apply scale and rotation
            node.Transform = t;

            //position
            Vector2 diff = Transform.origin - tOrigin.origin;
            diff.x *= positionRateX;
            diff.y *= positionRateY;
            node.Translate(diff);
        }
        tOrigin = Transform;
    }


}
