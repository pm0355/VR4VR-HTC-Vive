using UnityEngine;
using System.Collections;

public class cautiousnessAnimationS : MonoBehaviour {
	private Animator animator;
	private AnimatorStateInfo currentState;

	public bool tutorialAnimationCompleted { get; set; }
	static int finalState = Animator.StringToHash("Base Layer.Final");
	static int tutorialState = Animator.StringToHash("Base Layer.Tutorial");

	public Transform targetT;
	public Transform targetCartT;
	private bool targetActive = true;

	void Awake()
	{
		if (generalManagerS.ActiveSubTask == 2)
			Destroy(targetT.gameObject);
		else
			Destroy(targetCartT.gameObject);

		animator = GetComponent<Animator>();
		tutorialAnimationCompleted = false;
	}
	void Update()
	{
		currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (!tutorialAnimationCompleted)
		{
			if (currentState.nameHash == finalState)
			{
				tutorialAnimationCompleted = true;
			}
		}

		if (targetActive && currentState.nameHash == tutorialState && currentState.normalizedTime > 0.42f)
		{
			if (generalManagerS.ActiveSubTask == 2)
				Destroy(targetCartT.gameObject);
			else
				Destroy(targetT.gameObject);

			targetActive = false;
		}
	}
	public void SetTalk(bool _isTalking)
	{
		animator.SetBool("isTalking", _isTalking);
	}
}
