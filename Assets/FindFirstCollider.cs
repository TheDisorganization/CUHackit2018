using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    private bool previousGrip = false;
    private Vector3 currentPosition;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    
    // Update is called once per frame
    void Update () {
		if(Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && !previousGrip)
        {
            previousGrip = true;
            currentPosition = Controller.transform.pos;
            Controller.transform.rot;
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            previousGrip = false;
	}
}
