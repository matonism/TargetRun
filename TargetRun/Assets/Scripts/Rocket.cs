using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rocket : MonoBehaviour {

	public float speed = 10.0f;
	public Transform explosion;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {

		Destroy (this.gameObject, 1);
		Vector3 forceDirection = transform.forward * speed * 1.0f;
		rb.AddForce (forceDirection);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision col){
		Debug.Log (col.gameObject.name);
		Destroy (this.gameObject);
	
	}
		
}
