using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour {

    public TurnTrigger focus;
    public int selector;

    public void Set()
    {
        focus.Selector = selector;
    }
}
