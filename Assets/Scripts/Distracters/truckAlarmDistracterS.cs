using UnityEngine;
using System.Collections;

public class truckAlarmDistracterS : MonoBehaviour {

	public Renderer alarmPlane;

	// Use this for initialization
	void Start () {
		StartCoroutine(SwitchLight());
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying) Destroy(gameObject);
	}

	IEnumerator SwitchLight() 
	{
		for(;;) {
			//Debug.Log(alarmPlane.material.color);
			if(alarmPlane.material.color.a == 0.0f) alarmPlane.material.color = new Color(alarmPlane.material.color.r, alarmPlane.material.color.g, alarmPlane.material.color.b, 1.0f);
			else alarmPlane.material.color = new Color(alarmPlane.material.color.r, alarmPlane.material.color.g, alarmPlane.material.color.b, 0.0f);
			yield return new WaitForSeconds(0.5f);
		}
	}
}
