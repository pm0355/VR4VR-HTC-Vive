using UnityEngine;
using System.Collections;

public class forkliftS : MonoBehaviour {
	public Transform forklift;
	public Transform frontWheels;
	public Transform rearWheels;
	public Transform targetPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		forklift.Translate(new Vector3(2.0f * Time.deltaTime, 0.0f, 0.0f), Space.Self);
		frontWheels.Rotate(new Vector3(-240.0f * Time.deltaTime, 0.0f, 0.0f), Space.Self);
		rearWheels.Rotate(new Vector3(-300.0f * Time.deltaTime, 0.0f, 0.0f), Space.Self);

		if (forklift.position.z < targetPos.position.z)
			Destroy(gameObject);
	}
}
