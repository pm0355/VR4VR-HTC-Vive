using UnityEngine;
using System.Collections;

public class lowObstacleS : MonoBehaviour {
	public Transform[] textures;

	// Use this for initialization
	void Start () 
	{
		Destroy(textures[Random.Range(0, textures.Length)].gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
