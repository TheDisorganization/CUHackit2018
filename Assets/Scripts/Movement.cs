using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public SteamVR_TrackedObject Head;
    public ControllerData LeftController, RightController;
    public float WebPullSpeed = 5f;

    private Vector3 NetVelocity;

    void Awake()
    {
        NetVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (LeftController.IsConnected)
            LeftController.ShowWebLine();
        if (RightController.IsConnected)
            RightController.ShowWebLine();
    }

    void FixedUpdate()
    {
        if (NetVelocity == Vector3.zero)
        {
            Debug.Log("AAAAA");
        }

        if (LeftController.IsConnected)
            NetVelocity += (LeftController.ConnectionPoint - LeftController.WebSpoutPoint) * WebPullSpeed;

        if (RightController.IsConnected)
            NetVelocity += (RightController.ConnectionPoint - RightController.WebSpoutPoint) * WebPullSpeed;

        if(!LeftController.IsConnected && !RightController.IsConnected)


        transform.position = NetVelocity * Time.fixedDeltaTime;
    }

}
