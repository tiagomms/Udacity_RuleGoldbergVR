using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineJump : MonoBehaviour {

	public float forceMultiplier = 1.0f;
	public bool hasRegisteredValue = false;
	//private static readonly Vector3 nullYAxis = new Vector3(1.0f, 0f, 1.0f);

	private Vector3 ballVelocityBeforeImpact;

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Throwable")) {
			Rigidbody ballRB = other.gameObject.GetComponent<Rigidbody>();
			ContactPoint contact = other.contacts[0];
			Vector3 jump = -ballRB.mass * forceMultiplier * ballVelocityBeforeImpact.y * Vector3.up;
			//ballRB.velocity = new Vector3(ballRB.velocity.x, 0, ballRB.velocity.z);
			ballRB.AddForce(jump, ForceMode.Impulse);

			Debug.Log("TRAMPOLINE JUMP FORCE: " + jump + "; for normal: " + contact.normal);
		}
		
	}
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Throwable") && !hasRegisteredValue) {
			Rigidbody ballRB = other.gameObject.GetComponent<Rigidbody>();
			ballVelocityBeforeImpact = ballRB.velocity;
			Debug.Log("velocity before impact: " + ballVelocityBeforeImpact);
			hasRegisteredValue = true;
		}
	}
	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Throwable")) {
			hasRegisteredValue = false;
			Debug.Log("Ball has left");
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
