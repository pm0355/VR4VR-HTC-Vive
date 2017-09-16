using UnityEngine;
using System.Collections;

public class randomObjectsS : MonoBehaviour, IndicatableS {
	
	private bool indicating;
	public GameObject indicatorObj;
	
	public bool done {get; set;}
	private bool prevDone;
	// Use this for initialization
	void Awake () 
	{
		done = false;
		indicating = false;
		indicatorObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		indicatorObj.transform.rotation = Quaternion.identity;
	}
	
	public void CreateObject(GameObject[] _objs)
	{
		GameObject temp = Instantiate(_objs[Random.Range(0,_objs.Length)], transform.position, Quaternion.identity) as GameObject;
		temp.transform.parent = transform;
		indicatorObj.transform.parent = temp.transform;		
	}
	
	public void IndicateOn()
	{
		if(!indicating)
		{
			//Debug.Log("indicating at ");
			indicatorObj.SetActive(true);
			indicating = true;		
		}
	}
	public void IndicateOff()
	{
		if(indicating)
		{
			indicatorObj.SetActive(false);
			indicating = false;		
		}		
	}
	public bool Indicatable()
	{
		return (!done);
	}
}
