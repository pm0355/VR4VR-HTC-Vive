using UnityEngine;
using System.Collections;

public class dragableS : MonoBehaviour {

	private Transform RHDrager;
	private Transform LHDrager;
	
	public bool RightHandActivated {get;set;}
	public bool LeftHandActivated {get;set;}	
	
	public bool draging {get;set;}
	
	private Vector3 directionLtoR;
	private Vector3 positionAverage;
	
	// Use this for initialization
	void Start () 
	{
		draging = false;
		
		RHDrager = GameObject.Find("2DRightHand").transform;
		LHDrager = GameObject.Find("2DLeftHand").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(RightHandActivated && LeftHandActivated && !draging)
		{
			draging = true;
			positionAverage = (RHDrager.position + LHDrager.position) / 2.0f;
			directionLtoR = RHDrager.position - LHDrager.position;
		}
		
		if(!RightHandActivated || !LeftHandActivated) draging = false;
		
		if(draging)
		{
			transform.Translate(((RHDrager.position + LHDrager.position) / 2.0f) - positionAverage, Space.World);

			if((RHDrager.position - LHDrager.position).sqrMagnitude > .04f)
			{
				Quaternion tempQ = Quaternion.FromToRotation(directionLtoR, RHDrager.position - LHDrager.position);
				transform.Rotate(tempQ.eulerAngles,Space.World);
			}
			
			positionAverage = (RHDrager.position + LHDrager.position) / 2.0f;
			directionLtoR = RHDrager.position - LHDrager.position;
		}
	}
}
