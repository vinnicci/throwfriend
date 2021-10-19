using Godot;
using System;

/// <summary>
/// A modifier that runs a  simplified physics calculation on each joint to give bones a 'jiggly' movement.
/// </summary>
[Tool]
public class Twisted_ModifierJiggle3D : Twisted_Modifier3D
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
    /// When <c>true</c>, the Vector3 <c>target_vector</c> will be used as the target for the jiggle
    /// joint chain instead of a Spatial-based node.
    /// </summary>
    public bool use_target_vector_instead_of_node = false;
    /// <summary>
    /// A Vector3 that represents the normalized position that will be used as the target for the jiggle joint
    /// chain if <c>use_target_vector_instead_of_node</c> is <c>true</c>. Note that this vector is normalized when
    /// used and the length in the calculation is the length of all the bones in the jiggle joint chain plus 1.0.
    /// </summary>
    public Vector3 target_vector = Vector3.Zero;
    private float _target_vector_total_length = 0;

    // The position used by the jiggle joints
    private Vector3 _target_jiggle_joint_position;

    public bool make_target_vector_relative_to_node = false;
    public NodePath target_vector_relative_node_path = null;
    public Spatial target_vector_relative_node = null;

    /// <summary>
    /// The Struct used to hold all of the data for each joint in the Jiggle joint chain.
    /// </summary>
    public struct JIGGLE_JOINT {
        public NodePath path_to_twisted_bone;
        public Twisted_Bone3D twisted_bone;
        
        /// <summary>
        /// If <c>true</c>, then this joint will use its own stiffness, mass, damping, and gravity.
        /// </summary>
        public bool override_defaults;
        public float stiffness;
        public float mass;
        public float damping;
        public bool use_graivty;
        public Vector3 gravity;
        public Vector3 additional_rotation;
        public float parent_inertia;

        public Vector3 dynamic_pos;
        public Vector3 acceleration;
        public Vector3 velocity;
        public Vector3 force;
        public Vector3 last_position;
        public Vector3 last_noncollision_position;

        public JIGGLE_JOINT(NodePath path) {
            this.path_to_twisted_bone = path;
            this.twisted_bone = null;
            this.override_defaults = false;
            this.stiffness = 3;
            this.mass = 0.75f;
            this.damping = 0.75f;
            this.use_graivty = false;
            this.gravity = new Vector3(0, -6.0f, 0);
            this.additional_rotation = Vector3.Zero;
            this.parent_inertia = 0.25f;

            this.dynamic_pos = Vector3.Zero;
            this.acceleration = Vector3.Zero;
            this.velocity = Vector3.Zero;
            this.force = Vector3.Zero;
            this.last_position = Vector3.Zero;
            this.last_noncollision_position = Vector3.Zero;
        }
    }
    /// <summary>
    /// All of the joints in the Jiggle chain.
    /// </summary>
    public JIGGLE_JOINT[] jiggle_joints = new JIGGLE_JOINT[0];

    public float default_stiffness = 3.0f;
    public float default_mass = 0.75f;
    public float default_damping = 0.75f;
    public bool default_use_gravity = false;
    public Vector3 default_gravity = new Vector3(0, -6.0f, 0);
    public float default_parent_inertia = 0.25f;

    /// <summary>
    /// If <c>true</c>, then the Jiggle algorithm will use raycasting to take colliders into account.
    /// This will cause the algorithm to atempt to make sure that Jiggle joints do not rotate into collision objects.
    /// This feature only works if the modifier is set to execute in <c>_physics_process</c>.
    /// </summary>
    public bool use_colliders = false;
    /// <summary>
    /// The collision mask used in the raycasts
    /// </summary>
    public int collider_mask = 1;

    /// <summary>
    /// If <c>true</c>, the inertia motion generated by each bone in the jiggle joint chain is sent down to the
    /// next bones in the chain. This prevents extreme wiggling on the tips, as the velocity is not increasingly
    /// large as it travels down the chain.
    /// The amount of inertia passed is defined by the <c>parent_inertia</c> property, which can be set on all bones
    /// or overridden on a per-joint basis.
    /// </summary>
    public bool send_inertia_through_chain = false;
    /// <summary>
    /// If <c>true</c>, then the inertia motion will be sent through every child joint whenever its parent
    /// moves using simple physics. This is the most 'technically' correct way to pass inertia down, however
    /// it leads to the joints further in the chain being incredibly stiff, kinda negating the jiggle effect.
    /// (It's also more performance heavy!)
    /// </summary>
    public bool send_inertia_through_all_bones_in_chain = false;

    /// <summary>
    /// If <c>true</c>, Jiggle will use <c>look_at</c> to calculate the joint rotation.
    /// If <c>false</c>, Jiggle will use Quat-based rotation instead (does not allow for additional rotation, but does not twist the bones)
    /// 
    /// One thing to note: using Quat-based rotation can lead to situations where the bones get twisted up in themselves,
    /// at least this sometimes occured with lots of motion with jiggle joints in my testing. Not sure on a fix or if a fix is even possible,
    /// as it may just be a limitation of Quat-based rotation.
    /// </summary>
    public bool use_lookat = true;

    private int joint_count = 0;

    public override void _Ready()
    {
        if (path_to_target != null) {
            target_node = GetNodeOrNull<Spatial>(path_to_target);
        }
    }

    public override bool _Set(string property, object value)
    {   
        if (property == "Jiggle/target") {
            path_to_target = (NodePath)value;
            if (path_to_target != null) {
                target_node = GetNodeOrNull<Spatial>(path_to_target);
            }
            return true;
        }
        else if (property == "Jiggle/use_target_vector_instead_of_node") {
            use_target_vector_instead_of_node = (bool)value;
            calculate_target_vector_total_length();
            PropertyListChangedNotify();
        }
        else if (property == "Jiggle/make_target_vector_relative_to_node") {
            make_target_vector_relative_to_node = (bool)value;
            PropertyListChangedNotify();
        }
        else if (property == "Jiggle/target_vector_relative_node_path") {
            target_vector_relative_node_path = (NodePath)value;
            if (target_vector_relative_node_path != null) {
                target_vector_relative_node = GetNodeOrNull<Spatial>(target_vector_relative_node_path);
            }
        }
        else if (property == "Jiggle/target_vector") {
            target_vector = (Vector3)value;
            calculate_target_vector_total_length();

            if (target_vector.LengthSquared() == 0) {
                GD.PrintErr("ERROR - Jiggle target vector must have a non-zero length!");
            }
        }
        else if (property == "Jiggle/joint_count") {
            joint_count = (int)value;

            JIGGLE_JOINT[] new_array = new JIGGLE_JOINT[joint_count];
            for (int i = 0; i < joint_count; i++) {
                if (i < jiggle_joints.Length) {
                    new_array[i] = jiggle_joints[i];
                } else {
                    new_array[i] = new JIGGLE_JOINT(null);
                }
            }
            jiggle_joints = new_array;

            PropertyListChangedNotify();
            return true;
        }
        else if (property == "Jiggle/use_colliders") {
            use_colliders = (bool)value;
            return true;
        }
        else if (property == "Jiggle/collider_mask") {
            collider_mask = (int)value;
            return true;
        }
        else if (property == "Jiggle/send_inertia_through_chain") {
            send_inertia_through_chain = (bool)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "Jiggle/send_inertia_through_all_bones_in_chain") {
            send_inertia_through_all_bones_in_chain = (bool)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "Jiggle/use_lookat") {
            use_lookat = (bool)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "Jiggle/Default_Joint_Data/stiffness") {
            default_stiffness = (float)value;
            set_joint_data_to_defaults();
            return true;
        }
        else if (property == "Jiggle/Default_Joint_Data/mass") {
            default_mass = (float)value;
            set_joint_data_to_defaults();
            return true;
        }
        else if (property == "Jiggle/Default_Joint_Data/damping") {
            default_damping = (float)value;
            set_joint_data_to_defaults();
            return true;
        }
        else if (property == "Jiggle/Default_Joint_Data/use_gravity") {
            default_use_gravity = (bool)value;
            set_joint_data_to_defaults();
            return true;
        }
        else if (property == "Jiggle/Default_Joint_Data/gravity") {
            default_gravity = (Vector3)value;
            set_joint_data_to_defaults();
            return true;
        }
        else if (property == "Jiggle/Default_Joint_Data/parent_inertia") {
            default_parent_inertia = (float)value;
            set_joint_data_to_defaults();
            return true;
        }

        
        else if (property.StartsWith("Jiggle/joint/")) {
            String[] jiggle_data = property.Split("/");
            int joint_index = jiggle_data[2].ToInt();
            
            if (joint_index < 0 || joint_index > jiggle_joints.Length-1) {
                GD.PrintErr("ERROR - Cannot get Jiggle joint at index " + joint_index.ToString());
                return false;
            }
            JIGGLE_JOINT current_joint = jiggle_joints[joint_index];

            if (jiggle_data[3] == "twisted_bone") {
                current_joint.path_to_twisted_bone = (NodePath)value;
                if (current_joint.path_to_twisted_bone != null) {
                    current_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(current_joint.path_to_twisted_bone);
                }
            }
            else if (jiggle_data[3] == "override_defaults") {
                current_joint.override_defaults = (bool)value;
                
                if (current_joint.override_defaults == false) {
                    current_joint.stiffness = default_stiffness;
                    current_joint.mass = default_mass;
                    current_joint.damping = default_damping;
                    current_joint.use_graivty = default_use_gravity;
                    current_joint.gravity = default_gravity;
                    current_joint.parent_inertia = default_parent_inertia;
                }

                PropertyListChangedNotify();
            }
            else if (jiggle_data[3] == "stiffness") {
                current_joint.stiffness = (float)value;
            }
            else if (jiggle_data[3] == "mass") {
                current_joint.mass = (float)value;
            }
            else if (jiggle_data[3] == "damping") {
                current_joint.damping = (float)value;
            }
            else if (jiggle_data[3] == "use_gravity") {
                current_joint.use_graivty = (bool)value;
            }
            else if (jiggle_data[3] == "gravity") {
                current_joint.gravity = (Vector3)value;
            }
            else if (jiggle_data[3] == "parent_inertia") {
                current_joint.parent_inertia = (float)value;
            }
            else if (jiggle_data[3] == "additional_rotation") {
                Vector3 tmp = (Vector3)value;
                tmp.x = Mathf.Deg2Rad(tmp.x);
                tmp.y = Mathf.Deg2Rad(tmp.y);
                tmp.z = Mathf.Deg2Rad(tmp.z);
                current_joint.additional_rotation = tmp;
            }
            jiggle_joints[joint_index] = current_joint;

            if (use_target_vector_instead_of_node == true) {
                calculate_target_vector_total_length();
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
        if (property == "Jiggle/target") {
            return path_to_target;
        }
        else if (property == "Jiggle/use_target_vector_instead_of_node") {
            return use_target_vector_instead_of_node;
        }
        else if (property == "Jiggle/target_vector") {
            return target_vector;
        }
        else if (property == "Jiggle/make_target_vector_relative_to_node") {
            return make_target_vector_relative_to_node;
        }
        else if (property == "Jiggle/target_vector_relative_node_path") {
            return target_vector_relative_node_path;
        }
        else if (property == "Jiggle/joint_count") {
            return joint_count;
        }
        else if (property == "Jiggle/use_colliders") {
            return use_colliders;
        }
        else if (property == "Jiggle/collider_mask") {
            return collider_mask;
        }
        else if (property == "Jiggle/send_inertia_through_chain") {
            return send_inertia_through_chain;
        }
        else if (property == "Jiggle/send_inertia_through_all_bones_in_chain") {
            return send_inertia_through_all_bones_in_chain;
        }
        else if (property == "Jiggle/use_lookat") {
            return use_lookat;
        }

        else if (property == "Jiggle/Default_Joint_Data/stiffness") {
            return default_stiffness;
        }
        else if (property == "Jiggle/Default_Joint_Data/mass") {
            return default_mass;
        }
        else if (property == "Jiggle/Default_Joint_Data/damping") {
            return default_damping;
        }
        else if (property == "Jiggle/Default_Joint_Data/use_gravity") {
            return default_use_gravity;
        }
        else if (property == "Jiggle/Default_Joint_Data/gravity") {
            return default_gravity;
        }
        else if (property == "Jiggle/Default_Joint_Data/parent_inertia") {
            return default_parent_inertia;
        }

        
        else if (property.StartsWith("Jiggle/joint/")) {
            String[] jiggle_data = property.Split("/");
            int joint_index = jiggle_data[2].ToInt();

            if (jiggle_data[3] == "twisted_bone") {
                return jiggle_joints[joint_index].path_to_twisted_bone;
            }
            else if (jiggle_data[3] == "override_defaults") {
                return jiggle_joints[joint_index].override_defaults;
            }
            else if (jiggle_data[3] == "stiffness") {
                return jiggle_joints[joint_index].stiffness;
            }
            else if (jiggle_data[3] == "mass") {
                return jiggle_joints[joint_index].mass;
            }
            else if (jiggle_data[3] == "damping") {
                return jiggle_joints[joint_index].damping;
            }
            else if (jiggle_data[3] == "use_gravity") {
                return jiggle_joints[joint_index].use_graivty;
            }
            else if (jiggle_data[3] == "gravity") {
                return jiggle_joints[joint_index].gravity;
            }
            else if (jiggle_data[3] == "parent_inertia") {
                return jiggle_joints[joint_index].parent_inertia;
            }
            else if (jiggle_data[3] == "additional_rotation") {
                Vector3 tmp = jiggle_joints[joint_index].additional_rotation;
                tmp.x = Mathf.Rad2Deg(tmp.x);
                tmp.y = Mathf.Rad2Deg(tmp.y);
                tmp.z = Mathf.Rad2Deg(tmp.z);
                return tmp;
            }
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

        if (use_target_vector_instead_of_node == false) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/target");
            tmp_dict.Add("type", Variant.Type.NodePath);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);
        }

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/use_target_vector_instead_of_node");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (use_target_vector_instead_of_node == true) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/target_vector");
            tmp_dict.Add("type", Variant.Type.Vector3);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);

            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/make_target_vector_relative_to_node");
            tmp_dict.Add("type", Variant.Type.Bool);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);

            if (make_target_vector_relative_to_node == true) {
                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", "Jiggle/target_vector_relative_node_path");
                tmp_dict.Add("type", Variant.Type.NodePath);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);
            }
        }

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/joint_count");
        tmp_dict.Add("type", Variant.Type.Int);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (execution_mode == TWISTED_IK_MODIFIER_MODES_3D._PHYSICS_PROCESS) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/use_colliders");
            tmp_dict.Add("type", Variant.Type.Bool);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);

            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/collider_mask");
            tmp_dict.Add("type", Variant.Type.Int);
            tmp_dict.Add("hint", PropertyHint.Layers3dPhysics);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);
        }

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/send_inertia_through_chain");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (send_inertia_through_chain == true) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/send_inertia_through_all_bones_in_chain");
            tmp_dict.Add("type", Variant.Type.Bool);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);
        }

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/use_lookat");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        // Default joint data
        // ===================
        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/Default_Joint_Data/stiffness");
        tmp_dict.Add("type", Variant.Type.Real);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/Default_Joint_Data/mass");
        tmp_dict.Add("type", Variant.Type.Real);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/Default_Joint_Data/damping");
        tmp_dict.Add("type", Variant.Type.Real);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/Default_Joint_Data/use_gravity");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "Jiggle/Default_Joint_Data/gravity");
        tmp_dict.Add("type", Variant.Type.Vector3);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        if (send_inertia_through_chain == true) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", "Jiggle/Default_Joint_Data/parent_inertia");
            tmp_dict.Add("type", Variant.Type.Real);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);
        }

        // ===================

        // The Jiggle Joints
        // ===================
        String jiggle_string = "Jiggle/joint/";
        for (int i = 0; i < joint_count; i++) {
            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", jiggle_string + i.ToString() + "/twisted_bone");
            tmp_dict.Add("type", Variant.Type.NodePath);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);

            tmp_dict = new Godot.Collections.Dictionary();
            tmp_dict.Add("name", jiggle_string + i.ToString() + "/override_defaults");
            tmp_dict.Add("type", Variant.Type.Bool);
            tmp_dict.Add("hint", PropertyHint.None);
            tmp_dict.Add("usage", PropertyUsageFlags.Default);
            list.Add(tmp_dict);

            if (jiggle_joints[i].override_defaults == true) {
                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", jiggle_string + i.ToString() + "/stiffness");
                tmp_dict.Add("type", Variant.Type.Real);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);

                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", jiggle_string + i.ToString() + "/mass");
                tmp_dict.Add("type", Variant.Type.Real);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);

                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", jiggle_string + i.ToString() + "/damping");
                tmp_dict.Add("type", Variant.Type.Real);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);

                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", jiggle_string + i.ToString() + "/use_gravity");
                tmp_dict.Add("type", Variant.Type.Bool);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);

                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", jiggle_string + i.ToString() + "/gravity");
                tmp_dict.Add("type", Variant.Type.Vector3);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);

                if (send_inertia_through_chain == true) {
                    tmp_dict = new Godot.Collections.Dictionary();
                    tmp_dict.Add("name", jiggle_string + i.ToString() + "/parent_inertia");
                    tmp_dict.Add("type", Variant.Type.Real);
                    tmp_dict.Add("hint", PropertyHint.None);
                    tmp_dict.Add("usage", PropertyUsageFlags.Default);
                    list.Add(tmp_dict);
                }
            }

            if (use_lookat == false) {
                tmp_dict = new Godot.Collections.Dictionary();
                tmp_dict.Add("name", jiggle_string + i.ToString() + "/additional_rotation");
                tmp_dict.Add("type", Variant.Type.Vector3);
                tmp_dict.Add("hint", PropertyHint.None);
                tmp_dict.Add("usage", PropertyUsageFlags.Default);
                list.Add(tmp_dict);
            }
        }

        return list;
    }

    private void set_joint_data_to_defaults() {
        for (int i = 0; i < jiggle_joints.Length; i++) {
            JIGGLE_JOINT current_joint = jiggle_joints[i];
            if (current_joint.override_defaults == false) {
                current_joint.stiffness = default_stiffness;
                current_joint.mass = default_mass;
                current_joint.damping = default_damping;
                current_joint.use_graivty = default_use_gravity;
                current_joint.gravity = default_gravity;
                current_joint.parent_inertia = default_parent_inertia;
                jiggle_joints[i] = current_joint;
            }
        }
    }

    public override void _ExecuteModification(Twisted_ModifierStack3D modifier_stack, float delta)
    {
        base._ExecuteModification(modifier_stack, delta);

        if (use_target_vector_instead_of_node == false) {
            if (target_node == null) {
                // Try to get the target
                if (path_to_target != null) {
                    target_node = GetNodeOrNull<Spatial>(path_to_target);
                    if (target_node == null) {
                        GD.PrintErr("Cannot execute Jiggle: No target found!");
                        return;
                    }
                }
                else {
                    GD.PrintErr("Cannot execute Jiggle: No target found and/or nodepath to target is not setup!");
                    return;
                }
            }
        }
        else {
            if (target_vector.LengthSquared() == 0) {
                GD.PrintErr("Cannot execute Jiggle: Jiggle target vector must be more than zero!");
                return;
            }
        }

        if (jiggle_joints.Length <= 0) {
            GD.PrintErr("Cannot execute Jiggle: No Jiggle joints found!");
            return;
        }

        if (use_target_vector_instead_of_node == false) {
            _target_jiggle_joint_position = target_node.GlobalTransform.origin;
        } else {
            if (_target_vector_total_length == 0) {
                calculate_target_vector_total_length();
            }

            Vector3 target_origin_position = target_vector;
            Vector3 target_vector_direction = target_vector;

            // Make the position relative to the first bone in the chain
            if (jiggle_joints.Length > 0) {
                if (jiggle_joints[0].twisted_bone != null) {
                    target_origin_position = jiggle_joints[0].twisted_bone.GlobalTransform.origin;
                }
            }

            // Make the vector relative to a node?
            if (make_target_vector_relative_to_node == true) {
                if (target_vector_relative_node == null) {
                    if (target_vector_relative_node_path != null) {
                        target_vector_relative_node = GetNodeOrNull<Spatial>(target_vector_relative_node_path);
                    }
                    if (target_vector_relative_node == null) {
                        GD.PrintErr("Cannot execute Jiggle: Cannot get relative target vector node!");
                        return;
                    }
                }
                target_vector_direction = target_vector_relative_node.GlobalTransform.basis.Xform(target_vector_direction);
            }

            _target_jiggle_joint_position = target_origin_position + (target_vector_direction.Normalized() * _target_vector_total_length);
        }

        for (int i = 0; i < jiggle_joints.Length; i++) {
            _ExecuteJoint(i, modifier_stack, delta);
        }
    }


    private void _ExecuteJoint(int joint_id, Twisted_ModifierStack3D modifier_stack, float delta) {
        JIGGLE_JOINT current_joint = jiggle_joints[joint_id];
        if (current_joint.twisted_bone == null) {
            current_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(current_joint.path_to_twisted_bone);
            if (current_joint.twisted_bone == null) {
                GD.PrintErr("Cannot find TwistedBone3D for joint: " + joint_id.ToString() + ". ABORTING IK!");
                return;
            }
        }

        current_joint.force = (_target_jiggle_joint_position - current_joint.dynamic_pos) * current_joint.stiffness * delta;

        if (current_joint.use_graivty == true) {
            Vector3 gravity_to_apply;
            if (modifier_stack.twisted_skeleton != null) {
                gravity_to_apply = modifier_stack.twisted_skeleton.GlobalTransform.basis.Inverse().Xform(current_joint.gravity);
            }
            else {
                // TODO: print a warning that the gravity may not point in the correct direction!
                gravity_to_apply = current_joint.twisted_bone.GlobalTransform.basis.Inverse().Xform(current_joint.gravity);
            }
            current_joint.force += gravity_to_apply * delta;
        }

        current_joint.acceleration = current_joint.force * current_joint.mass;

        Vector3 last_velocity = current_joint.velocity;
        current_joint.velocity += current_joint.acceleration * (1 - current_joint.damping);

        current_joint.dynamic_pos += current_joint.velocity + current_joint.force;
        current_joint.dynamic_pos += current_joint.twisted_bone.GlobalTransform.origin - current_joint.last_position;
        current_joint.last_position = current_joint.twisted_bone.GlobalTransform.origin;

        if (use_colliders == true && execution_mode == TWISTED_IK_MODIFIER_MODES_3D._PHYSICS_PROCESS) {
            World world_3d = GetWorld();
            PhysicsDirectSpaceState space_state = world_3d.DirectSpaceState;
            
            Godot.Collections.Dictionary result = space_state.IntersectRay(
                current_joint.twisted_bone.GlobalTransform.origin, current_joint.dynamic_pos,
                null, (uint)collider_mask, true, false);
            
            if (result.Count > 0) {
                current_joint.dynamic_pos = current_joint.last_noncollision_position;
                current_joint.acceleration = Vector3.Zero;
                current_joint.velocity = Vector3.Zero;
                current_joint.force = Vector3.Zero;
            } else {
                current_joint.last_noncollision_position = current_joint.dynamic_pos;
            }
        }

        // Rotate the bone to look at the new position
        if (use_lookat == true) {
            // Works surprisingly well!
            Vector3 bone_up_dir = current_joint.twisted_bone.get_reset_bone_global_pose().basis.y.Normalized();
            /*
            current_joint.twisted_bone.LookAt(current_joint.dynamic_pos,
                current_joint.twisted_bone.get_bone_global_pose().basis.Xform(Vector3.Up)
            );
            */
            current_joint.twisted_bone.LookAt(current_joint.dynamic_pos, bone_up_dir);
            
            // Apply additional rotation
            current_joint.twisted_bone.RotateObjectLocal(Vector3.Right, current_joint.additional_rotation.x);
            current_joint.twisted_bone.RotateObjectLocal(Vector3.Up, current_joint.additional_rotation.y);
            current_joint.twisted_bone.RotateObjectLocal(Vector3.Forward, current_joint.additional_rotation.z);
        }
        else {
            current_joint.twisted_bone.GlobalTransform = Twisted_3DFunctions.quat_based_lookat(
                current_joint.twisted_bone.GlobalTransform, current_joint.dynamic_pos);
        }

        // Keep the scale consistent with the global pose
        current_joint.twisted_bone.Scale = current_joint.twisted_bone.get_reset_bone_global_pose(false).basis.Scale;
        
        // Force apply (if needed)
        if (force_bone_application == true) {
            current_joint.twisted_bone.force_apply_transform();
        }

        jiggle_joints[joint_id] = current_joint;

        if (send_inertia_through_chain == true) {
            for (int j = joint_id+1; j < jiggle_joints.Length; j++) {
                JIGGLE_JOINT child_joint = jiggle_joints[j];
                
                if (child_joint.twisted_bone == null) {
                    child_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(child_joint.path_to_twisted_bone);
                    if (child_joint.twisted_bone == null) {
                        continue;
                    }
                }

                float distance_to_remove = (current_joint.velocity - last_velocity).Length();
                child_joint.velocity -= (child_joint.velocity.Normalized() * distance_to_remove) * child_joint.parent_inertia;
                child_joint.dynamic_pos -= (child_joint.velocity.Normalized() * distance_to_remove) * child_joint.parent_inertia;
                jiggle_joints[j] = child_joint;

                // Debugging data - useful for visualization
                /*
                MeshInstance debug_mesh = GetNodeOrNull<MeshInstance>("DEBUG" + j.ToString());
                if (debug_mesh != null) {
                    Transform db_trans = debug_mesh.GlobalTransform;
                    db_trans.origin = child_joint.dynamic_pos;
                    debug_mesh.GlobalTransform = db_trans;
                }
                */

                if (send_inertia_through_all_bones_in_chain == false) {
                    break;
                }
            }
        }
    }

    private void calculate_target_vector_total_length() {
        float total_length = 0;
        for (int i = 0; i < jiggle_joints.Length; i++) {
            if (jiggle_joints[i].twisted_bone != null) {
                total_length += jiggle_joints[i].twisted_bone.bone_length;
            }
        }
        _target_vector_total_length = total_length + 1.0f;
    }
}