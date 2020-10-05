using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanWindScript : MonoBehaviour {

    public float radius = 5.0F;
    public float power = 10.0F;
	public float liftPower = 3.0f;

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag("Throwable")) {
			Vector3 ballPos = other.gameObject.transform.position;
			Rigidbody ballRB = other.GetComponent<Rigidbody>();
			if (ballRB != null) {
				Debug.Log("ball gets exploding force");
				ballRB.AddExplosionForce(power, ballPos, radius, liftPower);
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
