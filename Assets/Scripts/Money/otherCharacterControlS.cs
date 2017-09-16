using UnityEngine;
using System.Collections;

public class otherCharacterControlS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.x < -11.0f || transform.position.x > 11.0f)
			Destroy(transform.parent.gameObject);
	}
}
