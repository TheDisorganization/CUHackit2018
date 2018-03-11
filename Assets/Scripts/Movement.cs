using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public SteamVR_TrackedObject Head;
    public ControllerData LeftController, RightController;
    public float WebPullSpeed = 5f;
    public float Gravity = -9.8f;
    public float DragCoeff;

    private float PlayerHeight;
    private Vector3 GravityAccel;
    private Vector3 NetVelocity;

    void Awake()
    {
        NetVelocity = Vector3.zero;
        GravityAccel = new Vector3(0f, Gravity, 0f);
        PlayerHeight = Head.transform.position.y;
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
            NetVelocity += (LeftController.ConnectionPoint - LeftController.WebSpoutPoint).normalized * WebPullSpeed;

        if (RightController.IsConnected)
            NetVelocity += (RightController.ConnectionPoint - RightController.WebSpoutPoint).normalized * WebPullSpeed;

        if (!LeftController.IsConnected && !RightController.IsConnected)
            NetVelocity += GravityAccel * Time.fixedDeltaTime;

        Vector3 newPosition = transform.position + NetVelocity * Time.fixedDeltaTime;

        if (newPosition.y < PlayerHeight)
            newPosition.y = PlayerHeight;

        transform.position = newPosition;
    }

}
