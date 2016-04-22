using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpawnParameters))]
public class ObstacleSpawner : MonoBehaviour
{

    public Vector3 Rotation;
    public Vector3 Center;
    public Vector3 Size;
    public bool Randomize;
    public Vector3 SpawnPosition;
    public GameObject[] SpawnTypes;

    private SpawnParameters parameters;

    void Start()
    {
        if (Random.value < 0.4f)
        {
            parameters = GetComponent<SpawnParameters>();
            Quaternion rotation = Quaternion.Euler(Rotation);
            Vector3 halfSize = Size / 2.0f;
            Vector3 right = (rotation * Vector3.right);
            right.Normalize();
            Vector3 up = (rotation * Vector3.up);
            up.Normalize();
            Vector3 forward = (rotation * Vector3.forward);
            forward.Normalize();
            Vector3 rotatedHalfSize = right.MultiplyByElement(halfSize) + up.MultiplyByElement(halfSize) + forward.MultiplyByElement(halfSize);

            Vector3 lfl = transform.position + (parameters.Rotation * Center) - rotatedHalfSize;
            Vector3 ubr = transform.position + (parameters.Rotation * Center) + rotatedHalfSize;

            if (Randomize) { SpawnPosition = new Vector3(Random.Range(lfl.x, ubr.x), Random.Range(lfl.y, ubr.y), Random.Range(lfl.z, ubr.z)); }
            var spawn = SpawnTypes[Random.Range(0, SpawnTypes.Length)];
            var obj = (GameObject)GameObject.Instantiate(spawn, SpawnPosition, this.transform.rotation * spawn.transform.rotation);
            obj.transform.parent = this.transform;
        }
        GameObject.Destroy(this);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + Center + SpawnPosition, 0.25f);

        Quaternion rotation = Quaternion.Euler(Rotation);
        Vector3 halfSize = Size / 2.0f;
        Vector3 right = (rotation * Vector3.right);
        right.Normalize();
        Vector3 up = (rotation * Vector3.up);
        up.Normalize();
        Vector3 forward = (rotation * Vector3.forward);
        forward.Normalize();
        Vector3 rotatedHalfSize = right.MultiplyByElement(halfSize) + up.MultiplyByElement(halfSize) + forward.MultiplyByElement(halfSize);

        Vector3 lfl = transform.position + Center - rotatedHalfSize;
        Vector3 ubr = transform.position + Center + rotatedHalfSize;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lfl, 0.25f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(ubr, 0.25f);
        GizmosExtension.DrawCube(lfl, ubr, Color.red);
    }

}
