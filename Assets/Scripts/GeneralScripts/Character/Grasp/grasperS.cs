using UnityEngine;
using System.Collections;

public class grasperS : MonoBehaviour {
	
	public handModelS handModelsObj;
	
	private Transform graspCenter;
	private Vector3 graspCenterRelPos;	
	private bool grasping;
	private graspableS graspedObj;	
			
	// Use this for initialization
	void Awake () 
	{
		graspCenter = transform;
		graspCenterRelPos = transform.localPosition;
		grasping = false;
		handModelsObj.ChangeModel(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(grasping)
		{
			graspedObj.transform.position = graspCenter.position;
			
			graspedObj.transform.rotation = graspCenter.rotation;
		}
	}
	
    void OnTriggerEnter(Collider other) 
	{
		if(other.transform.GetComponent<graspableS>()!=null && !grasping)
		{
			graspedObj = other.transform.GetComponent<graspableS>();
			if(!graspedObj.Grasped)
			{
				graspedObj.Grasped = true;
				grasping = true;
				handModelsObj.ChangeModel(1);
				other.rigidbody.isKinematic = true;
				graspCenter.position = other.transform.position;
				graspCenter.rotation = other.transform.rotation;
				graspedObj.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, .5f, .5f);

				saverS.SaveText(transform.parent.name + " grasped at " + transform.position.ToString()); 
			}
		}
		
		if(other.transform.GetComponent<graspReleaserS>()!=null && grasping)
		{
			grasping = false;
			handModelsObj.ChangeModel(0);
			graspedObj.rigidbody.isKinematic = false;
			graspedObj.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
			
			transform.localPosition = graspCenterRelPos;

			saverS.SaveText(transform.parent.name + " released at " + transform.position.ToString()); 	
		}		
    }	
	
    void OnTriggerExit(Collider other) 
	{
		if(other.transform.GetComponent<graspableS>()!=null && !grasping)
		{
			graspedObj = other.transform.GetComponent<graspableS>();
			if(graspedObj.Grasped)
			{
				graspedObj.Grasped = false;
			}
		}		
    }		
	
}
