using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public SteamVR_TrackedObject Head;
    public ControllerData LeftController, RightController;
    public float WebPullForce = 5f;
    public float Gravity = -9.8f;
    public float BounceDampening = 0.5f;


    private float PlayerHeight;
    private Vector3 GravityAccel;
    private Vector3 NetVelocity;

    private CapsuleCollider PlayerCollider;

    public float AirDensity = 0.45f;
    public float DragCoeff = 0.45f;
    public float CrossSectionalArea = 0.68f;
    public float StaticFrictionCoeff = 1.0f;
    public float KineticFrictionCoeff = 0.8f;
    public float PlayerMass = 70f;
    private Vector3 InstantaneousVelocity;

    void Awake()
    {
        NetVelocity = Vector3.zero;
        GravityAccel = new Vector3(0f, Gravity, 0f);
        PlayerHeight = Head.transform.position.y;
        PlayerCollider = GetComponentInChildren<CapsuleCollider>();

        InstantaneousVelocity = Vector3.zero;
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
        Vector3 NetForce = PlayerMass * GravityAccel;
   
        if (LeftController.IsConnected)
            NetForce += (LeftController.ConnectionPoint - LeftController.WebSpoutPoint).normalized * WebPullForce;

        if (RightController.IsConnected)
            NetForce += (RightController.ConnectionPoint - RightController.WebSpoutPoint).normalized * WebPullForce;

        if (playerStanding())
        {
            //calculate normal force
            Vector3 NormalForce = -PlayerMass * GravityAccel;

            NetForce -= NormalForce;

            //calculate friction
            Vector3 KineticFrictionForce = NetForce;
            KineticFrictionForce.y = 0;
            KineticFrictionForce.Normalize();
            KineticFrictionForce *= (-1f)*(NormalForce * KineticFrictionCoeff).magnitude ;

            InstantaneousVelocity.y = 0;
        }
        else 
        {
            //calculate drag
            NetForce -=
                AirDensity *
                DragCoeff * 
                CrossSectionalArea * 
                .5f * 
                Mathf.Pow(InstantaneousVelocity.magnitude, 2f) * 
                InstantaneousVelocity.normalized;
        }

        InstantaneousVelocity += (NetForce / PlayerMass) * Time.fixedDeltaTime; 

        Vector3 newPosition = transform.position + InstantaneousVelocity * Time.fixedDeltaTime;

        if (newPosition.y < PlayerHeight)
            newPosition.y = PlayerHeight;

        transform.position = newPosition;
        updateColliderPosition();
    }

    public void OnCollision(Collision collision)
    {
        InstantaneousVelocity = Vector3.Reflect(InstantaneousVelocity, collision.contacts[0].normal)*BounceDampening;
    }

    void updateColliderPosition()
    {
        Vector3 colliderPos = Head.transform.position;
        colliderPos.y -= PlayerHeight - 1;
        PlayerCollider.transform.position = colliderPos;
    }

    private bool playerStanding()
    {
        return Head.transform.position.y <= PlayerHeight; 

    }
}

//if (LeftController.IsConnected)
//    NetVelocity += (LeftController.ConnectionPoint - LeftController.WebSpoutPoint).normalized * WebPullSpeed;

//if (RightController.IsConnected)
//    NetVelocity += (RightController.ConnectionPoint - RightController.WebSpoutPoint).normalized * WebPullSpeed;

//if (!LeftController.IsConnected && !RightController.IsConnected && transform.position.y > PlayerHeight+1)
//    NetVelocity += GravityAccel * Time.fixedDeltaTime;

//Vector3 newPosition = transform.position + NetVelocity * Time.fixedDeltaTime;

//if (newPosition.y < PlayerHeight)
//    newPosition.y = PlayerHeight;

//transform.position = newPosition;
//updateColliderPosition();
