using Godot;
using System;

[Tool]
public class Twisted_ModifierVelvet3D : Twisted_Modifier3D
{
	/// <summary>
    /// A NodePath to the Spatial-based node that will act as the target position.
    /// </summary>
	public NodePath path_to_target = null;
	/// <summary>
    /// The Spatial-based node that acts at the target position
    /// </summary>
	public Spatial target_node = null;

	/// <summary>
    /// The number of times/iterations the velvet algorithm will be run. More runs equal a more stable solve, however
	/// it costs more CPU processing. This is really only needed to prevent some unwanted wiggling. Around 2-4 seems to be best for most cases.
    /// </summary>
	public int iteration_count = 3;

    /// <summary>
    /// How much the accleration is multiplied by. Higher values equal quicker movement.
    /// </summary>
    public float acceleration_speed = 40.0f;

	/// <summary>
	/// How heavy the velvet joint is. Adds more mass, which results in slower movement but larger swings
	/// </summary>
	public float default_joint_mass = 1.0f;
	/// <summary>
	/// How much drag is applied to the motion of this velvet joint
	/// </summary>
	public float default_joint_drag = 0.1f;

	public struct VELVET_JOINT {
		public NodePath path_to_twisted_bone;
		public Twisted_Bone3D twisted_bone;

		/// <summary>
		/// If <c>true</c>, then the default mass and drag can be overriden on a per-joint basis.
		/// </summary>
		public bool override_defaults;

		/// <summary>
		/// How heavy the velvet joint is. Adds more mass, which results in slower movement but larger swings
		/// </summary>
		public float mass;
		/// <summary>
		/// How much drag is applied to the motion of this velvet joint
		/// </summary>
		public float drag;

		public Vector3 velvet_velocity;
		public Vector3 velvet_accel;

		public Transform last_transform;

		public VELVET_JOINT(NodePath path) {
			this.path_to_twisted_bone = path;
			this.twisted_bone = null;
			this.mass = 2.0f;
			this.drag = 0.25f;

			this.override_defaults = false;

			this.velvet_velocity = new Vector3(0, 0, 0);
			this.velvet_accel = new Vector3(0, 0, 0);

			this.last_transform = Transform.Identity;
		}
	}

	public VELVET_JOINT[] velvet_joints = new VELVET_JOINT[0];

	private int joint_count = 0;

	/// <summary>
	/// The mode the velvet modifier uses.
	/// 0 = follow target node
	/// 1 = following Skeleton animation
	/// </summary>
	public int velvet_mode = 0;

	public float minimum_required_velocity = 0.01f;

	public override void _Ready()
	{
		if (path_to_target != null) {
			target_node = GetNodeOrNull<Spatial>(path_to_target);
		}
	}

