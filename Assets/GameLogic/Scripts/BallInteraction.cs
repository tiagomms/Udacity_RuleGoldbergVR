using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallInteraction : MonoBehaviour {

    public AntiCheatScript gameLogic;

    [HideInInspector]
    private Hand thisHand;

    void Start()
    {
        thisHand = GetComponent<Hand>();
    }

	private void OnTriggerEnter(Collider other) {
        if (gameLogic != null && gameLogic.enabled)
        {
			if (!gameLogic.IsPlayerOnPlatform) {
				if (other.gameObject.CompareTag("Throwable")) {
					thisHand.HoverLock(null);
				}
			}
		}
	}
    private void OnTriggerExit(Collider other)
    {
        if (gameLogic != null && gameLogic.enabled)
        {
			if (other.gameObject.CompareTag("Throwable"))
			{
				thisHand.HoverUnlock(null);
			}
		}
    }
}
