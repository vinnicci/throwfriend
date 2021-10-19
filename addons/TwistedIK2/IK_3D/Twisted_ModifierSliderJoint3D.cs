using Godot;
using System;


[Tool]
public class Twisted_ModifierSliderJoint3D : Twisted_Modifier3D
{
    /// <summary>
    /// A NodePath to the Spatial-based node that is used as the target position
    /// </summary>
    public NodePath path_to_target = null;
    /// <summary>
    /// A reference to the Spatial-based node that is used as the target position
    /// </summary>
    public Spatial target_node = null;

    /// <summary>
    /// A NodePath to the first point that makes the 3D line
    /// </summary>
    public NodePath path_to_line_point_one = null;
    /// <summary>
    /// The Spatial node that makes the first point on the 3D line
    /// </summary>
    public Spatial line_point_one = null;

    /// <summary>
    /// A NodePath to the second point that makes the 3D line
    /// </summary>
    public NodePath path_to_line_point_two = null;
    /// <summary>
    /// The Spatial node that makes the second point on the 3D line
    /// </summary>
    public Spatial line_point_two = null;

    /// <summary>
    /// A NodePath to the Twisted_Bone3D that will be constrained to the line
    /// </summary>
    public NodePath path_to_twisted_bone = null;
    /// <summary>
    /// The Twisted_Bone3D that will be constrained to the line
    /// </summary>
    public Twisted_Bone3D twisted_bone = null;

    /// <summary>
    /// If true, the bone will rotate to look at the target
    /// </summary>
    public bool rotate_to_target = false;
    /// <summary>
    /// Additional rotation to be applied
    /// </summary>
    public Vector3 additional_rotation = Vector3.Zero;


