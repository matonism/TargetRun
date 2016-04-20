
using System;
using System.Collections.Generic;
using UnityEngine;


public class SpawnParameters : MonoBehaviour
{
    private static Dictionary<string, MultiConnectionBuilder> Connections = new Dictionary<string, MultiConnectionBuilder>();

    public static void AddConnectionBuilder(MobilePlatform platform)
    {
        if(platform.gameObject.activeInHierarchy) { throw new ArgumentException("Active connection builder prefabs are not allowed."); }
        SpawnParameters parameters = platform.Parameters;
        var builder = new MultiConnectionBuilder(platform.gameObject);
        foreach(var connection in parameters.Platforms)
        {
            foreach(var p in connection.Platforms)
            {
                if (p.Platform.gameObject.activeInHierarchy) { throw new ArgumentException("Active connection builder prefabs are not allowed."); }
                builder.AddClient(connection.HostExitConnection, new ConnectionBuilder(platform.gameObject, connection.HostExitConnection, p.Platform.gameObject, p.ClientEntranceConnection));
            }
        }
        Connections.Add(parameters.Name, builder);
        parameters.Platforms = null;
    }

    public static IEnumerable<GameObject> CreateConnection(MobilePlatform spawner)
    {
        return Connections[spawner.Parameters.Name].Build().Values;
    }

    public string Name;
    public Vector3 HostEntranceConnection;
    public ConnectionParameters[] Platforms;

    public void OnDrawGizmosSelected()
    {
        float size = 3.0f;
        Gizmos.color = Color.green;
        Vector3 startPos = this.transform.position + HostEntranceConnection;
        Gizmos.DrawRay(startPos, Vector3.forward * size);
        Gizmos.DrawRay(startPos, Vector3.up * size);
        Gizmos.DrawRay(startPos, Vector3.right * size);
        Gizmos.color = Color.red;

        if (Platforms == null)
        {
            Connections[Name].DrawGizoms(this.transform.position, size);
        }
        else
        {
            foreach (var connection in Platforms)
            {
                
                Vector3 endPos = this.transform.position + connection.HostExitConnection;
                Gizmos.DrawRay(endPos, Vector3.forward * size);
                Gizmos.DrawRay(endPos, Vector3.up * size);
                Gizmos.DrawRay(endPos, Vector3.right * size);
            }
        }
    }
}
