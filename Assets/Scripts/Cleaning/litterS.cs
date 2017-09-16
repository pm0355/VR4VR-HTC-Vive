using UnityEngine;
using System.Collections;

public class litterS : MonoBehaviour {
	
	private hudS hud;
	
	// Use this for initialization	
	void Start () {
		hud = GameObject.Find("HUD").transform.GetComponentInChildren<hudS>();
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.name == "Trash Bin Inside Volume")
		{
			randomObjectsS temp = transform.parent.GetComponent<randomObjectsS>();

			if(temp != null)
			{
				if(temp.done == false)
				{
					temp.done = true;
					transform.root.GetComponentInChildren<distributerS>().instanceCount--;

					timerS.lastActivitiyTime = Time.timeSinceLevelLoad;
				}
			}
			else
				transform.root.GetComponentInChildren<distributerS>().instanceCount--;

			saverS.successCount++;
			saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

			if (transform.root.GetComponentInChildren<distributerS>().instanceCount > 0)
			{
				//PHOTON CODE
				generalManagerS.PhotonProgressUpdate();
				//PHOTON CODE

				hud.visualNo = 2;
				hud.visualOn = true;
			}

			Destroy( transform.GetComponent<graspableS>() );
			Destroy( collider );
			rigidbody.isKinematic = true;

			audio.Play();
		}
	}
}
