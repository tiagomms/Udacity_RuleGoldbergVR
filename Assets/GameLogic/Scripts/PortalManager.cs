using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalManager : MonoBehaviour {

	public List<GameObject> portalEntryList = new List<GameObject>();
	public List<GameObject> portalExitList = new List<GameObject>();
	public Material			enabledEntryMaterial;
	public Material			enabledExitMaterial;

    // Use this for initialization
    void Start () {
	}
	
	public bool IsPortalEntrySpawnTime() {
		return portalEntryList.Count == portalExitList.Count;
	}

	public void addPortalEntry(GameObject portalEntry) {
		portalEntryList.Add(portalEntry);
	}

    /*
	 *  when adding a portal exit
	 *	1) connect PortalTriggerCollider TeleportPortal to TeleportPortal from entry
	 *	2) enable PortalTriggerCollider from matching entry
	 *	3) change matching entry and portal exit material to enable
	 *	4) set their texts updated with list length 
	 */
    public void addPortalExit(GameObject portalExit) {
		portalExitList.Add(portalExit);

		int nbrPairPortals = portalExitList.Count;
		Transform matchingEntry = portalEntryList[nbrPairPortals - 1].transform;
		GameObject matchingEntryPortalTriggerCollider = matchingEntry.Find("PortalTriggerCollider").gameObject;
		Text matchingEntryChildText = matchingEntry.GetComponentInChildren<Text>();

        matchingEntryPortalTriggerCollider.GetComponent<TeleportPortal>().exitPoint = portalExit;
        matchingEntryPortalTriggerCollider.SetActive(true);

        matchingEntry.Find("TeleportPoint/teleport_marker_mesh").gameObject.GetComponent<Renderer>().material = enabledEntryMaterial;
        portalExit.transform.Find("TeleportPoint/teleport_marker_mesh").gameObject.GetComponent<Renderer>().material = enabledExitMaterial;
        
		matchingEntryChildText.text = "Portal Entry " + nbrPairPortals.ToString();
        portalExit.GetComponentInChildren<Text>().text = "Portal Exit " + nbrPairPortals.ToString();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
