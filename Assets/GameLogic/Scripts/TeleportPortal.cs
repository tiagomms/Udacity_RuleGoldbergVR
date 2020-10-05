using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TeleportPortal : MonoBehaviour {

	public GameObject exitPoint;
	public float yNudgeAmount = 0.6f;
	
	private void OnTriggerStay(Collider col) {
		GameObject colGameObject = col.gameObject;
		if (colGameObject.CompareTag("Throwable")) {
			Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
			if (!colRigidbody.isKinematic) {
				/*
				* Problem: random velocity and ball direction after teleportation
				* Solution: https://answers.unity.com/questions/1108310/maintaining-speed-after-teleportation-c.html
				*  - dhore reply (I wanted the ball to go on the same direction so, I removed the minus sign)
				*/
				VelocityEstimator velocityScript = colGameObject.GetComponent<VelocityEstimator>();
				if (velocityScript != null) {
					colRigidbody.velocity = velocityScript.GetVelocityEstimate();
					//colRigidbody.velocity = Quaternion.FromToRotation(transform.forward, exitPoint.transform.forward) * velocityScript.GetVelocityEstimate();
				}
				colRigidbody.position = exitPoint.transform.position + (exitPoint.transform.up * yNudgeAmount);
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
