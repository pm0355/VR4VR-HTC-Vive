using UnityEngine;
using System.Collections;

public class moneySupervisorAnimationS : MonoBehaviour {

	private Animator animator;

	void Awake () 
	{
		animator = GetComponent<Animator>();
		gameObject.SetActive(true);
	}
	
	public void Talk(bool _isTalking)
	{
		animator.SetBool("isTalking", _isTalking);
	}

	public void Activate(bool _isActive)
	{
		if (_isActive)
			gameObject.SetActive(true);
		else
			gameObject.SetActive(false);
	}
}
