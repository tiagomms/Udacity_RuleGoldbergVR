using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class BallReset : MonoBehaviour {
	public GameObject 		spawnLocation;
	public AntiCheatScript 	gameLogic;
	public LevelManager		levelManager;
	public Text				nbrOfAttemptsText;
	private Rigidbody 		_ballRB;
    private Material 		_ballMaterial;
	private int				_nbrOfAttempts = -1;
    // Use this for initialization
    void Start () {
		_ballRB = GetComponent<Rigidbody>();
        _ballMaterial = GetComponent<Renderer>().material;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Ground") {
			resetBall();
		}
	}

	public void resetBall(bool isCheatButtonTriggered = false) {
		_ballRB.isKinematic = false;
		_ballRB.angularVelocity = Vector3.zero;
		_ballRB.velocity = Vector3.zero;
		gameObject.transform.position = spawnLocation.transform.position;
		gameObject.transform.rotation = spawnLocation.transform.rotation;

		if (gameLogic != null && gameLogic.enabled) {
			// enable / disable ball according to player's location
			if (gameLogic.IsPlayerOnPlatform) {
				gameLogic.enableBall();
			} else {
				gameLogic.disableBall();
			}
			// reset to not thrown state and not grabbed just in case
			gameLogic.setRestBallState();
			Debug.Log("RESET_BALL");
		}
		else {
            _ballMaterial.color = Color.white;
        }
		// reset Stars as well
		levelManager.resetStars();
		if (!isCheatButtonTriggered) {
			_nbrOfAttempts++;
			nbrOfAttemptsText.text = _nbrOfAttempts + " attempts";
		}
    }
	// Update is called once per frame
	void Update () {
		
	}
}
