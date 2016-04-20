﻿using System;
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

    public void AddClient(Vector3 hostPoint, ConnectionBuilder connection)
    {
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
            var vec = pair.Key;
            var list = pair.Value;
            int count = list.Count;
            int index = UnityEngine.Random.Range(0, count);
            while (list[index].Chance < UnityEngine.Random.value) { index = UnityEngine.Random.Range(0, count); }
            objs.Add(vec, list[index].Build(Host));
        }
        return objs;
    }

    public void DrawGizoms(Vector3 basePosition, float size)
    {
        foreach (var c in connections)
        {
            Vector3 endPos = basePosition + c.Key;
            Gizmos.DrawRay(endPos, Vector3.forward * size);
            Gizmos.DrawRay(endPos, Vector3.up * size);
            Gizmos.DrawRay(endPos, Vector3.right * size);
        }
    }
}