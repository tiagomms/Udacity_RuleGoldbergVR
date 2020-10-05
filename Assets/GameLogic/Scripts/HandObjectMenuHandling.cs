using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandObjectMenuHandling : MonoBehaviour {

	[SteamVR_DefaultAction("moveMenu", "default")]
	public SteamVR_Action_Vector2 moveTrackpadAction;

    [SteamVR_DefaultAction("popupMenu", "default")]
    public SteamVR_Action_Boolean menuOpenAction;

    [SteamVR_DefaultAction("InteractUI", "default")]
    public SteamVR_Action_Boolean interactUIAction;
	
	// Swipe
	public float touchCurrent;
	public bool hasSwipedLeft;
	public bool hasSwipedRight;
    public ObjectMenuManager objectMenuManagerScript;
    public GameObject objectMenu;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (menuOpenAction.GetStateDown(SteamVR_Input_Sources.RightHand)) {
            hasSwipedLeft = true;
            hasSwipedRight = true;
			objectMenu.SetActive(true);
            objectMenuManagerScript.setStockText();
			Debug.Log("Menu OPEN - TRACKPAD ON");
		}
		if (menuOpenAction.GetState(SteamVR_Input_Sources.RightHand)) {
            if (objectMenu.activeSelf) {
                touchCurrent = moveTrackpadAction.GetAxis(SteamVR_Input_Sources.RightHand).x;
                if (!hasSwipedRight)
                {
                    if (touchCurrent > 0.5f)
                    {
                        SwipeRight();
                        hasSwipedRight = true;
                    }
                }
                if (!hasSwipedLeft)
                {
                    if (touchCurrent < -0.4f)
                    {
                        SwipeLeft();
                        hasSwipedLeft = true;
                    }
                }
                // center
                if (hasSwipedLeft || hasSwipedRight) {
                    if (touchCurrent > -0.25f && touchCurrent < 0.3f) {
                        hasSwipedLeft = false;
                        hasSwipedRight = false;
                    }
                }

                if (interactUIAction.GetStateDown(SteamVR_Input_Sources.RightHand)) {
                    Debug.Log("InteractUI Button [Trigger] PRESSED");
                    SpawnObject();
                }
            }
		}
		if (menuOpenAction.GetStateUp(SteamVR_Input_Sources.RightHand)) {
            objectMenu.SetActive(false);
            hasSwipedRight = false;
            hasSwipedLeft = false;
			Debug.Log("Menu CLOSED - TRACKPAD OFF");
		}

	}

    void SpawnObject()
    {
        objectMenuManagerScript.SpawnCurrentObject();
    }
    void SwipeLeft()
    {
        objectMenuManagerScript.MenuLeft();
        Debug.Log("SwipeLeft");
    }
    void SwipeRight()
    {
        objectMenuManagerScript.MenuRight();
        Debug.Log("SwipeRight");
    }
}
