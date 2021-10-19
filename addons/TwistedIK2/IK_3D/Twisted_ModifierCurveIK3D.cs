using Godot;
using System;

/// <summary>
/// A modifier that makes a series of bones follow a Curve3D in a Path3D node.
/// </summary>
[Tool]
public class Twisted_ModifierCurveIK3D : Twisted_Modifier3D
{
    /// <summary>
    /// A NodePath to the Path3D node that contains the Curve3D that the modifier is supposed to follow.
    /// </summary>
    public NodePath path_to_curve = null;
    /// <summary>
    /// A reference to the Path3D node that contains the Curve3D that the modifier is supposed to follow.
    /// </summary>
    public Path curve_node = null;

    /// <summary>
    /// The offset where the first bone in the CurveIK chain starts. This allows you to move the entire
    /// chain of bones forwards (or backwards, I guess) on the Curve3D.
    /// </summary>
    public float starting_position_on_curve = 0.0f;
    /// <summary>
    /// If <c>true</c>, the CurveIK modifier will place the bones evenly across the Curve3D so they are each equal-distance
    /// from each other while going from the Curve3D origin to the Curve3D end.
    /// </summary>
    public bool stretch_bones_to_fit_curve = false;
    /// <summary>
    /// If <c>true</c>, the CurveIK modifier will scale the Twisted_Bone3D nodes based on their lengths. This will make
    /// bones larger if their combined length is too short for the Curve3D or shorter if their combined length is too long for the Curve3D.
    /// </summary>
    public bool scale_bones_to_fit_curve = false;

    /// <summary>
    /// The Struct used to hold all of the data for each joint in the CurveIK joint chain.
    /// </summary>
    public struct CURVE_JOINT {
        public NodePath path_to_twisted_bone;
        public Twisted_Bone3D twisted_bone;

        public Vector3 additional_rotation;

        public CURVE_JOINT(NodePath path) {
            this.path_to_twisted_bone = path;
            this.twisted_bone = null;
            this.additional_rotation = Vector3.Zero;
        }
    }
    /// <summary>
    /// All of the joints in the CurveIK chain.
    /// </summary>
    public CURVE_JOINT[] curve_joints = new CURVE_JOINT[0];

    private int joint_count = 0;

    public override void _Ready()
    {
        if (path_to_curve != null) {
            curve_node = GetNodeOrNull<Path>(path_to_curve);
        }
    }

