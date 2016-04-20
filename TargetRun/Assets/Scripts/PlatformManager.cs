using UnityEngine;
using System.Collections;
using System;

public class PlatformManager : MonoBehaviour {


    public GameObject player;
    public float xDestructionDistance;
    public float zDestructionDistance;
    public float creationDistance;
    public float speed;
    public float acceleration;

    private static PlatformManager manager;

    public static Vector3 SpeedVector
    {
        get { return new Vector3(0.0f, 0.0f, manager.speed); }
    }

    public static GameObject Player
    {
        get { return manager.player; }
    }

    public static float XDestructionDistance
    {
        get { return manager.xDestructionDistance; }
    }

    public static float ZDestructionDistance
    {
        get { return manager.zDestructionDistance; }
    }

    public static float CreationDistance
    {
        get { return manager.creationDistance; }
    }

    private float timePassed = 0.0f;

    public void Awake()
    {
        UnityEngine.Random.seed = DateTime.Now.Millisecond;
        if (manager == null) { manager = this; }
        else { throw new InvalidOperationException("You cannot have two platform managers."); }
    }

    public void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > 1.0f)
        {
            timePassed -= 1.0f;
            speed += acceleration;
        }
    }
}
