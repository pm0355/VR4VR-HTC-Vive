using UnityEngine;
using System.Collections;

public class groceryAlarmDistracterS : MonoBehaviour
{
	public Renderer[] alarmObjs;
	private bool lightsOn = false;

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
			if (!lightsOn) 
			{
				foreach (Renderer _renderer in alarmObjs)
				{
					_renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
					lightsOn = true;
				}
			}
			else
				foreach (Renderer _renderer in alarmObjs)
				{
					_renderer.material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
					lightsOn = false;
				}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
