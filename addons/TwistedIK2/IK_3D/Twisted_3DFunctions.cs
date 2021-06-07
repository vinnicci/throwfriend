using Godot;
using System;

///<summary>
/// A helper class that has several helper functions for 3D use.
///</summary>
public static class Twisted_3DFunctions {

    ///<summary>
    /// Used to get the Quat offset from one direction to the desired direction. The returned Quat will
    /// only calculate the swing needed to get the input direction to the desired direction, unlike <c>look_at</c>
    /// which will use twist rotation in addition to swing.
    /// </summary>
    /// <returns>
    /// Returns a Quat that is the offset needed to get the <paramref name="start_direction" /> to the
    /// <paramref name = "end_direction" />. This function just returns the offset required, the returned
    /// Quat is not the rotation itself.
    /// </returns>
    ///<param name="start_direction">The direction the Transform is currently facing in</param>
    ///<param name="end_direction">The direction you want the Transform to face</param>
    public static Quat quat_from_two_vectors(Vector3 start_direction, Vector3 end_direction) {
        Quat return_quat = Quat.Identity;

        Vector3 v0 = start_direction.Normalized();
        Vector3 v1 = end_direction.Normalized();
        float d = v0.Dot(v1);

        if (d >= 1.0f) {
            return Quat.Identity;
        }

        if (d < (1e-6 - 1.0)) {
            // Generate an axis (TODO: Add fallback support?)
            Vector3 axis = Vector3.Right.Cross(start_direction);
            if (axis.LengthSquared() == 0) {
                axis = Vector3.Up.Cross(start_direction);
            }
            axis = axis.Normalized();
            return_quat = new Quat(axis, Mathf.Pi);
        } 
        else
        {
            float s = Mathf.Sqrt((1+d) * 2);
            float invs = 1 / s;

            Vector3 c = v0.Cross(v1);

            return_quat.x = c.x * invs;
            return_quat.y = c.y * invs;
            return_quat.z = c.z * invs;
        }

        return return_quat.Normalized();
    }

    /// <summary>
    /// Works the same way as the LookAt function, but it uses quaternions instead of reconstructing the Basis.
    /// The advantage of doing it this way is that it doesn't twist to get to the desired position, it only uses swing rotation.
    /// </summary>
    /// <param name="input_trans">The transform you want to rotate</param>
    /// <param name="target_position">The point (in global space) that the transform rotates to</param>
    /// <returns>A transform where the negative Z axis faces the target position passed</returns>
    public static Transform quat_based_lookat(Transform input_trans, Vector3 target_position) {
        Transform output_trans = input_trans;
        Vector3 input_to_target_dir = input_trans.origin.DirectionTo(target_position);
        Quat rotation_quat = quat_from_two_vectors(-input_trans.basis.z.Normalized(), input_to_target_dir);
        output_trans.basis = new Basis(rotation_quat * get_rotation_quat(input_trans.basis));
        return output_trans;
    }

    /// <summary>
    ///     Returns the input Basis and converts it to a Quat. Identical to the <c>get_rotation_quat</c> function in GDScript.
    /// </summary>
    /// <returns>
    ///     Returns the Basis converted to a Quat.
    ///     This is different than the <c>Basis.Quat()</c> function, this function has some extra features
    ///     that handle special conditions in the Basis.
    ///     (This code is ported from the Godot C++ source, as for some reason C# doesn't include it by default)
    /// </returns>
    /// <param name="input">The Basis that you want in Quat form</param>
    public static Quat get_rotation_quat(Basis input) {
        Basis m = input.Orthonormalized();
        float det = m.Determinant();
        if (det < 0) {
            m = m.Scaled( new Vector3(-1, -1, -1));
        }

        return m.Quat();
    }
}