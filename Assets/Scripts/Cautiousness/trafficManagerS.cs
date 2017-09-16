using UnityEngine;
using System.Collections;

public class trafficManagerS : MonoBehaviour {

	public GameObject pedestrainObj;
	public Transform[] pedestrainSpawnPoints;
	public int maxNumberOfPedestrains;
	public float pedestrainPeriod;
	public float pedestrainPeriodRange;
	private float nextPedestrainTime;

	public GameObject carObj;
	public int maxNumberOfCars;
	public float carPeriod;
	public float carPeriodRange;
	private float nextCarTime;

	public GameObject parkingCarObj;
	public Transform[] carPositions;

	public GameObject lowObstacleObj;
	public int numberOfLowObstacles;

	public int numberOfPedestrains { get; set; }
	public int numberOfCars { get; set; }

	private bool pedestrainsActive = false;
	private bool carsActive = false;

	private Transform creations;

	public GameObject targetObj;
	public GameObject targetCartObj;
	private int maxTargetCount;
	private int targetCount;
	private bool targetIsCart;

	public Transform player;

	public bool isPlaying { get; set; }

	public cautiousnessVisualManagerS visualM;
	
	void Awake () 
	{
		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;

		//PlaceParkingCars(0.2f);
		//CreateTargets(10);
	}
	void Update () 
	{
		if (pedestrainsActive)
		{
			if (Time.timeSinceLevelLoad > nextPedestrainTime && numberOfPedestrains < maxNumberOfPedestrains)
			{
				int random1 = Random.Range(0, pedestrainSpawnPoints.Length);
				int random2 = Random.Range(0, pedestrainSpawnPoints.Length);
				while (Mathf.Abs(random2 - random1) < 3) random2 = Random.Range(0, pedestrainSpawnPoints.Length);
				int random3 = Random.Range(0, pedestrainSpawnPoints.Length);
				while (random3 == random2) random3 = Random.Range(0, pedestrainSpawnPoints.Length);

				GameObject tempObj = Instantiate(pedestrainObj, pedestrainSpawnPoints[random1].position, Quaternion.identity) as GameObject;
				tempObj.name = "Pedestrain Instance";
				tempObj.transform.parent = creations;
				numberOfPedestrains++;

				tempObj.GetComponent<pedestrianS>().targetPos = pedestrainSpawnPoints[random2].position;
				tempObj.GetComponent<pedestrianS>().alternativeTargetPos = pedestrainSpawnPoints[random3].position;

				//Debug.Log("Pedestrain created from:" + random1 + " to " + random2 + " at " + Time.timeSinceLevelLoad);

				nextPedestrainTime = Time.timeSinceLevelLoad + pedestrainPeriod + Random.Range(-pedestrainPeriodRange, pedestrainPeriodRange);
			} 
		}

		if (carsActive)
		{
			if (Time.timeSinceLevelLoad > nextCarTime && numberOfCars < maxNumberOfCars + (int)(generalManagerS.generalVariable*6.0f))
			{
				GameObject tempObj = Instantiate(carObj) as GameObject;
				tempObj.name = "Car Instance";
				tempObj.transform.parent = creations;
				numberOfCars++;

				//Debug.Log("Car created at " + Time.timeSinceLevelLoad);

				nextCarTime = Time.timeSinceLevelLoad + carPeriod + Random.Range(-carPeriodRange, carPeriodRange);
			} 
		}
	}

	public void PlaceLowLevelObstacles()
	{
		for (int i = 0; i < numberOfLowObstacles; i++)
		{
			Vector3 range = GetComponent<BoxCollider>().size * 0.8f;
			Vector3 refPos = GetComponent<BoxCollider>().center;

			GameObject tempObj = Instantiate(lowObstacleObj, refPos + new Vector3(Random.Range(range.x * -0.5f, range.x * 0.5f), 0.0f, Random.Range(range.z * -0.5f, range.z * 0.5f)), Quaternion.identity) as GameObject;
			tempObj.transform.localScale = Vector3.one * Random.Range(0.8f, 1.5f);
			tempObj.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0, 359), 0.0f);

