using UnityEngine;
using System.Collections;

public class vacuumCleanerAttacherS : MonoBehaviour {
	
	private Collider RightHandCollider;
	private Collider LeftHandCollider;	
	
	private vacuumCleanerS vacuumCleaner;
	
	// Use this for initialization
	void Start () 
	{
		vacuumCleaner = GetComponentInChildren<vacuumCleanerS>();
		
		RightHandCollider = GameObject.Find("HRHolder").collider;
		LeftHandCollider = GameObject.Find("HLHolder").collider;	
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other == RightHandCollider || other == LeftHandCollider)
		{
			if(vacuumCleaner.vacuumHeld)
			{	
				vacuumCleaner.UnholdVacuum();
				vacuumCleaner.transform.localPosition = Vector3.zero;
				vacuumCleaner.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
			}
			else vacuumCleaner.HoldVacuum();
		}
	}		
}
