using UnityEngine;
using System.Collections;

public class shelfLevelS : MonoBehaviour {

	public int shelf;
	public int level;
	public int numberOfBoxesInside { get; set; }

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
			boxS script = other.GetComponent<boxS>();
			script.atShelf = shelf;
			script.atLevel = level;

			numberOfBoxesInside++;
			script.referenceDirection = transform;
			//Debug.Log ("entered"+shelf+"-"+level);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<boxS>() != null)
		{
			boxS script = other.GetComponent<boxS>();
			script.atShelf = 0;
			script.atLevel = 0;

			numberOfBoxesInside--;
			//Debug.Log ("exited"+shelf+"-"+level);
		}
	}	
}
