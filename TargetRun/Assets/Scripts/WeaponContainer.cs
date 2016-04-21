using UnityEngine;
using System.Collections;

public delegate void WeaponPickUpOrDropEventHandler();

public class WeaponContainer : MonoBehaviour {

	public Weapon[] Weapons;
	private Weapon[] WeaponClones;
	private Transform lastWeaponTransform;
	private Weapon weaponClone;
	protected Transform player;
	private int weaponIndex;
	private float weaponPositionX;
	private float weaponPositionY;
	private float weaponPositionZ;
	private Vector3 weaponPosition;
	private bool rightHanded = false;
	private Weapon ActiveWeapon;
	private Vector3 hipPosition;
	private Vector3 gunRotation;
	private Vector3 aimPosition;
	private Quaternion aimRotation;
	private Quaternion hipRotation;
	private bool onHip;
	private bool setAim;
	private bool canAim;

	private Transform myCamera;

	private Vector3 defaultHipOffset = new Vector3(-.75f, -0.25f, 1.0f);
	private Vector3 defaultAimOffset = new Vector3(0.0f, -0.1f, .5f);

	private float aimSpeed = 25.0f;

	public static event WeaponPickUpOrDropEventHandler PickUpOrDropWeapon;


	// Use this for initialization
	void Start ()
	{
		rightHanded = true;
		onHip = true;
		setAim = false;
		canAim = true;
		WeaponClones = new Weapon[Weapons.Length];
		weaponIndex = 0;
		player = transform.parent;

		myCamera = player.parent.transform.FindChild ("Main Camera");
		//transform.position = myCamera.position;
		while (weaponIndex < Weapons.Length) {
			if (Weapons [weaponIndex] != null)
				CreateNewWeapon ();
			weaponIndex++;
		}
		weaponIndex = 0;

		makeWeaponActive (WeaponClones [weaponIndex], true);

	}

	// Update is called once per frame
	void Update () {



		if (Input.GetButtonDown (Controls.SWAP_HANDS)) {
			if (WeaponClones[weaponIndex] != null) 
				switchHands ();
		}

		Vector3 hipOffset = getWeaponOffset (defaultHipOffset + WeaponClones[weaponIndex].getHipPositionOffset());
		if (Vector3.Distance (hipOffset, WeaponClones [weaponIndex].transform.position) < .01f) {
			onHip = true;
		} else {
			onHip = false;
		}

		aimDownSights ();

	}


	void CreateNewWeapon(){

		//position and rotation when the game starts
		Quaternion q = Quaternion.Euler (myCamera.eulerAngles.x, myCamera.eulerAngles.y, myCamera.eulerAngles.z);
		hipPosition = getWeaponOffset (defaultHipOffset + Weapons[weaponIndex].getHipPositionOffset());

		// Assign the instantiated weapon as a child to the WeaponContainer
		WeaponClones[weaponIndex] = Instantiate (Weapons[weaponIndex], hipPosition, q) as Weapon;
		WeaponClones[weaponIndex].transform.position = getWeaponOffset(defaultHipOffset + WeaponClones[weaponIndex].getHipPositionOffset());
		WeaponClones[weaponIndex].transform.parent = transform;
		WeaponClones[weaponIndex].gameObject.SetActive(false);

	}



	void switchWeapons(int direction){
		//canAim = false;
		//		WeaponClones [weaponIndex].transform.position = getWeaponOffset (WeaponClones [weaponIndex].getHipPositionOffset () + defaultHipOffset);
		makeWeaponActive(WeaponClones [weaponIndex], false);

		if (direction >= 0) {
			weaponIndex++;
			if (weaponIndex == Weapons.Length) {
				weaponIndex = 0;
			}
		} else {
			weaponIndex--;
			if (weaponIndex < 0) {
				weaponIndex = Weapons.Length-1;
			}
		}

		makeWeaponActive(WeaponClones [weaponIndex], true);
		WeaponClones [weaponIndex].transform.position = getWeaponOffset (WeaponClones [weaponIndex].getHipPositionOffset () + defaultHipOffset);
		WeaponClones[weaponIndex].transform.rotation = Quaternion.Euler (myCamera.eulerAngles.x, myCamera.eulerAngles.y, myCamera.eulerAngles.z);

	}


