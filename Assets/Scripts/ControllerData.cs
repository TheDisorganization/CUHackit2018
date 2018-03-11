using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerData : MonoBehaviour
{
    public bool IsConnected
    {
        get { return _IsConnected; }
        private set { _IsConnected = value; }
    }
    public Vector3 ConnectionPoint
    {
        get
        {
            if (!IsConnected)
                throw new System.Exception("It is not connected");
            return _ConnectionPoint;
        }

        private set
        {
            _ConnectionPoint = value;
        }
    }
    public Vector3 WebSpoutPoint
    {
        get { return WebSpout.transform.position; }
    }

    public float WebRange = 100f;
    public GameObject WebSpout;
    public bool TriggerPressed = false;

    private Vector3 _ConnectionPoint;
    private bool _IsConnected;

    private int ShootableMask;
    private LineRenderer WebLine;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake ()
    {
        ShootableMask = LayerMask.GetMask("Shootable");
        WebLine = GetComponent<LineRenderer>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        IsConnected = false;
        ConnectionPoint = Vector3.zero;
    }

    void Update()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //store the hit
            ShootWeb();
        }
        else if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            //remove hit
            IsConnected = false;
            WebLine.enabled = false;
        }
        TriggerPressed = Controller.GetHairTrigger();
	}

    void ShootWeb()
    {
        Ray webRay = new Ray();
        RaycastHit webHit;

        webRay.origin = WebSpout.transform.position;
  
        webRay.direction = -transform.up;

        IsConnected = Physics.Raycast(webRay, out webHit, WebRange, ShootableMask);
        ConnectionPoint = webHit.point;
    }

    public void ShowWebLine()
    {
        if (!IsConnected)
            throw new System.Exception("Web not connected");

        WebLine.enabled = true;
        WebLine.SetPosition(0, WebSpout.transform.position);
        WebLine.SetPosition(1, ConnectionPoint);
    }

}
