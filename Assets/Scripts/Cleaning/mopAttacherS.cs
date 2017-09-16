using UnityEngine;
using System.Collections;

public class mopAttacherS : MonoBehaviour {
	
	private Collider RightHandCollider;
	private Collider LeftHandCollider;	
	
	private mopCleanerS mopCleaner;
	
	// Use this for initialization
	void Start () 
	{
		mopCleaner = GetComponentInChildren<mopCleanerS>();
		
		RightHandCollider = GameObject.Find("HRHolder").collider;
		LeftHandCollider = GameObject.Find("HLHolder").collider;	
	}
	
	void OnTriggerEnter(Collider other) 
	{
        if(other == RightHandCollider || other == LeftHandCollider)
		{
			if(mopCleaner.mopHeld)
			{	
				mopCleaner.UnholdMop();
				mopCleaner.transform.localPosition = Vector3.zero;
				mopCleaner.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
			}
			else mopCleaner.HoldMop();
		}
    }		
}
