using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionBuilder : IBuilder<GameObject, GameObject>
{
    public GameObject Client
    {
        get; private set;
    }

    public string ClientName
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
        ClientName = Client.GetComponent<SpawnParameters>().Name;
        clientInitialRotation = p_client.transform.rotation;
        clientConnectionPoint = p_client_connection_point;
        ClientConnectionRotation = p_client_connection_rotation;
        Chance = p_chance;
    }

    public GameObject Build(GameObject Host)
    {
        var hostSpawn = Host.GetComponent<SpawnParameters>();      
        var hostWorldPoint = Host.transform.position + hostSpawn.Rotation * HostConnectionPoint;
        var turnRotation = Quaternion.AngleAxis(ClientConnectionRotation, Vector3.up);
        var obj = (GameObject)GameObject.Instantiate(Client, hostWorldPoint - hostSpawn.Rotation * turnRotation * ClientConnectionPoint, hostSpawn.Rotation * turnRotation * clientInitialRotation);
        var objSpawn = obj.GetComponent<SpawnParameters>();
        obj.transform.parent = Host.transform.parent;
        objSpawn.RotationY = hostSpawn.RotationY + ClientConnectionRotation;
        obj.SetActive(true);
        return obj;
    }
}