using UnityEngine;
using System.Collections;

public class nightTimeDistracterS : MonoBehaviour {
	
	public SpriteRenderer nightPlaneRenderer;
	public SpriteRenderer lightRenderer;

	private float lifetime = 20.0f;
	private float startTime;
	private bool fadeOutStarted = false;

	// Use this for initialization
	void Start () {
		StartCoroutine(FadeIn());
		startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(!fadeOutStarted && Time.timeSinceLevelLoad > startTime + lifetime ) 
		{
			fadeOutStarted = true;
			StartCoroutine(FadeOut());
			StopCoroutine("LightFlicker");
		}
	}

	IEnumerator LightFlicker() 
	{
		for(;;) 
		{
			//Debug.Log(lightRenderer.color);
			if(lightRenderer.color.a == 1.0f) 
			{
				lightRenderer.color = new Color(1.0f, 1.0f, 1.0f, Random.Range(0.4f, 0.9f));
				yield return new WaitForSeconds(0.01f);
			}
			else 
			{
				lightRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));
			}
		}
	}

	IEnumerator FadeIn() 
	{
		for (float f = 0f; f <= 1f; f += 0.025f) 
		{
			nightPlaneRenderer.color = new Color (1.0f, 1.0f, 1.0f, f);
			lightRenderer.color = new Color (1.0f, 1.0f, 1.0f, f);
			yield return new WaitForSeconds(.05f);
		}
		StartCoroutine("LightFlicker");
	}

	IEnumerator FadeOut() 
	{
		for (float f = 1f; f >= 0; f -= 0.025f) 
		{
			nightPlaneRenderer.color = new Color (1.0f, 1.0f, 1.0f, f);
			lightRenderer.color = new Color (1.0f, 1.0f, 1.0f, f);
			yield return new WaitForSeconds(.05f);
		}
		Destroy(gameObject);
	}
}
