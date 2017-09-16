using UnityEngine;
using System.Collections;

public class trackedVacuumCleanerS : MonoBehaviour {

	public Transform cleaner;
	public Transform collectedDirt;	
	
	public bool vacuumOn {get;set;}

	// Use this for initialization
	void Awake () 
	{
		vacuumOn = true;
		transform.parent = GameObject.Find("CharacterCollider").transform;
		
		GetComponent<followVRS>().followNode = GameObject.Find("StickCenter").transform;
	}
	
	void Start () {
		
	}	
	
	// Update is called once per frame
	void Update () 
	{
		if(vacuumOn)
		{
			collectedDirt.Rotate(Vector3.up*500.0f*Time.deltaTime,Space.Self);
			if(!audio.isPlaying) audio.Play();
		}
		else
		{
			if(audio.isPlaying) audio.Stop();
		}
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