			tempObj.name = "LowObstacle";
			tempObj.transform.parent = creations;
		}
	}
	public void PlaceParkingCars(float _percentage)
	{
		for(int i=0; i<6; i++)
		{
			for(int j=0; j<10; j++)
			{
				if(!((i==1 && j==5) || (i==3 && j==2) || (i==5 && j==8)))
				{
					if(Random.value<_percentage)
					{
						GameObject tempObj = Instantiate(parkingCarObj, carPositions[i].position + new Vector3(0.0f, 0.0f, 2.5f*j), Quaternion.identity) as GameObject;

						tempObj.name = "Parking Car";
						tempObj.transform.parent = creations;

						if (Random.value < 0.5f)
							tempObj.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
						else
							tempObj.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
					}
				}
			}
		}
	}
	public void ActivateTraffic(bool _pedestrainsAreActive, bool _carsAreActive)
	{
		pedestrainsActive = _pedestrainsAreActive;
		carsActive = _carsAreActive;
	}
	public void ClearItems()
	{
		ActivateTraffic(false, false);
		foreach (Transform child in creations) Destroy(child.gameObject);
		isPlaying = false;

		numberOfPedestrains = 0;
		numberOfCars = 0;

		nextPedestrainTime = 0.0f;
		nextCarTime = 0.0f;
	}

	public void CreateTargetAt(Vector3 _targetPos, bool _isCart)
	{
		maxTargetCount = 1;
		targetCount = 0;
		isPlaying = true;

		GameObject tempObj = null;
		if(!_isCart)
			tempObj = Instantiate(targetObj, _targetPos, Quaternion.identity) as GameObject;
		else
			tempObj = Instantiate(targetCartObj, _targetPos, Quaternion.identity) as GameObject;

		saverS.SaveText("Target created at: " + _targetPos.ToString());
		targetCount++;

		tempObj.name = "Target";
		tempObj.transform.parent = creations;
	}
	public void CreateTargets(int _maxCount, bool _isCart)
	{
		maxTargetCount = _maxCount;
		targetCount = 0;
		isPlaying = true;
		targetIsCart = _isCart;

		CreateTarget();
	}
	private void CreateTarget()
	{
		GameObject tempObj = null;
		if (!targetIsCart)
		{
			Vector3 range = GetComponent<BoxCollider>().size;
			Vector3 refPos = GetComponent<BoxCollider>().center;
			Vector3 instancePos = Vector3.zero;
			bool collide = false;
			do
			{
				instancePos = refPos + new Vector3(Random.Range(range.x * -0.5f, range.x * 0.5f), 0.0f, Random.Range(range.z * -0.5f, range.z * 0.5f));
				collide = Physics.CheckCapsule(new Vector3(transform.TransformPoint(instancePos).x, 0.8f, transform.TransformPoint(instancePos).z), new Vector3(transform.TransformPoint(instancePos).x, 1.00f, transform.TransformPoint(instancePos).z), 0.75f);
			}
			while (collide || (player.position - instancePos).sqrMagnitude < 200.0f);

			tempObj = Instantiate(targetObj, instancePos, Quaternion.identity) as GameObject;
			saverS.SaveText("Target created at: " + instancePos.ToString());
		}
		else
		{
			Vector3 range = new Vector3(4.0f, 0.0f, 12.5f);
			Vector3 refPos = new Vector3(0.0f, 0.0f, 22.5f);
			Vector3 instancePos = Vector3.zero;
			bool collide = false;
			do
			{
				instancePos = refPos + new Vector3(Random.Range(range.x * -0.5f, range.x * 0.5f), 0.0f, Random.Range(range.z * -0.5f, range.z * 0.5f));
				instancePos.x += (Random.Range(0, 3) - 1) * 18.0f;
				collide = Physics.CheckCapsule(new Vector3(transform.TransformPoint(instancePos).x, 0.8f, transform.TransformPoint(instancePos).z), new Vector3(transform.TransformPoint(instancePos).x, 1.00f, transform.TransformPoint(instancePos).z), 0.75f);
			}
			while (collide || (player.position - instancePos).sqrMagnitude < 200.0f);

			tempObj = Instantiate(targetCartObj, instancePos, Quaternion.identity) as GameObject;
			tempObj.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0, 359), 0.0f);
			saverS.SaveText("Target created at: " + instancePos.ToString());
		}
		targetCount++;

		visualM.info = "Destinations: " + (maxTargetCount - targetCount +1).ToString();

		tempObj.name = "Target";
		tempObj.transform.parent = creations;
	}
	public void TargetReached() 
	{
		visualM.ActivateVisuals(false);
		timerS.setLastActivitiyTime();

		saverS.successCount++;
		saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

		if(targetCount<maxTargetCount)
		{
			//PHOTON CODE
			generalManagerS.PhotonProgressUpdate();
			//PHOTON CODE

			visualM.visualNo = 2;
			visualM.ActivateVisuals(true);

			CreateTarget();
		}
		else
		{
			isPlaying = false;
			//finished
		}		
	}
	public void Failed()
	{
		saverS.failCount++;
		saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

		//PHOTON CODE
		generalManagerS.PhotonProgressUpdate();
		//PHOTON CODE

		visualM.ActivateVisuals(false);
		visualM.visualNo = 4;
		visualM.ActivateVisuals(true);
	}
}
