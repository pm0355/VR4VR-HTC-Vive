using UnityEngine;
using System.Collections;

public class fireworksS : MonoBehaviour {

	public GameObject fireworksPar;
	public Transform fireworksPos;
	public Vector3 size;

	private float period = 1.3f;
	private float periodRange = 0.3f;
	private float nextTime;
	private float startTime;
	private int count = 0;

	// Use this for initialization
	void Start () 
	{
		startTime = Time.timeSinceLevelLoad;
		nextTime = startTime + period + Random.Range(-periodRange, periodRange);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.timeSinceLevelLoad > nextTime && Time.timeSinceLevelLoad < startTime + 8.0f)
		{
			Vector3 pos = fireworksPos.position + new Vector3(Random.Range(-size.x * 0.5f, size.x * 0.5f), Random.Range(-size.y * 0.5f, size.y * 0.5f), Random.Range(-size.z * 0.5f, size.z * 0.5f));
			GameObject tempObj = Instantiate(fireworksPar, pos, Quaternion.identity) as GameObject;
			tempObj.name = "Firework Instance " + count.ToString();
			tempObj.transform.parent = transform;

			nextTime = Time.timeSinceLevelLoad + period + Random.Range(-periodRange, periodRange);
			count++;
		}

		if (Time.timeSinceLevelLoad > startTime + 10.0f)
		{
			Destroy(gameObject);
		}
	}
}
