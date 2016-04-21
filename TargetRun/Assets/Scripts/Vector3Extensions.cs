
using UnityEngine;

public static class Vector3Extensions
{


    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        return (rotation * (point - pivot)) + pivot;
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 rotation)
    {
        return (Quaternion.Euler(rotation) * (point - pivot)) + pivot;
    }

    public static Vector3 MultiplyByElement(this Vector3 vec, Vector3 multiplier)
    {
        return new Vector3(vec.x * multiplier.x, vec.y * multiplier.y, vec.z * multiplier.z);
    }

}
