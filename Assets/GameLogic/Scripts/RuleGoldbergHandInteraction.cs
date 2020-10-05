using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Hand))]
[RequireComponent(typeof(SteamVR_Behaviour_Pose))]

public class RuleGoldbergHandInteraction : MonoBehaviour {

    [SteamVR_DefaultAction("GrabPinch", "default")]
    public SteamVR_Action_Boolean grabRGObjectAction;
    public AntiCheatScript gameLogic;

    [HideInInspector]
	private Hand thisHand;
    private SteamVR_Input_Sources thisInputSource;
    private bool isObjectInHand = false;

    void Start()
    {
		thisHand = GetComponent<Hand>();
        thisInputSource = GetComponent<SteamVR_Behaviour_Pose>().inputSource;
    }

    private void OnTriggerStay(Collider col)
    {
        if (gameLogic != null && gameLogic.enabled)
        {
            if (col.gameObject.CompareTag("Structure"))
            {
                // release object if stopped pressing or,
                // you are pressing but the ball has been thrown or it is grabbed by the other hand
                if (grabRGObjectAction.GetStateUp(thisInputSource)
                    || (grabRGObjectAction.GetState(thisInputSource) && gameLogic.getIsBallGrabbedOrThrown()))
                {
                    ReleaseObject(col);
                }
                else if (!isObjectInHand && grabRGObjectAction.GetStateDown(thisInputSource) && !gameLogic.getIsBallGrabbedOrThrown())
                {
                    GrabObject(col);
                }
            }
        } 
        else {
            if (col.gameObject.CompareTag("Structure"))
            {    
                if (grabRGObjectAction.GetStateUp(thisInputSource))
                {
                    ReleaseObject(col);
                }
                else if (!isObjectInHand && grabRGObjectAction.GetStateDown(thisInputSource))
                {
                    GrabObject(col);
                }
            }
        }
    }

    private void GrabObject(Collider coli)
    {
        isObjectInHand = true;
        coli.transform.SetParent(gameObject.transform);
		thisHand.TriggerHapticPulse(2000);  // vibrate controller for 2 seconds
    }

    private void ReleaseObject(Collider coli)
    {
        isObjectInHand = false;
        coli.transform.SetParent(null);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
