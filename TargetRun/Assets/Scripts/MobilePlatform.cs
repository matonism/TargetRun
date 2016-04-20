using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpawnParameters))]
public class MobilePlatform : MonoBehaviour {

    private IEnumerable<GameObject> Next;
    private bool destroyed = false;

    public SpawnParameters Parameters
    {
        get; private set;
    }

    public void Initialize()
    {
        Parameters = GetComponent<SpawnParameters>();
    }

    public void Awake()
    {
        if(Parameters == null) { Initialize(); }
    }
	
	void Update ()
    {
        if (!destroyed)
        {
            this.transform.position = this.transform.position - (PlatformManager.SpeedVector * Time.deltaTime);

            if (PlatformManager.Player.transform.position.z + transform.position.z < PlatformManager.ZDestructionDistance /* || Mathf.Abs(PlatformManager.Player.transform.position.x - transform.position.x) > Mathf.Abs(PlatformManager.XDestructionDistance)*/)
            {
                GameObject.Destroy(this.gameObject);
                destroyed = true;
            }
            else if (Next == null && transform.position.z + PlatformManager.Player.transform.position.z < PlatformManager.CreationDistance)
            {
                Next = SpawnParameters.CreateConnection(this);
            }
        }
    }
}
