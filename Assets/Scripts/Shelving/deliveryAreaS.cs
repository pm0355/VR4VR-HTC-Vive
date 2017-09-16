using UnityEngine;
using System.Collections;

public class deliveryAreaS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<boxS>() != null)
		{
			other.GetComponent<boxS>().atDelivery = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<boxS>() != null)
		{
			other.GetComponent<boxS>().atDelivery = false;
		}
	}	
}
