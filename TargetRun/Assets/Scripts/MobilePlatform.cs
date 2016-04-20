using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpawnParameters))]
public class MobilePlatform : MonoBehaviour {

    public Vector3 speedVector;
    public GameObject Player;
    public float DestructionDistance;
    public float CreationDistance;

    private IEnumerable<GameObject> Next;

    public SpawnParameters Parameters
    {
        get; private set;
    }

    public void Start()
    {
        Parameters = GetComponent<SpawnParameters>();
    }
	
	void FixedUpdate ()
    {
        this.transform.position = this.transform.position - (speedVector * Time.fixedDeltaTime);

        if(Player != null && Player.transform.position.z - transform.position.z > DestructionDistance)
        {
            GameObject.Destroy(gameObject);
        }
        else if(Next == null && transform.position.z - Player.transform.position.z < CreationDistance)
        {
            Next = SpawnParameters.CreateConnection(this);
        }
    }
}
