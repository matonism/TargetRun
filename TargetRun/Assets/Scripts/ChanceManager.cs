using System.Collections.Generic;
using UnityEngine;

public class ChanceManager : MonoBehaviour
{
    private static ChanceManager manager;

    public DistanceLimiting[] DistanceLimiting;
    private Dictionary<string, float> distanceLimit = new Dictionary<string, float>();
    private Dictionary<string, float> lastBuild = new Dictionary<string, float>();
    private float traveled;

    public static GameObject Build(GameObject host, List<ConnectionBuilder> list)
    {
        int count = list.Count;
        int index = UnityEngine.Random.Range(0, count);
        while (list[index].Chance < UnityEngine.Random.value && manager.traveled - manager.lastBuild[list[index].ClientName] > manager.distanceLimit[list[index].ClientName]) { index = UnityEngine.Random.Range(0, count); }
        var builder = list[index];
        manager.lastBuild.Remove(builder.ClientName);
        manager.lastBuild.Add(builder.ClientName, manager.traveled);
        return builder.Build(host);
    }

    public static void RegisterTravel(float travel)
    {
        manager.traveled += travel;
    }

    public void Awake()
    {
        if(manager == null) { manager = this; }
        foreach(var pair in DistanceLimiting)
        {
            distanceLimit.Add(pair.name, pair.distance);
            lastBuild.Add(pair.name, traveled);
        }
    }
}
