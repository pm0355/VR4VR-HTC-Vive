using UnityEngine;
using System.Collections;

public class handModelS : MonoBehaviour {
public bool isTriggered 
{
	get;
	set;
}	
private Vector3 initialLocalPos;
private Quaternion initialLocalRot;
	
public GameObject[] models;

	// Use this for initialization
	void Awake () {
		initialLocalPos = transform.localPosition;
		initialLocalRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isTriggered)
		{
			transform.localPosition = initialLocalPos;
			transform.localRotation = initialLocalRot;
		}
	}
	
	public void ChangeModel(int _modelIndex)
	{
		for (int i=0; i<models.Length; i++) 
		{
			if(i==_modelIndex) models[i].SetActive(true);
			else models[i].SetActive(false);
		}
	}
}
