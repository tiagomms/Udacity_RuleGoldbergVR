using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;

public class ObjectMenuManager : MonoBehaviour {
    private const string STOCK_PREPEND_STRING = "Stock: ";
    public List<int> objectStock;
	public List<float> spawnDistanceFromHand;
	public List<GameObject> objectUIList; //handled automatically at start

	/*
	* There is a conflict between the rule goldberg object parents (which do not have the tag Structure)
	* and spawnObject. The same button (Right trigger) activates the spawning event and the grab object event
	* at the same time. For this reason, if we use the gameObject prefab, the gameObject is broken
	* Solution: use the RuleGoldberg tagged child instead of the prefab
	*/
    public List<GameObject> objectPrefabList;	// set manually in inspector and MUST match order
												// of scene menu objects
	public AudioSource playerAudioSource;
	public AudioClip spawnObjectClip;
	public AudioClip spawnObjectFailClip;
	public Color noStockColor;
	public PortalManager portalManager;
	
	private int currentObject = 0;

	// Use this for initialization
	void Start () {
		foreach(Transform child in transform) {
			objectUIList.Add(child.gameObject);
		}
		gameObject.SetActive(false);
	}
	
	public void setStockText() {
		GameObject thisStockText = transform.Find(objectUIList[currentObject].name + "/Canvas/StockText").gameObject;
		Text stockText = thisStockText.GetComponent<Text>();
		int currentStock = objectStock[currentObject];
		stockText.text = STOCK_PREPEND_STRING + currentStock.ToString();
		if (currentStock == 0) {
			stockText.color = noStockColor;
		}
	}

	// if is Adding, then add current Object and check if surpasses count
	// if is not, subtract and check if is less than zero
    private void ChangeCurrentObjectIndex(bool isAdding)
    {
		if (isAdding) {
			currentObject++;
			if (currentObject > objectUIList.Count - 1)
			{
				currentObject = 0;
			}
		} else {
			currentObject--;
			if (currentObject < 0)
			{
				currentObject = objectUIList.Count - 1;
			}
		}
    }
    private void HandlePortalCase(bool isAdding)
    {
		// is portal entry spawn time and current object is Portal_Exit
		if (portalManager.IsPortalEntrySpawnTime() && objectPrefabList[currentObject].name.Contains("Portal_Exit")) {
            ChangeCurrentObjectIndex(isAdding);
			Debug.Log("IsPortalEntrySpawnTime: " + portalManager.IsPortalEntrySpawnTime()  + " - Skip Portal_Exit");
		}

		// is NOT portal entry spawn time and current object is Portal_Entry
		if (!portalManager.IsPortalEntrySpawnTime() && objectPrefabList[currentObject].name.Contains("Portal_Entry")) {
            ChangeCurrentObjectIndex(isAdding);
			Debug.Log("IsPortalEntrySpawnTime: " + portalManager.IsPortalEntrySpawnTime()  + " - Skip Portal_Entry");
		}
    }
	public void MenuLeft()
    {
        objectUIList[currentObject].SetActive(false);
        
		ChangeCurrentObjectIndex(false);
        HandlePortalCase(false);

        setStockText();
        objectUIList[currentObject].SetActive(true);
    }


    public void MenuRight()
    {
        objectUIList[currentObject].SetActive(false);
        
		ChangeCurrentObjectIndex(true);
        HandlePortalCase(true);

        setStockText();
        objectUIList[currentObject].SetActive(true);
    }


    public void SpawnCurrentObject()
    {
		int currentStock = objectStock[currentObject];
		if (currentStock != 0)
        {
            Vector3 distanceFromHand = gameObject.transform.forward * spawnDistanceFromHand[currentObject];
            Transform obj = Instantiate(objectPrefabList[currentObject],
                objectUIList[currentObject].transform.position + distanceFromHand,
                objectUIList[currentObject].transform.rotation * objectPrefabList[currentObject].transform.rotation
            ).transform;
            objectStock[currentObject]--;
            setStockText();
            // play sound
            playerAudioSource.clip = spawnObjectClip;

            // teleportPortalSpawn
			Debug.Log("Spawn: " + objectPrefabList[currentObject].name);
            HandleNewPortalIfExists(obj.gameObject);
			Debug.Log("After HandleNewPortalIfExists - currentObject: " + objectPrefabList[currentObject].name);
        }
        else {
			// play fail sound
			playerAudioSource.clip = spawnObjectFailClip;
		}
		playerAudioSource.Play();
    }

    private void HandleNewPortalIfExists(GameObject spawnObj)
    {
        if (spawnObj.name.Contains("Portal_Exit"))
        {
            portalManager.addPortalExit(spawnObj);
			MenuRight();
        }
        if (spawnObj.name.Contains("Portal_Entry"))
        {
            portalManager.addPortalEntry(spawnObj);
			MenuRight();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
