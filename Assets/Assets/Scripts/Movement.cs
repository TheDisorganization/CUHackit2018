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


    private float PlayerHeight
    {
        get { return Head.transform.position.y - transform.position.y; }
    }
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
    //    PlayerHeight = Head.transform.position.y;
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


    Vector3 NonClimbingPos;

    void FixedUpdate()
    {
        if (LeftController.IsClimbing)
        {

            Vector3 positionDelta = LeftController.ClimbingStartPosition - LeftController.CurrentPosition ;
            positionDelta.x = 0f;
            positionDelta.z = 0f;
            transform.position = NonClimbingPos + positionDelta;
            InstantaneousVelocity = Vector3.zero;
            InstantaneousVelocity.y = 7f;
        }
        else if (RightController.IsClimbing)
        {
            Vector3 positionDelta = RightController.ClimbingStartPosition - RightController.CurrentPosition;
            positionDelta.x = 0f;
            positionDelta.z = 0f;
            transform.position = NonClimbingPos + positionDelta;
            InstantaneousVelocity = Vector3.zero;
            InstantaneousVelocity.y = 7f;
        }
        else
        {
            Vector3 NetForce = PlayerMass * GravityAccel;

            if (LeftController.IsConnected && !LeftController.IsGrabbing)
                NetForce += (LeftController.ConnectionPoint - LeftController.WebSpoutPoint).normalized * WebPullForce;

            if (RightController.IsConnected && !RightController.IsGrabbing)
                NetForce += (RightController.ConnectionPoint - RightController.WebSpoutPoint).normalized * WebPullForce;

            if (playerStanding())
            {
                //calculate normal force
                Vector3 NormalForce = -PlayerMass * GravityAccel;

                NetForce += NormalForce;



                if (!RightController.IsConnected && !LeftController.IsConnected)
                    InstantaneousVelocity = Vector3.zero;

                //calculate friction
                //Vector3 KineticFrictionForce = NetForce;
                //KineticFrictionForce.y = 0;
                //KineticFrictionForce.Normalize();
                //KineticFrictionForce *= (-1f)*(NormalForce * KineticFrictionCoeff).magnitude ;

                //InstantaneousVelocity.y = 0;
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
            NonClimbingPos = transform.position;
        }

        //if (LeftController.IsConnected && LeftController.IsGrabbing)
        //{
        //    Vector3 WebLength = (LeftController.ConnectionPoint - LeftController.WebSpoutPoint);
        //    if (WebLength.magnitude > LeftController.ConnectionLength)
        //        transform.position = LeftController.ConnectionPoint - WebLength.normalized * LeftController.ConnectionLength;
        //}

        //if (RightController.IsConnected && RightController.IsGrabbing)
        //{
        //    Vector3 WebLength = (RightController.ConnectionPoint - RightController.WebSpoutPoint);
        //    if (WebLength.magnitude > RightController.ConnectionLength)
        //        transform.position = RightController.ConnectionPoint - WebLength.normalized * RightController.ConnectionLength;
        //}
        preventDecapitation();
        updateColliderPosition();
    }

    public void OnCollision(Collision collision)
    {
        Vector3 average = Vector3.zero;
        foreach (ContactPoint cp in collision.contacts)
            average += cp.normal;


        average /= collision.contacts.Length;

        InstantaneousVelocity = Vector3.Reflect(InstantaneousVelocity, average) * BounceDampening;

        //might remove this (less elastic collisions)
        //transform.position += (transform.position - collision.contacts[0].point).normalized * (0.5f);

        //InstantaneousVelocity = Vector3.Reflect(InstantaneousVelocity, collision.contacts[0].normal)*BounceDampening;

        ////might remove this (less elastic collisions)
        //transform.position += (transform.position - collision.contacts[0].point).normalized * (0.5f);
    }

    void updateColliderPosition()
    {
        Vector3 colliderPos = Head.transform.position;
        colliderPos.y -= PlayerHeight ;
        PlayerCollider.transform.position = colliderPos;
    }

    private bool playerStanding()
    {
        //return Head.transform.position.y <= PlayerHeight; 
        Ray heightRay = new Ray();
        RaycastHit heightHit;

        heightRay.origin = transform.position;

        heightRay.direction = new Vector3(0f,-1f,0f);

        Physics.Raycast(heightRay, out heightHit, float.MaxValue, LayerMask.GetMask("Shootable"));
        return heightHit.distance < 0.5;
    }

    private void preventDecapitation()
    {
        float playerHeight = Head.transform.position.y - transform.position.y; 

        Ray heightRay = new Ray();
        RaycastHit heightHit;

        heightRay.origin = Head.transform.position;

        heightRay.direction = new Vector3(0f, -1f, 0f);

        bool aboveShootable = Physics.Raycast(heightRay, out heightHit, float.MaxValue, LayerMask.GetMask("Shootable"));

        float exposedPlayerPortion = heightRay.origin.y - heightHit.point.y;

        if ( exposedPlayerPortion < playerHeight)
        {

            Vector3 pos = transform.position;
            pos.y += (playerHeight - exposedPlayerPortion);
            transform.position = pos;
        }


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
