using UnityEngine;
using System.Collections;

public class characterS : MonoBehaviour {
	
	public bool allowWalkInPlace = true;
	public bool allowRealWalk = false;

	public GameObject[] holderObjects;
	public bool allowHold = true;
	public GameObject[] grasperObjects;
	public bool allowGrasp = true;
	
	private bool holdingOn = true; 		//always true at the beginning
	private bool graspingOn = true;		//always true at the beginning
	
	private Vector3 colliderOffset;
	private CharacterController charCont;
	
	private Transform RightHandModels;
	private Transform LeftHandModels;	
	// Use this for initialization
	void Awake () 
	{
		charCont = GetComponentInChildren<CharacterController>();
		colliderOffset = charCont.center;
		
		RightHandModels = GameObject.Find("HRModel").transform;
		LeftHandModels = GameObject.Find("HLModel").transform;			
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 temp = new Vector3( Mathf.Sin ( vrS.VRHead.eulerAngles.y * Mathf.Deg2Rad ) * colliderOffset.z,
									colliderOffset.y,
									Mathf.Cos ( vrS.VRHead.eulerAngles.y * Mathf.Deg2Rad ) * colliderOffset.z);
		charCont.center = temp;  //cozum
		
		if(holdingOn != allowHold){foreach (GameObject item in holderObjects) item.SetActive(allowHold); holdingOn = allowHold;}
		if(graspingOn != allowGrasp){foreach (GameObject item in grasperObjects) item.SetActive(allowGrasp); graspingOn = allowGrasp;}		
	}
	
	public void ChangeHandModel(bool _isRightHand, int _modelIndex)
	{
		if(_isRightHand) RightHandModels.GetComponent<handModelS>().ChangeModel(_modelIndex);
		else LeftHandModels.GetComponent<handModelS>().ChangeModel(_modelIndex);
	}
}
