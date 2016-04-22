using UnityEngine;
using System.Collections;

public class ObstacleKiller : MonoBehaviour {

    private static bool initialized = false;
    private static int mask;

    public void Awake()
    {
        if (!initialized)
        {
            initialized = true;
            mask = LayerMask.GetMask("Player");
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & mask) != 0)
        {
            GameOverControl.SetGameOver();
        }
    }

}
