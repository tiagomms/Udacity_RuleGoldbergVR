using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlatformScript : MonoBehaviour
{
    public AntiCheatScript gameLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (gameLogic != null && gameLogic.enabled)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                gameLogic.IsPlayerOnPlatform = true;
                gameLogic.enableBall();
                Debug.Log("Player entered platform - ball enabled");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameLogic != null && gameLogic.enabled)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                gameLogic.IsPlayerOnPlatform = false;
                gameLogic.disableBall();
                if (gameLogic.BallStateValue == AntiCheatScript.BallState.GRAB) {
                    gameLogic.detachBallFromHand();
                }
                Debug.Log("Player Left Platform - ball disabled");
            }
        }
    }
}
