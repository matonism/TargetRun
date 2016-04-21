using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	private Vector3 startingPosition;
	private float movementSpeed;
	private Animator ani;
	private float jumpTime;
	private bool jumping;
	private bool rolling;
	private Rigidbody rb;
	private float rollSpeed;
	private float jumpSpeed;
	private float jumpDuration;
	private Camera myCamera;
	private Vector3 crouchPosition;
	private Vector3 standPosition;
	private Vector3 currentStandPosition;
	private Vector3 currentCrouchPosition;
	private bool getUpStart;
	private BoxCollider bc;
	private Vector3 originalBoxCenter;
	private Vector3 originalBoxSize;


	void Start(){
		//Play running animation
		Animator[] animators = GetComponentsInChildren<Animator> ();
		foreach (Animator animator in animators){
			if(animator.gameObject.name.ToUpper().Contains("PLAYER")){
				ani = animator;
			}
		}
		rb = GetComponent<Rigidbody> ();

		movementSpeed = 0.05f;
		jumpTime = 1.0f;
		jumping = false;
		rolling = false;
		getUpStart = false;
		setRollTime ();
		setJumpTime ();
		setFallTime ();
		jumpDuration = 1 / jumpSpeed;
		myCamera = GetComponentInChildren<Camera> ();
		crouchPosition = new Vector3 (myCamera.transform.position.x, myCamera.transform.position.y - 2.0f, myCamera.transform.position.z + 0.5f);
		standPosition = new Vector3 (myCamera.transform.position.x, myCamera.transform.position.y, myCamera.transform.position.z);
		bc = transform.GetComponent<BoxCollider> ();
		originalBoxCenter = bc.center;
		originalBoxSize = bc.size;

	}

	void Update(){
		
		//check for jump or roll
		float jumpOrRoll = Input.GetAxisRaw("Jump");
		if (rolling == false && jumping == false) {
			
			if (jumpOrRoll > 0) {
				jumpTime = jumpDuration * 2.0f;
				ani.SetTrigger ("jump");
				rb.AddForce (new Vector3 (0.0f, 300.0f, 0.0f));
				jumping = true;

			} else if (jumpOrRoll < 0) {
				currentStandPosition = new Vector3 (myCamera.transform.position.x, myCamera.transform.position.y, myCamera.transform.position.z);
//				myCamera.transform.position = new Vector3(myCamera.transform.position.x, crouchPosition.y, crouchPosition.z);
				jumpTime = jumpDuration * 2.0f;
				ani.SetTrigger ("roll");
				rolling = true;
			}

		}
			
		//count down till next jump or roll can be activated
		if (jumping == true || rolling == true) {
			
			jumpTime -= Time.deltaTime;
			if (jumpTime <= jumpDuration) {
				ani.SetTrigger ("fall");
			}

			if (rolling && jumpTime < 1.5f && jumpTime > 1.0f) {
				bc.size = new Vector3 (originalBoxSize.x, originalBoxSize.y / 2, originalBoxSize.z);
				bc.center = new Vector3 (originalBoxCenter.x, originalBoxCenter.y - 0.5f, originalBoxCenter.z);

				Vector3 newPosition = new Vector3(myCamera.transform.position.x, crouchPosition.y, crouchPosition.z);
				myCamera.transform.position = Vector3.Lerp (currentStandPosition, newPosition, 2 * (1.5f - jumpTime));
				getUpStart = true;
			} else if (rolling && jumpTime < 0.5f) {
				if (getUpStart) {
					currentCrouchPosition = new Vector3 (myCamera.transform.position.x, myCamera.transform.position.y, myCamera.transform.position.z);
					getUpStart = false;
				}
				bc.size = new Vector3 (originalBoxSize.x, originalBoxSize.y, originalBoxSize.z);
				bc.center = new Vector3 (originalBoxCenter.x, originalBoxCenter.y, originalBoxCenter.z);
				Vector3 newPosition = new Vector3(myCamera.transform.position.x, standPosition.y-1f, standPosition.z);
				myCamera.transform.position = Vector3.Lerp (currentCrouchPosition, newPosition, 2 * (0.5f-jumpTime));

			}

			if (jumpTime <= 0) {
				ani.SetTrigger ("run");
				rolling = false;
				jumping = false;
//				myCamera.transform.position = new Vector3(myCamera.transform.position.x, standPosition.y, standPosition.z);
			}

		}

		moveLeftOrRight ();

	}


	public void moveLeftOrRight(){
		//Move left or right
		float movementDirection = Input.GetAxis ("Move X");
		if (movementDirection > 0.2f) {
			transform.Translate (new Vector3 (movementSpeed, 0.0f, 0.0f));
		} else if (movementDirection < -0.2f) {
			transform.Translate (new Vector3 (-movementSpeed, 0.0f, 0.0f));
		} else {
			transform.Translate (Vector3.zero);
		}
	}


	public void setRollTime(){
		var pmeters = ani.parameters;
		foreach (var p in pmeters) {
			if (p.name == "roll") {
				float initialTime = 1.0f;
				var clips = ani.runtimeAnimatorController.animationClips;
				foreach (var clip in clips) {
					if (clip.name == "roll") {
						initialTime = clip.length;
					}
				}
				float multiplier = initialTime * 1.5f;

				var newSpeed = 1 / initialTime;
				float result = multiplier * initialTime;

				ani.SetFloat ("rollSpeed", initialTime / 1.5f);
				rollSpeed = 1.5f;

			}
		}
	}

	public void setJumpTime(){
		var pmeters = ani.parameters;
		foreach (var p in pmeters) {
			if (p.name == "jump") {
				float initialTime = 1.0f;
				var clips = ani.runtimeAnimatorController.animationClips;
				foreach (var clip in clips) {
					if (clip.name == "jump") {
						initialTime = clip.length;
					}
				}
				var newSpeed = 1 / initialTime;
				float multiplier = initialTime;

				ani.SetFloat ("jumpSpeed", initialTime / .5f);
				jumpSpeed =  1.0f/.75f;


			}
		}
	}

	public void setFallTime(){
		var pmeters = ani.parameters;
		foreach (var p in pmeters) {
			if (p.name == "fall") {
				float initialTime = 1.0f;
				var clips = ani.runtimeAnimatorController.animationClips;
				foreach (var clip in clips) {
					if (clip.name == "fall") {
						initialTime = clip.length;
					}
				}
				//1 second 
				var newSpeed = 1 / initialTime;
				float multiplier = initialTime;

				ani.SetFloat ("fallSpeed", initialTime / .5f);
				//rollSpeed = newSpeed;

			}
		}
	}

}