
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 RotateAroundPivot(this Vector3 initialPosition, Quaternion rotation, Vector3 pivotPoint)
    {
        return (rotation * (initialPosition - pivotPoint)) + pivotPoint;
    }

}
