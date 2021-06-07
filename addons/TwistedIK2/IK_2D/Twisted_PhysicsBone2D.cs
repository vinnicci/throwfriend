using Godot;
using System;

[Tool]
public class Twisted_PhysicsBone2D : RigidBody2D
{
    /// <summary>
    /// A NodePath to the Twisted_Bone2D that this physics bone should apply its result to
    /// </summary>
    public NodePath path_to_twisted_bone2d;
    /// <summary>
    /// The Twisted_Bone2D that this physics bone should apply its result to
    /// </summary>
    public Twisted_Bone2D twisted_bone_2d;

    /// <summary>
    /// If <c>true</c>, then the result of the RigidBody2D physics will be applied to the given Twisted_Bone2D
    /// </summary>
    public bool apply_to_bone = true;
    /// <summary>
    /// If <c>true</c>, then this node will follow the Twisted_Bone2D node until it starts simulating physics
    /// </summary>
    public bool follow_bone_transform = true;

    private bool simulate_physics = false;
    private bool simulate_physics_on_ready = false;
    private bool _internal_simulate_physics = false;
    private float internal_delta = 0.0f;
    private Transform2D internal_transform = new Transform2D();

    /// <summary>
    /// A Vector2 representing the offset from the origin of the bone to the center of the RigidBody.
    /// Rigidbody nodes use the origin of the RigidBody2D as the center of mass, but the center of mass in a bone
    /// is likely not at the joint connection. This offset allows you to position the RigidBody center of mass correctly.
    /// </summary>
    public Vector2 origin_offset = Vector2.Zero;
    /// <summary>
    /// A Vector2 representing the scale that the Twisted_Bone2D has. This allows the Twisted_PhysicsBone2D to be scaled and still
    /// work correctly with Godot's physics engine. If you're Skeleton has a scale of 4, you will need to put (4, 4) for it
    /// to work correctly. This works around the Godot scaled RigidBody2D limitation.
    /// </summary>
    public Vector2 scale_offset = Vector2.One;

    public override void _Ready()
    {
        if (path_to_twisted_bone2d != null) {
            twisted_bone_2d = GetNodeOrNull<Twisted_Bone2D>(path_to_twisted_bone2d);
        }
        _internal_simulate_physics = false;

        if (simulate_physics_on_ready == true && Engine.EditorHint == false) {
            start_physics_simulation();
        }
    }

    public override void _Notification(int what)
    {
        base._Notification(what);
        if (what == NotificationEnterTree) {
            // Connect the post-process signal
            GetTree().Connect("physics_frame", this, "on_physics_frame");
        }
        else if (what == NotificationExitTree) {
            // Disconnect the post-process signal
            GetTree().Disconnect("physics_frame", this, "on_physics_frame");
        }
    }

