using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
	protected Vector3 aimPositionOffset = Vector3.zero;
	protected Vector3 aimRotationOffset = Vector3.zero;
	protected Vector3 hipPositionOffset = Vector3.zero;
	protected Vector3 hipRotationOffset = Vector3.zero;

	public float aimSpeed = 10.0f;

	//Always call overridding method
	public virtual string displayAmmoRemaining(){
		return "?";
	}

	public virtual string displayAmmoInClip(){
		return "?";
	}

	public virtual string displayAmmoType(){
		return "Bullets";
	}

	public virtual string displayMannaRemaining(){
		return "?";
	}

	public bool isHeld(){
		if (transform.parent != null && GetComponentInParent<WeaponContainer>()) {
			return true;
		} else {
			return false;
		}
	}

	public Vector3 getAimPositionOffset(){
		return aimPositionOffset;
	}

	public Vector3 getAimRotationOffset(){
		return aimRotationOffset;
	}

	public Vector3 getHipPositionOffset(){
		return hipPositionOffset;
	}

	public Vector3 getHipRotationOffset(){
		return hipRotationOffset;
	}

	public bool isAiming(){
		if (isHeld()) {
			WeaponContainer weaponContainer = GetComponentInParent<WeaponContainer> ();
			if (weaponContainer.inAimingState ()) {
				return true;
			}
		}

		return false;
	}

	protected void initializeForHeldOrDropped(){

		if (isHeld ()) {
//			this.setPlayerMask ();

		} else {
			if (!this.gameObject.GetComponent<Rigidbody> ()) {
				Rigidbody rigidb = this.gameObject.AddComponent<Rigidbody> ();
				rigidb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
		}


	}

//	public void setPlayerMask(){
//		playerMask = 1 << transform.parent.parent.gameObject.layer;
//		//		playerMask = 1 << LayerMask.NameToLayer ("Player");
//		playerMask = ~playerMask;
//	}

	public virtual string getWeaponName(){
		return "Weapon";
	}
}

