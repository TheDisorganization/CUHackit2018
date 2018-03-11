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
    public bool IsGrabbing
    {
        get { return _IsGrabbing;  }
        private set { _IsGrabbing = value; }
    }
    public float ConnectionLength
    {
        get { return _ConnectionLength; }
        private set { _ConnectionLength = value; }
    }
    public bool IsClimbing
    {
        get { return _IsClimbing; }
        private set { _IsClimbing = value; }
    }
    public Vector3 ClimbingStartPosition
    {
        get
        {
            if (!IsClimbing)
                throw new System.Exception("It is not climbing");
            return _ClimbingStartPosition;
        }
        private set { _ClimbingStartPosition = value; }
    }
    public Vector3 CurrentPosition
    {
        get { return transform.position; }//Controller.transform.pos; }
    }

    private Vector3 _ClimbingStartPosition;
    private Vector3 _ConnectionPoint;
    private float _ConnectionLength;
    private bool _IsConnected;
    private bool _IsGrabbing;
    private bool _IsClimbing;

    private bool CanClimb;
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
        IsGrabbing = false;
        IsClimbing = false;
        CanClimb = false;
        ConnectionLength = 0f;
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
            //IsGrabbing = false;
            WebLine.enabled = false;
        }
        //if (IsConnected && Controller.GetHairTrigger())
        //{
        //    GrabWeb();

        //}
        //else
        //{
        //    IsGrabbing = false;
        //}
        IsGrabbing = Controller.GetHairTrigger();


        if (CanClimb && Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            IsClimbing = true;
            ClimbingStartPosition = transform.position;
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            CanClimb = false;
            IsClimbing = false;
        }
    }


    public void OnEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            CanClimb = true;

            IsClimbing = Controller.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);

            if (IsClimbing)
                ClimbingStartPosition = transform.position;
        }
    }
    public void OnStay(Collider other)
    {
        //if (other.gameObject.layer == 8)
        //{

        //    IsClimbing = Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
        //}
    }
    public void OnExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {

            CanClimb = false;
            IsClimbing = false;
        }
    }


    void GrabWeb()
    {
        //if we just grabbed the web
        if (IsGrabbing == false)
        {
            IsGrabbing = true;

            ConnectionLength = (ConnectionPoint - WebSpoutPoint).magnitude;
        }
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
