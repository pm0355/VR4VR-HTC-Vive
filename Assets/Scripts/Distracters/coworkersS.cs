using UnityEngine;
using System.Collections;

public class coworkersS : MonoBehaviour {

	public Transform coworker1;
	public Transform coworker2;
	public Transform targetPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!audio.isPlaying && coworker1.position.z > targetPos.position.z && coworker2.position.z > targetPos.position.z) 
			Destroy(gameObject);
	}
}
