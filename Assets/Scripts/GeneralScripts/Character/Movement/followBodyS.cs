using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]	

public class followBodyS : MonoBehaviour {	
	
	public float headWeight=0.4f;
	public float rfootWeight=0.3f;	
	public float lfootWeight=0.3f;	
		
	public Vector3 center {get;set;}
		
	private CharacterController controller;
	private characterS character; 
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<CharacterController>();
		character = transform.parent.GetComponent<characterS>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 prevCenter = center;
		center = (vrS.VRHead.position*headWeight + vrS.VRFootRight.position*rfootWeight + vrS.VRFootLeft.position*lfootWeight);
		center = (vrS.HeadCenter.position*headWeight + vrS.FootRight.position*rfootWeight + vrS.FootLeft.position*lfootWeight); //cozum

		Physics.IgnoreLayerCollision (9, 10);
		
		if (character.allowRealWalk) 
		{
			Vector3 horizontalMove = center - prevCenter;
			horizontalMove.y = 0.0f;
			transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
			controller.Move (horizontalMove);
		} 
	}
}
