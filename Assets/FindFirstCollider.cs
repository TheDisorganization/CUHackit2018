using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFirstCollider : MonoBehaviour {

    public float range = 100f;

    private bool previousGrip = false;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    Ray webRay = new Ray();
    RaycastHit webHit;
    LineRenderer webLine;
    int shootableMask;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        webLine = GetComponent<LineRenderer>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    
    // Update is called once per frame
    void Update ()
    {
		if(Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && !previousGrip)
        {
            previousGrip = true;
            ShootWeb();
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            previousGrip = false;
            DisableWebEffects();
        }
	}

    void ShootWeb()
    {
        webLine.enabled = true;
        webLine.SetPosition(0, transform.position);

        webRay.origin = transform.position;
        webRay.direction = transform.forward;

        if (Physics.Raycast(webRay, out webHit, range, shootableMask))
        {
            webLine.SetPosition(1, webHit.point);
        }
        else
        {
            webLine.SetPosition(1, webRay.origin + webRay.direction * range);
        }
    }

    public void DisableWebEffects()
    {
        webLine.enabled = false;
    }
}