    public override void _Ready()
    {
        if (path_to_target != null) {
            target_node = GetNodeOrNull<Spatial>(path_to_target);
        }
        if (path_to_line_point_one != null) {
            line_point_one = GetNodeOrNull<Spatial>(path_to_line_point_one);
        }
        if (path_to_line_point_two != null) {
            line_point_two = GetNodeOrNull<Spatial>(path_to_line_point_two);
        }
        if (path_to_twisted_bone != null) {
            twisted_bone = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone);
        }
    }

    public override bool _Set(string property, object value)
    {
        if (property == "SliderJoint/target") {
            path_to_target = (NodePath)value;
            if (path_to_target != null) {
                target_node = GetNodeOrNull<Spatial>(path_to_target);
            }
            return true;
        }
        else if (property == "SliderJoint/point_one") {
            path_to_line_point_one = (NodePath)value;
            if (path_to_target != null) {
                line_point_one = GetNodeOrNull<Spatial>(path_to_line_point_one);
            }
            return true;
        }
        else if (property == "SliderJoint/point_two") {
            path_to_line_point_two = (NodePath)value;
            if (path_to_target != null) {
                line_point_two = GetNodeOrNull<Spatial>(path_to_line_point_two);
            }
            return true;
        }
        else if (property == "SliderJoint/twisted_bone") {
            path_to_twisted_bone = (NodePath)value;
            if (path_to_target != null) {
                twisted_bone = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone);
            }
            return true;
        }
        else if (property == "SliderJoint/rotate_to_target") {
            rotate_to_target = (bool)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "SliderJoint/additional_rotation") {
            Vector3 value_convert = (Vector3)value;
            value_convert.x = Mathf.Deg2Rad(value_convert.x);
            value_convert.y = Mathf.Deg2Rad(value_convert.y);
            value_convert.z = Mathf.Deg2Rad(value_convert.z);
            additional_rotation = value_convert;
            return true;
        }

        
        try {
            return base._Set(property, value);
        } catch {
            return false;
        }
    }

    public override object _Get(string property)
    {
        if (property == "SliderJoint/target") {
            return path_to_target;
        }
        else if (property == "SliderJoint/point_one") {
            return path_to_line_point_one;
        }
        else if (property == "SliderJoint/point_two") {
            return path_to_line_point_two;
        }
        else if (property == "SliderJoint/twisted_bone") {
            return path_to_twisted_bone;
        }
        else if (property == "SliderJoint/rotate_to_target") {
            return rotate_to_target;
        }
        else if (property == "SliderJoint/additional_rotation") {
            Vector3 value_convert = additional_rotation;
            value_convert.x = Mathf.Rad2Deg(value_convert.x);
            value_convert.y = Mathf.Rad2Deg(value_convert.y);
            value_convert.z = Mathf.Rad2Deg(value_convert.z);
            return value_convert;
        }

        try {
            return base._Get(property);
        } catch {
            return false;
        }
    }

    public override Godot.Collections.Array _GetPropertyList()
    {
        Godot.Collections.Array list = base._GetPropertyList();
        Godot.Collections.Dictionary tmp_dict;

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "SliderJoint/target");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "SliderJoint/point_one");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "SliderJoint/point_two");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "SliderJoint/twisted_bone");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "SliderJoint/rotate_to_target");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (rotate_to_target == true) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "SliderJoint/additional_rotation");
            tmp_dict.Add("type", Variant.Type.Vector3);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);
        }

        return list;
    }

    public override void _ExecuteModification(Twisted_ModifierStack3D modifier_stack, float delta)
    {
        base._ExecuteModification(modifier_stack, delta);

        if (target_node == null) {
            if (path_to_target != null) {
                target_node = GetNodeOrNull<Spatial>(path_to_target);
            }
            if (target_node == null) {
                GD.PrintErr("Cannot execute 3D SliderJoint: No target found!");
            }
        }

        if (line_point_one == null) {
            if (path_to_line_point_one != null) {
                line_point_one = GetNodeOrNull<Spatial>(path_to_line_point_one);
            }
            if (line_point_one == null) {
                GD.PrintErr("Cannot execute 3D SliderJoint: No point 1 node found!");
            }
        }

        if (line_point_two == null) {
            if (path_to_line_point_two != null) {
                line_point_two = GetNodeOrNull<Spatial>(path_to_line_point_two);
            }
            if (line_point_two == null) {
                GD.PrintErr("Cannot execute 3D SliderJoint: No point 2 node found!");
            }
        }

        if (twisted_bone == null) {
            if (path_to_twisted_bone != null) {
                twisted_bone = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone);
            }
            if (twisted_bone == null) {
                GD.PrintErr("Cannot execute 3D SliderJoint: No twisted bone 3D node found!");
            }
        }

        _ExecuteSliderJoint();
    }

    private void _ExecuteSliderJoint() {
        Transform transform_to_apply = twisted_bone.GlobalTransform;

        // Get the point on a line
        // Credit (https://stackoverflow.com/questions/3120357/get-closest-point-to-a-line)
        Vector3 one_to_target = target_node.GlobalTransform.origin - line_point_one.GlobalTransform.origin;
        Vector3 one_to_two = line_point_two.GlobalTransform.origin - line_point_one.GlobalTransform.origin;
        float point_distance = one_to_target.Dot(one_to_two) / one_to_two.LengthSquared();
        if (point_distance < 0) {
            transform_to_apply.origin = line_point_one.GlobalTransform.origin;
        }
        else if (point_distance > 1) {
            transform_to_apply.origin = line_point_two.GlobalTransform.origin;
        }
        else {
            transform_to_apply.origin = line_point_one.GlobalTransform.origin + (one_to_two * point_distance);
        }

        twisted_bone.GlobalTransform = transform_to_apply;

        if (rotate_to_target == true) {
            Vector3 bone_up_dir = twisted_bone.get_reset_bone_global_pose().basis.y.Normalized();
            twisted_bone.LookAt(target_node.GlobalTransform.origin, bone_up_dir);

            twisted_bone.Rotate(twisted_bone.Transform.basis.x.Normalized(), additional_rotation.x);
            twisted_bone.Rotate(twisted_bone.Transform.basis.y.Normalized(), additional_rotation.y);
            twisted_bone.Rotate(twisted_bone.Transform.basis.z.Normalized(), additional_rotation.z);

             // Keep the scale consistent with the global pose
            twisted_bone.Scale = twisted_bone.get_reset_bone_global_pose(false).basis.Scale;
        }

        if (force_bone_application) {
            twisted_bone.force_apply_transform();
        }
    }
}