using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionBuilder : IBuilder<GameObject>
{
    private GameObject host;
    private Vector3 hostConnectionPoint;
    private GameObject client;
    private Vector3 clientConnectionPoint;

    public GameObject Host
    {
        get
        {
            return host;
        }
    }

    public Vector3 HostConnectionPoint
    {
        get
        {
            return hostConnectionPoint;
        }
    }

    public GameObject Client
    {
        get
        {
            return client;
        }
    }

    public Vector3 ClientConnectionPoint
    {
        get
        {
            return clientConnectionPoint;
        }
    }

    public ConnectionBuilder(GameObject p_host, Vector3 p_host_connection_point, GameObject p_client, Vector3 p_client_connection_point)
    {
        host = p_host;
        hostConnectionPoint = p_host_connection_point;
        client = p_client;
        clientConnectionPoint = p_client_connection_point;
    }

    public GameObject Build()
    {
        Vector3 hostPosition = Host.transform.position;
        var hostWorldPoint = hostPosition + hostConnectionPoint;
        var obj = (GameObject)GameObject.Instantiate(client, hostWorldPoint - clientConnectionPoint, client.transform.rotation);
        obj.SetActive(true);
        return obj;
    }
}