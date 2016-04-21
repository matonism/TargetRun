using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionBuilder : IBuilder<GameObject, GameObject>
{
    public GameObject Client
    {
        get; private set;
    }

    private Vector3 clientConnectionPoint;
    private Vector3 hostConnectionPoint;
    public Quaternion clientInitialRotation;

    public Vector3 ClientConnectionPoint
    {
        get
        {
            return clientConnectionPoint;
        }
    }

    public float ClientConnectionRotation
    {
        get; private set;
    }

    public Vector3 HostConnectionPoint
    {
        get
        {
            return hostConnectionPoint;
        }
    }

    public float Chance
    {
        get; private set;
    }

    public ConnectionBuilder(Vector3 p_host_connection_point, GameObject p_client, Vector3 p_client_connection_point, float p_client_connection_rotation, float p_chance)
    {
        hostConnectionPoint = p_host_connection_point;
        Client = p_client;
        clientInitialRotation = p_client.transform.rotation;
        clientConnectionPoint = p_client_connection_point;
        ClientConnectionRotation = p_client_connection_rotation;
        Chance = p_chance;
    }

    public GameObject Build(GameObject Host)
    {
        var hostSpawn = Host.GetComponent<SpawnParameters>();      
        Vector3 hostPosition = Host.transform.position;
        var hostWorldPoint = hostPosition + hostSpawn.Rotation * HostConnectionPoint;
        var obj = (GameObject)GameObject.Instantiate(Client, hostWorldPoint - ClientConnectionPoint, clientInitialRotation);
        var objSpawn = obj.GetComponent<SpawnParameters>();
        objSpawn.InfoDebug = SpawnParameters.Print();
        obj.transform.parent = Host.transform.parent;
        obj.transform.RotateAround(hostWorldPoint, Vector3.up, hostSpawn.RotationY + ClientConnectionRotation);
        objSpawn.RotationY = hostSpawn.RotationY + ClientConnectionRotation;
        obj.SetActive(true);
        return obj;
    }
}