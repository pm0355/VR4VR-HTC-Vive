using UnityEngine;
using System.Collections;

public class leftHandHolderS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void OnTriggerEnter(Collider other) {
		if(other.transform.GetComponent<holdableS>()!=null)
			other.transform.GetComponent<holdableS>().LeftHandActivated = true;
    }		
    void OnTriggerExit(Collider other) {
		if(other.transform.GetComponent<holdableS>()!=null)
			other.transform.GetComponent<holdableS>().LeftHandActivated = false;
    }		
}
