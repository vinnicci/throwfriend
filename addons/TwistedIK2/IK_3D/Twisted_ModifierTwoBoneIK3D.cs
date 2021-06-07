using Godot;
using System;

/// <summary>
/// A modifier that rotates two Twisted_Bone3D nodes using the Law of Cosigns.
/// 
/// IMPORTANT: This IK solver is kinda buggy right now! Please use Duo IK 3D
/// instead as it has the same functionality and is more stable.
/// </summary>
[Tool]
public class Twisted_ModifierTwoBoneIK3D : Twisted_Modifier3D
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
    /// A NodePath to the first Twisted_Bone3D node that this modifier operates on
    /// </summary>
    public NodePath path_to_twisted_bone_one;
    /// <summary>
    /// A reference to the first Twisted_Bone3D node that this modifier operates on
    /// </summary>
    public Twisted_Bone3D twisted_bone_one;

    /// <summary>
    /// A NodePath to the second Twisted_Bone3D node that this modifier operates on
    /// </summary>
    public NodePath path_to_twisted_bone_two;
    /// <summary>
    /// A reference to the second Twisted_Bone3D node that this modifier operates on
    /// </summary>
    public Twisted_Bone3D twisted_bone_two;

    /// <summary>
    /// If <c>true</c>, the algorithm will rotate the bones so they are aligned on a plane with the Spatial-based
    /// node set as the pole. This offers a great level of control and stability to the algorithm.
    /// </summary>
    public bool use_pole_node = false;
    /// <summary>
    /// A NodePath to the node that is used as the pole for the TwoBoneIK algorithm.
    /// </summary>
    public NodePath path_to_pole_node;
    /// <summary>
    /// A reference to the node that is used as the pole for the TwoBoneIK algorithm.
    /// The pole node is used to tell the bones which direction to bend towards when contracting! For example, if set to a leg, you would 
    /// place the pole node in front of the knee position so the bones bend like a knee would (instead of bending backwards)
    /// </summary>
    public Spatial pole_node;

    public Vector3 bone_one_additional_rotation = Vector3.Zero;
    public Vector3 bone_two_additional_rotation = Vector3.Zero;

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

        if (use_pole_node) {
            pole_node = GetNodeOrNull<Spatial>(path_to_pole_node);
        }
    }

    public override bool _Set(string property, object value)
    {
        if (property == "two_bone/target") {
            path_to_target = (NodePath)value;
            if (path_to_target != null) {
                target_node = GetNodeOrNull<Spatial>(path_to_target);
            }
            return true;
        }
        else if (property == "two_bone/twisted_bone_one") {
            path_to_twisted_bone_one = (NodePath)value;
            if (path_to_twisted_bone_one != null) {
                twisted_bone_one = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_one);
            }
            return true;
        }
        else if (property == "two_bone/twisted_bone_two") {
            path_to_twisted_bone_two = (NodePath)value;
            if (path_to_twisted_bone_two != null) {
                twisted_bone_two = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_two);
            }
            return true;
        }
        else if (property == "two_bone/bone_one_additional_rotation") {
            Vector3 tmp = (Vector3)value;
            tmp.x = Mathf.Deg2Rad(tmp.x);
            tmp.y = Mathf.Deg2Rad(tmp.y);
            tmp.z = Mathf.Deg2Rad(tmp.z);
            bone_one_additional_rotation = tmp;
            return true;
        }
        else if (property == "two_bone/bone_two_additional_rotation") {
            Vector3 tmp = (Vector3)value;
            tmp.x = Mathf.Deg2Rad(tmp.x);
            tmp.y = Mathf.Deg2Rad(tmp.y);
            tmp.z = Mathf.Deg2Rad(tmp.z);
            bone_two_additional_rotation = tmp;
            return true;
        }
        else if (property == "two_bone/use_pole_node") {
            use_pole_node = (bool)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "two_bone/pole_node") {
            path_to_pole_node = (NodePath)value;
            if (path_to_pole_node != null) {
                pole_node = GetNodeOrNull<Spatial>(path_to_pole_node);
            }
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
        if (property == "two_bone/target") {
            return path_to_target;
        }
        else if (property == "two_bone/twisted_bone_one") {
            return path_to_twisted_bone_one;
        }
        else if (property == "two_bone/twisted_bone_two") {
            return path_to_twisted_bone_two;
        }
        else if (property == "two_bone/bone_one_additional_rotation") {
            Vector3 tmp = bone_one_additional_rotation;
            tmp.x = Mathf.Rad2Deg(tmp.x);
            tmp.y = Mathf.Rad2Deg(tmp.y);
            tmp.z = Mathf.Rad2Deg(tmp.z);
            return tmp;
        }
        else if (property == "two_bone/bone_two_additional_rotation") {
            Vector3 tmp = bone_two_additional_rotation;
            tmp.x = Mathf.Rad2Deg(tmp.x);
            tmp.y = Mathf.Rad2Deg(tmp.y);
            tmp.z = Mathf.Rad2Deg(tmp.z);
            return tmp;
        }
        else if (property == "two_bone/use_pole_node") {
            return use_pole_node;
        }
        else if (property == "two_bone/pole_node") {
            return path_to_pole_node;
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
        tmp_dict.Add("name", "two_bone/target");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "two_bone/twisted_bone_one");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "two_bone/twisted_bone_two");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "two_bone/bone_one_additional_rotation");
        tmp_dict.Add("type", Variant.Type.Vector3);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "two_bone/bone_two_additional_rotation");
        tmp_dict.Add("type", Variant.Type.Vector3);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "two_bone/use_pole_node");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (use_pole_node) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "two_bone/pole_node");
            tmp_dict.Add("type", Variant.Type.NodePath);
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
            target_node = GetNodeOrNull<Spatial>(path_to_target);
            if (target_node == null) {
                GD.PrintErr("TwoBoneIK: Cannot find target!");
                return;
            }
        }
        if (twisted_bone_one == null) {
            twisted_bone_one = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_one);
            if (twisted_bone_one == null) {
                GD.PrintErr("TwoBoneIK: Cannot find twisted_bone one!");
                return;
            }
        }
        if (twisted_bone_two == null) {
            twisted_bone_two = GetNodeOrNull<Twisted_Bone3D>(path_to_twisted_bone_two);
            if (twisted_bone_two == null) {
                GD.PrintErr("TwoBoneIK: Cannot find twisted_bone two!");
                return;
            }
        }
        if (use_pole_node == true) {
            if (pole_node == null && path_to_pole_node != null) {
                pole_node = GetNodeOrNull<Spatial>(path_to_pole_node);
            }
            if (pole_node == null) {
                GD.PrintErr("TwoBoneIK: Cannot find pole node!");
                return;
            }
        }

        Vector3 twisted_bone_one_scale = twisted_bone_one.Scale;
        Vector3 twisted_bone_two_scale = twisted_bone_two.Scale;

        // Account for the pole (if needed).
        // This works by making the first joint look at the pole and the second at the target
        // effectively making the IK algorithm only handle expansion/contraction.
        if (use_pole_node == true) {
            twisted_bone_one.LookAt(pole_node.GlobalTransform.origin, Vector3.Up);
            twisted_bone_two.LookAt(target_node.GlobalTransform.origin, Vector3.Up);
        }

        // The IK algorithm
        // =================
        // Convert all the transforms to be relative to the global pose.
        // This is needed to make them work with any rotation
        Twisted_Skeleton3D twisted_skeleton = modifier_stack.twisted_skeleton;
        Transform bone_one_trans = twisted_skeleton.world_transform_to_global_pose(twisted_bone_one.GlobalTransform);
        Transform bone_two_trans = twisted_skeleton.world_transform_to_global_pose(twisted_bone_two.GlobalTransform);
        Transform target_trans = twisted_skeleton.world_transform_to_global_pose(target_node.GlobalTransform);
        // Get the tip position and convert it to be relative to the global pose
        Transform bone_two_tip_trans = twisted_bone_two.GlobalTransform;
        bone_two_tip_trans.origin += -bone_two_tip_trans.basis.z.Normalized() * twisted_bone_two.bone_length;
        bone_two_tip_trans = twisted_skeleton.world_transform_to_global_pose(bone_two_tip_trans);

        // We will directly be operating on the Twisted_Bone3Ds, as we need to keep the parent-child relationship
        // so we will set their global transforms. We just need to convert them back to world transforms when finished.
        twisted_bone_one.GlobalTransform = bone_one_trans;
        twisted_bone_two.GlobalTransform = bone_two_trans;

        float length_joint_one_to_target = bone_one_trans.origin.DistanceTo(target_trans.origin);
        float bone_one_length = twisted_bone_one.get_global_pose_length();
        float bone_two_length = twisted_bone_two.get_global_pose_length();

        if (bone_one_length + bone_two_length < length_joint_one_to_target) {
            // Set the target *just* out of reach to straighten the bones
            length_joint_one_to_target = bone_one_length + bone_two_length + 0.01f;
            twisted_bone_two.Rotation = Vector3.Zero;
        }

        float sqr_one_length = bone_one_length * bone_one_length;
        float sqr_two_length = bone_two_length * bone_two_length;
        float sqr_one_to_target_length = length_joint_one_to_target * length_joint_one_to_target;

        // Calculate the angles using the law of cosigns
        // (NOTE - it seems this code acts differently with rotation in human example in showcase)
        // (NOTE 2 - the issue seems to be where the pole moves the target when assembly is rotated. You can see this in test scene 5)
        float ac_ab_0 = Mathf.Acos(Mathf.Clamp(
                bone_two_tip_trans.origin.DirectionTo(bone_one_trans.origin).Dot(bone_two_trans.origin.DirectionTo(bone_one_trans.origin)), 
                -1, 1));
        float ac_at_0 = Mathf.Acos(Mathf.Clamp(
                bone_one_trans.origin.DirectionTo(bone_two_tip_trans.origin).Dot(bone_one_trans.origin.DirectionTo(target_trans.origin)), 
                -1, 1));
        float ac_ab_1 = Mathf.Acos(Mathf.Clamp(
                (sqr_one_to_target_length + sqr_one_length - sqr_two_length) / (2.0f * bone_one_length * length_joint_one_to_target), 
                -1, 1));
        
        // Calculate the angles of rotation
        Vector3 axis_0 = bone_one_trans.origin.DirectionTo(bone_two_tip_trans.origin).Cross(bone_one_trans.origin.DirectionTo(bone_two_trans.origin));
        Vector3 axis_1 = bone_one_trans.origin.DirectionTo(bone_two_tip_trans.origin).Cross(bone_one_trans.origin.DirectionTo(target_trans.origin));

        // Apply additional rotation (Bone One)
        twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.x.Normalized(), bone_one_additional_rotation.x);
        twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.y.Normalized(), bone_one_additional_rotation.y);
        twisted_bone_one.Rotate(twisted_bone_one.Transform.basis.z.Normalized(), bone_one_additional_rotation.z);

        // Apply the rotation to the first bone
        twisted_bone_one.Rotate(axis_0.Normalized(), ac_ab_1 - ac_ab_0);
        twisted_bone_one.Rotate(axis_1.Normalized(), ac_at_0);
        
        // Apply the rotation to the second bone
        if (use_pole_node == true) {
            // Just look at the target
            twisted_bone_two.LookAt(target_trans.origin, Vector3.Up);

             // Apply additional rotation (Bone One)
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.x.Normalized(), bone_two_additional_rotation.x);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.y.Normalized(), bone_two_additional_rotation.y);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.z.Normalized(), bone_two_additional_rotation.z);
        } else {
            // Apply additional rotation (Bone One)
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.x.Normalized(), bone_two_additional_rotation.x);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.y.Normalized(), bone_two_additional_rotation.y);
            twisted_bone_two.Rotate(twisted_bone_two.Transform.basis.z.Normalized(), bone_two_additional_rotation.z);

            // (NOTE - it seems this code acts differently with rotation in human example in showcase)
            float ba_bc_0 = Mathf.Acos(Mathf.Clamp(
                bone_two_trans.origin.DirectionTo(bone_one_trans.origin).Dot(bone_two_trans.origin.DirectionTo(bone_two_tip_trans.origin)), 
                -1, 1));
            float ba_bc_1 = Mathf.Acos(Mathf.Clamp(
                (sqr_one_to_target_length - sqr_one_length - sqr_two_length) / (-2.0f * bone_one_length * bone_two_length), 
                -1, 1));

            twisted_bone_two.Rotate(axis_0.Normalized(), ba_bc_1 - ba_bc_0);
        }
        // Convert the global-pose transforms back to world transforms
        // (We need to cache the transforms in bone_one_trans and bone_two_trans so they do not change as we apply them to the bones)
        bone_one_trans = twisted_bone_one.GlobalTransform;
        bone_two_trans = twisted_bone_two.GlobalTransform;
        twisted_bone_one.GlobalTransform = twisted_skeleton.global_pose_to_world_transform(bone_one_trans);
        twisted_bone_two.GlobalTransform = twisted_skeleton.global_pose_to_world_transform(bone_two_trans);

        twisted_bone_one.Scale = twisted_bone_one_scale;
        twisted_bone_two.Scale = twisted_bone_two_scale;

        if (force_bone_application) {
            twisted_bone_one.force_apply_transform();
            twisted_bone_two.force_apply_transform();
        }
        // ==============
    }
}