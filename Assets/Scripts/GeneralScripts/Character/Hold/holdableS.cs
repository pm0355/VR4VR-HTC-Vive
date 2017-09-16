using UnityEngine;
using System.Collections;

public class holdableS : MonoBehaviour {

	GameObject RHModel;
	GameObject LHModel;
	GameObject RHPosition;
	GameObject LHPosition;	
	
	Vector3 RHRelativePosition;	
	Vector3 LHRelativePosition;	
	Vector3 LastLtoR;
	Quaternion LastRight;
	Quaternion LastLeft;
	
	Vector3 LastSpeed;
	Vector3 LastAngularSpeed;
	
	float InitialLtoRDistance;
	Vector3 BoxRelativePosition;
		
	public bool RightHandActivated {get;set;}
	public bool LeftHandActivated {get;set;}	
	bool droping=false;

//private BoxManager boxManager;
public bool virtualActive = true;	
	// Use this for initialization
	void Start () {
		
		RHModel = GameObject.Find("HRModel");
		LHModel = GameObject.Find("HLModel");
		RHPosition = GameObject.Find("HRHolder");
		LHPosition = GameObject.Find("HLHolder");			
		
		//LHModel.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
		//RHModel.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
		GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
		
//		boxManager = transform.root.GetComponent<BoxManager>();
//
//		if (boxManager.virtualBoxes) 
//		{
//			transform.rigidbody.isKinematic=false;
//			transform.collider.isTrigger = false;
//			virtualActive = true;
//		}
//		else
//		{
//			transform.rigidbody.isKinematic=true;
//			transform.collider.isTrigger = true;		
//			virtualActive = false;
//		}
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if (boxManager.virtualBoxes && !virtualActive) 
//		{
//			transform.rigidbody.isKinematic=false;
//			transform.collider.isTrigger = false;
//			virtualActive = true;
//		}
//		if (!boxManager.virtualBoxes && virtualActive) 
//		{
//			transform.rigidbody.isKinematic=true;
//			transform.collider.isTrigger = true;
//			virtualActive = false;
//		}


		if(virtualActive)
		{
			if(RightHandActivated && LeftHandActivated && !transform.rigidbody.isKinematic && !droping)
			{
				//LHModel.GetComponentInChildren<Renderer>().material.color = new Color(.5f, 1.0f, .5f);
				//RHModel.GetComponentInChildren<Renderer>().material.color = new Color(.5f, 1.0f, .5f);
				GetComponentInChildren<Renderer>().material.color = new Color(.5f, 1.0f, .5f);
				
				
				transform.rigidbody.isKinematic = true;
				transform.collider.isTrigger = true;
				RHModel.collider.isTrigger = true;
				LHModel.collider.isTrigger = true;
				RHModel.GetComponent<handModelS>().isTriggered = true;
				LHModel.GetComponent<handModelS>().isTriggered = true;
				
				//Initial hand parameters
				RHRelativePosition = transform.InverseTransformPoint(RHModel.transform.position);
				LHRelativePosition = transform.InverseTransformPoint(LHModel.transform.position);	
				InitialLtoRDistance = LastLtoR.magnitude;
				
				LastLtoR = RHPosition.transform.position - LHPosition.transform.position;
				LastRight = RHPosition.transform.rotation;
				LastLeft = LHPosition.transform.rotation;
			
				//Initial box parameters
				BoxRelativePosition = transform.position - (RHPosition.transform.position + LHPosition.transform.position)*0.5f;
			 	LastSpeed = Vector3.zero;
				LastAngularSpeed = Vector3.zero;
			}
			if(!(RightHandActivated && LeftHandActivated) && transform.rigidbody.isKinematic)
			{
				//If one hand is no more activated
			}	
			
			if(transform.rigidbody.isKinematic)
			{
				Vector3 initialPosition = transform.position;
				Vector3 initialRotation = transform.rotation.eulerAngles;

				transform.position = (RHPosition.transform.position + LHPosition.transform.position)*0.5f + BoxRelativePosition;

				//Rotation of the box in the World coordinate
				Quaternion temp = Quaternion.FromToRotation(LastLtoR, RHPosition.transform.position - LHPosition.transform.position);
				transform.Rotate(temp.eulerAngles,Space.World);
				LastLtoR = RHPosition.transform.position - LHPosition.transform.position;

				//y-axis rotation of the box in its own local coordinate
				float left_X_Rot1 = LastLeft.eulerAngles.x - LHPosition.transform.rotation.eulerAngles.x;
				if(left_X_Rot1 < -180.0) left_X_Rot1 += 360.0f;
				if(left_X_Rot1 >  180.0) left_X_Rot1 -= 360.0f;
				float right_X_Rot1 = LastRight.eulerAngles.x - RHPosition.transform.rotation.eulerAngles.x;
				if(right_X_Rot1 < -180.0) right_X_Rot1 += 360.0f;
				if(right_X_Rot1 >  180.0) right_X_Rot1 -= 360.0f;

				transform.Rotate(LastLtoR , -left_X_Rot1*0.5f, Space.World);
				transform.Rotate(LastLtoR , -right_X_Rot1*0.5f, Space.World);
				LastRight = RHPosition.transform.rotation;
				LastLeft = LHPosition.transform.rotation;

				//Hand model positions update
				RHModel.transform.position = transform.TransformPoint(RHRelativePosition);
				LHModel.transform.position = transform.TransformPoint(LHRelativePosition);			
				
				//Speed check
				LastSpeed = ( transform.position - initialPosition ) / Time.deltaTime;
				LastAngularSpeed = ( transform.rotation.eulerAngles - initialRotation ) / Time.deltaTime;
				//Debug.Log(LastSpeed.ToString() + LastAngularSpeed.ToString());
				
				
				
				/* Drop of the holdable if hands are too close */
				/*
				if( (LastLtoR.magnitude < InitialLtoRDistance*0.2f) && droping==false)
				{
					//LHModel.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
					//RHModel.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
					GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
					transform.rigidbody.isKinematic = false;
					transform.collider.isTrigger = false;
					RHModel.collider.isTrigger = false;
					LHModel.collider.isTrigger = false;					
				
					RHModel.GetComponent<handModelS>().isTriggered = false;
					LHModel.GetComponent<handModelS>().isTriggered = false;
					
					RHModel.transform.collider.isTrigger = true;
					LHModel.transform.collider.isTrigger = true;
					droping = true;
				}	
				*/
				/* Drop of the holdable if hands are too close */
				
				
				if( (LastLtoR.magnitude > InitialLtoRDistance*1.1f ) && droping==false)
				{
					//LHModel.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
					//RHModel.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
					GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
					transform.rigidbody.isKinematic = false;
					transform.collider.isTrigger = false;
					RHModel.collider.isTrigger = false;
					LHModel.collider.isTrigger = false;	
					
					RHModel.GetComponent<handModelS>().isTriggered = false;
					LHModel.GetComponent<handModelS>().isTriggered = false;

					//Speed update
					if(LastSpeed.magnitude > 0.5f)
					{
						transform.rigidbody.velocity = LastSpeed.magnitude < 2.0f ? LastSpeed : LastSpeed.normalized*2.0f;
						transform.rigidbody.angularVelocity = LastAngularSpeed;
					}
				}
			}
			
			if(!RightHandActivated && !LeftHandActivated && droping)
			{
				RHModel.transform.collider.isTrigger = false;
				LHModel.transform.collider.isTrigger = false;
				droping = false;			
			}
	//		Debug.DrawLine(transform.position, transform.position+RHRelativePosition*10.0f,Color.blue);
	//		Debug.DrawLine(transform.position, transform.position+LHRelativePosition*10.0f,Color.blue);		
	//		
	//		Debug.DrawLine(transform.position, transform.position+transform.up*10.0f,Color.white);
	//		Debug.DrawLine(transform.position, transform.position+transform.forward*10.0f,Color.white);
	//		
	//		Debug.DrawLine(LHModel.transform.position,RHModel.transform.position);
	//		Debug.DrawLine(LHPosition.transform.position,RHPosition.transform.position);
	//		
	//		Debug.DrawLine(LHModel.transform.position, LHModel.transform.position+LHModel.transform.up*10.0f,Color.green);
	//		Debug.DrawLine(RHModel.transform.position, RHModel.transform.position+RHModel.transform.up*10.0f,Color.red);
	//		Debug.DrawLine(LHPosition.transform.position, LHPosition.transform.position+LHPosition.transform.up*10.0f,Color.green);
	//		Debug.DrawLine(RHPosition.transform.position, RHPosition.transform.position+RHPosition.transform.up*10.0f,Color.red);	
	//		
	//		Debug.DrawLine(LHModel.transform.position, LHModel.transform.position+LHModel.transform.forward*10.0f,Color.green);
	//		Debug.DrawLine(RHModel.transform.position, RHModel.transform.position+RHModel.transform.forward*10.0f,Color.red);
	//		Debug.DrawLine(LHPosition.transform.position, LHPosition.transform.position+LHPosition.transform.forward*10.0f,Color.green);
	//		Debug.DrawLine(RHPosition.transform.position, RHPosition.transform.position+RHPosition.transform.forward*10.0f,Color.red);		
		}
	}
}