    public override bool _Set(string property, object value)
    {
        if (property == "CurveIK/path") {
            path_to_curve = (NodePath)value;
            if (path_to_curve != null) {
                curve_node = GetNodeOrNull<Path>(path_to_curve);
            }
            return true;
        }
        else if (property == "CurveIK/starting_position_on_curve") {
            starting_position_on_curve = (float)value;
            return true;
        }
        else if (property == "CurveIK/stretch_bones_to_fit_curve") {
            stretch_bones_to_fit_curve = (bool)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "CurveIK/scale_bones_to_fit_curve") {
            scale_bones_to_fit_curve = (bool)value;
            return true;
        }
        else if (property == "CurveIK/joint_count") {
            joint_count = (int)value;

            CURVE_JOINT[] new_array = new CURVE_JOINT[joint_count];
            for (int i = 0; i < joint_count; i++) {
                if (i < curve_joints.Length) {
                    new_array[i] = curve_joints[i];
                } else {
                    new_array[i] = new CURVE_JOINT(null);
                }
            }
            curve_joints = new_array;

            PropertyListChangedNotify();
            return true;
        }
        else if (property.StartsWith("CurveIK/joint/")) {
            String[] curve_data = property.Split("/");
            int joint_index = curve_data[2].ToInt();
            
            if (joint_index < 0 || joint_index > curve_joints.Length-1) {
                GD.PrintErr("ERROR - Cannot get Curve joint at index " + joint_index.ToString());
                return false;
            }
            CURVE_JOINT current_joint = curve_joints[joint_index];

            if (curve_data[3] == "twisted_bone") {
                current_joint.path_to_twisted_bone = (NodePath)value;
                if (current_joint.path_to_twisted_bone != null) {
                    current_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(current_joint.path_to_twisted_bone);
                }
            }
            else if (curve_data[3] == "additional_rotation") {
                Vector3 tmp = (Vector3)value;
                tmp.x = Mathf.Deg2Rad(tmp.x);
                tmp.y = Mathf.Deg2Rad(tmp.y);
                tmp.z = Mathf.Deg2Rad(tmp.z);
                current_joint.additional_rotation = tmp;
            }
            curve_joints[joint_index] = current_joint;
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
        if (property == "CurveIK/path") {
            return path_to_curve;
        }
        else if (property == "CurveIK/starting_position_on_curve") {
            return starting_position_on_curve;
        }
        else if (property == "CurveIK/stretch_bones_to_fit_curve") {
            return stretch_bones_to_fit_curve;
        }
        else if (property == "CurveIK/scale_bones_to_fit_curve") {
            return scale_bones_to_fit_curve;
        }
        else if (property == "CurveIK/joint_count") {
            return joint_count;
        }

        else if (property.StartsWith("CurveIK/joint/")) {
            String[] curve_data = property.Split("/");
            int joint_index = curve_data[2].ToInt();

            if (curve_data[3] == "twisted_bone") {
                return curve_joints[joint_index].path_to_twisted_bone;
            }
            else if (curve_data[3] == "additional_rotation") {
                Vector3 tmp = curve_joints[joint_index].additional_rotation;
                tmp.x = Mathf.Rad2Deg(tmp.x);
                tmp.y = Mathf.Rad2Deg(tmp.y);
                tmp.z = Mathf.Rad2Deg(tmp.z);
                return tmp;
            }
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
        tmp_dict.Add("name", "CurveIK/path");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "CurveIK/starting_position_on_curve");
        tmp_dict.Add("type", Variant.Type.Real);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "CurveIK/stretch_bones_to_fit_curve");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (stretch_bones_to_fit_curve == true) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "CurveIK/scale_bones_to_fit_curve");
            tmp_dict.Add("type", Variant.Type.Bool);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);
        }

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "CurveIK/joint_count");
        tmp_dict.Add("type", Variant.Type.Int);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        // The Curve Joints
        // ===================
        String curve_string = "CurveIK/joint/";
        for (int i = 0; i < joint_count; i++) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", curve_string + i.ToString() + "/twisted_bone");
            tmp_dict.Add("type", Variant.Type.NodePath);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);

            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", curve_string + i.ToString() + "/additional_rotation");
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

        if (curve_node == null) {
            GD.PrintErr("Cannot execute Curve IK: No path found!");
            return; // TODO: print an error!
        }

        Curve3D curve_to_use = curve_node.Curve;
        if (curve_to_use == null) {
            return;
        }

        float scale_mod = 1;
        if (stretch_bones_to_fit_curve) {
            float total_length = 0.0f;
            for (int i = 0; i < curve_joints.Length; i++) {
                CURVE_JOINT current_joint = curve_joints[i];
                if (current_joint.twisted_bone != null) {
                    total_length += current_joint.twisted_bone.bone_length;
                }
            }
            scale_mod = (curve_to_use.GetBakedLength() * curve_node.Scale.x) / total_length;

            if (scale_bones_to_fit_curve == true) {
                if (curve_joints.Length > 0) {
                    CURVE_JOINT current_joint = curve_joints[0];
                    if (current_joint.twisted_bone != null) {
                        current_joint.twisted_bone.Scale = (Vector3.One * scale_mod);
                    }
                }
            }
        }

        float position_on_curve = starting_position_on_curve;
        for (int i = 0; i < curve_joints.Length; i++) {
            CURVE_JOINT current_joint = curve_joints[i];

            if (current_joint.twisted_bone == null) {
                if (current_joint.path_to_twisted_bone != null) {
                    current_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(current_joint.path_to_twisted_bone);
                    if (current_joint.twisted_bone == null) {
                        GD.PrintErr("Curve joint " + i.ToString() + " not setup. Skipping!");
                        continue;
                    }
                    curve_joints[i] = current_joint;
                }
                else {
                    GD.PrintErr("Curve joint " + i.ToString() + " not setup. Skipping!");
                    continue;
                }
            }

            Transform current_transform = current_joint.twisted_bone.GlobalTransform;
            Vector3 tmp_scale = current_joint.twisted_bone.Scale;

            // Position:
            current_transform.origin = curve_node.ToGlobal(curve_to_use.InterpolateBaked(position_on_curve));
            // Offset for bone length:
            if (stretch_bones_to_fit_curve == false) {
                position_on_curve += (current_joint.twisted_bone.bone_length) * curve_node.Scale.Inverse().x;
            }
            else {
                position_on_curve += (current_joint.twisted_bone.bone_length * scale_mod) * curve_node.Scale.Inverse().x;
            }
            // Rotation
            Vector3 rotation_position = curve_node.ToGlobal(curve_to_use.InterpolateBaked(position_on_curve));
            Vector3 bone_up_dir = current_joint.twisted_bone.get_reset_bone_global_pose().basis.y.Normalized();
            current_transform = current_transform.LookingAt(rotation_position, bone_up_dir);

            current_joint.twisted_bone.GlobalTransform = current_transform;

            // Apply additional rotation
            current_joint.twisted_bone.RotateObjectLocal(Vector3.Right, current_joint.additional_rotation.x);
            current_joint.twisted_bone.RotateObjectLocal(Vector3.Up, current_joint.additional_rotation.y);
            current_joint.twisted_bone.RotateObjectLocal(Vector3.Forward, current_joint.additional_rotation.z);

            // Keep the scale consistent with the global pose
            if (scale_bones_to_fit_curve == false) {
                current_joint.twisted_bone.Scale = current_joint.twisted_bone.get_reset_bone_global_pose(false).basis.Scale;
            } else {
                current_joint.twisted_bone.Scale = tmp_scale;
            }

            if (force_bone_application == true) {
                current_joint.twisted_bone.force_apply_transform();
            }
        }
    }
}