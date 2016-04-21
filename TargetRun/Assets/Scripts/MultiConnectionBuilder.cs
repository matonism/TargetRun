using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiConnectionBuilder : IBuilder<Dictionary<Vector3, GameObject>, GameObject>
{
    public GameObject Host
    {
        get; private set;
    }

    //Connection points with possible connecting pieces.
    private Dictionary<Vector3, List<ConnectionBuilder>> connections = new Dictionary<Vector3, List<ConnectionBuilder>>();

    public MultiConnectionBuilder(GameObject p_host)
    {
        Host = p_host;
    }

    public void AddClient(Vector3 hostPoint, GameObject client, Vector3 clientConnectionPoint, float clientConnectionRotation, float chance)
    {
        var connection = new ConnectionBuilder(hostPoint, client, clientConnectionPoint, clientConnectionRotation, chance);
        if (connection == null)
        {
            throw new ArgumentException("Invalid use of null connection argument for connection list.");
        }

        List<ConnectionBuilder> builders;
        if (!connections.TryGetValue(hostPoint, out builders))
        {
            builders = new List<ConnectionBuilder>();
            connections.Add(hostPoint, builders);
        }

        builders.Add(connection);
    }

    public Dictionary<Vector3, GameObject> Build(GameObject Host)
    {
        var objs = new Dictionary<Vector3, GameObject>(connections.Count);
        foreach (var pair in connections)
        {
            objs.Add(pair.Key, ChanceManager.Build(Host, pair.Value));
        }
        return objs;
    }

    public void DrawGizoms(Vector3 basePosition, Quaternion rotation, float size)
    {
        foreach (var c in connections)
        {
            Vector3 endPos = basePosition + rotation * c.Key;
            Gizmos.DrawRay(endPos, rotation * Vector3.forward * size);
            Gizmos.DrawRay(endPos, rotation * Vector3.up * size);
            Gizmos.DrawRay(endPos, rotation * Vector3.right * size);
        }
    }
}