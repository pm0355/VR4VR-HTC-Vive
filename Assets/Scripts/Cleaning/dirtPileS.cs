using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dirtPileS : MonoBehaviour , IndicatableS {
	
	public GameObject dirtObj;
			
	private float size = 1.0f;	//This is 1 for now
		
	public int minCount = 4;
	public int maxCount = 10;
		
	public float minSize = 0.005f;
	public float maxSize = 0.015f;
		
	public int count {get;set;}
	public bool cleaned {get;set;}
	
	private bool indicating;
	public GameObject indicatorObj;
	
	private hudS hud;
	
	void Start () 
	{
		transform.localScale = new Vector3(size, 1.0f, size);
		
		count = Random.Range(minCount, maxCount);
		count = CreateDirts(count);
		
		cleaned = false;
		indicating = false;
		indicatorObj.SetActive(false);
		
		hud = GameObject.Find("HUD").transform.root.GetComponentInChildren<hudS>();
	}
	void Update () 
	{
		if(count == 0 && !cleaned)
		{
			//Debug.Log("CLEANED");
			cleaned = true;
			transform.parent.GetComponent<distributerS>().instanceCount--;

			saverS.successCount++;
			saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";
			saverS.SaveText("Cleaned at: " + transform.position.ToString());

			if(transform.parent.GetComponent<distributerS>().instanceCount > 0 )
			{
				//PHOTON CODE
				generalManagerS.PhotonProgressUpdate();
				//PHOTON CODE

				hud.visualNo = 2;
				hud.visualOn = true;
				//Debug.Log("VISUAL");
			}
			Destroy(gameObject);
		}
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
		return (!cleaned);	
	}
	int CreateDirts ( int _count )
	{
		//Debug.Log("dirt wanted: "+_count);
		
		List<Vector3> dirtPos = new List<Vector3>();
		float dirtSize = 0.0f;
		Vector3 posAndSize = Vector3.zero;
		
		dirtSize = Random.Range(minSize, maxSize);
		posAndSize = new Vector3(Random.Range(-(size/2.0f)+(dirtSize*1.415f), (size/2.0f)-(dirtSize*1.415f)), Random.Range(-(size/2.0f)+(dirtSize*1.415f), (size/2.0f)-(dirtSize*1.415f)), dirtSize);
		dirtPos.Add(posAndSize);
		
		int trial = 0;
		for(int i=1; i<_count; i++)
		{
			bool collide = false;
			do 
			{
				dirtSize = Random.Range(minSize, maxSize);
				posAndSize = new Vector3(Random.Range(-(size/2.0f)+(dirtSize*1.415f), (size/2.0f)-(dirtSize*1.415f)), Random.Range(-(size/2.0f)+(dirtSize*1.415f), (size/2.0f)-(dirtSize*1.415f)), dirtSize);
		
				foreach (Vector3 item in dirtPos) 
				{
					float sqrDistance = (item.x-posAndSize.x)*(item.x-posAndSize.x) + (item.y-posAndSize.y)*(item.y-posAndSize.y);
					collide = sqrDistance > (item.z+posAndSize.z)*(item.z+posAndSize.z)*2.0f ? false:true;
					
					if(collide) break;
				}
				
				trial++;
				if(trial>100) 
				{	
					//Debug.Log("try failed");
					i=_count;
					break;
				}
			} 
			while (collide);
			
			if(trial<=100) dirtPos.Add(posAndSize);
		}
		
		foreach (Vector3 item in dirtPos) 
		{
			GameObject dirt = Instantiate(dirtObj, Vector3.zero, Quaternion.identity) as GameObject;
			dirt.transform.localRotation = Quaternion.Euler(0.0f, Random.Range(0,360),0.0f);
			dirt.transform.localScale = new Vector3(item.z/5.0f, 1.0f, item.z/5.0f);
			
			dirt.transform.parent = transform;
			dirt.transform.localPosition = new Vector3(item.x, 0.0f, item.y);
		}	
		//Debug.Log("dirt created: "+dirtPos.Count);
		//Debug.Log("trials: "+trial);

		return dirtPos.Count;
	}
}
