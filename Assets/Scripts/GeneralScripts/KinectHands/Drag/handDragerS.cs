using UnityEngine;
using System.Collections;

public class handDragerS : MonoBehaviour {

	public bool isRightHand;

	private bool pushed = false;
	
	private dragableS movingDragable;
	private dragableS onDragable;

	public handDragerS otherHand;

	private float armLenght;

	private Material handMaterial;
	public Texture[] handTextures;
	
	// Use this for initialization
	void Start () 
	{
		armLenght = transform.parent.GetComponent<KinectHandsS>().armLenght;
		handMaterial = transform.GetComponentInChildren<Renderer>().material;

		handMaterial.mainTexture = handTextures[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Vector3 shouldersLtoR = kinectS.ShoulderRight.position - kinectS.ShoulderLeft.position;
		Vector3 shoulderToHand = isRightHand ? kinectS.HandRight.position - kinectS.ShoulderRight.position : kinectS.HandLeft.position - kinectS.ShoulderLeft.position;
		

		/*   Same code with middleVR kinect data

		Vector3 sholdersLtoR = vrS.VRShoulderRight.position - vrS.VRShoulderLeft.position;
		Vector3 shoulderToHand = isRightHand ? vrS.VRHandRight.position - vrS.VRShoulderRight.position : vrS.VRHandLeft.position - vrS.VRShoulderLeft.position;

		*/


		/*
		float dotP = Vector3.Dot( shoulderToHand, (-shouldersLtoR).normalized);
		Vector3 dist = shoulderToHand - shouldersLtoR.normalized * (-dotP);
		dist.y=0;
		if(dist.magnitude > armLenght*0.7f && !pushed) pushed = true;
		if(dist.magnitude < armLenght*0.7f && pushed)	pushed = false;
		*/


		float dotP = Vector3.Dot( shoulderToHand.normalized, Vector3.down);

		if(dotP>.707f) pushed = false;
		else
		{
			if(shoulderToHand.magnitude > armLenght*0.8f) pushed = true;
			if(shoulderToHand.magnitude < armLenght*0.8f) pushed = false;
		}

		RaycastHit2D hit;
		hit = Physics2D.Raycast(transform.position, Vector3.forward, 1.5f, 2048);
		if(hit.collider != null)
			onDragable = hit.transform.GetComponent<dragableS>();
		else
			onDragable = null;

		if(pushed && onDragable && !movingDragable) 
		{
			movingDragable = onDragable;
			if(isRightHand) movingDragable.RightHandActivated = true;
			else movingDragable.LeftHandActivated = true;
		}

		if(movingDragable)
		{
			if(movingDragable == otherHand.movingDragable)
			{
				if(!pushed && !otherHand.pushed)
				{
					if(isRightHand) movingDragable.RightHandActivated = false;
					else movingDragable.LeftHandActivated = false;
					movingDragable = null;
				}
			}
			else
			{
				if(!pushed || !onDragable)
				{
					if(isRightHand) movingDragable.RightHandActivated = false;
					else movingDragable.LeftHandActivated = false;
					movingDragable = null;
				}
				else if(onDragable!=movingDragable)
				{
					if(isRightHand) movingDragable.RightHandActivated = false;
					else movingDragable.LeftHandActivated = false;
					movingDragable = onDragable;
					if(isRightHand) movingDragable.RightHandActivated = true;
					else movingDragable.LeftHandActivated = true;
				}
			}
		}

		if(pushed)
		{
			handMaterial.color = new Color( 0.702f, 1.0f, 0.502f );
			if(movingDragable && movingDragable == otherHand.movingDragable)
				handMaterial.mainTexture = handTextures[1];
			else 
				handMaterial.mainTexture = handTextures[0];
		}
		else
		{
			handMaterial.color = new Color( 1.0f, 1.0f, 1.0f );
			handMaterial.mainTexture = handTextures[0];
		}
	}
}
