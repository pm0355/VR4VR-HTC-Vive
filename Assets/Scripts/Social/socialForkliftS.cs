using UnityEngine;
using System.Collections;

public class socialForkliftS : MonoBehaviour {

	public float speed = 10.0f;
	public float wheelSpeed = 10.0f;
	public Transform[] wheels;
	public Transform model;

	private Vector3 spawnPos = new Vector3(0.0f, 0.0f, 9.56f);

	// Use this for initialization
	void Start () 
	{
		transform.position = spawnPos;
		transform.eulerAngles = new Vector3(0.0f, Random.value < 0.5f ? 0.0f : 180.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		model.Translate(speed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
		foreach (Transform wheel in wheels)
		{
			wheel.Rotate(wheelSpeed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
		}

		if (model.localPosition.x < -19.0f) Destroy(gameObject);
	}
}
