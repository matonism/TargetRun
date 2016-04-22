using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameOverControl : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
        if(singleton == null) { singleton = this; }
    }

    private static GameOverControl singleton;

    public static bool IsGameOver
    {
        get; private set;
    }

    public static void SetGameOver()
    {
        singleton.text.enabled = true;
        IsGameOver = true;
    }
}
