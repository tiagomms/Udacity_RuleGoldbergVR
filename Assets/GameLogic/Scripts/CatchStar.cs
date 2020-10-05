using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchStar : MonoBehaviour {

	public LevelManager levelManager;
	public AudioSource	playerAudioSource;
	public AudioClip	catchStarClip;

	private bool _isStarCaught;

    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();
	// Rotation Storage Variables
    private float rotationSpeed = 1.0f;

    // Star Collected Animation Variables
    private float posIncrement = 0.01f;
    private float tempRotSpeed = 1.0f;

    void Start()
    {
        posOffset = transform.position;
		tempPos = transform.position;
    }

    void Update()
    {
        if (_isStarCaught)
        { // has been collected start flying and increase rotationSpeed
            tempPos.y += posIncrement;
            tempRotSpeed += 1.2f;
        	transform.position = tempPos;
        }
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond * tempRotSpeed, 0f), Space.World);
    }

	private void OnTriggerEnter(Collider other) {
		if (!_isStarCaught && other.gameObject.CompareTag("Throwable")) {
			StartCoroutine(CatchStarAnimation());
		}
	}

    private IEnumerator CatchStarAnimation()
    {
		_isStarCaught = true;
		levelManager.aStarWasCaught();
		playerAudioSource.clip = catchStarClip;
		playerAudioSource.Play();
		yield return new WaitForSecondsRealtime(1);
		_isStarCaught = false;
		gameObject.SetActive(false);
		transform.position = posOffset;
		tempPos = posOffset;
		tempRotSpeed = rotationSpeed;
    }
}
