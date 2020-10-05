using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AntiCheatScript : MonoBehaviour {

	public enum BallState
	{
		REST,
		GRAB,
		THROW
	}

	private BallState _ballStateValue = BallState.REST;
	public GameObject ball;
	public BallReset ballResetScript;
	public Color ballActiveColor;
	public Color ballDisabledColor;
	public GameObject platform;
	public GameObject player;

	private bool isPlayerOnPlatform = false;

    private Throwable _ballThrowable;
	private Interactable _ballInteractable;
	private Rigidbody _ballRigidbody;
	private Material _ballMaterial;

    public bool IsPlayerOnPlatform
    {
        get
        {
            return isPlayerOnPlatform;
        }

        set
        {
            isPlayerOnPlatform = value;
        }
    }

    public BallState BallStateValue
    {
        get
        {
            return _ballStateValue;
        }

        private set
        {
            _ballStateValue = value;
        }
    }

    public void setThrownBallState() {
		if (this.enabled) {
			if (IsPlayerOnPlatform) {
				if (BallStateValue == BallState.GRAB) {
					BallStateValue = BallState.THROW;
				}
			} else {
				if (BallStateValue != BallState.THROW) {
					ballResetScript.resetBall();
				}
			}
		} 
		Debug.Log("SET_THROWN_BALL_STATE - ball state: " + BallStateValue.ToString());
	}
	public void setRestBallState() { 
		if (enabled) {
			BallStateValue = BallState.REST;
		}
	}
	public void setGrabbedBallState() {
		if (enabled) {
			if (IsPlayerOnPlatform) {
				BallStateValue = BallState.GRAB;
			} 
			Debug.Log("SET_GRABBED_BALL_STATE - ball grabbed: " + BallStateValue.ToString());
		}
	}
	public bool getIsBallGrabbedOrThrown() {
		return BallStateValue != BallState.REST;
	}

    internal void detachBallFromHand()
    {
        Hand hand = _ballInteractable.attachedToHand;
        StartCoroutine(DetachBallBecauseOutOfPlatform(hand));
    }
	
	public void enableBall() {
		_ballMaterial.color = ballActiveColor;
	}
	public void disableBall() {
		_ballMaterial.color = ballDisabledColor;
	}

	private void Awake() {
		_ballThrowable = ball.GetComponent<Throwable>();
		_ballInteractable = ball.GetComponent<Interactable>();
		_ballMaterial = ball.GetComponent<Renderer>().material;
		_ballRigidbody = ball.GetComponent<Rigidbody>();
	}
	// Use this for initialization
	void Start () {
	}

    private IEnumerator DetachBallBecauseOutOfPlatform(Hand attachedHand)
    {
		yield return new WaitForFixedUpdate();
        attachedHand.DetachObject(ball);
    }

    // Update is called once per frame
    void Update () {
		
	}
	private void FixedUpdate()
	{
	}
}
