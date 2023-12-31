using Godot;
using System;

/// <summary>
/// Rotates two bones to reach the target. This is an alternate version/implementation of TwoBoneIK that doesn't
/// rely as heavily on the law of Cosigns for placement. This solver should be a little more performant than
/// the TwoBoneIK solver.
/// This IK solver is currently WAY more stable in 3D than TwoBoneIK and therefore is the recommended replacement.
/// </summary>
[Tool]
public class Twisted_ModifierDuoIK3D : Twisted_Modifier3D
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
    /// A NodePath to the first Twisted_Bone3D node in this modifier
    /// </summary>
    public NodePath path_to_twisted_bone_one;
    /// <summary>
    /// A reference to the first Twisted_Bone3D node in this modifier
    /// </summary>
    public Twisted_Bone3D twisted_bone_one;

    /// <summary>
    /// A NodePath to the second Twisted_Bone3D node in this modifier
    /// </summary>
    public NodePath path_to_twisted_bone_two;
    /// <summary>
    /// A reference to the second Twisted_Bone3D node in this modifier
    /// </summary>
    public Twisted_Bone3D twisted_bone_two;

    /// <summary>
    /// A NodePath to the pole node used for bending the bones in this modifier
    /// </summary>
    public NodePath path_to_pole_node;
    /// <summary>
    /// A reference to the pole node used for bending the bones in this modifier
    /// </summary>
    public Spatial pole_node;

    /// <summary>
    /// The amount of additional rotation to be applied to the first bone after solving the IK.
    /// 
    /// Note: Additional twist rotation (z axis) can be applied but requires
    /// a bit more computing.
    /// </summary>
    public Vector3 bone_one_additional_rotation = Vector3.Zero;

    /// <summary>
    /// The amount of additional rotation to be applied to the second bone after solving the IK.
    /// 
    /// Note: Additional twist rotation (z axis) can be applied but requires
    /// a bit more computing.
    /// </summary>
    public Vector3 bone_two_additional_rotation = Vector3.Zero;

    // Needed for properly resetting any twist rotation applied, if twist rotation is applied.
    private bool have_set_additional_twist = false;


    public override void _Ready()
    {
        if (path_to_target != null) {
            target_node = GetNodeOrNull<Spatial>(path_to_target);
        }
        if (twisted_bone_one != null) {
            twisted_bone_one = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_one);
        }
        if (twisted_bone_two != null) {
            twisted_bone_two = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_two);
        }
        if (path_to_pole_node != null) {
            pole_node = GetNodeOrNull<Spatial>(path_to_pole_node);
        }
    }

    public override bool _Set(string property, object value)
    {
        if (property == "duo_ik/target") {
            path_to_target = (NodePath)value;
            if (path_to_target != null) {
                target_node = GetNodeOrNull<Spatial>(path_to_target);
            }
            return true;
        }
        else if (property == "duo_ik/twisted_bone_one") {
            path_to_twisted_bone_one = (NodePath)value;
            if (path_to_twisted_bone_one != null) {
                twisted_bone_one = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_one);
            }
            return true;
        }
        else if (property == "duo_ik/twisted_bone_two") {
            path_to_twisted_bone_two = (NodePath)value;
            if (path_to_twisted_bone_two != null) {
                twisted_bone_two = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_two);
            }
            return true;
        }
        else if (property == "duo_ik/pole_node") {
            path_to_pole_node = (NodePath)value;
            if (path_to_pole_node != null) {
                pole_node = GetNodeOrNull<Spatial>(path_to_pole_node);
            }
            return true;
        }
        else if (property == "duo_ik/bone_one_additional_rotation") {
            Vector3 tmp = (Vector3)value;
            tmp.x = Mathf.Deg2Rad(tmp.x);
            tmp.y = Mathf.Deg2Rad(tmp.y);
            tmp.z = Mathf.Deg2Rad(tmp.z);
            bone_one_additional_rotation = tmp;
            return true;
        }
        else if (property == "duo_ik/bone_two_additional_rotation") {
            Vector3 tmp = (Vector3)value;
            tmp.x = Mathf.Deg2Rad(tmp.x);
            tmp.y = Mathf.Deg2Rad(tmp.y);
            tmp.z = Mathf.Deg2Rad(tmp.z);
            bone_two_additional_rotation = tmp;
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
        if (property == "duo_ik/target") {
            return path_to_target;
        }
        else if (property == "duo_ik/twisted_bone_one") {
            return path_to_twisted_bone_one;
        }
        else if (property == "duo_ik/twisted_bone_two") {
            return path_to_twisted_bone_two;
        }
        else if (property == "duo_ik/pole_node") {
            return path_to_pole_node;
        }
        else if (property == "duo_ik/bone_one_additional_rotation") {
            Vector3 tmp = bone_one_additional_rotation;
            tmp.x = Mathf.Rad2Deg(tmp.x);
            tmp.y = Mathf.Rad2Deg(tmp.y);
            tmp.z = Mathf.Rad2Deg(tmp.z);
            return tmp;
        }
        else if (property == "duo_ik/bone_two_additional_rotation") {
            Vector3 tmp = bone_two_additional_rotation;
            tmp.x = Mathf.Rad2Deg(tmp.x);
            tmp.y = Mathf.Rad2Deg(tmp.y);
            tmp.z = Mathf.Rad2Deg(tmp.z);
            return tmp;
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
        tmp_dict.Add("name", "duo_ik/target");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "duo_ik/twisted_bone_one");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "duo_ik/twisted_bone_two");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "duo_ik/pole_node");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "duo_ik/bone_one_additional_rotation");
        tmp_dict.Add("type", Variant.Type.Vector3);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "duo_ik/bone_two_additional_rotation");
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
            target_node = GetNodeOrNull<Spatial>(path_to_target);
            if (target_node == null) {
                GD.PrintErr("TwoBoneIK 3D: Cannot find target!");
                return;
            }
        }
        if (twisted_bone_one == null) {
            twisted_bone_one = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_one);
            if (twisted_bone_one == null) {
                GD.PrintErr("TwoBoneIK 3D: Cannot find twisted_bone one!");
                return;
            }
        }
        if (twisted_bone_two == null) {
            twisted_bone_two = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_two);
            if (twisted_bone_two == null) {
                GD.PrintErr("TwoBoneIK 3D: Cannot find twisted_bone two!");
                return;
            }
        }
        if (pole_node == null) {
            pole_node = GetNodeOrNull<Spatial>(path_to_pole_node);
            if (twisted_bone_two == null) {
                GD.PrintErr("TwoBoneIK 3D: Cannot find pole node!");
                return;
            }
        }

        // The IK algorithm
        // =================
        // NOTE - currently does not support non-uniform scaling very well

        // Needed to avoid twisting bones (CREDIT: https://forum.unity.com/threads/lookrotation-lookat-problem.122378/)
        Vector3 bone_one_original_up = twisted_bone_one.GlobalTransform.basis.y.Normalized();
        Vector3 bone_two_original_up = twisted_bone_two.GlobalTransform.basis.y.Normalized();

        // If we want to apply additional twist, then we need to get it based on the global pose
        if (bone_one_additional_rotation.z != 0 || bone_two_additional_rotation.z != 0) {
            bone_one_original_up = twisted_bone_one.get_reset_bone_global_pose().basis.y.Normalized();
            bone_two_original_up = twisted_bone_two.get_reset_bone_global_pose().basis.y.Normalized();
            twisted_bone_one.apply_transform_to_bone();
            twisted_bone_two.apply_transform_to_bone();
            have_set_additional_twist = true;
        }
        else if (have_set_additional_twist == true) {
            bone_one_original_up = twisted_bone_one.get_reset_bone_global_pose().basis.y.Normalized();
            bone_two_original_up = twisted_bone_two.get_reset_bone_global_pose().basis.y.Normalized();
            twisted_bone_one.apply_transform_to_bone();
            twisted_bone_two.apply_transform_to_bone();
            have_set_additional_twist = false;
        }

        // Make the bones straight
        twisted_bone_one.LookAt(target_node.GlobalTransform.origin, bone_one_original_up);
        twisted_bone_two.LookAt(target_node.GlobalTransform.origin, bone_two_original_up);
        
        Vector3 target_difference = target_node.GlobalTransform.origin - twisted_bone_one.GlobalTransform.origin;
        float joint_one_to_target = target_difference.Length();
        float bone_one_length = twisted_bone_one.bone_length;
        float bone_two_length = twisted_bone_two.bone_length;
        float bone_length_min = Mathf.Min(bone_one_length, bone_two_length);

        bool override_angles_due_to_out_of_range = false;

        if (bone_one_length + bone_two_length < joint_one_to_target) {
            override_angles_due_to_out_of_range = true;
        }
        else if (joint_one_to_target < bone_length_min) {
            joint_one_to_target = bone_length_min;
        }

        if (override_angles_due_to_out_of_range == false) {
            // Use Law of Cosigns to figure out the first angle (only one we need!)
            float angle_0 = Mathf.Acos(((joint_one_to_target * joint_one_to_target) + (bone_one_length * bone_one_length) - (bone_two_length * bone_two_length)) / (2.0f * joint_one_to_target * bone_one_length));
            if (Mathf.IsNaN(angle_0) == true) {
                return; // Cannot solve this angle!
            }

            Vector3 middle_point = twisted_bone_one.GlobalTransform.origin;

            Vector3 move_direction = twisted_bone_one.ToLocal(pole_node.GlobalTransform.origin).Normalized();
            move_direction.z = 0;
            if (move_direction.LengthSquared() == 0) {
                move_direction = Vector3.Right;
            }
            move_direction = twisted_bone_one.ToGlobal(move_direction);
            move_direction = twisted_bone_one.GlobalTransform.origin.DirectionTo(move_direction).Normalized();

            // Get the axis of rotation:
            Vector3 rotation_direction = -twisted_bone_one.GlobalTransform.basis.z.Normalized().Cross(move_direction).Normalized();
            middle_point += (-twisted_bone_one.GlobalTransform.basis.z.Normalized().Rotated(rotation_direction, angle_0)) * bone_one_length;

            twisted_bone_one.LookAt(middle_point, bone_one_original_up);
            // Additional rotation
            twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.x.Normalized(), bone_one_additional_rotation.x);
            twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.y.Normalized(), bone_one_additional_rotation.y);
            twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.z.Normalized(), bone_one_additional_rotation.z);

            twisted_bone_two.LookAt(target_node.GlobalTransform.origin, bone_two_original_up);
            // Additional rotation
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.x.Normalized(), bone_two_additional_rotation.x);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.y.Normalized(), bone_two_additional_rotation.y);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.z.Normalized(), bone_two_additional_rotation.z);

        } else {
            twisted_bone_one.LookAt(target_node.GlobalTransform.origin, bone_one_original_up);
            // Additional rotation
            twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.x.Normalized(), bone_one_additional_rotation.x);
            twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.y.Normalized(), bone_one_additional_rotation.y);
            twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.z.Normalized(), bone_one_additional_rotation.z);

            twisted_bone_two.LookAt(target_node.GlobalTransform.origin, bone_two_original_up);
            // Additional rotation
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.x.Normalized(), bone_two_additional_rotation.x);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.y.Normalized(), bone_two_additional_rotation.y);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.z.Normalized(), bone_two_additional_rotation.z);
        }

        // Keep the scale consistent with the global pose
        twisted_bone_one.Scale = twisted_bone_one.get_reset_bone_global_pose(false).basis.Scale;
        twisted_bone_two.Scale = twisted_bone_two.get_reset_bone_global_pose(false).basis.Scale;

        if (force_bone_application) {
            twisted_bone_one.force_apply_transform();
            twisted_bone_two.force_apply_transform();
        }
        // ==============
    }
}