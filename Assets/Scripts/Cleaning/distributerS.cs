using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class distributerS : MonoBehaviour {
	
	public GameObject randomCreatorObj;
		
	public int instanceCount {get;set;}
	public int count { get; set; }
	
	public bool indicating {get;set;}
	
	void Start () 
	{

	}
	void Update () 
	{
		if( indicating && Time.timeSinceLevelLoad < timerS.lastActivitiyTime + timerS.indicationTime ) ClearIndicates();
		
		hudS.debugLines[6] = "Is indicating: " +indicating;		
	}
	public int CreateObjects (GameObject _obj)
	{
		int created = 0;

		Vector3 instancePos = Vector3.zero;
		
		int trial = 0;
		for(int i=0; i<count; i++)
		{
			trial = 0;
			bool collide = false;
			do 
			{
				instancePos = new Vector3( Random.Range((-GetComponent<BoxCollider>().size.x*0.5f)+0.5f, (GetComponent<BoxCollider>().size.x*0.5f)-0.5f), 0.0f, Random.Range((-GetComponent<BoxCollider>().size.z*0.5f)+0.5f, (GetComponent<BoxCollider>().size.z*0.5f)-0.5f));
					
				if(Physics.CheckCapsule (new Vector3(transform.TransformPoint(instancePos).x, 0.8f, transform.TransformPoint(instancePos).z), new Vector3(transform.TransformPoint(instancePos).x, 1.00f, transform.TransformPoint(instancePos).z), 0.75f)) 
				{
					collide = true;		
					//Debug.Log("fail2");
				}
				else
				{
					GameObject instance = Instantiate(_obj, Vector3.zero, Quaternion.identity) as GameObject;
					instance.transform.parent = transform;
					instance.transform.localPosition = new Vector3(instancePos.x, 0.0f, instancePos.z);
					saverS.SaveText("Object created at: " + instance.transform.position.ToString());
					created++;
					break;
				}
				
				trial++;
				if(trial>200) 
				{	
					//Debug.Log("try failed");
					i=count;
					break;
				}
			} 
			while (collide);
		}	
		//Debug.Log("created: "+created);
		//Debug.Log("trial: "+trial);
		
		instanceCount = created;
		return created;
	}
	
	public int CreateRandomObjects (GameObject[] _objs)
	{
		int created = 0;

		Vector3 instancePos = Vector3.zero;
		
		int trial = 0;
		for(int i=0; i<count; i++)
		{
			trial = 0;
			bool collide = false;
			do 
			{
				instancePos = new Vector3( Random.Range((-GetComponent<BoxCollider>().size.x*0.5f)+0.5f, (GetComponent<BoxCollider>().size.x*0.5f)-0.5f), 0.0f, Random.Range((-GetComponent<BoxCollider>().size.z*0.5f)+0.5f, (GetComponent<BoxCollider>().size.z*0.5f)-0.5f));
					
				if(Physics.CheckCapsule (new Vector3(transform.TransformPoint(instancePos).x, 0.8f, transform.TransformPoint(instancePos).z), new Vector3(transform.TransformPoint(instancePos).x, 1.00f, transform.TransformPoint(instancePos).z), 0.75f)) 
				{
					collide = true;		
					//Debug.Log("fail2");
				}
				else
				{
					GameObject instance = Instantiate(randomCreatorObj, Vector3.zero, Quaternion.identity) as GameObject;
					instance.transform.parent = transform;
					instance.transform.localPosition = new Vector3(instancePos.x, 0.0f, instancePos.z);
					saverS.SaveText("Object created at: " + instancePos.ToString());
					instance.GetComponent<randomObjectsS>().CreateObject(_objs);
					
					created++;
					break;
				}
				
				trial++;
				if(trial>200) 
				{	
					//Debug.Log("try failed");
					i=count;
					break;
				}
			} 
			while (collide);
		}	
		//Debug.Log("created: "+created);
		//Debug.Log("trial: "+trial);
		
		instanceCount = created;
		return created;
	}	
	
	public bool CreateSingleObject (GameObject _obj, Transform _pos, bool _checkCollision)
	{
		if(_checkCollision)
		{
			if(Physics.CheckCapsule (new Vector3(_pos.position.x, 0.8f, _pos.position.z), new Vector3(_pos.position.x, 1.00f, _pos.position.z), 0.75f)) 
			{
				//Debug.Log("fail!");
				instanceCount = 0;
				return false;			
			}
		}

		GameObject instance = null;
		instance = Instantiate(_obj, _pos.position, _pos.rotation) as GameObject;
		instance.transform.parent = transform;
		saverS.SaveText("Object created at: " + _pos.position.ToString());

		instanceCount = 1;
		return true;
	}
	
	public void IndicateOne()
	{
		if(!indicating)
		{
			ClearIndicates();
		
			foreach (Transform child in transform)
			{
				IndicatableS script = child.GetComponentInChildren(typeof(IndicatableS)) as IndicatableS;
				if(script!=null && script.Indicatable()) 
				{
					script.IndicateOn();	
					indicating = true;
					break;
				}
			}
		}
	}
	
	public void ClearIndicates()
	{
		if(indicating)
		{
			foreach (Transform child in transform)
			{
				IndicatableS script = child.GetComponentInChildren(typeof(IndicatableS)) as IndicatableS;
				if(script!=null) script.IndicateOff();				
			}
	
			indicating = false;
		}
	}
}