	void switchHands(){
		//Switch Hands
		if (rightHanded) {
			rightHanded = false;
		} else {
			rightHanded = true;
		}

		//position and rotation when the game starts
		Vector3 weaponPositionXYZ = getWeaponOffset (defaultHipOffset + WeaponClones[weaponIndex].getHipPositionOffset());
		weaponPosition = new Vector3 (weaponPositionXYZ.x, WeaponClones [weaponIndex].transform.position.y, weaponPositionXYZ.z);
		Quaternion q = Quaternion.Euler (myCamera.forward);

		// Assign the instantiated weapon as a child to the WeaponContainer
		foreach (Weapon t in WeaponClones) {
			if (WeaponClones [weaponIndex] != null) {
				WeaponClones [weaponIndex].transform.localRotation = q;
				WeaponClones [weaponIndex].transform.position = weaponPosition;
			}
			switchWeapons (1);
		}

	}

	void aimDownSights(){

		if ((Input.GetButtonDown (Controls.ZOOM)) && canAim) {
			//			aimPosition = getWeaponOffset (defaultAimOffset);
			onHip = false;
			setAim = true;

		}

		if (Input.GetButtonUp (Controls.ZOOM)) {
			setAim = false;
			hipPosition = getWeaponOffset (defaultHipOffset + WeaponClones[weaponIndex].getHipPositionOffset());
			canAim = true;
		}

		// Continues to move and rotate into place
		if (Input.GetButton (Controls.ZOOM)) {
			onHip = false;
			Vector3 additionalAimPositionOffset = WeaponClones [weaponIndex].getAimPositionOffset ();
			Vector3 additionalAimRotationOffset = WeaponClones [weaponIndex].getAimRotationOffset ();

			if (rightHanded){
				additionalAimRotationOffset.z = -additionalAimRotationOffset.z;
			}

			aimPosition = getWeaponOffset (defaultAimOffset + additionalAimPositionOffset);
			WeaponClones [weaponIndex].transform.position = Vector3.Lerp (WeaponClones [weaponIndex].transform.position, aimPosition, Time.deltaTime * aimSpeed);

			Quaternion q = Quaternion.Euler (myCamera.eulerAngles + additionalAimRotationOffset);
			WeaponClones [weaponIndex].transform.rotation = Quaternion.Lerp (WeaponClones [weaponIndex].transform.rotation, q, Time.deltaTime * aimSpeed);


		} else if (!onHip) {
			hipPosition = getWeaponOffset (defaultHipOffset + WeaponClones[weaponIndex].getHipPositionOffset());
			WeaponClones [weaponIndex].transform.position = Vector3.Lerp (WeaponClones [weaponIndex].transform.position, hipPosition, Time.deltaTime * aimSpeed);

			Quaternion q = Quaternion.Euler (myCamera.eulerAngles.x, myCamera.eulerAngles.y, myCamera.eulerAngles.z);
			if (!onHip) {
				WeaponClones [weaponIndex].transform.rotation = Quaternion.Lerp (WeaponClones [weaponIndex].transform.rotation, q, Time.deltaTime * aimSpeed);
			}
		}

	}



	Vector3 getWeaponOffset(Vector3 v){
		float distanceFromOriginX;
		float distanceFromOriginZ;
		float distanceFromOriginY;

		if (player.GetComponent<CapsuleCollider> () != null) {
			CapsuleCollider playerCollider = player.GetComponent<CapsuleCollider> ();

			// Find the 2D magnitude and direction from the player to the weapon when the player is at (0,0,0)
			// and the weapon is at (?,?,?)
			distanceFromOriginX = -(playerCollider.bounds.size.x);
			distanceFromOriginZ = (playerCollider.bounds.size.z * 2.0f);
			distanceFromOriginY = (playerCollider.bounds.size.y / 1.25f);
		} else {
			distanceFromOriginX = v.x;
			distanceFromOriginZ = v.z;
			distanceFromOriginY = v.y;
		}

		if (rightHanded) {
			distanceFromOriginX = -distanceFromOriginX;
		}

		Vector3 setXYZ = myCamera.position + (myCamera.right * distanceFromOriginX) + (myCamera.up * distanceFromOriginY) + (myCamera.forward * distanceFromOriginZ);
		return setXYZ;
	}


	void makeWeaponActive(Weapon weapon, bool shouldBeActive){
		if (weapon != null) {
			weapon.gameObject.SetActive (shouldBeActive);
			if (shouldBeActive) {
				aimSpeed = 10.0f;
			}
		}
	}


	public Weapon getActiveWeapon(){
		if(WeaponClones [weaponIndex] != null)
			return WeaponClones [weaponIndex];

		return null;
	}


	public bool inAimingState(){
		if (setAim == true)
			return true;

		return false;
	}

}
