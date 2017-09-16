using UnityEngine;
using System.Collections;

public class socialCharacterDestroyerS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Mathf.Abs( transform.position.x ) > 19.0f) Destroy(gameObject);
	}
}
