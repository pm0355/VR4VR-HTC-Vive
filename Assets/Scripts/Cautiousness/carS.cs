using UnityEngine;
using System.Collections;

public class carS : MonoBehaviour {

	public GameObject[] carObjs;
	private carMovementS carMover;

	private Vector3[] path;
	private float totolLenght;
	private float movedLenght;

	public int numberOfPaths;

	public float randomMaxSpeed;
	public float randomMaxSpeedRange;
	public float acceleration;

	private float maxSpeed;
	private float speed;
	private Vector3 lastPos;

	private bool run = true;
	private NavMeshObstacle obstacle;

	// Use this for initialization
	void Start () {
		GameObject tempObj = Instantiate(carObjs[Random.Range(0,carObjs.Length)], transform.position, Quaternion.identity) as GameObject;
		tempObj.name = "CarModel";
		tempObj.transform.parent = transform;
		carMover = tempObj.GetComponent<carMovementS>();

		maxSpeed = randomMaxSpeed + Random.Range(-randomMaxSpeedRange, randomMaxSpeedRange);

		int RandomPath = Random.Range(0, numberOfPaths);
		path = iTweenPath.GetPath("Path" + RandomPath.ToString());
		totolLenght = iTween.PathLength(path);

		speed = maxSpeed;
		lastPos = transform.position;

		iTween.PutOnPath(gameObject, path, 0.0f);
		iTween.LookTo(gameObject, iTween.PointOnPath(path, 0.001f), 0.0f);

		obstacle = GetComponentInChildren<NavMeshObstacle>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (run && speed != maxSpeed) speed = Mathf.Clamp(speed + Time.deltaTime * acceleration, 0.0f, maxSpeed);
		if (!run && speed != 0.0f) speed = Mathf.Clamp(speed - Time.deltaTime * acceleration * 10.0f, 0.0f, maxSpeed);
		movedLenght += speed * Time.deltaTime;

		float percentageRoad = Mathf.Clamp01(movedLenght / totolLenght);

		Vector3 pos = iTween.PointOnPath(path, percentageRoad);
		iTween.MoveUpdate(gameObject, iTween.Hash("position", pos, "orienttopath", true));

		carMover.SetWheels((transform.position - lastPos).magnitude, transform.position - lastPos);
		lastPos = transform.position;

		LayerMask collisionMask = (1 << 12);
		RaycastHit[] hits;
		float castSize = run ? 0.75f : 1.0f;
		float castDistance = run ? 6.0f : 8.0f; 
		Vector3 startP = transform.position + transform.forward * -2.0f + transform.right * -castSize + transform.up * 1.0f;
		Vector3 endP = transform.position + transform.forward * -2.0f + transform.right * 2.0f * castSize + transform.up * 1.0f;

		Debug.DrawLine(startP - transform.forward * 0.5f + transform.right * -0.5f, endP - transform.forward * 0.5f + transform.right * 0.5f);
		Debug.DrawLine(startP + transform.forward * (0.5f + castDistance) + transform.right * -0.5f, endP + transform.forward * (0.5f + castDistance) + transform.right * 0.5f);

		hits = Physics.CapsuleCastAll(startP, endP, 0.5f, transform.forward, castDistance, collisionMask);
		if(hits.Length>1)
		{
			if (run) run = false;
		}
		else
		{
			if (!run) run = true;
		}

		if (speed == 0.0f && !run)
		{
			obstacle.carving = true;
			obstacle.radius = 0.1f;
		}
		else
		{
			obstacle.carving = false;
			obstacle.radius = 0.25f;
		}

		if (percentageRoad == 1.0f)
		{
			Destroy(gameObject);
			transform.root.GetComponent<trafficManagerS>().numberOfCars--;
		}
	}
}
