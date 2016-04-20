using UnityEngine;
using System.Collections;

public class PlatformCollector : MonoBehaviour {

    public void Start()
    {
        var platforms = this.GetComponentsInChildren<MobilePlatform>(true);
        foreach (var p in platforms)
        {
            SpawnParameters.AddConnectionBuilder(p);
        }
    }
}
