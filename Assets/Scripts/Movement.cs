using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public SteamVR_TrackedObject Head;
    public ControllerData LeftController, RightController;
    public float WebPullSpeed = 5f;
    public float Gravity = -9.8f;
    public float BounceDampening = 0.5f;

    public float DragCoeff;

    private float PlayerHeight;
    private Vector3 GravityAccel;
    private Vector3 NetVelocity;
    private CapsuleCollider PlayerCollider;

    void Awake()
    {
        NetVelocity = Vector3.zero;
        GravityAccel = new Vector3(0f, Gravity, 0f);
        PlayerHeight = Head.transform.position.y;
        PlayerCollider = GetComponentInChildren<CapsuleCollider>();
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
        if (LeftController.IsConnected)
            NetVelocity += (LeftController.ConnectionPoint - LeftController.WebSpoutPoint).normalized * WebPullSpeed;

        if (RightController.IsConnected)
            NetVelocity += (RightController.ConnectionPoint - RightController.WebSpoutPoint).normalized * WebPullSpeed;

        if (!LeftController.IsConnected && !RightController.IsConnected && transform.position.y > PlayerHeight+1)
            NetVelocity += GravityAccel * Time.fixedDeltaTime;

        Vector3 newPosition = transform.position + NetVelocity * Time.fixedDeltaTime;

        if (newPosition.y < PlayerHeight)
            newPosition.y = PlayerHeight;

        transform.position = newPosition;
        updateColliderPosition();
    }

    public void OnCollision(Collision collision)
    {
        NetVelocity = Vector3.Reflect(NetVelocity, collision.contacts[0].normal)*BounceDampening;
    }

    void updateColliderPosition()
    {
        Vector3 colliderPos = Head.transform.position;
        colliderPos.y -= PlayerHeight - 1;
        PlayerCollider.transform.position = colliderPos;

    }

    private bool playerStanding()
    {
        return transform.position.y <= PlayerHeight; 

    }
}
