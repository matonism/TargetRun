using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public bool randomizeSeed;
    public int seed;
    public GameObject player;
    public float creationDistance;
    public float destructionDistance;
    public float maxspeed;
    public float speed;
    public float distanceTraveled;
    public float acceleration;
    public SpawnParameters[] platformTypes;

    private float worldRotation = 0.0f;
    private float timePassed = 0.0f;

    private static PlatformManager manager;

    public static Vector3 SpeedVector
    {
        get { return new Vector3(0.0f, 0.0f, manager.speed); }
    }

    public static float WorldRotationY
    {
        get { return manager.worldRotation; }
    }

    public static Quaternion WorldRotation
    {
        get { return Quaternion.AngleAxis(PlatformManager.WorldRotationY, Vector3.up); }
    }

    public static GameObject Player
    {
        get { return manager.player; }
    }

    public static float CreationDistance
    {
        get { return manager.creationDistance; }
    }

    public static float DestructionDistance
    {
        get { return manager.destructionDistance; }
    }

    public static void RotateWorld(float amount)
    {
        manager.worldRotation += amount;
        while(manager.worldRotation >= 360.0f) { manager.worldRotation -= 360.0f; }
        while (manager.worldRotation <= -360.0f) { manager.worldRotation += 360.0f; }
        manager.gameObject.transform.Rotate(Vector3.up, amount, Space.World);
    }

    public void Awake()
    {
        seed = (randomizeSeed) ? DateTime.Now.Millisecond : seed;
        UnityEngine.Random.seed = seed;
        if (manager == null) { manager = this; }
        else { throw new InvalidOperationException("You cannot have two platform managers."); }


        var startType = platformTypes[UnityEngine.Random.Range(0, platformTypes.Length)].gameObject;
        while(startType.name.ToLower().Contains("turn")) { startType = platformTypes[UnityEngine.Random.Range(0, platformTypes.Length)].gameObject; }
        GameObject blueprint = null;
        Dictionary<string, SpawnParameters> copies = new Dictionary<string, SpawnParameters>();
        foreach (var p in platformTypes)
        {
            string name = p.gameObject.name;
            var copyObj = (GameObject)GameObject.Instantiate(p.gameObject, p.transform.position, p.transform.rotation);
            copyObj.transform.parent = this.transform;
            if(startType.name.Equals(name)) { blueprint = copyObj; }
            copyObj.name = name + "Blueprint";
            copies.Add(name, copyObj.GetComponent<SpawnParameters>());
        }
        SpawnParameters.AddConnectionBuilders(copies);

        var startObj = (GameObject)GameObject.Instantiate(blueprint, this.transform.position, this.transform.rotation);
        startObj.transform.parent = this.transform;
        startObj.SetActive(true);
    }

    public void Update()
    {
        distanceTraveled += speed * Time.deltaTime;
        ChanceManager.RegisterTravel(speed * Time.deltaTime);
        timePassed += Time.deltaTime;
        if(timePassed > 1.0f)
        {
            timePassed -= 1.0f;
            speed = Mathf.Min(speed + acceleration, maxspeed);
        }
    }

    public static void DrawLocalAxis(Vector3 position, Quaternion rotation, float size)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(position, rotation * Vector3.forward * size);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, rotation * Vector3.up * size);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(position, rotation * Vector3.right * size);
    }

    public void OnDrawGizmosSelected()
    {
        DrawLocalAxis(Vector3.zero, WorldRotation, 10.0f);
    }

}
