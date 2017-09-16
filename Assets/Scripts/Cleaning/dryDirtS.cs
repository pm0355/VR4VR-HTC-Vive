using UnityEngine;
using System.Collections;

public class dryDirtS : MonoBehaviour {
	
	private Collider innerVacuum;
	private Collider outterVacuum;	
		
	public float speed=1.0f;	
	public Texture[] textures;	
		
	private dirtPileS myPile;
	
	// Use this for initialization
	void Start () 
	{
		innerVacuum = GameObject.Find("VacuumIC").collider;
		outterVacuum = GameObject.Find("VacuumOC").collider;
		
		myPile = transform.parent.GetComponent<dirtPileS>();
		
		renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnTriggerStay(Collider other) 
	{
        if (other == outterVacuum)
		{
			Vector3 temp = other.transform.position - transform.position;
			temp.y = 0.0f;
            transform.Translate( temp.normalized * speed * Time.deltaTime ,Space.World);
		}
    }
	void OnTriggerEnter(Collider other) 
	{
        if (other == innerVacuum)
		{
			myPile.count--;
            Destroy(gameObject);	
			
			timerS.setLastActivitiyTime();
		}
    }	
}
