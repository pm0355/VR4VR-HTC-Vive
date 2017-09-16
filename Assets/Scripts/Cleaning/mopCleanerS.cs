using UnityEngine;
using System.Collections;

public class mopCleanerS : MonoBehaviour {

	public Transform cleaner;
		
	private Transform RightHandTransform;
	private Transform LeftHandTransform;	
	
	private Transform RightHandGripPos;
	private Transform LeftHandGripPos;		
	
	private Transform RightHandModels;
	private Transform LeftHandModels;		
	
	public bool mopHeld {get;set;}

	// Use this for initialization
	void Awake () {
		RightHandTransform = GameObject.Find("HandRight").transform;
		LeftHandTransform = GameObject.Find("HandLeft").transform;
		
		RightHandGripPos = GameObject.Find("HRGripCenter").transform;
		LeftHandGripPos = GameObject.Find("HLGripCenter").transform;		
		
		RightHandModels = GameObject.Find("HRModel").transform;
		LeftHandModels = GameObject.Find("HLModel").transform;			
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mopHeld)
		{
			RaycastHit hit;
			if(Physics.Raycast( RightHandGripPos.position, LeftHandGripPos.position-RightHandGripPos.position, out hit, 1.4f, 256))
			{
				Vector3 forwardProj = transform.forward-(Vector3.Dot(transform.forward, Vector3.up)*Vector3.up);
				float distFromHit = 0.05f / Mathf.Sin( Vector3.Angle(RightHandGripPos.position-LeftHandGripPos.position, forwardProj)*Mathf.Deg2Rad );
				
				transform.position = hit.point + distFromHit*(RightHandGripPos.position-LeftHandGripPos.position).normalized;

				if(forwardProj != Vector3.zero) cleaner.rotation = Quaternion.LookRotation(forwardProj, Vector3.up);
			}
			else
			{
				transform.position = RightHandGripPos.position + (LeftHandGripPos.position-RightHandGripPos.position).normalized * 1.3f;
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
			RightHandTransform.rotation = Quaternion.LookRotation(newForward, -Vector3.Cross(newForward, -handleDir));
			
			dotProd = Vector3.Dot(vrS.VRHandLeft.transform.forward, handleDir);
			if(dotProd != 0.0f) newForward = vrS.VRHandLeft.transform.forward - dotProd*handleDir;
			else newForward = vrS.VRHandLeft.transform.forward;
			//Debug.DrawRay(LeftHandTransform.position, newForward, Color.red);
			LeftHandTransform.rotation = Quaternion.LookRotation(newForward, Vector3.Cross(newForward, -handleDir));
//			
			
		}
	}
	
	public void HoldMop()
	{
		RightHandModels.GetComponent<handModelS>().ChangeModel(1);
		LeftHandModels.GetComponent<handModelS>().ChangeModel(1);
		
		RightHandTransform.GetComponent<followVRS>().restrictXRot = true;
		RightHandTransform.GetComponent<followVRS>().restrictYRot = true;
		RightHandTransform.GetComponent<followVRS>().restrictZRot = true;
		LeftHandTransform.GetComponent<followVRS>().restrictXRot = true;
		LeftHandTransform.GetComponent<followVRS>().restrictYRot = true;
		LeftHandTransform.GetComponent<followVRS>().restrictZRot = true;
		
		mopHeld = true;
	}
	public void UnholdMop()
	{
		RightHandModels.GetComponent<handModelS>().ChangeModel(0);
		LeftHandModels.GetComponent<handModelS>().ChangeModel(0);
		
		RightHandTransform.GetComponent<followVRS>().restrictXRot = false;
		RightHandTransform.GetComponent<followVRS>().restrictYRot = false;
		RightHandTransform.GetComponent<followVRS>().restrictZRot = false;		
		LeftHandTransform.GetComponent<followVRS>().restrictXRot = false;
		LeftHandTransform.GetComponent<followVRS>().restrictYRot = false;
		LeftHandTransform.GetComponent<followVRS>().restrictZRot = false;
		
		mopHeld = false;
	}	
}
