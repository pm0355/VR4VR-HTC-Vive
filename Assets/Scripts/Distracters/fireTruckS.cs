using UnityEngine;
using System.Collections;

public class fireTruckS : MonoBehaviour {

	private Vector3[] path;
	
	public Material mat;
	private bool isRed;

	// Use this for initialization
	void Start()
	{
		path = iTweenPath.GetPath("FireTruckPath");
		iTween.MoveTo(gameObject, iTween.Hash("path", path, "orienttopath", true, "time", 14.0, "movetopath", false, "easetype", "linear"));
		isRed = true;

		StartCoroutine(LightChange());
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position == iTween.PointOnPath(path, 1.0f))
		{
			Destroy(transform.parent.gameObject);
		}
	}

	IEnumerator LightChange()
	{
		for (; ; )
		{
			mat.color = isRed ? Color.blue : Color.red;
			isRed = !isRed;

			yield return new WaitForSeconds(1.0f);
		}
	}
}
