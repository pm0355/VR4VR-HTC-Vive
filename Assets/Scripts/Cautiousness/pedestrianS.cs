using UnityEngine;
using System.Collections;

public class pedestrianS : MonoBehaviour {

	private NavMeshAgent agent;
	public Vector3 targetPos { get; set; }
	public Vector3 alternativeTargetPos { get; set; }

	public GameObject[] pedestrainObjs;
	public RuntimeAnimatorController[] walkAnimationControllers;

	public float dirChangeProbablility;
	public float dirChangeBaseTime;
	public float dirChangeTimeWindow;
	private bool changeDir;
	private float dirChangeTime;

	// Use this for initialization
	void Start () 
	{
		GameObject tempObj = Instantiate(pedestrainObjs[Random.Range(0, pedestrainObjs.Length)], transform.position, Quaternion.identity) as GameObject;
		tempObj.name = "PedestrainModel";
		tempObj.transform.parent = transform;
		tempObj.GetComponent<Animator>().runtimeAnimatorController = walkAnimationControllers[Random.Range(0, walkAnimationControllers.Length)];

		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(targetPos);

		agent.speed = Random.Range(0.8f, 1.8f);
		agent.GetComponentInChildren<Animator>().speed = agent.speed * 0.8f;

		if(Random.value < dirChangeProbablility)
		{
			changeDir = true;
			dirChangeTime = Time.timeSinceLevelLoad + dirChangeBaseTime + Random.value * dirChangeTimeWindow;
		}
		else
		{
			changeDir = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.DrawLine(transform.position, targetPos);

		if (changeDir && dirChangeTime > Time.timeSinceLevelLoad)
		{
			agent.SetDestination(alternativeTargetPos);
			changeDir = false;
		}

		if((transform.position-targetPos).sqrMagnitude<3.0f)
		{
			Destroy(gameObject);
			transform.root.GetComponent<trafficManagerS>().numberOfPedestrains--;
		}
	}
}
