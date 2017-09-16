using UnityEngine;
using System.Collections;

public class dogBarkingDistracterS : MonoBehaviour {

	private float lifetime = 10.0f;
	private float startTime;

	public SpriteRenderer dogRenderer; 
	public Sprite[] barkingDog;
	private int textureIndex = 0;

	// Use this for initialization
	void Start () {
		startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad > startTime + lifetime ) 
		{
			Destroy(gameObject);
		}	
	}

	void FixedUpdate()
	{
		textureIndex++;
		if(textureIndex == barkingDog.Length) textureIndex = 0;
		dogRenderer.sprite = barkingDog[textureIndex];
	}
}
