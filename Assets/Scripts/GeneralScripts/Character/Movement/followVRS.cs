using UnityEngine;
using System.Collections;

public class followVRS : MonoBehaviour {
	
	public Transform followNode;

	public bool relative = true;
	
	public bool restrictXRot = false;
	public bool restrictYRot = false;
	public bool restrictZRot = false;
		
	private followBodyS body;
	
	// Use this for initialization
	void Start () 
	{
		body = transform.parent.GetComponent<followBodyS>();
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if (relative)
		{
			transform.localPosition = followNode.position;

			Vector3 tempRot = transform.localEulerAngles;
			if (!restrictXRot) tempRot.x = followNode.rotation.eulerAngles.x;
			if (!restrictYRot) tempRot.y = followNode.rotation.eulerAngles.y;
			if (!restrictZRot) tempRot.z = followNode.rotation.eulerAngles.z;
			transform.localEulerAngles = tempRot;
			//transform.localRotation = followNode.rotation;	

			Vector3 temp = body.center;
			temp.y = 0;
			transform.position -= temp;
		}
		else
		{
			transform.position = followNode.position;

			Vector3 tempRot = transform.eulerAngles;
			if (!restrictXRot) tempRot.x = followNode.rotation.eulerAngles.x;
			if (!restrictYRot) tempRot.y = followNode.rotation.eulerAngles.y;
			if (!restrictZRot) tempRot.z = followNode.rotation.eulerAngles.z;
			transform.eulerAngles = tempRot;
			//transform.rotation = followNode.rotation;
		} 
	}
}
