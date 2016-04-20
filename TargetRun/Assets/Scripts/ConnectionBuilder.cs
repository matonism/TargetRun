using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionBuilder : IBuilder<GameObject, GameObject>
{
    public GameObject Client
    {
        get; private set;
    }

    public Vector3 ClientConnectionPoint
    {
        get; private set;
    }

    public float ClientConnectionRotation
    {
        get; private set;
    }

    public Vector3 HostConnectionPoint
    {
        get; private set;
    }

    public float Chance
    {
        get; private set;
    }

    public ConnectionBuilder(Vector3 p_host_connection_point, GameObject p_client, Vector3 p_client_connection_point, float p_client_connection_rotation, float p_chance)
    {
        HostConnectionPoint = p_host_connection_point;
        Client = p_client;
        ClientConnectionPoint = p_client_connection_point;
        ClientConnectionRotation = p_client_connection_rotation;
        Chance = p_chance;
    }

    public GameObject Build(GameObject Host)
    {
        Vector3 hostPosition = Host.transform.position;
        var hostWorldPoint = hostPosition + HostConnectionPoint;
        var obj = (GameObject)GameObject.Instantiate(Client, hostWorldPoint - ClientConnectionPoint, Client.transform.rotation);
        obj.transform.parent = Host.transform.parent;
        obj.transform.RotateAround(Host.transform.position, Vector3.up, ClientConnectionRotation);
        obj.SetActive(true);
        return obj;
    }
}