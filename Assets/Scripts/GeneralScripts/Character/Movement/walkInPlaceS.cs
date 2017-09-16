using UnityEngine;
using System.Collections;

enum foot 
{
	none,
	right,
	left
}

[RequireComponent (typeof (CharacterController))]	
public class walkInPlaceS : MonoBehaviour {

	private CharacterController controller;
	private characterS character; 
		
	bool RightFootUp;	
	bool LeftFootUp;	
	foot upFoot;	
	int WalkingState;	
	float LastStateChangeTime;
	
	float oneFootCycle;
	float stepCycle;
	
	public float oneStepLenght = 0.5f;	
	public float firstStepTime = 0.5f;	
	public float acc = 5.0f;
	public float heightTheshold = 0.1f;
		
	float vel = 0.0f;
	float max_vel = 0.0f;
		
	bool walking;

	// Use this for initialization
	void Start () 
	{		
		controller = GetComponent<CharacterController>();
		character = transform.parent.GetComponent<characterS>();
		
		WalkingState = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(character.allowWalkInPlace)
		{
			//Debug.Log("state: "+WalkingState);
			switch(WalkingState)
			{
			case 0:									//Reset State	
				if(!RightFootUp && !LeftFootUp) 
				{
					upFoot = foot.none;
					walking = false;
					WalkingState = 1;
				}
				break;
			case 1:									//Initial State; Looking for one rise
				if(RightFootUp ^ LeftFootUp) 
				{
					if(RightFootUp) upFoot = foot.right;
					if(LeftFootUp) upFoot = foot.left;
					//if(upFoot == foot.left) Debug.Log("Left foot started");
					//if(upFoot == foot.right) Debug.Log("Right foot started");
					
					LastStateChangeTime = Time.timeSinceLevelLoad;
					walking = false;
					WalkingState = 2;					
				}
				break;
			case 2:									//Initial State; Looking for one fall
				if(Time.timeSinceLevelLoad < LastStateChangeTime + firstStepTime)
				{
					if((upFoot==foot.right && !RightFootUp) || (upFoot==foot.left && !LeftFootUp))
					{
						//if(upFoot == foot.left) Debug.Log("Left foot ended");
						//if(upFoot == foot.right) Debug.Log("Right foot ended");					
						
						oneFootCycle = Time.timeSinceLevelLoad - LastStateChangeTime;
						//Debug.Log("one foot cycle: "+oneFootCycle);
						
						LastStateChangeTime = Time.timeSinceLevelLoad;
						walking = false;
						WalkingState = 3;
					}
				}
				else
				{
					//Debug.Log("****************************walking stoped 2");
					WalkingState = 0;
				}
				break;
			case 3:									//Looking for other rise									
				if(Time.timeSinceLevelLoad < LastStateChangeTime + firstStepTime)
				{
					if((upFoot==foot.right && LeftFootUp) || (upFoot==foot.left && RightFootUp))
					{
						if(RightFootUp) upFoot = foot.right;
						if(LeftFootUp) upFoot = foot.left;
						//if(upFoot == foot.left) Debug.Log("Left foot started");
						//if(upFoot == foot.right) Debug.Log("Right foot started");
						
						stepCycle = oneFootCycle + (Time.timeSinceLevelLoad - LastStateChangeTime);
						//Debug.Log("step cycle: "+stepCycle);
						
						max_vel = oneStepLenght/stepCycle;
						
						LastStateChangeTime = Time.timeSinceLevelLoad;
						walking = true;
						WalkingState = 4;
					}
				}
				else
				{
					//Debug.Log("****************************walking stoped 3");
					WalkingState = 0;
				}
				break;
			case 4:									//Looking for other fall
				//if(Time.timeSinceLevelLoad < LastStateChangeTime + stepCycle*1.2f)
				if(Time.timeSinceLevelLoad < LastStateChangeTime + firstStepTime)
				{
					if((upFoot==foot.right && !RightFootUp) || (upFoot==foot.left && !LeftFootUp))
					{
						//if(upFoot == foot.left) Debug.Log("Left foot ended");
						//if(upFoot == foot.right) Debug.Log("Right foot ended");					
						
						oneFootCycle = Time.timeSinceLevelLoad - LastStateChangeTime;
						//Debug.Log("one foot cycle: "+oneFootCycle);
						
						LastStateChangeTime = Time.timeSinceLevelLoad;
						walking = true;
						WalkingState = 3;
					}
				}
				else
				{
					//Debug.Log("****************************walking stoped 4");
					WalkingState = 0;
				}
				break;			
			}
			
			if(walking)
			{
				if(vel<max_vel)
				{
					vel+=acc*Time.deltaTime;
					if(vel>max_vel)
					{
						vel=max_vel;
					}
				}
				if(vel>max_vel)
				{
					vel-=acc*Time.deltaTime;
				}			
			}
			else
			{
				if(vel>0.0f)
				{
					vel-=acc*Time.deltaTime;
					if(vel<0.0f)
					{
						vel=0.0f;
					}
				}		
			}

			//CONTROLLER MOVE
			Vector3 walkDirection = vrS.VRHead.forward;
			walkDirection.y=0.0f;
			transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
			controller.Move(walkDirection * vel * Time.deltaTime);

			if (vrS.VRFootRight.position.y - vrS.VRFootLeft.position.y > heightTheshold || Input.GetKey(KeyCode.RightArrow)) RightFootUp = true;
			else RightFootUp=false;
			if (vrS.VRFootLeft.position.y - vrS.VRFootRight.position.y > heightTheshold || Input.GetKey(KeyCode.LeftArrow)) LeftFootUp = true;
			else LeftFootUp=false;				
			
		}
	}
}
