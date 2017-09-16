using UnityEngine;
using System.Collections;

public class vacuumCleanerS : MonoBehaviour {

	public Transform cleaner;
	public Transform collectedDirt;
		
	private Transform RightHandTransform;
	private Transform LeftHandTransform;	
	
	private Transform RightHandGripPos;
	private Transform LeftHandGripPos;			
	
	public bool vacuumOn {get;set;}
	public bool vacuumHeld {get;set;}

	// Use this for initialization
	void Awake () {
		vacuumOn = true;
		
		RightHandTransform = GameObject.Find("HandRight").transform;
		LeftHandTransform = GameObject.Find("HandLeft").transform;
		
		RightHandGripPos = GameObject.Find("HRGripCenter").transform;
		LeftHandGripPos = GameObject.Find("HLGripCenter").transform;				
	}
	
	void Start () {
		
	}	
	
	// Update is called once per frame
	void Update () 
	{
		if(vacuumHeld)
		{
			RaycastHit hit;
			if(Physics.Raycast( RightHandGripPos.position, LeftHandGripPos.position-RightHandGripPos.position, out hit, 1.4f, 256))
			{
				transform.position = hit.point;
			
				Vector3 forwardProj = transform.forward-(Vector3.Dot(transform.forward, Vector3.up)*Vector3.up);
				if(forwardProj != Vector3.zero) cleaner.rotation = Quaternion.LookRotation(forwardProj, Vector3.up);
			}
			else
			{
				transform.position = RightHandGripPos.position + (LeftHandGripPos.position-RightHandGripPos.position).normalized * 1.4f;
			}
			transform.rotation = Quaternion.LookRotation(LeftHandGripPos.position-RightHandGripPos.position);
			
			
			
			/*Hand rotations*/
			float dotProd = 0.0f;
			Vector3 newForward = Vector3.zero;
			Vector3 handleDir = (LeftHandGripPos.position-RightHandGripPos.position).normalized;
			
			dotProd = Vector3.Dot(vrS.VRHandRight.transform.forward, handleDir);
			if(dotProd != 0.0f) newForward = vrS.VRHandRight.transform.forward - dotProd*handleDir;
			else newForward = vrS.VRHandRight.transform.forward;
			//Debug.DrawRay(RightHandTransform.position, newForward, Color.red);
			RightHandTransform.rotation = Quaternion.LookRotation(newForward, -Vector3.Cross(newForward, handleDir));
			
			dotProd = Vector3.Dot(vrS.VRHandLeft.transform.forward, handleDir);
			if(dotProd != 0.0f) newForward = vrS.VRHandLeft.transform.forward - dotProd*handleDir;
			else newForward = vrS.VRHandLeft.transform.forward;
			//Debug.DrawRay(LeftHandTransform.position, newForward, Color.red);
			LeftHandTransform.rotation = Quaternion.LookRotation(newForward, Vector3.Cross(newForward, handleDir));
		}
		
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
	
	public void HoldVacuum()
	{
		RightHandTransform.GetComponent<followVRS>().restrictXRot = true;
		RightHandTransform.GetComponent<followVRS>().restrictYRot = true;
		RightHandTransform.GetComponent<followVRS>().restrictZRot = true;
		LeftHandTransform.GetComponent<followVRS>().restrictXRot = true;
		LeftHandTransform.GetComponent<followVRS>().restrictYRot = true;
		LeftHandTransform.GetComponent<followVRS>().restrictZRot = true;
		
		vacuumHeld = true;
		vacuumOn = true;
	}
	public void UnholdVacuum()
	{
		RightHandTransform.GetComponent<followVRS>().restrictXRot = false;
		RightHandTransform.GetComponent<followVRS>().restrictYRot = false;
		RightHandTransform.GetComponent<followVRS>().restrictZRot = false;		
		LeftHandTransform.GetComponent<followVRS>().restrictXRot = false;
		LeftHandTransform.GetComponent<followVRS>().restrictYRot = false;
		LeftHandTransform.GetComponent<followVRS>().restrictZRot = false;
		
		vacuumHeld = false;
		vacuumOn = false;
	}	
}
