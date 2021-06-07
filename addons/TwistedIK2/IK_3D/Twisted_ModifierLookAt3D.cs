using Godot;
using System;

/// <summary>
/// A modifier that rotates a single Twisted_Bone3D node to look at a target node using the <c>look_at</c> function.
/// The simplest of all of the Twisted_Modifier3D classes and a good reference to use when making your own.
/// </summary>
[Tool]
public class Twisted_ModifierLookAt3D : Twisted_Modifier3D
{
    /// <summary>
    /// A NodePath to the Spatial-based node that is used as the target position
    /// </summary>
    public NodePath path_to_target;
    /// <summary>
    /// A reference to the Spatial-based node that is used as the target position
    /// </summary>
    public Spatial target_node;

    /// <summary>
    /// A NodePath to the Twisted_Bone3D node that this modifier operates on
    /// </summary>
    public NodePath path_to_twisted_bone;
    /// <summary>
    /// A reference to the Twisted_Bone3D node that this modifier operates on
    /// </summary>
    public Twisted_Bone3D twisted_bone;

    /// <summary>
    /// A Vector3 that stores a rotation offset (in radiens) that is applied after making the bone look at the target
    /// </summary>
    public Vector3 tweak_additional_rotation = Vector3.Zero;

    public override void _Ready()
    {
        if (path_to_target != null) {
            target_node = GetNodeOrNull<Spatial>(path_to_target);
        }
        if (path_to_twisted_bone != null) {
            twisted_bone = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone);
        }
    }

    public override bool _Set(string property, object value)
    {
        if (property == "look_at/target") {
            path_to_target = (NodePath)value;
            if (path_to_target != null) {
                target_node = GetNodeOrNull<Spatial>(path_to_target);
            }
            return true;
        }
        else if (property == "look_at/twisted_bone") {
            path_to_twisted_bone = (NodePath)value;
            if (path_to_twisted_bone != null) {
                twisted_bone = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone);
            }
            return true;
        }
        else if (property == "look_at/additional_rotation") {
            Vector3 degree_rot = (Vector3)value;
            tweak_additional_rotation.x = Mathf.Deg2Rad(degree_rot.x);
            tweak_additional_rotation.y = Mathf.Deg2Rad(degree_rot.y);
            tweak_additional_rotation.z = Mathf.Deg2Rad(degree_rot.z);
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
        if (property == "look_at/target") {
            return path_to_target;
        }
        else if (property == "look_at/twisted_bone") {
            return path_to_twisted_bone;
        }
        else if (property == "look_at/additional_rotation") {
            Vector3 degree_rot = Vector3.One;
            degree_rot.x = Mathf.Rad2Deg(tweak_additional_rotation.x);
            degree_rot.y = Mathf.Rad2Deg(tweak_additional_rotation.y);
            degree_rot.z = Mathf.Rad2Deg(tweak_additional_rotation.z);
            return degree_rot;
        }
        
        try {
            return base._Get(property);
        } catch {
            return null;
        }
    }

    public override Godot.Collections.Array _GetPropertyList()
    {
        Godot.Collections.Array list = base._GetPropertyList();
        Godot.Collections.Dictionary tmp_dict;

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "look_at/target");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "look_at/twisted_bone");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "look_at/additional_rotation");
        tmp_dict.Add("type", Variant.Type.Vector3);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

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
                GD.PrintErr("Cannot execute LookAt: No target found!");
                return;
            }
        }
        if (twisted_bone == null) {
            if (path_to_twisted_bone != null) {
                twisted_bone = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone);
            }
            if (twisted_bone == null) {
                GD.PrintErr("Cannot execute LookAt: No Twisted_Bone2D found!");
                return;
            }
        }

        twisted_bone.LookAt(target_node.GlobalTransform.origin, Vector3.Up);

        twisted_bone.Rotate(twisted_bone.Transform.basis.x.Normalized(), tweak_additional_rotation.x);
        twisted_bone.Rotate(twisted_bone.Transform.basis.y.Normalized(), tweak_additional_rotation.y);
        twisted_bone.Rotate(twisted_bone.Transform.basis.z.Normalized(), tweak_additional_rotation.z);

        if (force_bone_application) {
            twisted_bone.force_apply_transform();
        }
    }
}
