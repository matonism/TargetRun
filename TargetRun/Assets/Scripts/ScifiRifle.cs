using UnityEngine;
using System.Collections;

public class ScifiRifle :  Gun {

	void Awake(){
		base.accuracy = 15.0f;
		base.aimAccuracy = 20.0f;
		base.aimSpeed = 2.0f;
		base.reloadSpeed = 1.0f;
	}


	void Start () {
		//Initialize ammo, timer, position of edgeOfBarrel
		base.InitializeGun ();
//		base.aimPositionOffset = new Vector3 (0.005f, -0.115f, -0.03f);
		base.aimPositionOffset = new Vector3 (0.005f, -0.115f, -0.5f);


	}

	// Update is called once per frame
	void Update () {
		// Update the timer to control fire rate
		base.UpdateTimer ();

		//Since ScifiRifle is automatic, use automatic controls
		base.SingleShotControl ();

	}


	public override string getWeaponName(){
		return "Scifi Rifle";
	}
}