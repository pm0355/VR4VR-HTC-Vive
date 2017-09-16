using UnityEngine;
using System.Collections;

public class helicopterS : MonoBehaviour {

	public Transform rotor;
	public float rotationSpeed;

	private Vector3[] path;
	// Use this for initialization
	void Start () {
		path = iTweenPath.GetPath("HeliPath");
		iTween.MoveTo(gameObject, iTween.Hash("path", path, "orienttopath", true, "time", 20.0, "movetopath", false, "easetype", "linear"));
	}
	
	// Update is called once per frame
	void Update () {
		rotor.Rotate(new Vector3(0.0f, 0.0f, rotationSpeed * Time.deltaTime), Space.Self);

		if(transform.position == iTween.PointOnPath(path, 1.0f))
		{
			Destroy(transform.parent.gameObject);
		}
	}
}