	public override bool _Set(string property, object value)
	{
		if (property == "Velvet/target_node") {
			path_to_target = (NodePath)value;
			if (path_to_target != null) {
				target_node = GetNodeOrNull<Spatial>(path_to_target);
			}
			return true;
		}
		else if (property == "Velvet/iteration_count") {
			iteration_count = (int)value;
			if (iteration_count < 1) {
				iteration_count = 1;
			}
			return true;
		}
        else if (property == "Velvet/acceleration_speed") {
			acceleration_speed = (float)value;
			if (acceleration_speed < 1) {
				acceleration_speed = 1;
			}
			return true;
		}
		else if (property == "Velvet/minimum_required_velocity") {
			minimum_required_velocity = (float)value;
			if (minimum_required_velocity < 0) {
				minimum_required_velocity = 0.001f;
			}
			return true;
		}
		else if (property == "Velvet/mode") {
			velvet_mode = (int)value;
			PropertyListChangedNotify();
			return true;
		}

		else if (property == "Velvet/default_data/mass") {
			default_joint_mass = (float)value;
			set_joint_data_to_defaults();
			return true;
		}
		else if (property == "Velvet/default_data/drag") {
			default_joint_drag = (float)value;
			set_joint_data_to_defaults();
			return true;
		}

		else if (property == "Velvet/joint_count") {
			joint_count = (int)value;

			VELVET_JOINT[] new_array = new VELVET_JOINT[joint_count];
			for (int i = 0; i < joint_count; i++) {
				if (i < velvet_joints.Length) {
					new_array[i] = velvet_joints[i];
				} else {
					VELVET_JOINT new_joint = new VELVET_JOINT(null);
					new_joint.mass = default_joint_mass;
					new_joint.drag = default_joint_drag;
					new_array[i] = new_joint;
				}
			}
			velvet_joints = new_array;

			PropertyListChangedNotify();
			return true;
		}
		else if (property.StartsWith("Velvet/joint/")) {
			String[] velvet_data = property.Split("/");
			int joint_index = velvet_data[2].ToInt();
			
			if (joint_index < 0 || joint_index > velvet_joints.Length-1) {
				GD.PrintErr("ERROR - Cannot get Curve joint at index " + joint_index.ToString());
				return false;
			}
			VELVET_JOINT current_joint = velvet_joints[joint_index];

			if (velvet_data[3] == "twisted_bone") {
				current_joint.path_to_twisted_bone = (NodePath)value;
				if (current_joint.path_to_twisted_bone != null) {
					current_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(current_joint.path_to_twisted_bone);
				}
			}
			else if (velvet_data[3] == "mass") {
				current_joint.mass = (float)value;
			}
			else if (velvet_data[3] == "drag") {
				current_joint.drag = (float)value;
			}
			else if (velvet_data[3] == "override_defaults") {
				current_joint.override_defaults = (bool)value;
				PropertyListChangedNotify();
			}

			velvet_joints[joint_index] = current_joint;
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
		if (property == "Velvet/target_node") {
			return path_to_target;
		}
		else if (property == "Velvet/iteration_count") {
			return iteration_count;
		}
        else if (property == "Velvet/acceleration_speed") {
			return acceleration_speed;
		}
		else if (property == "Velvet/minimum_required_velocity") {
			return minimum_required_velocity;
		}
		else if (property == "Velvet/mode") {
			return velvet_mode;
		}

		else if (property == "Velvet/default_data/mass") {
			return default_joint_mass;
		}
		else if (property == "Velvet/default_data/drag") {
			return default_joint_drag;
		}

		else if (property == "Velvet/joint_count") {
			return joint_count;
		}

		else if (property.StartsWith("Velvet/joint/")) {
			String[] velvet_data = property.Split("/");
			int joint_index = velvet_data[2].ToInt();

			if (velvet_data[3] == "twisted_bone") {
				return velvet_joints[joint_index].path_to_twisted_bone;
			}
			else if (velvet_data[3] == "mass") {
				return velvet_joints[joint_index].mass;
			}
			else if (velvet_data[3] == "drag") {
				return velvet_joints[joint_index].drag;
			}
			else if (velvet_data[3] == "override_defaults") {
				return velvet_joints[joint_index].override_defaults;
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

		if (velvet_mode == 0) {
			tmp_dict = new Godot.Collections.Dictionary();
			tmp_dict.Add("name", "Velvet/target_node");
			tmp_dict.Add("type", Variant.Type.NodePath);
			tmp_dict.Add("hint", PropertyHint.None);
			tmp_dict.Add("usage", PropertyUsageFlags.Default);
			list.Add(tmp_dict);
		}

		tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/iteration_count");
		tmp_dict.Add("type", Variant.Type.Int);
		tmp_dict.Add("hint", PropertyHint.None);
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/acceleration_speed");
		tmp_dict.Add("type", Variant.Type.Real);
		tmp_dict.Add("hint", PropertyHint.None);
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);

		tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/minimum_required_velocity");
		tmp_dict.Add("type", Variant.Type.Real);
		tmp_dict.Add("hint", PropertyHint.None);
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);

		tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/mode");
		tmp_dict.Add("type", Variant.Type.Int);
		tmp_dict.Add("hint", PropertyHint.Enum);
		tmp_dict.Add("hint_string", "follow_target,follow_animation");
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);


		tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/default_data/mass");
		tmp_dict.Add("type", Variant.Type.Real);
		tmp_dict.Add("hint", PropertyHint.None);
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);

		tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/default_data/drag");
		tmp_dict.Add("type", Variant.Type.Real);
		tmp_dict.Add("hint", PropertyHint.None);
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);


		tmp_dict = new Godot.Collections.Dictionary();
		tmp_dict.Add("name", "Velvet/joint_count");
		tmp_dict.Add("type", Variant.Type.Int);
		tmp_dict.Add("hint", PropertyHint.None);
		tmp_dict.Add("usage", PropertyUsageFlags.Default);
		list.Add(tmp_dict);

		// The Velvet Joints
		// ===================
		String curve_string = "Velvet/joint/";
		for (int i = 0; i < joint_count; i++) {
			tmp_dict = new Godot.Collections.Dictionary();
			tmp_dict.Add("name", curve_string + i.ToString() + "/twisted_bone");
			tmp_dict.Add("type", Variant.Type.NodePath);
			tmp_dict.Add("hint", PropertyHint.None);
			tmp_dict.Add("usage", PropertyUsageFlags.Default);
			list.Add(tmp_dict);

			tmp_dict = new Godot.Collections.Dictionary();
			tmp_dict.Add("name", curve_string + i.ToString() + "/override_defaults");
			tmp_dict.Add("type", Variant.Type.Bool);
			tmp_dict.Add("hint", PropertyHint.None);
			tmp_dict.Add("usage", PropertyUsageFlags.Default);
			list.Add(tmp_dict);

			if (velvet_joints[i].override_defaults == true) {
				tmp_dict = new Godot.Collections.Dictionary();
				tmp_dict.Add("name", curve_string + i.ToString() + "/mass");
				tmp_dict.Add("type", Variant.Type.Real);
				tmp_dict.Add("hint", PropertyHint.None);
				tmp_dict.Add("usage", PropertyUsageFlags.Default);
				list.Add(tmp_dict);

				tmp_dict = new Godot.Collections.Dictionary();
				tmp_dict.Add("name", curve_string + i.ToString() + "/drag");
				tmp_dict.Add("type", Variant.Type.Real);
				tmp_dict.Add("hint", PropertyHint.None);
				tmp_dict.Add("usage", PropertyUsageFlags.Default);
				list.Add(tmp_dict);
			}
		}

		return list;
	}

	private void set_joint_data_to_defaults() {
        for (int i = 0; i < velvet_joints.Length; i++) {
            VELVET_JOINT current_joint = velvet_joints[i];
            if (current_joint.override_defaults == false) {
                current_joint.mass = default_joint_mass;
                current_joint.drag = default_joint_drag;
                velvet_joints[i] = current_joint;
            }
        }
    }

	public override void _ExecuteModification(Twisted_ModifierStack3D modifier_stack, float delta)
	{
		base._ExecuteModification(modifier_stack, delta);

		if (velvet_mode == 0) {
			if (target_node == null) {
				GD.PrintErr("Cannot execute Velvet IK 3D: No target found!");
				return;
			}
		}

		for (int k = 0; k < 4; k++) {
			for (int i = 0; i < velvet_joints.Length; i++) {
				VELVET_JOINT current_joint = velvet_joints[i];

				if (current_joint.twisted_bone == null) {
					if (current_joint.path_to_twisted_bone != null) {
						current_joint.twisted_bone = GetNodeOrNull<Twisted_Bone3D>(current_joint.path_to_twisted_bone);
						if (current_joint.twisted_bone == null) {
							GD.PrintErr("Velvet 3D joint " + i.ToString() + " not setup. Skipping!");
							continue;
						}
						current_joint.last_transform = current_joint.twisted_bone.GlobalTransform;
						velvet_joints[i] = current_joint;
					}
					else {
						GD.PrintErr("Velvet 2D joint " + i.ToString() + " not setup. Skipping!");
						continue;
					}
				}

				Transform current_transform = current_joint.twisted_bone.GlobalTransform;

				// Change functionality based on the mode
				// MODE - follow target node
				if (velvet_mode == 0) {
					current_transform = current_joint.last_transform;

					// Set the acceleration to reach the target position
					Vector3 additional_accel = current_transform.origin.DirectionTo(target_node.GlobalTransform.origin) * (current_joint.twisted_bone.bone_length);
					if (additional_accel.LengthSquared() >= minimum_required_velocity) {
						current_joint.velvet_accel += additional_accel;
					}
				}
				// MODE - follow skeleton animation
				else if (velvet_mode == 1) {
					current_transform = current_joint.last_transform;

					Transform desired_trans = current_joint.twisted_bone.get_reset_bone_global_pose();
					Vector3 desired_bone_pos = desired_trans.origin + (-desired_trans.basis.z.Normalized() * current_joint.twisted_bone.bone_length);

					Vector3 current_bone_pos = current_transform.origin + (-current_transform.basis.z.Normalized() * current_joint.twisted_bone.bone_length);

					Vector3 additional_accel = (desired_bone_pos - current_bone_pos);
					if (additional_accel.LengthSquared() >= minimum_required_velocity) {
						current_joint.velvet_accel += additional_accel;
					}
				}

				// Velvet integration source credit: Wikipedia article
				// https://en.wikipedia.org/wiki/Verlet_integration
				Vector3 new_pos = current_transform.origin + current_joint.velvet_velocity * delta + current_joint.velvet_accel * (delta * delta * 0.5f);
				Vector3 new_accel = velvet_apply_forces(current_joint) * acceleration_speed;
				Vector3 new_vel = current_joint.velvet_velocity + (current_joint.velvet_accel + new_accel) * (delta * 0.5f);

				Vector3 direction_to_new_pos = current_transform.origin.DirectionTo(new_pos);
				if (direction_to_new_pos.LengthSquared() > minimum_required_velocity) {
                    Vector3 bone_up_dir = current_joint.twisted_bone.get_reset_bone_global_pose().basis.y.Normalized();
					current_joint.twisted_bone.LookAt(current_transform.origin + direction_to_new_pos, bone_up_dir);

					// Keep the scale consistent with the global pose
            		current_joint.twisted_bone.Scale = current_joint.twisted_bone.get_reset_bone_global_pose(false).basis.Scale;
				}

				current_joint.velvet_velocity = new_vel;
				current_joint.velvet_accel = new_accel;

				// Needed for motion following differences in position
				current_joint.last_transform = current_joint.last_transform.InterpolateWith(current_joint.twisted_bone.GlobalTransform, Mathf.Min(acceleration_speed * delta, 1.0f));

				velvet_joints[i] = current_joint;

                if (force_bone_application == true) {
                    current_joint.twisted_bone.force_apply_transform();
                }
			}
		}
	}

	public Vector3 velvet_apply_forces(VELVET_JOINT joint) {
		Vector3 drag_force = 0.5f * joint.drag * (joint.velvet_velocity * joint.velvet_velocity.Abs());
		Vector3 drag_accel = drag_force / joint.mass;
		return -drag_accel;
	}
}
