using UnityEngine;
using System.Collections;

public class wetDirtS : MonoBehaviour {
	
	public int passToClean = 5;
	
	private Collider mopCollider;
	public Texture[] textures;	
		
	private dirtPileS myPile;
	
	private int pass = 0;
	
	// Use this for initialization
	void Start () 
	{
		mopCollider = GameObject.Find("MopC").collider;
		myPile = transform.parent.GetComponent<dirtPileS>();
		
		renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnTriggerEnter(Collider other) 
	{
        if (other == mopCollider)
		{
			if(pass < passToClean)
			{
				transform.localScale *= 1.15f;
				renderer.material.color = new Color(1.0f, 1.0f, 1.0f, renderer.material.color.a*0.6f);
				pass++;
			}
			else
			{
				myPile.count--;
            	Destroy(gameObject);			
			}
			
			timerS.setLastActivitiyTime();
		}
    }	
}
