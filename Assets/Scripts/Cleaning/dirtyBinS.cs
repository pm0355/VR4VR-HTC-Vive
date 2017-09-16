using UnityEngine;
using System.Collections;

public class dirtyBinS : MonoBehaviour {
public bool isEmpty {get;set;}
public ParticleSystem smellParticle;
public ParticleSystem emptyingParticle;
	
public float degreeThreshold = 30.0f;

	// Use this for initialization
	void Start () 
	{
		isEmpty = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
    void OnTriggerStay(Collider other) 
	{
		if(transform.rigidbody.isKinematic  && !isEmpty)
		{
			if(other.transform.GetComponent<dirtyBinEmptyingAreaS>()!=null)
			{
				if( Vector3.Dot(transform.up, Vector3.down)> Mathf.Cos(degreeThreshold*Mathf.Deg2Rad) )
				{
					smellParticle.Stop();
					emptyingParticle.Play();
					isEmpty = true;
				}				
			}
		}
    }		
}
