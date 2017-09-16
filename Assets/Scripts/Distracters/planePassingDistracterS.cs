using UnityEngine;
using System.Collections;

public class planePassingDistracterS : MonoBehaviour {
	
	public Transform plane;

	private float speed = 0.02f;
	private float x = -20.0f;
	private float y = 3.0f;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		plane.Translate(x * speed * Time.deltaTime, y * speed * Time.deltaTime, 0.0f);

		if(!audio.isPlaying) Destroy(gameObject);
	}
}
