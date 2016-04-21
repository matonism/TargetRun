using UnityEngine;

public static class GizmosExtension
{
    public static void DrawCube(Vector3 lowerFrontLeft, Vector3 upperBackRight, Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, lowerFrontLeft.y, lowerFrontLeft.z), new Vector3(upperBackRight.x, lowerFrontLeft.y, lowerFrontLeft.z));
        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, lowerFrontLeft.y, lowerFrontLeft.z), new Vector3(lowerFrontLeft.x, lowerFrontLeft.y, upperBackRight.z));
        Gizmos.DrawLine(new Vector3(upperBackRight.x, lowerFrontLeft.y, lowerFrontLeft.z), new Vector3(upperBackRight.x, lowerFrontLeft.y, upperBackRight.z));
        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, lowerFrontLeft.y, upperBackRight.z), new Vector3(upperBackRight.x, lowerFrontLeft.y, upperBackRight.z));

        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, upperBackRight.y, lowerFrontLeft.z), new Vector3(upperBackRight.x, upperBackRight.y, lowerFrontLeft.z));
        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, upperBackRight.y, lowerFrontLeft.z), new Vector3(lowerFrontLeft.x, upperBackRight.y, upperBackRight.z));
        Gizmos.DrawLine(new Vector3(upperBackRight.x, upperBackRight.y, lowerFrontLeft.z), new Vector3(upperBackRight.x, upperBackRight.y, upperBackRight.z));
        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, upperBackRight.y, upperBackRight.z), new Vector3(upperBackRight.x, upperBackRight.y, upperBackRight.z));

        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, lowerFrontLeft.y, lowerFrontLeft.z), new Vector3(lowerFrontLeft.x, upperBackRight.y, lowerFrontLeft.z));
        Gizmos.DrawLine(new Vector3(lowerFrontLeft.x, lowerFrontLeft.y, upperBackRight.z), new Vector3(lowerFrontLeft.x, upperBackRight.y, upperBackRight.z));
        Gizmos.DrawLine(new Vector3(upperBackRight.x, lowerFrontLeft.y, lowerFrontLeft.z), new Vector3(upperBackRight.x, upperBackRight.y, lowerFrontLeft.z));
        Gizmos.DrawLine(new Vector3(upperBackRight.x, lowerFrontLeft.y, upperBackRight.z), new Vector3(upperBackRight.x, upperBackRight.y, upperBackRight.z));
    }

}