    public override bool _Set(string property, object value)
    {
        if (property == "twisted_bone2d") {
            path_to_twisted_bone2d = (NodePath)value;
            if (path_to_twisted_bone2d != null) {
                twisted_bone_2d = GetNodeOrNull<Twisted_Bone2D>(path_to_twisted_bone2d);
            }
            return true;
        }
        else if (property == "apply_to_bone") {
            apply_to_bone = (bool)value;
            return true;
        }
        else if (property == "follow_bone_transform") {
            follow_bone_transform = (bool)value;
            return true;
        }
        else if (property == "origin_offset") {
            origin_offset = (Vector2)value;
            return true;
        }
        else if (property == "scale_offset") {
            scale_offset = (Vector2)value;
            return true;
        }
        else if (property == "simulate_physics") {
            simulate_physics = (bool)value;

            if (Engine.EditorHint == false) {
                set_simulate_physics(simulate_physics);
            }
            
            return true;
        }
        else if (property == "simulate_physics_on_ready") {
            simulate_physics_on_ready = (bool)value;
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
        if (property == "twisted_bone2d") {
            return path_to_twisted_bone2d;
        }
        else if (property == "apply_to_bone") {
            return apply_to_bone;
        }
        else if (property == "follow_bone_transform") {
            return follow_bone_transform;
        }
        else if (property == "origin_offset") {
            return origin_offset;
        }
        else if (property == "scale_offset") {
            return scale_offset;
        }
        else if (property == "simulate_physics") {
            return simulate_physics;
        }
        else if (property == "simulate_physics_on_ready") {
            return simulate_physics_on_ready;
        }
        
        try {
            return base._Get(property);
        } catch {
            return null;
        }
    }

    public override Godot.Collections.Array _GetPropertyList()
    {
        Godot.Collections.Array list = new Godot.Collections.Array();
        Godot.Collections.Dictionary tmp_dict;

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "twisted_bone2d");
        tmp_dict.Add("type", Variant.Type.NodePath);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "apply_to_bone");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "follow_bone_transform");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "origin_offset");
        tmp_dict.Add("type", Variant.Type.Vector2);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "scale_offset");
        tmp_dict.Add("type", Variant.Type.Vector2);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "simulate_physics");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "simulate_physics_on_ready");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        tmp_dict.Add("default", true);
        list.Add(tmp_dict);

        return list;
    }


    /// <summary>
    /// Internally used to set whether the Twisted_PhysicsBone2D is simulating physics using Godot's physics engine.
    /// This turns on/off the RigidBody2D part of this node.
    /// </summary>
    /// <param name="physics_state"></param>
    public void set_simulate_physics(bool physics_state) {
        if (physics_state == true) {
            start_physics_simulation();
        } else {
            stop_physics_simulation();
        }
    }

    /// <summary>
    /// Starts the physics simulation on the RigidBody2D aspect of this node.
    /// This function should NOT be used directly. Instead, please use <c>set_simulate_physics(true)</c> instead!
    /// </summary>
    public void start_physics_simulation() {
        if (_internal_simulate_physics == true) {
            return;
        }

        if (follow_bone_transform) {
            retrieve_transform_from_bone();
        }

        // Let the RigidBody execute its force integration
        Physics2DServer.BodySetForceIntegrationCallback(GetRid(), this, "_direct_state_changed");

        // Set the physics mode to the mode this RigidBody uses
        if (Mode == ModeEnum.Rigid) {
            Physics2DServer.BodySetMode(GetRid(), Physics2DServer.BodyMode.Rigid);
        }
        else if (Mode == ModeEnum.Kinematic) {
            Physics2DServer.BodySetMode(GetRid(), Physics2DServer.BodyMode.Kinematic);
        }
        else if (Mode == ModeEnum.Character) {
            Physics2DServer.BodySetMode(GetRid(), Physics2DServer.BodyMode.Character);
        }
        else if (Mode == ModeEnum.Static) {
            Physics2DServer.BodySetMode(GetRid(), Physics2DServer.BodyMode.Static);
        }
        else {
            Physics2DServer.BodySetMode(GetRid(), Physics2DServer.BodyMode.Rigid);
        }
        
        // Apply collision layer(s) and mask(s)
        Physics2DServer.BodySetCollisionLayer(GetRid(), CollisionLayer);
        Physics2DServer.BodySetCollisionMask(GetRid(), CollisionMask);
        
        _internal_simulate_physics = true;
    }

    /// <summary>
    /// Stops the physics simulation on the RigidBody2D aspect of this node.
    /// This function should NOT be used directly. Instead, please use <c>set_simulate_physics(false)</c> instead!
    /// </summary>
    public void stop_physics_simulation() {
        if (_internal_simulate_physics == false) {
            return;
        }

        // Stop the RigidBody from executing its force integration
        Physics2DServer.BodySetForceIntegrationCallback(GetRid(), null, "");
        // Set the RigidBody mode to static, to avoid physics calculations internally
        Physics2DServer.BodySetMode(GetRid(), Physics2DServer.BodyMode.Static);

        // Remove the collision layer(s) and mask(s)
        Physics2DServer.BodySetCollisionLayer(GetRid(), 0);
        Physics2DServer.BodySetCollisionMask(GetRid(), 0);

        if (follow_bone_transform) {
            retrieve_transform_from_bone();
        }

        _internal_simulate_physics = false;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (IsInsideTree() == false) {
            return;
        }
        internal_delta = delta;
    }

    public override void _Process(float delta)
    {
        if (_internal_simulate_physics == true) {
            if (apply_to_bone == true && twisted_bone_2d != null) {
                twisted_bone_2d.set_executing_ik(true);
                twisted_bone_2d.GlobalTransform = internal_transform;
            }
        }
    }

    
    /// <summary>
    /// Called at the end of each <c>_physics_process</c> call. Used to apply post-processing.
    /// </summary>
    public void on_physics_frame() {
        if (IsInsideTree() == false || twisted_bone_2d == null) {
            return;
        }
        _On_Post_Process(internal_delta);
    }

    /// <summary>
    /// Called after <c>_physics_process</c> has been called on the node. This function is directly responsible for
    /// applying the node's Transform to the Twisted_Bone2D.
    /// 
    /// This is also where the Twisted_Bone2D's Transform2D is applied to this node, if this node is following the
    /// transform of the Twisted_Bone2D it's attached to.
    /// </summary>
    /// <param name="delta">The amount of time that has elapsed</param>
    public virtual void _On_Post_Process(float delta) {
        if (twisted_bone_2d == null) {
            if (path_to_twisted_bone2d != null) {
                twisted_bone_2d = GetNodeOrNull<Twisted_Bone2D>(path_to_twisted_bone2d);
            }
            if (twisted_bone_2d == null) {
                return;
            }
        }

        if (_internal_simulate_physics == false) {
            if (follow_bone_transform == true) {
                retrieve_transform_from_bone();
            }
        }
        else {
            if (apply_to_bone == true) {
                apply_transform_to_bone();
            }
        }
    }


    /// <summary>
    /// Sets the Transform of the Twisted_Bone2D to the Transform of the Twisted_PhysicsBone2D node.
    /// </summary>
    public void apply_transform_to_bone() {
        if (twisted_bone_2d == null) {
            return;
        }

        Transform2D transform_to_apply = GlobalTransform;

        // Adjust for the offset
        transform_to_apply.origin -= origin_offset.Rotated(Rotation);

        // Adjust for the scale offset
        if (scale_offset.LengthSquared() != 0) {
            transform_to_apply.Scale = transform_to_apply.Scale * scale_offset;
        }
        // Cache the transform. We cannot apply it here because Twisted_Bone2D nodes
        // can only have their IK position set in _process.
        internal_transform = transform_to_apply;
    }

    /// <summary>
    /// Gets the Transform of the Twisted_Bone2D and applies it to the Twisted_PhysicsBone2D node.
    /// </summary>
    public void retrieve_transform_from_bone() {
        if (twisted_bone_2d == null) {
            return;
        }

        Transform2D bone_transform = twisted_bone_2d.GlobalTransform;
        
        // Adjust for scale offset
        if (scale_offset.LengthSquared() != 0) {
            bone_transform.Scale = bone_transform.Scale * (scale_offset.Inverse());
        }

        // Adjust for the offset
        bone_transform.origin += origin_offset.Rotated(bone_transform.Rotation);

        // Apply to self
        GlobalTransform = bone_transform;

        // Adjust the Rigidbody forces (since we scaled things)
        if (scale_offset.LengthSquared() != 0) {
            LinearVelocity = LinearVelocity * scale_offset.Inverse();
            AngularVelocity = AngularVelocity * scale_offset.Inverse().x; // We'll assume uniform scales
        }
    }


    /// <summary>
    /// This is a fun little function that rotates a RigidBody2D-based node to look at a target position.
    //
    /// If you are looking to control a set of bones, please use the <c>Twisted_PhysicsBoneMotor2D</c> node, as otherwise your
    /// bone chain will have wiggly movement issues!
    /// </summary>
    /// <param name="target_position">The target position, in global space, that you want to rotate the RigidBody to look at</param>
    /// <param name="look_at_strength">The amount of force you want to apply to the rotation</param>
    public void look_at_target(Vector2 target_position, float look_at_strength) {
        Vector2 desired_direction = -GlobalPosition.DirectionTo(target_position);
        Vector2 current_direction = new Vector2(Mathf.Cos(GlobalRotation), Mathf.Sin(GlobalRotation));
        AngularVelocity = current_direction.Dot(desired_direction) * look_at_strength;
    }

}