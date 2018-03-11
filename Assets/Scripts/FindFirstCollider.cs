using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFirstCollider : MonoBehaviour
{

    public float range = 100f;

    public float scalingFactor = 1f;

    public GameObject rayOriginObject;

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
    void Update()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) //initial press frame
        {
            ShootWeb();
        }
        else if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            webLine.SetPosition(0, rayOriginObject.transform.position);

        }
        else if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            DisableWebEffects();
        }
    }

    void ShootWeb()
    {
        webLine.enabled = true;
        webLine.SetPosition(0, rayOriginObject.transform.position);

        var x = transform.position.x / transform.forward.x;



        webRay.origin = rayOriginObject.transform.position;
        //webRay.origin = transform.position+ transform.forward* scalingFactor;


        webRay.direction = -transform.up;

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
        if (webLine)
            webLine.enabled = false;
    }
}
