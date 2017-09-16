using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadingBoxManagerS : MonoBehaviour
{
	private List<LoadingBoxLocation> boxLocations = new List<LoadingBoxLocation>();
	private List<loadingBoxS> boxes = new List<loadingBoxS>();
	private Transform allBoxes;
	public GameObject boxObj;
	
	public Transform distributionCenter;
	
	private int activeBox = -1;
	private Vector3 lastMousePos;
	
	private bool isCheckingSleep = false;
	public bool isPlaying { get; set; }
	
	//public Camera activeCamera;
	
	public int remainingBoxes { get; set; }
	
	public loadingVisualManagerS visualM;
	public GravityEffect omniM;
	
	public loadingCursorS cursor;
	
	//private bool hapticKey = true;
	//private bool hapticKeyDown = false;
	//private bool hapticKeyUp = false;
	
	void Start () 
	{
		allBoxes = new GameObject().transform;
		allBoxes.name = "All Boxes";
		allBoxes.parent = transform;
		
		isPlaying = false;
		isCheckingSleep = false;
	}
	void Update () 
	{
		//hapticKeyDown = false;
		//if(!hapticKey && hapticKey.On())
		//{
		//	hapticKeyDown = true;
		//	hapticKey = true;
		//}
		//hapticKeyUp = false;
		//if (hapticKey && !hapticKey.On())
		//{
		//	hapticKeyUp = true;
		//	hapticKey = false;
		//}
		if(isPlaying)
		{
			if ( (!omniM.isOmniControlActive && Input.GetMouseButtonDown(0)) || (omniM.isOmniControlActive && omniM.button1Activated) )
			{
				//Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
				Ray ray = new Ray(cursor.transform.position, Vector3.forward);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
				
				if (hit.collider != null && hit.collider.transform.GetComponent<loadingBoxS>())
				{
					activeBox = hit.collider.transform.GetComponent<loadingBoxS>().order;
					boxes[activeBox].ActivatePhysics(false);
					//lastMousePos = activeCamera.ScreenToWorldPoint(Input.mousePosition);
					lastMousePos = cursor.transform.position;
					
					Vector3 temp = boxes[activeBox].transform.position;
					temp.z = distributionCenter.position.z + (boxLocations.Count + 1) * -0.2f;
					boxes[activeBox].transform.position = temp;
					
					cursor.SetSprite(true);
					if (omniM.isOmniControlActive) 
					{
						if(boxes[activeBox].hasHeavyLabel) omniM.ApplyGravity(0.7f);
						else omniM.ApplyGravity(0.4f);
					}
				}
				
				isCheckingSleep = false;
			}
			
			if (activeBox >= 0 && (Input.GetMouseButtonUp(0) || omniM.button1Deactivated))
			{
				Vector3 temp = boxes[activeBox].transform.position;
				temp.z = distributionCenter.position.z + boxes[activeBox].order * -0.2f;
				boxes[activeBox].transform.position = temp;
				
				activeBox = -1;
				
				CheckNewBoxPosition();
				
				cursor.SetSprite(false);
				if (omniM.isOmniControlActive) omniM.ApplyGravity(0.1f);
			}
			
			if (activeBox >= 0)
			{
				//boxes[activeBox].transform.Translate(activeCamera.ScreenToWorldPoint(Input.mousePosition) - lastMousePos, Space.World);
				boxes[activeBox].transform.Translate(cursor.transform.position - lastMousePos, Space.World);
				//lastMousePos = activeCamera.ScreenToWorldPoint(Input.mousePosition);
				lastMousePos = cursor.transform.position;
			}
			
			if ((!omniM.isOmniControlActive && Input.GetMouseButtonDown(1)) || (omniM.isOmniControlActive && omniM.button2Activated))
			{
				//Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
				Ray ray = new Ray(cursor.transform.position, Vector3.forward);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
				
				if (activeBox >= 0)
				{
					boxes[activeBox].transform.RotateAround(ray.origin, ray.direction, 90.0f);
				}
				else
				{
					if (hit.collider != null && hit.collider.transform.GetComponent<loadingBoxS>())
					{
						hit.collider.transform.RotateAround(ray.origin, ray.direction, 90.0f);
						hit.collider.transform.GetComponent<loadingBoxS>().ActivatePhysics(false);
					}
					CheckNewBoxPosition();
				}
			}
			CheckAllBoxes();
		}
		/*
		if (Input.GetKeyDown(KeyCode.W))
		{
			for (int i = 0; i < boxes.Count; i++)
			{
				boxes[i].transform.position = (Vector3)boxLocations[i].position + new Vector3(0.0f, 0.0f, i * 0.2f);
				boxes[i].transform.rotation = Quaternion.identity;
			}
		}
		*/
	}
	void CheckNewBoxPosition()
	{
		bool tempPhysicsActive = true;
		
		foreach (loadingBoxS box in boxes)
		{
			if (box.physicsActive == false)
			{
				if (box.CheckPosition())
				{
					box.ActivatePhysics(true);
				}
				else
				{
					tempPhysicsActive = false;
				}
			}
		}
		if (tempPhysicsActive) isCheckingSleep = true;
	}
	void CheckAllBoxes()
	{
		if(isCheckingSleep)
		{
			bool allSleep = true;
			foreach (loadingBoxS box in boxes)
			{
				if (!box.rigidbody2D.IsSleeping())
				{
					allSleep = false;
					break;
				}
			}
			
			if(allSleep)
			{
				bool allLabelsCorrect = true;
				foreach (loadingBoxS box in boxes)
				{
					if (!box.CheckLabels()) allLabelsCorrect = false;
				}
				if (allLabelsCorrect)
				{
					timerS.setLastActivitiyTime();
					saverS.successCount++;
					saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

					if (boxes.Count < boxLocations.Count)
					{
						//PHOTON CODE
						generalManagerS.PhotonProgressUpdate();
						//PHOTON CODE

						CreateBox(boxes.Count);
						
						visualM.visualNo = 2;
						if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
						visualM.visualOn = true;
					}
					else
					{
						//level ended
						isPlaying = false;
					}
				}
				else
				{
					//wrong label
				}
				isCheckingSleep = false;
			}
		}
		
		
	}
	
	public void DivideArea(int _partCount, float _sizeModifier, bool _hasLabels)
	{
		boxLocations.Add(new LoadingBoxLocation(transform.position, transform.lossyScale));
		
		bool firstOrderRandom = Random.value < 0.5f;
		for (int i = 0; i < _partCount-1; i++)
		{
			if (firstOrderRandom)
			{
				if (i == 0) DivideOnce(true, _sizeModifier);
				else if (i == 1) DivideOnce(false, _sizeModifier);
				else DivideOnce(Random.value < 0.5f ? true : false, _sizeModifier);
			}
			else
			{
				if (i == 0) DivideOnce(false, _sizeModifier);
				else if (i == 1) DivideOnce(true, _sizeModifier);
				else DivideOnce(Random.value < 0.5f ? true : false, _sizeModifier);
			}
		}
		
		if(_hasLabels)
		{
			for (int i = 0; i < _partCount; i++) ProduceObj(i);
			for (int i = 0; i < _partCount; i++) ShrinkObj(i);
			ChooseUp();
			ChooseHeavy();
			ChooseFragile();
			ClearItems();
			//DebugBoxes();
		}
		
		remainingBoxes = _partCount;
		CreateBox(0);
		isPlaying = true;
	}
	
	void DivideOnce(bool isHorizontal, float _sizeModifier)
	{
		//VARIABLE 1//
		if (isHorizontal)
		{
			boxLocations.Sort(delegate(LoadingBoxLocation x, LoadingBoxLocation y)
			                  {
				if (x.size.y < y.size.y) return 1;
				else return -1;
			});
		}
		else
		{
			boxLocations.Sort(delegate(LoadingBoxLocation x, LoadingBoxLocation y)
			                  {
				if (x.size.x < y.size.x ) return 1;
				else return -1;
			});
		}
		int boxIndex = 0;
		//VARIABLE 1//
		
		//VARIABLE 2//
		//float sizeModifier = 0.99f;	 //1% - 99%
		//float sizeModifier = 0.5f;	//25% - 75%
		//float sizeModifier = 0.2f;	//40% - 60%
		//float sizeModifier = 0.0f;	//50%
		//VARIABLE 2//
		
		
		Vector2 boxXRange = new Vector2(boxLocations[boxIndex].position.x - boxLocations[boxIndex].size.x / 2.0f, boxLocations[boxIndex].position.x + boxLocations[boxIndex].size.x / 2.0f);
		Vector2 boxYRange = new Vector2(boxLocations[boxIndex].position.y - boxLocations[boxIndex].size.y / 2.0f, boxLocations[boxIndex].position.y + boxLocations[boxIndex].size.y / 2.0f);
		
		Vector2 dividePoint = new Vector2(Random.Range(boxLocations[boxIndex].position.x - (boxLocations[boxIndex].size.x / 2.0f) * _sizeModifier, boxLocations[boxIndex].position.x + (boxLocations[boxIndex].size.x / 2.0f) * _sizeModifier), Random.Range(boxLocations[boxIndex].position.y - (boxLocations[boxIndex].size.y / 2.0f) * _sizeModifier, boxLocations[boxIndex].position.y + (boxLocations[boxIndex].size.y / 2.0f) * _sizeModifier));
		
		Vector2 box1Pos = Vector2.zero;
		Vector2 box1Size = Vector2.zero;
		Vector2 box2Pos = Vector2.zero;
		Vector2 box2Size = Vector2.zero;
		
		if(isHorizontal)
		{
			box1Pos = new Vector2(boxLocations[boxIndex].position.x, (boxYRange.x + dividePoint.y) / 2.0f);
			box1Size = new Vector2(boxLocations[boxIndex].size.x, dividePoint.y - boxYRange.x);
			
			box2Pos = new Vector2(boxLocations[boxIndex].position.x, (boxYRange.y + dividePoint.y) / 2.0f);
			box2Size = new Vector2(boxLocations[boxIndex].size.x, boxYRange.y - dividePoint.y);
		}
		else
		{
			box1Pos = new Vector2((boxXRange.x + dividePoint.x) / 2.0f, boxLocations[boxIndex].position.y);
			box1Size = new Vector2(dividePoint.x - boxXRange.x, boxLocations[boxIndex].size.y);
			
			box2Pos = new Vector2((boxXRange.y + dividePoint.x) / 2.0f, boxLocations[boxIndex].position.y);
			box2Size = new Vector2(boxXRange.y - dividePoint.x, boxLocations[boxIndex].size.y);
		}
		
		LoadingBoxLocation box1 = new LoadingBoxLocation(box1Pos, box1Size);
		LoadingBoxLocation box2 = new LoadingBoxLocation(box2Pos, box2Size);
		
		boxLocations.RemoveAt(boxIndex);
		boxLocations.Add(box1);
		boxLocations.Add(box2);
	}
	int  ChooseHeavy()
	{
		int totalHeavyCount = Random.Range(1, Mathf.Min((int)(boxLocations.Count / 1.25f), 4));
		//Debug.Log("Heavy Count:" + totalHeavyCount);
		int heavyCount = 0;
		
		List<loadingBoxS> tempBoxes = new List<loadingBoxS>();
		foreach (var item in boxes) tempBoxes.Add(item);
		
		while (heavyCount < totalHeavyCount && tempBoxes.Count > 0)
		{
			int index = Random.Range(0, tempBoxes.Count);
			
			RaycastHit2D[] boxesBelow = Physics2D.BoxCastAll(tempBoxes[index].transform.position, tempBoxes[index].transform.lossyScale, 0.0f, -Vector2.up, transform.localScale.y, 2048);
			bool noBoxBelow = true;
			
			foreach (RaycastHit2D box in boxesBelow)
			{
				if (!box.transform.GetComponent<loadingBoxS>().hasHeavyLabel)
				{
					noBoxBelow = false;
					break;
				}
			}
			if (noBoxBelow)
			{
				boxLocations[index].isHeavy = true;
				heavyCount++;
			}
			tempBoxes.RemoveAt(index);
		}
		
		//Debug.Log("Created Heavy Count:" + heavyCount);
		return heavyCount;
	}
	void ChooseUp()
	{
		int totalUpCount = Random.Range(1, Mathf.Min((int)(boxLocations.Count / 1.25f), 4));
		//Debug.Log("Up Count:" + totalUpCount);
		int upCount = 0;
		
		while (upCount < totalUpCount)
		{
			int index = Random.Range(0, boxLocations.Count);
			if (!boxLocations[index].isUp)
			{
				boxLocations[index].isUp = true;
				upCount++;
			}
		}
	}
	void ChooseFragile()
	{
		int totalUpCount = Random.Range(1, Mathf.Min((int)(boxLocations.Count / 1.25f), 4));
		//Debug.Log("Fragile Count:" + totalUpCount);
		int fragileCount = 0;
		
		while (fragileCount < totalUpCount)
		{
			int index = Random.Range(0, boxLocations.Count);
			if (!boxLocations[index].isFragile && (!boxLocations[index].isHeavy || !boxLocations[index].isUp))
			{
				boxLocations[index].isFragile = true;
				fragileCount++;
			}
		}
	}
	public void ClearItems()
	{
		foreach (Transform child in allBoxes) Destroy(child.gameObject);
		boxes.Clear();
	}
	public void ClearAll()
	{
		ClearItems();
		boxLocations.Clear();
	}
	
	void CreateBox(int _index)
	{
		ProduceObj(_index);
		ShrinkObj(_index);
		UpdateLabel(_index);
		
		boxes[_index].transform.position = distributionCenter.position + new Vector3(0.0f, 0.0f, _index * -0.2f);
		boxes[_index].transform.Rotate(0.0f, 0.0f, Random.Range(0, 4) * 90.0f);
		
		remainingBoxes--;
	}
	
	void ProduceObj(int _index)
	{
		GameObject newBox = Instantiate(boxObj, (Vector3)boxLocations[_index].position, Quaternion.identity) as GameObject;
		newBox.transform.localScale = boxLocations[_index].size;
		newBox.GetComponent<loadingBoxS>().order = _index;
		newBox.gameObject.name = "Box" + _index;
		newBox.transform.parent = allBoxes;
		
		boxes.Add(newBox.GetComponent<loadingBoxS>());
	}
	void ShrinkObj(int _index)
	{
		boxes[_index].transform.localScale = boxes[_index].transform.localScale - new Vector3(0.05f / transform.lossyScale.x, 0.05f / transform.lossyScale.y, -1.0f);
		boxes[_index].GetComponent<BoxCollider2D>().size = new Vector2((boxes[_index].transform.localScale.x - 0.05f / transform.lossyScale.x) / boxes[_index].transform.localScale.x, (boxes[_index].transform.localScale.y - 0.05f / transform.lossyScale.y) / boxes[_index].transform.localScale.y);
		Vector2 temp = boxes[_index].GetComponent<BoxCollider2D>().size;
		if (temp.x < 0.051f) temp.x = 0.051f;
		if (temp.y < 0.051f) temp.y = 0.051f;
		boxes[_index].GetComponent<BoxCollider2D>().size = temp;
	}
	void UpdateLabel(int _index)
	{
		if (boxLocations[_index].isHeavy) boxes[_index].hasHeavyLabel = true;
		if (boxLocations[_index].isUp) boxes[_index].hasUpLabel = true;
		if (boxLocations[_index].isFragile) boxes[_index].hasFragileLabel = true;
		boxes[_index].UpdateLabel();
		
		saverS.SaveText(boxes[_index].transform.localScale.x.ToString() + " by " + boxes[_index].transform.localScale.y.ToString()
		                + ", Heavy: " + boxLocations[_index].isHeavy.ToString()
		                + ", Up: " + boxLocations[_index].isUp.ToString()
		                + ", Fragile: " + boxLocations[_index].isFragile.ToString());
	}
	
	void DebugBoxes()
	{
		Debug.Log("------------------------------");
		Debug.Log("Number of boxes:" + boxLocations.Count);
		int i = 0;
		foreach (var box in boxLocations)
		{
			Debug.Log(i++);
			Debug.Log(box.isHeavy + " " + box.isUp + " " + box.isFragile);
			//Debug.Log("Heavy:" + box.isHeavy);
			//Debug.Log("Up:" + box.isUp);
			//Debug.Log("Fragile:" + box.isFragile);
		}
		Debug.Log("------------------------------");
	}
	
	public void CreateTutorial(bool _hasLabel)
	{
		LoadingBoxLocation tutorialBox = new LoadingBoxLocation(Vector2.zero, new Vector2(Random.Range(0.40f, 0.50f), Random.Range(0.54f, 0.64f)));
		boxLocations.Add(tutorialBox);
		
		if(_hasLabel)
		{
			boxLocations[0].isHeavy = true;
			boxLocations[0].isUp = true;
		}
		
		remainingBoxes = 1;
		CreateBox(0);
		isPlaying = true;
	}

	public void PauseHepticDevice()
	{
		if(omniM.isOmniWanted && omniM.isOmniControlActive)
		{
			omniM.StopOmni();
		}
	}
	public void ResumeHepticDevice()
	{
		if(omniM.isOmniWanted && !omniM.isOmniControlActive)
		{
			omniM.StartOmni();
		}		
	}
}

public class LoadingBoxLocation
{
	public Vector2 position { get; set; }
	public Vector2 size { get; set; }
	
	public bool isHeavy { get; set; }
	public bool isUp { get; set; }
	public bool isFragile { get; set; }
	
	
	public LoadingBoxLocation()
	{
		position = Vector2.zero;
		size = Vector2.one;
	}
	
	public LoadingBoxLocation(Vector2 _pos, Vector2 _size)
	{
		position = _pos;
		size = _size;
	}
	
	~LoadingBoxLocation()
	{
		
	}
}
