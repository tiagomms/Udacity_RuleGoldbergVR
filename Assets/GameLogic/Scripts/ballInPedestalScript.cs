using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballInPedestalScript : MonoBehaviour {

	public AntiCheatScript gameLogic;
	private void OnTriggerEnter(Collider other) {
		if (gameLogic != null && gameLogic.enabled) {
			if (other.gameObject.CompareTag("Throwable")) {
				if (gameLogic.BallStateValue == AntiCheatScript.BallState.THROW) {
					gameLogic.setRestBallState();
					Debug.Log("ballInPedestalScript OnTriggerEnter - REST STATE");
				}
			}
		}
	}
    private void OnTriggerExit(Collider other)
    {
		if (gameLogic != null && gameLogic.enabled) {
			if (other.gameObject.CompareTag("Throwable"))	
			{
				if (gameLogic.BallStateValue == AntiCheatScript.BallState.REST)
				{
					gameLogic.setThrownBallState();
					Debug.Log("ballInPedestalScript OnTriggerExit - THROW STATE");
				}
			}
		}
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
