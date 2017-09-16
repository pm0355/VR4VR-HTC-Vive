using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class boxManagerS : MonoBehaviour {

	public boxS[] boxes;
	public Texture[] boxTextures;

	public Renderer[] shelfLabels;
	public Texture[] labelTextures;

	public shelfLevelS[] shelfLevels;
	private int[] shelfLabelIndices = new int[] { 0, 0 };

	public GameObject otherBoxesObj;
	private int[] st2Request = new int[] { 0, 0 };

	private List<int> st3Request = new List<int>();

	public Transform deliveryAreaPos;
	public Transform magicBoxPos;
	public Material magicBoxTreadMill;
	private float treadMillSpeed = 1.0f;

	public bool isActive = false;
	private int activeLevel = -1;
	private int cycle;
	private int maxCycle;
	private bool activeIsOrder;
	private bool boxesMoving;

	private Transform creations;

	public Renderer[] requestLabels;
	public Renderer deliveryLabel;

	public Texture[] st2CountingTestures;
	public Texture[] st3BoxLabelTestures;
	public Transform[] st3ShelfLevelNumbers;

	private shelvingManagerS shelvingM;

	// Use this for initialization
	void Awake()
	{
		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;

		shelvingM = transform.parent.GetComponent<shelvingManagerS>();
	}
	void Update()
	{

	}

	public void StartLevel(int _level, int _maxCycle)
	{
		ResetLevel();

		isActive = true;
		cycle = 0;
		maxCycle = _maxCycle;
		activeLevel = _level;

		foreach (var box in boxes)
		{
			box.ShowBox(true);
		}

		switch (_level)
		{
			case 0:
				ChooseLabels();

				foreach (var level in shelfLevels)
				{
					if(level.numberOfBoxesInside==0)
					{
						GameObject tempObj = Instantiate(otherBoxesObj, level.transform.position, level.transform.rotation) as GameObject;
						tempObj.transform.parent = creations;

						tempObj.GetComponent<otherBoxesS>().SetBoxTextures(boxTextures[shelfLabelIndices[level.shelf - 1]]);
					}
				}

				foreach (var box in boxes)
				{
					if (box.atShelf>0)
						box.SetBoxTexture(boxTextures[shelfLabelIndices[box.atShelf - 1]], shelfLabelIndices[box.atShelf - 1]);
				}
				for (int i = 0; i < shelfLabels.Length; i++)
				{
					shelfLabels[i].gameObject.SetActive(true);
					shelfLabels[i].material.mainTexture = labelTextures[shelfLabelIndices[i]];
				}
				break;
			case 1:
				ChooseLabels();

				foreach (var box in boxes)
				{
					if (box.atShelf > 0)
						box.SetBoxTexture(boxTextures[shelfLabelIndices[box.atShelf - 1]], shelfLabelIndices[box.atShelf - 1]);
				}
				for (int i = 0; i < shelfLabels.Length; i++)
				{
					shelfLabels[i].gameObject.SetActive(true);
					shelfLabels[i].material.mainTexture = labelTextures[shelfLabelIndices[i]];
				}
				break;
			case 2:
				foreach (var box in boxes)
				{
					box.labelObj.gameObject.SetActive(true);

					if (box.atShelf > 0)
					{
						box.SetLabelTexture(st3BoxLabelTestures[(box.atShelf - 1) * 3 + (box.atLevel - 1)], (box.atShelf - 1) * 3 + (box.atLevel - 1));
						box.SetBoxTexture(boxTextures[6], -1);
					}
				}
				foreach (var numberlabel in st3ShelfLevelNumbers)
				{
					numberlabel.gameObject.SetActive(true);
				}
				break;
		}
		MakeRequest();
	}
	public void MakeRequest()
	{
		activeIsOrder = true;
		shelvingM.visualM.info = "Remaining Job: " + (maxCycle - cycle).ToString();

		switch (activeLevel)
		{
			case 0:
				activeIsOrder = false;
				foreach (var box in boxes)
				{
					if (maxCycle==1 || maxCycle - cycle - 1 > 0) //if tutorial or not last shuffle
						box.RandomRotate(false);
					else
						box.RandomRotate(true);
				}
				break;
			case 1:
				int[] boxCount = new int[2];
				foreach (var level in shelfLevels)
				{
					boxCount[level.shelf - 1] += level.numberOfBoxesInside;
				}

				do
				{
					st2Request[0] = Random.Range(0, boxCount[0]+1);
					st2Request[1] = Random.Range(0, boxCount[1]+1);
				} while (st2Request[0] + st2Request[1] == 0 || st2Request[0] + st2Request[1] == 4);

				ShowRequest();
				//Debug.Log("COUNT: " + boxCount[0] + " adet " + shelfLabelIndices[0] + ", and " + boxCount[1] + " adet " + shelfLabelIndices[1]);
				//Debug.Log("REQUEST: "+st2Request[0] + " adet " + shelfLabelIndices[0] + ", and " + st2Request[1] + " adet " + shelfLabelIndices[1]);

				break;
			case 2:
				List<boxS> allBoxes = new List<boxS>();
				foreach (var box in boxes)
				{
					if (box.atLevel > 0)
						allBoxes.Add(box);
				}

				st3Request = new List<int>();

				int tempRand = 0;
				float randomComparator = 1.001f;

				/*
					Probabilities:
					--------------
					1 -> 0.328125
					2 -> 0.484375
					3 -> 0.171875
					4 -> 0.015625
				 */

				while (allBoxes.Count>0)
				{
					tempRand = Random.Range(0, allBoxes.Count);
					if (Random.value < randomComparator) st3Request.Add(allBoxes[tempRand].targetLevelIndex);
					randomComparator *= 0.5f;
					allBoxes.RemoveAt(tempRand);
				}

				ShowRequest();
				break;
		}
	}
	public bool CheckBoxes()
	{
		bool result = true;
		ClearWarings();

		switch (activeLevel)
		{
			case 0:
				foreach (var box in boxes)
				{
					if (box.atShelf > 0) //if at one of the shelves
					{
						if (box.activeTexture == shelfLabelIndices[box.atShelf - 1]) //if at the right shelf
						{
							if (!box.CheckDirection()) //if direction is not correct
							{
								box.SetBoxWarning(true);
								result = false;
							}
						}
						else
						{
							box.SetBoxWarning(true);
							result = false;
						}
					}
					else   // if box is not in any shelf
					{
						box.SetBoxWarning(true);
						result = false;
					}
				}
				break;
			case 1:
				if (activeIsOrder)
				{
					int[] deliveryCount = new int[2];
					bool otherBoxesAreCorrect = true;
					foreach (var box in boxes)
					{
						if (box.atDelivery)
						{
							if (box.activeTexture == shelfLabelIndices[0]) deliveryCount[0]++;
							else deliveryCount[1]++;
						}
						else //if box is at wrong shelf (changed during game by player)
						{
							if (box.atShelf > 0)
							{
								if (box.activeTexture != shelfLabelIndices[box.atShelf - 1])
								{
									otherBoxesAreCorrect = false;
									box.SetBoxWarning(true);
								}
							}
							else   // if box is not in any shelf or delivery area
							{
								otherBoxesAreCorrect = false;
								box.SetBoxWarning(true);
							}
						}
					}
					//Debug.Log("GIVEN: " + deliveryCount[0] + " adet " + shelfLabelIndices[0] + ", and " + deliveryCount[1] + " adet " + shelfLabelIndices[1]);

					if (st2Request[0] == deliveryCount[0] && st2Request[1] == deliveryCount[1] && otherBoxesAreCorrect) result = true;
					else result = false;

					for (int s = 0; s < 2; s++)
					{
						foreach (var box in boxes)
						{
							if (st2Request[s] > deliveryCount[s])
							{
								if (!box.atDelivery && box.activeTexture == shelfLabelIndices[s])
								{
									box.SetBoxWarning(true);
									deliveryCount[s]++;
								}
							}
							else
								break;
						}
						foreach (var box in boxes)
						{
							if (st2Request[s] < deliveryCount[s])
							{
								if (box.atDelivery && box.activeTexture == shelfLabelIndices[s])
								{
									box.SetBoxWarning(true);
									deliveryCount[s]--;
								}
							}
							else
								break;
						}
					} 
				}
				else
				{
					foreach (var box in boxes)
					{
						if (box.atShelf > 0) //if at one of the shelves
						{
							if (box.activeTexture != shelfLabelIndices[box.atShelf - 1]) //if not at the right shelf
							{
								box.SetBoxWarning(true);
								result = false;
							}
						}
						else   // if box is not in any shelf
						{
							box.SetBoxWarning(true);
							result = false;
						}
					} 
				}
				
				saverS.SaveText("CHECK Textured Boxes, isOrder: " + activeIsOrder);
				foreach (var box in boxes)
				{
					saverS.SaveText(box.name + " is " + !box.atWarning
						+ " Texture: " + box.activeTexture
						+ " At Delivery: " + box.atDelivery
						+ " At Shelf: " + box.atShelf
						+ " At Level: " + box.atLevel);
				}
				break;
			case 2:
				if (activeIsOrder)
				{
					List<boxS> deliveredBoxes = new List<boxS>();
					List<boxS> notDeliveredBoxes = new List<boxS>();
					foreach (var box in boxes)
					{
						if (box.atDelivery)
							deliveredBoxes.Add(box);
						else
							notDeliveredBoxes.Add(box);
					}
					List<int> tempRequest = new List<int>();
					foreach (var request in st3Request) tempRequest.Add(request);


					foreach (var box in deliveredBoxes)
					{
						if (tempRequest.Contains(box.targetLevelIndex))
						{
							tempRequest.Remove(box.targetLevelIndex);
						}
						else
						{
							box.SetBoxWarning(true);
							result = false;
						}
					}
					foreach (var box in notDeliveredBoxes)
					{
						if (tempRequest.Contains(box.targetLevelIndex))
						{
							tempRequest.Remove(box.targetLevelIndex);
							box.SetBoxWarning(true);
							result = false;
						}
						else
						{
							if (box.atShelf > 0) //if at one of the shelves
							{
								if ( box.targetLevelIndex != (box.atShelf - 1) * 3 + (box.atLevel - 1)) //if not at the right shelf
								{
									box.SetBoxWarning(true);
									result = false;
								}
							}
							else   // if box is not in any shelf
							{
								box.SetBoxWarning(true);
								result = false;
							}
						}
					}
				}
				else
				{
					foreach (var box in boxes)
					{
						if (box.atShelf > 0) //if at one of the shelves
						{
							if (box.targetLevelIndex != (box.atShelf - 1) * 3 + (box.atLevel - 1)) //if not at the right shelf
							{
								box.SetBoxWarning(true);
								result = false;
							}
						}
						else   // if box is not in any shelf
						{
							box.SetBoxWarning(true);
							result = false;
						}
					}
				}

				saverS.SaveText("CHECK Labeled Boxes, isOrder: " + activeIsOrder);
				foreach (var box in boxes)
				{
					saverS.SaveText(box.name + " is " + !box.atWarning
						+ " Requested Shelf: " + box.targetLevelIndex
						+ " At Delivery: " + box.atDelivery
						+ " At Shelf: " + box.atShelf
						+ " At Level: " + box.atLevel);
				}
				break;
		}

		//Debug.Log("Result: " + result);
		return result;
	}
	public void ChangeBoxLabels(List<boxS> _boxes)
	{
		switch (activeLevel)
		{
			case 0:
				//Not used
				break;
			case 1:
				foreach (var box in _boxes)
				{
					int tempIndex = Random.Range(0, 2);
					box.SetBoxTexture(boxTextures[shelfLabelIndices[tempIndex]], shelfLabelIndices[tempIndex]);
				}
				break;
			case 2:
				int[] tempBoxes = new int[6];
				foreach (var box in boxes) tempBoxes[box.targetLevelIndex]++;

				foreach (var box in _boxes)
				{
					int tempIndex = 0;
					do
					{
						tempIndex = Random.Range(0, 6);
					} while (tempBoxes[tempIndex] > 1);

					tempBoxes[box.targetLevelIndex]--;
					box.SetLabelTexture(st3BoxLabelTestures[tempIndex], tempIndex);
					tempBoxes[tempIndex]++;
				}
				break;
		}
	}

	public void ResetLevel()
	{
		//switch (activeLevel)
		//{
		//	case 0:
		//		break;
		//	case 1:
		//		break;
		//	case 2:
		//		break;
		//}

		foreach (Transform child in creations) Destroy(child.gameObject);

		foreach (var box in boxes)
		{
			box.ResetRotate();
			box.SetBoxWarning(false);
			box.ClearBoxTexture();
			box.labelObj.gameObject.SetActive(false);
		}
		foreach (var label in requestLabels)
		{
			label.gameObject.SetActive(false);
		}
		deliveryLabel.gameObject.SetActive(false);
		foreach (var shelflabel in shelfLabels)
		{
			shelflabel.material.mainTexture = null;
			shelflabel.gameObject.SetActive(false);
		}
		foreach (var numberlabel in st3ShelfLevelNumbers)
		{
			numberlabel.gameObject.SetActive(false);
		}
		foreach (var box in boxes)
		{
			box.ShowBox(false);
		}


		activeLevel = -1;
	}

	private void ChooseLabels()
	{
		shelfLabelIndices[0] = Random.Range(0, labelTextures.Length);

		do
		{
			shelfLabelIndices[1] = Random.Range(0, labelTextures.Length);
		} while (shelfLabelIndices[1] == shelfLabelIndices[0]);

		//Debug.Log("labels: 1:" + shelfLabelIndices[0] + " 2:" + shelfLabelIndices[1]);
	}
	private void ClearWarings()
	{
		foreach (var box in boxes)
		{
			box.SetBoxWarning(false);
		}
	}
	private void ShowRequest()
	{
		switch (activeLevel)
		{
			case 0:
				//Not used
				break;
			case 1:
				//Debug.Log("REQUEST: " + st2Request[0] + " adet " + shelfLabelIndices[0] + ", and " + st2Request[1] + " adet " + shelfLabelIndices[1]);
				saverS.SaveText("REQUEST Subtask2:");
				saverS.SaveText(st2Request[0] + " " + shelfLabelIndices[0] + ", and " + st2Request[1] + " " + shelfLabelIndices[1]);

				foreach (var label in requestLabels)
				{
					label.gameObject.SetActive(true);
				}
				deliveryLabel.gameObject.SetActive(false);

				if(st2Request[0]>0 && st2Request[1]>0)
				{
					if(Random.value>0.5f)
					{
						requestLabels[0].material.mainTexture = st2CountingTestures[st2Request[0]-1];
						requestLabels[1].material.mainTexture = labelTextures[shelfLabelIndices[0]];
						requestLabels[2].material.mainTexture = st2CountingTestures[st2Request[1]-1];
						requestLabels[3].material.mainTexture = labelTextures[shelfLabelIndices[1]];
					}
					else
					{
						requestLabels[0].material.mainTexture = st2CountingTestures[st2Request[1]-1];
						requestLabels[1].material.mainTexture = labelTextures[shelfLabelIndices[1]];
						requestLabels[2].material.mainTexture = st2CountingTestures[st2Request[0]-1];
						requestLabels[3].material.mainTexture = labelTextures[shelfLabelIndices[0]];
					}
				}
				else
				{
					if(st2Request[0]>0)
					{
						requestLabels[0].material.mainTexture = st2CountingTestures[st2Request[0]-1];
						requestLabels[1].material.mainTexture = labelTextures[shelfLabelIndices[0]];
						requestLabels[2].material.mainTexture = null;
						requestLabels[3].material.mainTexture = null;
					}
					else
					{
						requestLabels[0].material.mainTexture = st2CountingTestures[st2Request[1]-1];
						requestLabels[1].material.mainTexture = labelTextures[shelfLabelIndices[1]];
						requestLabels[2].material.mainTexture = null;
						requestLabels[3].material.mainTexture = null;
					}
				}
				break;
			case 2:
				foreach (var label in requestLabels)
				{
					label.gameObject.SetActive(true);
				}
				deliveryLabel.gameObject.SetActive(false);

				saverS.SaveText("REQUEST Subtask3:");
				for (int i = 0; i < requestLabels.Length-1; i++)
				{
					if (i < st3Request.Count)
					{
						requestLabels[i].material.mainTexture = st3BoxLabelTestures[st3Request[i]];
						saverS.SaveText(st3Request[i].ToString());
					}
					else
						requestLabels[i].material.mainTexture = null;
				}
				break;
		}
	}

	private IEnumerator MoveBoxes()
	{
		boxesMoving = true;
		List<boxS> movingBoxes = new List<boxS>();
		List<Vector3> movingBoxesPositions = new List<Vector3>();

		foreach (var box in boxes)
		{
			if (box.atDelivery)
			{
				movingBoxes.Add(box);
				movingBoxesPositions.Add(box.transform.position);
				box.updateVRNode = false;
			}
		}

		for (float t = 0; t <= 1.0; t+=Time.deltaTime / 2.0f)
		{
			magicBoxTreadMill.mainTextureOffset = new Vector2(magicBoxTreadMill.mainTextureOffset.x + treadMillSpeed * Time.deltaTime, 0.0f);
			for (int i = 0; i < movingBoxes.Count; i++)
			{
				movingBoxes[i].transform.position = movingBoxesPositions[i] + Vector3.Slerp(Vector3.zero, magicBoxPos.position - deliveryAreaPos.position, t);
			}
			yield return null;
		}

		ChangeBoxLabels(movingBoxes);

		foreach (var label in requestLabels)
		{
			label.gameObject.SetActive(false);
		}

		for (float t = 0; t <= 1.0; t += Time.deltaTime / 2.0f)
		{
			magicBoxTreadMill.mainTextureOffset = new Vector2(magicBoxTreadMill.mainTextureOffset.x - treadMillSpeed * Time.deltaTime, 0.0f);
			for (int i = 0; i < movingBoxes.Count; i++)
			{
				movingBoxes[i].transform.position = movingBoxesPositions[i] + Vector3.Slerp(magicBoxPos.position - deliveryAreaPos.position, Vector3.zero, t);
			}
			yield return null;
		}

		for (int i = 0; i < movingBoxes.Count; i++)
		{
			movingBoxes[i].transform.position = movingBoxesPositions[i];
			movingBoxes[i].updateVRNode = true;
		}
		deliveryLabel.gameObject.SetActive(true);

		activeIsOrder = false;
		boxesMoving = false;
	}

	[RPC]
	void MobileInput()
	{
		if (!boxesMoving && activeLevel >= 0)
		{
			if (shelvingM.visualM.visualsActive) shelvingM.visualM.ActivateVisuals(false);
			if (CheckBoxes())
			{
				timerS.setLastActivitiyTime();

				saverS.successCount++;
				saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

				if (activeIsOrder)
				{
					//PHOTON CODE
					generalManagerS.PhotonProgressUpdate();
					//PHOTON CODE

					StartCoroutine(MoveBoxes());
					shelvingM.visualM.visualNo = 2;
					shelvingM.visualM.ActivateVisuals(true);
				}
				else
				{
					cycle++;
					if (cycle < maxCycle)
					{
						//PHOTON CODE
						generalManagerS.PhotonProgressUpdate();
						//PHOTON CODE

						MakeRequest();
						shelvingM.visualM.visualNo = 2;
						shelvingM.visualM.ActivateVisuals(true);
					}
					else
					{
						//ResetLevel();
						isActive = false;
					}
				}
			}
			else
			{
				saverS.failCount++;
				saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

				//PHOTON CODE
				generalManagerS.PhotonProgressUpdate();
				//PHOTON CODE

				shelvingM.visualM.visualNo = 4;
				shelvingM.visualM.ActivateVisuals(true);
			}
		}
	}
}
