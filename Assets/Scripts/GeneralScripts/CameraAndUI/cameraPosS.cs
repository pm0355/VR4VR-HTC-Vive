using UnityEngine;
using System.Collections;

public class cameraPosS : MonoBehaviour {
public Camera[] cameras;	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Camera camera in cameras) {
			camera.transform.position = transform.position;
			camera.transform.rotation = transform.rotation;
		}
	}
}
