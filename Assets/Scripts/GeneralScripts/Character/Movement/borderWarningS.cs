using UnityEngine;
using System.Collections;

public class borderWarningS : MonoBehaviour {
private Renderer planeRenderer;

	// Use this for initialization
	void Start ()
	{
		planeRenderer = GetComponentInChildren<Renderer>();
		planeRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 center = (vrS.VRHead.position*.4f + vrS.VRFootRight.position*.3f + vrS.VRFootLeft.position*.3f);
		center.y = 0.0f;
	
		if(center.magnitude > 0.8f)
		{
			planeRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			transform.rotation = Quaternion.LookRotation(center, Vector3.up);
		}
		else
		{
			planeRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
		
		
	}
}
