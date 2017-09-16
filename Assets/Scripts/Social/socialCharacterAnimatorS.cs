using UnityEngine;
using System.Collections;

public class socialCharacterAnimatorS : MonoBehaviour {

	private Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void SetTalk(bool _isTalking, int _mood)
	{
		animator.SetInteger("mood", _mood);
		animator.SetBool("isTalking", _isTalking);
	}
}
