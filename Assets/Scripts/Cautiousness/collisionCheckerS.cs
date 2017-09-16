using UnityEngine;
using System.Collections;

public class collisionCheckerS : MonoBehaviour {

	public trafficManagerS trafficMan;
	public AudioClip[] hitSounds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer != 9 && other.gameObject.layer != 10 && other.gameObject.layer != 13)
		{
			if(other.gameObject.name == "Target")
			{
				trafficMan.TargetReached();
				Destroy(other.gameObject);
			}
			else
			{
				string info = "Stable object hit! ";
				if (other.transform.parent.gameObject.name == "Car Instance")
				{
					audio.PlayOneShot(hitSounds[0]);
					info = "Car hit! ";
				}
				if (other.gameObject.name == "Pedestrain Instance")
				{
					audio.PlayOneShot(hitSounds[1]);
					info = "Pedestrain hit! ";
				}
				if (other.gameObject.name == "HornRange")
				{
					audio.PlayOneShot(hitSounds[2]);
					info = "Horn range hit! ";
				}

				info += "Collision at: " + transform.position.ToString();
				saverS.SaveText(info);
				trafficMan.Failed();
			}
		}
	}
}
