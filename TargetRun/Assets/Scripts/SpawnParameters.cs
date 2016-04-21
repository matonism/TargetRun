
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class SpawnParameters : MonoBehaviour
{
    private static Dictionary<string, MultiConnectionBuilder> Connections = new Dictionary<string, MultiConnectionBuilder>();

    public static void AddConnectionBuilder(SpawnParameters parameters)
    {
        Debug.Log("Trying to Add Connection Builder: " + parameters.Name);
        //if(parameters.gameObject.activeSelf) { throw new ArgumentException("Active connection builder prefabs are not allowed."); }
        var platforms = parameters.Platforms;
        parameters.Platforms = null;
        var builder = new MultiConnectionBuilder(parameters.gameObject);
        foreach (var connection in platforms)
        {
            foreach (var p in connection.Platforms)
            {
                //if (p.Platform.gameObject.activeInHierarchy) { throw new ArgumentException("Active connection builder prefabs are not allowed."); }
                builder.AddClient(connection.HostExitConnection, p.Platform.gameObject, p.ClientEntranceConnection, connection.ExitRotation, p.Chance);
            }
        }
        Connections.Add(parameters.Name, builder);
        Debug.Log("Added ConnectionBuilder " + parameters.Name);
    }

    public static string Print()
    {
        StringBuilder builder = new StringBuilder("Keys Present: ");
        foreach (var c in Connections)
        {
            builder.Append(c.Key);
            builder.Append(", ");
        }
        return builder.ToString();
    }

    public static IEnumerable<GameObject> CreateConnection(MobilePlatform spawner)
    {
        MultiConnectionBuilder builder;
        if (!Connections.TryGetValue(spawner.Parameters.Name, out builder))
        {
            Print();
            throw new KeyNotFoundException("Could not find key " + spawner.Parameters.Name);
        }
        return builder.Build(spawner.gameObject).Values;
    }

    public string Name;
    public float RotationY;
    public Vector3 HostEntranceConnection;
    public ConnectionParameters[] Platforms;
    public string InfoDebug;

    public Quaternion Rotation
    {
        get
        {
            return Quaternion.AngleAxis(RotationY, Vector3.up);
        }
    }

    public void OnDrawGizmosSelected()
    {
        PlatformManager.DrawLocalAxis(this.transform.position, Rotation, 10.0f);

        float size = 3.0f;
        Gizmos.color = Color.green;
        Vector3 startPos = this.transform.position + Rotation * HostEntranceConnection;
        Gizmos.DrawRay(startPos, Rotation * Vector3.forward * size);
        Gizmos.DrawRay(startPos, Rotation * Vector3.up * size);
        Gizmos.DrawRay(startPos, Rotation * Vector3.right * size);
        Gizmos.color = Color.red;

        if (Platforms == null)
        {
            Connections[Name].DrawGizoms(this.transform.position, Rotation, size);
        }
        else
        {
            foreach (var connection in Platforms)
            {
                Vector3 endPos = this.transform.position + Rotation * connection.HostExitConnection;
                Gizmos.DrawRay(endPos, Rotation * Vector3.forward * size);
                Gizmos.DrawRay(endPos, Rotation * Vector3.up * size);
                Gizmos.DrawRay(endPos, Rotation * Vector3.right * size);
            }
        }
    }
}
