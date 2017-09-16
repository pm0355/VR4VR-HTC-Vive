using UnityEngine;
using System.Collections;

public class shelvingAnimationS : MonoBehaviour {

	private Animator animator;
	private AnimatorStateInfo currentState;

	public bool tutorialAnimationCompleted { get; set; }
	static int talking2State = Animator.StringToHash("Base Layer.Talking2");

	void Awake () 
	{
		animator = GetComponent<Animator>();
		tutorialAnimationCompleted = false;
	}
	void Update()
	{
		if (!tutorialAnimationCompleted)
		{
			currentState = animator.GetCurrentAnimatorStateInfo(0);
			if (currentState.nameHash == talking2State)
			{
				tutorialAnimationCompleted = true;
			}
		}
	}
	public void SetTalk1 (bool _isTalking) 
	{
		animator.SetBool("isTalking1", _isTalking);
	}
	public void SetTalk2(bool _isTalking)
	{
		animator.SetBool("isTalking2", _isTalking);
	}

}
