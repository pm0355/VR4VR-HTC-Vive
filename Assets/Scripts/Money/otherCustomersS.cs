using UnityEngine;
using System.Collections;

public class otherCustomersS : MonoBehaviour
{

	public GameObject[] customerObjs;
	public Transform[] customerSpawnPoints;
	public Transform customerEndPoint;

	public GameObject[] boxesObjs;
	public Transform boxSpawnPoint;

	private GameObject boxes1Instance;
	private GameObject boxes2Instance;

	private MoneyCustomer[] customers;

	private GameObject customer1Instance;
	private GameObject customer2Instance;

	// Use this for initialization
	void Start()
	{
		customers = new MoneyCustomer[2];
		customers[0].type = customers[1].type = -1;

		CreateCustomer(1);
		customers[1].spawnTime = Time.timeSinceLevelLoad + Random.Range(5.0f, 15.0f);
	}

	// Update is called once per frame
	void Update()
	{
		if(customers[0].isActive && customer1Instance.transform.GetChild(0).position.x < customerEndPoint.position.x)
		{
			Destroy(customer1Instance);
			Destroy(boxes1Instance);
			customers[0].isActive = false;

			customers[0].spawnTime = Time.timeSinceLevelLoad + Random.Range(10.0f, 30.0f);
			customers[0].boxesActive = false;
			customers[0].bagsActive = false;
		}
		if (customers[1].isActive && customer2Instance.transform.GetChild(0).position.x < customerEndPoint.position.x)
		{
			Destroy(customer2Instance);
			Destroy(boxes2Instance);
			customers[1].isActive = false;

			customers[1].spawnTime = Time.timeSinceLevelLoad + Random.Range(10.0f, 30.0f);
			customers[1].boxesActive = false;
			customers[1].bagsActive = false;
		}

		if (!customers[0].isActive && Time.timeSinceLevelLoad > customers[0].spawnTime) CreateCustomer(1);
		if (!customers[1].isActive && Time.timeSinceLevelLoad > customers[1].spawnTime) CreateCustomer(2);

		if (customers[0].isActive && (!customers[0].boxesActive || !customers[0].bagsActive))
		{
			AnimatorStateInfo stateInfo1 = customers[0].anim.GetCurrentAnimatorStateInfo(0);
			if (!customers[0].boxesActive && stateInfo1.IsName("Idle"))
			{
				boxes1Instance = Instantiate(boxesObjs[0], boxSpawnPoint.position, boxSpawnPoint.rotation) as GameObject;
				boxes1Instance.transform.parent = transform;				
				customers[0].boxesActive = true;
			}

			if (!customers[0].bagsActive && stateInfo1.IsName("Exit"))
			{
				customer1Instance.transform.GetChild(0).Find("master/Reference/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/Bag").renderer.enabled = true;
				customer1Instance.transform.GetChild(0).Find("master/Reference/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Bag").renderer.enabled = true;

				customers[0].bagsActive = true;
			}
		}
		if (customers[1].isActive && (!customers[1].boxesActive || !customers[1].bagsActive))
		{
			AnimatorStateInfo stateInfo2 = customers[1].anim.GetCurrentAnimatorStateInfo(0);
			if (!customers[1].boxesActive && stateInfo2.IsName("Idle"))
			{
				boxes2Instance = Instantiate(boxesObjs[1], boxSpawnPoint.position, boxSpawnPoint.rotation) as GameObject;
				boxes2Instance.transform.parent = transform;
				customers[1].boxesActive = true;
			}

			if (!customers[1].bagsActive && stateInfo2.IsName("Exit"))
			{
				customer2Instance.transform.GetChild(0).Find("master/Reference/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/Bag").renderer.enabled = true;
				customer2Instance.transform.GetChild(0).Find("master/Reference/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Bag").renderer.enabled = true;

				customers[1].bagsActive = true;
			}
		}
	}

	void CreateCustomer(int _customerID)
	{
		if(_customerID==1 && !customers[0].isActive)
		{
			int newType = Random.Range(0, 4);
			while(newType==customers[0].type || newType==customers[1].type)
			{
				newType = Random.Range(0, 4);
			}
			customers[0].type = newType;
			customer1Instance = Instantiate(customerObjs[newType], customerSpawnPoints[0].position, customerSpawnPoints[0].rotation) as GameObject;
			customer1Instance.transform.parent = transform;
			customers[0].isActive = true;
			customers[0].anim = customer1Instance.transform.GetChild(0).GetComponent<Animator>();
		}

		else if (_customerID == 2 && !customers[1].isActive)
		{
			int newType = Random.Range(0, 4);
			while (newType == customers[0].type || newType == customers[1].type)
			{
				newType = Random.Range(0, 4);
			}
			customers[1].type = newType;
			customer2Instance = Instantiate(customerObjs[newType], customerSpawnPoints[1].position, customerSpawnPoints[1].rotation) as GameObject;
			customer2Instance.transform.parent = transform;
			customers[1].isActive = true;
			customers[1].anim = customer2Instance.transform.GetChild(0).GetComponent<Animator>();
		}
	}
}

public struct MoneyCustomer
{
	public bool isActive;
	public int type;
	public float spawnTime;

	public Animator anim;
	public bool boxesActive;

	public bool bagsActive;
}
