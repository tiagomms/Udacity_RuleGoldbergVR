using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LevelManager : MonoBehaviour {

    [SteamVR_DefaultAction("enableCheatMode", "default")]
    public SteamVR_Action_Boolean enableCheatModeAction;
    [SteamVR_DefaultAction("resetLevel", "default")]
    public SteamVR_Action_Boolean resetLevelAction;
	public List<GameObject> starList;
    public SteamVR_LoadLevel loadNextLevel;
    public SteamVR_LoadLevel loadThisLevel;
	public Image cheatModePanel;
	public Color enableCheatModeColor;
	public Color disableCheatModeColor;
	public Text cheatModeText;
	public Text nbrStarsText;
	public bool isFinalLevel = false;
	public GameObject finalLevelCanvas;
    private int _nbrOfStarsCaught = 0;
	private AntiCheatScript antiCheatScript;
	private bool hasResetLevel = false;
	private bool isLevelWon = false;


    // Use this for initialization
    void Start () {
		antiCheatScript = GetComponent<AntiCheatScript>();
		if (nbrStarsText != null) {
			nbrStarsText.text = starList.Count.ToString();
		}
		triggerCheatUI();
	}
	
	// Update is called once per frame
	void Update () {
		//butoes
		if (enableCheatModeAction.GetStateUp(SteamVR_Input_Sources.Any)) {
			antiCheatScript.enabled = !antiCheatScript.enabled;
			triggerCheatUI();
		}
		if (resetLevelAction.GetState(SteamVR_Input_Sources.Any) && !hasResetLevel) {
			StartCoroutine(startResetLevel());
			hasResetLevel = true;
		}
	}

    private void triggerCheatUI()
    {
		if (antiCheatScript.enabled) {
			cheatModePanel.GetComponent<Image>().color = disableCheatModeColor;
			cheatModeText.text = "Cheat Mode: DISABLED";
		} else {
			cheatModePanel.GetComponent<Image>().color = enableCheatModeColor;
			cheatModeText.text = "Cheat Mode: ENABLED";
		}
		antiCheatScript.ballResetScript.resetBall(true);
    }

    public void resetNbrOfStarsCaught() {
        _nbrOfStarsCaught = 0;
	}

    internal void aStarWasCaught()
    {
		_nbrOfStarsCaught++;
		if (_nbrOfStarsCaught == starList.Count) {
			StartCoroutine(startLoadingNextLevel());
		}
    }

    private IEnumerator startLoadingNextLevel()
    {
		if (antiCheatScript.enabled) {
			isLevelWon = true;
			yield return new WaitForSecondsRealtime(2.0f);
			if (loadNextLevel != null) {
				if (isFinalLevel) {
					finalLevelCanvas.SetActive(true);
					yield return new WaitForSecondsRealtime(12.0f);	
				}
				loadNextLevel.Trigger();
			} else {
				Debug.Log("LoadLevel - Next level missing");
			}
		}
    }

    private IEnumerator startResetLevel() {
        yield return new WaitForSecondsRealtime(2.0f);
        loadThisLevel.Trigger();
	}

	public void resetStars() {
		StartCoroutine(waitForStarAnimationsToEndAndThenReset());
	}

    private IEnumerator waitForStarAnimationsToEndAndThenReset()
    {
		if (!isLevelWon) {
			resetNbrOfStarsCaught();
			yield return new WaitForSecondsRealtime(1.1f);
			foreach (GameObject star in starList)
			{
				star.SetActive(true);
			}
		}
	}
}
