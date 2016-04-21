using System;

[Serializable()]
public struct DistanceLimiting
{
    public string name;
    public float distance;

    public DistanceLimiting(string p_name, float p_distance)
    {
        name = p_name;
        distance = p_distance;
    }
}
