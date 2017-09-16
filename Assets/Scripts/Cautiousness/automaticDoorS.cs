using UnityEngine;
using System.Collections;

public class automaticDoorS : MonoBehaviour {

	private Vector3 colliderPos = new Vector3(0.0f, 1.0f, -4.0f);
	private Animator animator;

	public AudioClip doorOpeningSound;
	public AudioClip doorClosingSound;

	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator>();
		animator.SetBool("activated", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		LayerMask collisionMask = (1 << 12);
		if (Physics.CheckSphere(colliderPos, 2.4f, collisionMask) && !animator.GetBool("activated"))
		{
			animator.SetBool("activated", true);
			audio.PlayOneShot(doorOpeningSound);
		}

		if (!Physics.CheckSphere(colliderPos, 2.4f, collisionMask) && animator.GetBool("activated"))
		{
			animator.SetBool("activated", false);
			audio.PlayOneShot(doorClosingSound);
		}
	}
}
