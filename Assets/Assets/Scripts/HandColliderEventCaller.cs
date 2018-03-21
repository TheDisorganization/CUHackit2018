using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColliderEventCaller : MonoBehaviour
{
    public ControllerData cd;

    private void OnTriggerEnter(Collider collision)
    {
        cd.OnEnter(collision);
    }
    private void OnTriggerStay(Collider collision)
    {
        cd.OnStay(collision);
    }
    private void OnTriggerExit(Collider collision)
    {
        cd.OnExit(collision);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    cd.OnEnter(collision);
    //}
    //private void OnCollisionStay(Collision collision)
    //{
    //    cd.OnStay(collision);
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    cd.OnExit(collision);
    //}
}
