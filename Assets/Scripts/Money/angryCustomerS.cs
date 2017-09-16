using UnityEngine;
using System.Collections;

public class angryCustomerS : MonoBehaviour
{
	private Animator anim;
	private bool isSpeaking = false;
	// Use this for initialization
	void Start()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.IsName("ToIdle"))
		{
			isSpeaking = true;
			anim.SetBool("isSpeaking", true);
			audio.Play();
		}

		if(isSpeaking && !audio.isPlaying)
		{
			isSpeaking = false;
			anim.SetBool("isSpeaking", false);
		}

		if (stateInfo.IsName("End"))
		{
			Destroy(gameObject);
		}
	}
}