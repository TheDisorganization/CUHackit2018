using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventTrigger : MonoBehaviour {

    public Movement CollisionManager;

    private void OnCollisionEnter(Collision collision)
    {
        CollisionManager.OnCollision(collision);
    }


}
