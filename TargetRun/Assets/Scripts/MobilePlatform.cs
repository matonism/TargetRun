using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpawnParameters))]
public class MobilePlatform : MonoBehaviour
{
    private static bool initialized = false;
    private static int mask;

    private IEnumerable<GameObject> Next;

    public SpawnParameters Parameters
    {
        get; private set;
    }

    public void Awake()
    {
        if (Parameters == null)
        {
            Parameters = GetComponent<SpawnParameters>();
        }
        if (!initialized)
        {
            initialized = true;
            mask = LayerMask.GetMask("Player");
        }
    }

    void Update()
    {
        this.transform.position = this.transform.position - (PlatformManager.SpeedVector * Time.deltaTime);

        if (Next == null && Vector3.Distance(transform.position, PlatformManager.Player.transform.position) < PlatformManager.CreationDistance)
        {
            Next = SpawnParameters.CreateConnection(this);
        }

        if(PlatformManager.Player.transform.position.z + transform.position.z < PlatformManager.DestructionDistance)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
