using UnityEngine;
using System.Collections;

public class butterflyS : MonoBehaviour {

	public Transform butterflyModel;
	private Vector3[] path;
	// Use this for initialization
	void Start()
	{
		path = iTweenPath.GetPath("ButterflyPath");
		iTween.MoveTo(gameObject, iTween.Hash("path", path, "orienttopath", true, "time", 20.0, "movetopath", false, "easetype", "linear"));
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position == iTween.PointOnPath(path, 1.0f))
		{
			Destroy(transform.parent.gameObject);
		}

		butterflyModel.position = transform.position;
	}
}
