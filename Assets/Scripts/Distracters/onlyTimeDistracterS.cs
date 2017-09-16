using UnityEngine;
using System.Collections;

public class onlyTimeDistracterS : MonoBehaviour {
	private float startTime;
	public float life = 15.0f;

	// Use this for initialization
	void Start () 
	{
		startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > startTime + life)
			Destroy(gameObject);
	}
}
