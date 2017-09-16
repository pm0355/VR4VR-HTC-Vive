using UnityEngine;
using System.Collections;

public class otherCharactersS : MonoBehaviour {

	public GameObject[] customerObjs;
	private float[] nextTime;

	public float period = 40.0f;
	public float range = 15.0f;

	// Use this for initialization
	void Start () 
	{
		nextTime = new float[customerObjs.Length];

		for (int i = 0; i < nextTime.Length; i++)
		{
			nextTime[i] = Time.timeSinceLevelLoad + Random.Range(period - range, period + range);
			//Debug.Log(i+" --- "+nextTime[i]);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < nextTime.Length; i++)
		{
			if(Time.timeSinceLevelLoad > nextTime[i])
			{
				GameObject instance = Instantiate(customerObjs[i]) as GameObject;
				instance.transform.parent = transform;
				nextTime[i] = Time.timeSinceLevelLoad + Random.Range(period - range, period + range);
				//Debug.Log(i + " --- " + nextTime[i]);
			}
		}
	}
}
