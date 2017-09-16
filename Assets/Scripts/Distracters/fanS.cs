using UnityEngine;
using System.Collections;

public class fanS : MonoBehaviour {

	private GameObject stableFan;
	public Transform fanModel;
	private float speed;
	private bool isSlowingDown;

	private float startTime;
	private float totalTime = 14.0f;
	

	// Use this for initialization
	void Start () 
	{
		stableFan = GameObject.Find("StableFan");
		stableFan.SetActive(false);

		startTime = Time.timeSinceLevelLoad;

		speed = 0;
		isSlowingDown = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isSlowingDown && speed <= 300.0f) speed += 5.0f;
		if (isSlowingDown && speed >= 30.0f) speed -= 5.0f;

		fanModel.Rotate(0.0f, speed*Time.deltaTime, 0.0f, Space.World);

		if (!isSlowingDown && Time.timeSinceLevelLoad > startTime + totalTime) isSlowingDown = true;

		if (isSlowingDown && speed < 30.0f && fanModel.localEulerAngles.y > 355.0f)
		{
			Destroy(gameObject);
		}



		//if it can not catch the position 20secs
		if (Time.timeSinceLevelLoad > startTime + 20.0f)
		{
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		stableFan.SetActive(true);
	}
}
