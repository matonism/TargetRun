using UnityEngine;
using System.Collections;

public class Gun : Weapon {

	[Header("Effects")]
	public Transform bulletSpark;
	public Transform gunSpark;
	public Transform bullet;

	[Header("Gun Metrics")]
	public float fireRateFPS = 1.0f;
	public int ammo = 100;
	public int clipSize = 50;
	public float range = 100.0f;
	//Moved Damage to Weapon
	//public float damage = 1.0f;
	public float reloadSpeed = 5.0f;
	//Accuracy 1 - 25
	[Range(0.0f, 25.0f)]public float accuracy = 5.0f;
	[Range(0.0f, 25.0f)]public float aimAccuracy = 10.0f;

	private float maxAccuracy = 25.0f;
	private int ammoInClip;
	private float timer;
	private float timeDelayBetweenShots = 0.0f;
	private Transform edgeOfBarrel;
	private bool firing = false;
	private bool reloading = false;
	private float reloadingTime = 0.0f;

	protected void InitializeGun(){
		edgeOfBarrel = transform.FindChild ("FiringPoint");
		timer = 0.0f;
		if(ammo > clipSize){
			ammoInClip = clipSize;
			ammo -= clipSize;
		}else{
			ammoInClip = ammo;
			ammo = 0;
		}

		base.initializeForHeldOrDropped ();
	}

	//Logic to control a non-automatic gun 
	protected void SingleShotControl(){
		if (isHeld ()) {
			if (reloadingTime < timer) {reloading = false;}

			if (Input.GetButtonDown (Controls.SHOOT) && ammoInClip > 0 && !reloading) {
				Fire (1);
			}

			if (Input.GetButtonDown (Controls.RELOAD)) {
				Reload ();
			}
		}
	}

	//Logic to control a buckshot
	protected void BuckShotControl(){

		if (isHeld ()) {

			if (reloadingTime < timer) {reloading = false;}

			if (Input.GetButtonDown (Controls.SHOOT) && ammoInClip > 0 && !reloading) {Fire (8);}

			if (Input.GetButtonDown (Controls.RELOAD)) {Reload ();}
		}

	}

	//Logic to control an automatic gun 
	protected void AutomaticControl(){
		if (isHeld ()) {

			if (reloadingTime < timer) {reloading = false;}

			if (Input.GetButtonDown (Controls.SHOOT) && ammoInClip > 0 && !reloading) {
				firing = true;
			}


			if (Input.GetButtonUp (Controls.SHOOT) || ammoInClip == 0) {
				firing = false;

			}

			if (firing) {
				Fire (1);
			}

			if (Input.GetButtonDown (Controls.RELOAD)) {Reload ();}

		}
	}


	protected void Fire(int numOfShots){
		//    	this._crosshair.StartFire();
		//		if (ammoInClip > 0) {
		if ((((timer - timeDelayBetweenShots) > (1.0f / fireRateFPS) && ammoInClip > 0))) {

			if (GetComponentInChildren<Animator> () != null) {
				var parameters = GetComponentInChildren<Animator> ().parameters;
				foreach (var p in parameters) {
					if (p.name == "fire")
						GetComponentInChildren<Animator> ().SetTrigger ("fire");

				}
			}

			if (gunSpark) {
				Transform gunSparkClone = Instantiate (gunSpark, edgeOfBarrel.position, edgeOfBarrel.rotation) as Transform;
				gunSparkClone.parent = this.edgeOfBarrel;
				Destroy (gunSparkClone.gameObject, .5f);
			}

			for (int i = 0; i < numOfShots; i++) {
				ShootRay ();
			}

			if (GetComponent<AudioSource> ()) {
				GetComponent<AudioSource> ().Play ();
			}
			ammoInClip--;
			timeDelayBetweenShots = timer;

		}

	}

	protected void Reload(){
		if (ammoInClip < clipSize && ammo > 0  && isHeld()) {
			reloading = true;
			//Checks for reload animation and applies it if present
			Animator ani = GetComponentInChildren<Animator>();
			if (ani != null) {
				var parameters = ani.parameters;
				foreach (var p in parameters) {
					if (p.name == "Reload") {
						var clips = ani.runtimeAnimatorController.animationClips;
						var initialTime = clips [0].length;
						var newSpeed = 1/reloadSpeed; //getReloadSpeed();

						//1.7 is the time it will take
						//run at 1.7 * .3 times the speed
						float multiplier = initialTime * reloadSpeed; //getReloadSpeed ();
						ani.SetFloat ("reloadSpeed", multiplier);
						ani.SetTrigger ("Reload");

						//						var clips = ani.runtimeAnimatorController.animationClips;
						//						var animationTime = clips [0].length / newSpeed;
						reloadingTime = timer + newSpeed;


					}
				}
			} else {
				reloadingTime = reloadSpeed; //getReloadSpeed ();
			}

			//Updates ammunition
			if (ammo > clipSize - ammoInClip) {
				ammo -= clipSize - ammoInClip;
				ammoInClip += clipSize - ammoInClip;
			} else {
				ammoInClip += ammo;
				ammo = 0;
			}
		}

	}

