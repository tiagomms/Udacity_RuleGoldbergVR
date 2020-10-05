using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerOnWoodPlank : MonoBehaviour {

    // private bool _isPlayerOnPlank = false;
    public bool IsPlayerOnPlank = false;
	public AntiCheatScript gameLogic;

	private TeleportPoint _woodPlankTeleportPoint;

    // public bool IsPlayerOnPlank
    // {
    //     get
    //     {
    //         return _isPlayerOnPlank;
    //     }

    //     set
    //     {
    //         _isPlayerOnPlank = value;
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
		if (gameLogic != null && gameLogic.enabled) {
			GameObject obj = other.gameObject;
			if (!IsPlayerOnPlank) {
				// layer 10 - player
				if (obj.layer == 10 && !obj.CompareTag("Hand")) {
					IsPlayerOnPlank = !_woodPlankTeleportPoint.ShouldActivate(obj.transform.position);
				}

			} else {
				if (obj.CompareTag("Hand")) {
					obj.GetComponent<Hand>().HoverLock(null);
					Debug.Log("WOOD  Lock Hand");
				}
			}
		}
	}
    private void OnTriggerStay(Collider other)
    {
        if (gameLogic != null && gameLogic.enabled)
        {
			GameObject obj = other.gameObject;
            if (obj.layer == 10 && !obj.CompareTag("Hand"))
            {
                IsPlayerOnPlank = !_woodPlankTeleportPoint.ShouldActivate(obj.transform.position);
			}
        }
    }
	private void OnTriggerExit(Collider other) {
		if (gameLogic != null && gameLogic.enabled) {
			GameObject obj = other.gameObject;
			if (obj.CompareTag("Hand"))
			{
				obj.GetComponent<Hand>().HoverUnlock(null);
				Debug.Log("WOOD  Unlock Hand");
			} else {
				if (obj.layer == 10) {
					IsPlayerOnPlank = false;
				}
			}
		}
	}
	// Use this for initialization
	void Start () {
        _woodPlankTeleportPoint = transform.parent.gameObject.GetComponentInChildren<TeleportPoint>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
