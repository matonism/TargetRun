using UnityEngine;
using System.Collections;

public class PlayerKiller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(this.transform.position.y < 3.0f) { GameOverControl.SetGameOver(); }
	}
}
