using Godot;
using System;

/// <summary>
/// The possible execution that Twisted_Modifier3D nodes can use.
/// </summary>
public enum TWISTED_IK_MODIFIER_MODES_3D {
    _PROCESS,
    _PHYSICS_PROCESS,
}

/// <summary>
/// The base class for all IK operations. This provides a constant, streamlined interface that allows
/// for mixing and matching IK and other Twisted_Bone3D-based operations.
/// </summary>
[Tool]
public class Twisted_Modifier3D : Spatial
{
    
    /// <summary>
    /// If <c>true</c>, this modifier will execute when the Twisted_ModifierStack3D executes.
    /// </summary>
    public bool execution_enabled = true;

    /// <summary>
    /// The execution mode this modifier uses. Currently, this is mostly to give some modifiers optional physics features.
    /// </summary>
    public TWISTED_IK_MODIFIER_MODES_3D execution_mode;
    /// <summary>
    /// If <c>true</c>, this modifier will override the Twisted_Bone3D settings when executing and force the Twisted_Bone3D node(s)
    /// it uses to apply their transform to the bones in the Skeleton.
    /// </summary>
    public bool force_bone_application = true;

    public override void _Ready()
    {
        if (IsInsideTree() == true) {
            if (GetParentOrNull<Twisted_ModifierStack3D>() != null) {
                GetParentOrNull<Twisted_ModifierStack3D>().update_modification_children_array();
            }
        }
    }

    /// <summary>
    /// This is where the modifier is actually executed! Any code in this function will be called when the
    /// Twisted_ModifierStack3D is executing.
    /// 
    /// This function is a virtual stub and is intended to be overriden with Twisted_Bone3D functionality!
    /// </summary>
    /// <param name="modifier_stack"></param>
    /// <param name="delta"></param>
    public virtual void _ExecuteModification(Twisted_ModifierStack3D modifier_stack, float delta) {
        return;
    }

    public override bool _Set(string property, object value)
    {
        if (property == "base_settings/execution_enabled") {
            execution_enabled = (bool)value;
            return true;
        }
        else if (property == "base_settings/execution_mode") {
            execution_mode = (TWISTED_IK_MODIFIER_MODES_3D)value;
            PropertyListChangedNotify();
            return true;
        }
        else if (property == "base_settings/force_bone_application") {
            force_bone_application = (bool)value;
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
        if (property == "base_settings/execution_enabled") {
            return execution_enabled;
        }
        else if (property == "base_settings/execution_mode") {
            return execution_mode;
        }
        else if (property == "base_settings/force_bone_application") {
            return force_bone_application;
        }

        try {
            return base._Get(property);
        } catch {
            return false;
        }
    }

    public override Godot.Collections.Array _GetPropertyList()
    {
        Godot.Collections.Array list = new Godot.Collections.Array();
        Godot.Collections.Dictionary tmp_dict;

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "base_settings/execution_enabled");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "base_settings/execution_mode");
        tmp_dict.Add("type", Variant.Type.Int);
        tmp_dict.Add("hint", PropertyHint.Enum);
        tmp_dict.Add("hint_string", "_Process, _Physics_Process");
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        tmp_dict = new Godot.Collections.Dictionary();
        tmp_dict.Add("name", "base_settings/force_bone_application");
        tmp_dict.Add("type", Variant.Type.Bool);
        tmp_dict.Add("hint", PropertyHint.None);
        tmp_dict.Add("usage", PropertyUsageFlags.Default);
        list.Add(tmp_dict);

        return list;
    }

}
