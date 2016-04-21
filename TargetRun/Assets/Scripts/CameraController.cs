using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform target;
	private WeaponContainer weapons;

	public Vector3 targetOffset = Vector3.zero;
	public float distance = 4.0f;

	public LayerMask lineOfSightMask = 0;
	public float closerRadius = 0.2f;
	public float closerSnapLag = 0.2f;

	public float xSpeed = 200.0f;
	public float ySpeed = 80.0f;

	public float yMinLimit = 80.0f;
	public float yMaxLimit = -50.0f;
	public float xMinLimit = -40.0f;
	public float xMaxLimit = 40.0f;

	private float currentDistance = 10.0f;
	private float x = 0.0f;
	private float y = 0.0f;
	private float distanceVelocity = 0.0f;



	void Start () {

		weapons = GetComponentInChildren<WeaponContainer> ();
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		currentDistance = distance;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;


	}

	void LateUpdate () {
		if (target) {
			if (weapons.inAimingState()) {
				x += Input.GetAxis(Controls.HORIZONTAL_LOOK) * xSpeed * 0.02f / 2;
				y -= Input.GetAxis(Controls.VERTICAL_LOOK) * ySpeed * 0.02f / 2;
			} else {
				x += Input.GetAxis(Controls.HORIZONTAL_LOOK) * xSpeed * 0.02f;
				y -= Input.GetAxis(Controls.VERTICAL_LOOK) * ySpeed * 0.02f;
			}


			y = ClampAngle(y, yMinLimit, yMaxLimit);
			x = ClampAngle (x, xMinLimit, xMaxLimit);

			var rotation = Quaternion.Euler(y, x, 0);
			var targetPos = target.position + targetOffset;
			var direction = rotation * -Vector3.forward;

			var targetDistance = AdjustLineOfSight(targetPos, direction);
			currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref distanceVelocity, closerSnapLag * .3f);

			transform.rotation = rotation;
			weapons.gameObject.transform.rotation = rotation;
		}
	}

	float AdjustLineOfSight (Vector3 target, Vector3 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast (target, direction, out hit, distance, lineOfSightMask.value))
			return hit.distance - closerRadius;
		else
			return distance;
	}

	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
		

}
