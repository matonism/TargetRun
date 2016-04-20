using UnityEngine;
using System.Collections;

public class TurnTrigger : MonoBehaviour {

    private static bool initialized;
    private static int mask;
    private float angleTurned = 0.0f;

    public int Selector;
    public float[] AnglesOfRotation;
    public GameObject Focus;

    public void Awake()
    {
        if(!initialized)
        {
            initialized = true;
            mask = LayerMask.GetMask("Player");
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & mask) != 0)
        {
            float angle = AnglesOfRotation[Selector] * Time.fixedDeltaTime * PlatformManager.SpeedVector.z * 0.5f;
            if(angleTurned + angle > AnglesOfRotation[Selector]) { angle = AnglesOfRotation[Selector] - angleTurned; }
            Focus.transform.Rotate(Vector3.up, angle);
            angleTurned += angle;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & mask) != 0)
        {
            Focus.transform.Rotate(Vector3.up, AnglesOfRotation[Selector] - angleTurned);
        }
    }
}
