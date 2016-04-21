using UnityEngine;

[System.Serializable]
public class ConnectionParameters
{
    [System.Serializable]
    public class PlatformParamaters
    {
        public float Chance;
        public MobilePlatform Platform;

        public Vector3 ClientEntranceConnection
        {
            get
            {
                Platform.Awake();
                return Platform.Parameters.HostEntranceConnection;
            }
        }
    }

    public Vector3 HostExitConnection;
    public float ExitRotation;
    public PlatformParamaters[] Platforms;
}
