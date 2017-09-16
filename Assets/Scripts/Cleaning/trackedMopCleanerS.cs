using UnityEngine;
using System.Collections;

public class trackedMopCleanerS : MonoBehaviour {

	public Transform cleaner;	
	
	public bool mopHeld {get;set;}

	// Use this for initialization
	void Awake () 
	{
		transform.parent = GameObject.Find("CharacterCollider").transform;
		
		GetComponent<followVRS>().followNode = GameObject.Find("StickCenter").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
    void OnTriggerStay(Collider other) 
	{
        if(other.gameObject.name == "Ground")
		{
			Vector3 forwardProj = transform.forward-(Vector3.Dot(transform.forward, Vector3.up)*Vector3.up);
			if(forwardProj != Vector3.zero) cleaner.rotation = Quaternion.LookRotation(forwardProj, Vector3.up);
		}	        
    }
}
