using UnityEngine;
using System.Collections;

public class parkingCarS : MonoBehaviour {

	public GameObject[] carObjs;
	private carMovementS carMover;


	// Use this for initialization
	void Start () {
		GameObject tempObj = Instantiate(carObjs[Random.Range(0, carObjs.Length)], transform.position, Quaternion.identity) as GameObject;
		tempObj.name = "CarModel";
		tempObj.transform.parent = transform;
		carMover = tempObj.GetComponent<carMovementS>();

		carMover.RotateRandom();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