	//Updates the timer that controls fire rate
	protected void UpdateTimer(){
		timer += Time.deltaTime;
	}

	protected virtual void ShootRay() {
		Transform weaponContainer = transform.parent;
		float missRadius = 0.1f; //getMissRadius (weaponContainer);

		//  Generate a random XY point inside a circle:
		Vector3 direction = Random.insideUnitCircle * missRadius;
		//direction.y += 1;
		direction.z = 50.0f; // circle is at Z units 
//		direction = transform.TransformDirection( direction );
		direction = edgeOfBarrel.TransformDirection( direction );


		//Raycast and debug
//		Ray r = new Ray( transform.parent.position, direction );
		Ray r = new Ray( edgeOfBarrel.position, direction );

		if (bullet) {
			Transform bulletClone = Instantiate (bullet, edgeOfBarrel.transform.position, transform.rotation) as Transform;
			bulletClone.parent = edgeOfBarrel;
			Rigidbody rb = bulletClone.GetComponent<Rigidbody> ();
			rb.velocity = direction * 5;
			Destroy(bulletClone.gameObject, .5f);
		}

//		Debug.DrawLine( transform.position, direction); 

		RaycastHit hit;     
		if( Physics.Raycast( r, out hit, range ) ) {
			Debug.DrawLine( transform.position, hit.point ); 
			Debug.Log (hit.collider.name);
			string hitName = hit.collider.name.ToUpper();
			Transform bulletSparkClone = Instantiate (bulletSpark, hit.point, Quaternion.LookRotation (hit.normal)) as Transform;
			Destroy (bulletSparkClone.gameObject, 4.0f);
			Rigidbody hitBody = hit.collider.GetComponent<Rigidbody> ();
			if (hitBody) {
				hitBody.AddForce (direction.normalized * 500.0f);
			}
			PhysicsController_Child breakTarget;
			if ((breakTarget = hit.collider.gameObject.GetComponent<PhysicsController_Child> ()) != null) {
				var inO = breakTarget.GetComponents<MeshCollider> ();
				foreach (MeshCollider meshc in inO) {
					meshc.enabled = false;
				}

				var underO = breakTarget.GetComponentsInChildren<MeshCollider> ();
				foreach (MeshCollider meshc in underO){
					meshc.enabled = false;
				}

                hit.collider.gameObject.transform.parent.gameObject.GetComponent<BoxCollider>().enabled = false;
                Debug.Log("Found Collider.");
                breakTarget.breakObject(true, new Vector3(0,0,0));
				Destroy (hit.collider.gameObject, 2);
			}
		} 
	}

//	protected float getMissRadius(Transform weaponContainer){
//		float currentAccuracy = accuracy;
//		Characteristics characteristics = weaponContainer.GetComponentInParent<Characteristics> ();
//		if(isAiming()){
//			currentAccuracy = aimAccuracy;
//		}
//		float maxThreshold = maxAccuracy - currentAccuracy;
//		float x = characteristics._perception;
//
//		float perceptionEffect = (maxThreshold * Mathf.Pow (x, 1.1f) + x) / (Mathf.Pow (x, 1.1f) + 40);
//		currentAccuracy += perceptionEffect;
//
//		if (maxAccuracy - currentAccuracy < 0) {
//			return 0.1f;
//		}
//
//		float foreignWeaponPenalty = 0.0f;
//		if (isForeignWeapon ()) {
//			foreignWeaponPenalty = getForeignWeaponEffect ();
//		}
//		return maxAccuracy - currentAccuracy + foreignWeaponPenalty;
//	}



	public override string displayAmmoRemaining(){
		return ammo.ToString();
	}

	public override string displayAmmoInClip ()
	{
		return ammoInClip.ToString();
	}

	public override string displayAmmoType ()
	{
		return "Bullets";
	}

//	public float getReloadSpeed(){
//		float current = reloadSpeed;
//		Characteristics characteristics = GetComponentInParent<Characteristics> ();
//		float max = 20.0f;
//		float maxThreshold = max - current;
//		float x = characteristics._agility;
//
//		float agilityEffect = (maxThreshold * Mathf.Pow (x, 1.1f) + x) / (Mathf.Pow (x, 1.1f) + 100);
//		current += agilityEffect;
//
//		return current/10.0f;
//
//	}

}
