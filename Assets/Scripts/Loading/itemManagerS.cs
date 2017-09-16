using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class itemManagerS : MonoBehaviour {

	public GameObject itemObj;
	public Transform origin;
	private Transform creations;

	public loadingVisualManagerS visualM;

	public int numberOfItems{get; set;}
	public int maxNumberOfItems{get; set;}

	private List<loadingItemS> items;
	private List<dragableS> dragables;

	private bool itemMoving = false;
	private dragableS movingItem;

	public bool readyForNew{get; set;}

	public bool withLabels{get; set;}
	
	void Awake()
	{
		Random.seed = System.DateTime.Now.Millisecond;

		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;	

		items = new List<loadingItemS>();
		dragables = new List<dragableS>();

		readyForNew = false;
		numberOfItems = 0;
		maxNumberOfItems = 0;

		withLabels = false;
	}

	void Update () 
	{
		CheckDrag();

		if(!readyForNew)
		{
			if(!itemMoving)
			{
				if(CheckPositionAll())
				{
					if(CheckSleepAll())
					{
						if(CheckLabelAll())
						{
							readyForNew = true;
							timerS.setLastActivitiyTime();

							if (numberOfItems > 0)
							{
								saverS.successCount++;
								saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";
							}

							if(numberOfItems < maxNumberOfItems)
							{
								//PHOTON CODE
								generalManagerS.PhotonProgressUpdate();
								//PHOTON CODE

								visualM.visualNo = 2;
								visualM.visualOn = true;
							}
						}
					}
				}
			}
		}

		if(readyForNew) 
		{
			if(numberOfItems < maxNumberOfItems)
			{
				CreateItem();
				readyForNew = false;
			}
		}


	}
	
	void CreateItem()
	{
		GameObject tempObj = Instantiate(itemObj, origin.position, Quaternion.identity) as GameObject;
		tempObj.transform.parent = creations;
		numberOfItems++;
		tempObj.name = "Item"+numberOfItems;
		tempObj.transform.position += new Vector3(0.0f, 0.0f, -0.02f * numberOfItems);

		items.Add(tempObj.GetComponent<loadingItemS>());
		dragables.Add(tempObj.GetComponent<dragableS>());

		if(withLabels) tempObj.GetComponent<loadingItemS>().CreateLabels();
	}
	bool CheckDrag()
	{
		if(itemMoving)
		{
			if(!movingItem.draging) 
			{
				itemMoving = false;
				movingItem = null;
			}
		}
		else
		{
			foreach (dragableS item in dragables) 
			{
				if(item.draging) 
				{
					itemMoving = true;
					movingItem = item;
					ClearWarningsAll();
					//TurnPhysicsAll(false);
					return itemMoving;
				}
			}
		}
		return itemMoving;
	}	
	bool CheckPositionAll()
	{
		bool allInside = true;
		foreach (loadingItemS item in items) 
		{
			item.ClearWarnings();
			item.CheckPosition();
			if(item.wrongPosition) allInside=false;
		}	
		return allInside;
	}	
	bool CheckSleepAll()
	{
		bool allSleep = true;
		foreach (loadingItemS item in items) 
		{
			if(item.transform.rigidbody2D.IsAwake()) allSleep=false;
		}	
		return allSleep;
	}	
	bool CheckLabelAll()
	{
		bool allCorrectLabel = true;
		foreach (loadingItemS item in items) 
		{
			item.ClearWarnings();
			item.CheckLabels();
			if(item.wrongLabel) allCorrectLabel=false;
		}	
		return allCorrectLabel;
	}	
	void ClearWarningsAll()
	{
		foreach (loadingItemS item in items) 
		{
			item.ClearWarnings();
		}	
	}
	public void ClearItems()
	{
		readyForNew = false;
		foreach (Transform child in creations) Destroy(child.gameObject);

		items = new List<loadingItemS>();
		dragables = new List<dragableS>();

		numberOfItems = 0;
		maxNumberOfItems = 0;
	}
}
