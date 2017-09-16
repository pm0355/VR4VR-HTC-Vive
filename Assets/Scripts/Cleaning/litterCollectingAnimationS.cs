using UnityEngine;
using System.Collections;

public class litterCollectingAnimationS : MonoBehaviour, AnimatableS {

	private Animator animator;	
	private AnimatorStateInfo currentState;
		
	//static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int taklingState = Animator.StringToHash("Base Layer.Talking");
	static int litterCollectingState = Animator.StringToHash("Base Layer.LitterCollecting");	
	
	public GameObject litterObj;
	public GameObject trashBinObj;
	public Transform litterPos;
	public Transform trashBinPos;
	
	private GameObject litterInstance;
	private GameObject trashBinInstance;
	
	private int state = 0;
	private bool animating = false;
	
	public AudioClip[] dubs;

	public AudioClip litterDropSound;

	private bool bypass = false;
	
	// Use this for initialization
	void Awake () 
	{
		litterInstance = Instantiate(litterObj, litterPos.position, litterPos.rotation) as GameObject;
		litterInstance.transform.parent = transform;
		trashBinInstance = Instantiate(trashBinObj, trashBinPos.position, trashBinPos.rotation) as GameObject;
		trashBinInstance.transform.parent = transform;
		
		animator = GetComponent<Animator>();
		
		animating = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			bypass = true;
			audio.Stop();
		}
	}


	public void PlayAnimation()
	{
		animator.SetInteger("LitterCollectingAnimationState", 1);
		audio.clip = dubs[0];
		audio.Play();
		animating = true;
	}
	
	public void PlayTalk(int _index)
	{
		animator.SetInteger("LitterCollectingAnimationState", 1);
		audio.clip = dubs[_index];
		audio.Play();			
		state = 4;
		animating = true;
	}
	
	public bool IsPlaying()
	{
		return animating;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		currentState = animator.GetCurrentAnimatorStateInfo(0);

		switch(state)
		{
		case 0://talking
			if(!audio.isPlaying || bypass) 
			{
				animator.SetInteger("LitterCollectingAnimationState", 2);
				state = 1;
			}
			break;
		case 1://litter collecting before taking litter
			if ((!animator.IsInTransition(0) && currentState.normalizedTime > 0.29f) || bypass) 
			{
				litterInstance.GetComponent<Rigidbody>().isKinematic = true;
				litterInstance.transform.parent = GameObject.Find("LitterCenter").transform;

				state = 2;
			}
			break;	
		case 2://litter collecting before putting litter
			if ((currentState.nameHash == litterCollectingState && currentState.normalizedTime > 0.76f) || bypass) 
			{
				litterInstance.GetComponent<Rigidbody>().isKinematic = false;
				litterInstance.transform.parent = transform;

				audio.PlayOneShot(litterDropSound, 1.0F);

				animator.SetInteger("LitterCollectingAnimationState", 1);
				state = 3;
			}
			break;			
		case 3://litter collecting before finishing
			if ((!animator.IsInTransition(0) && currentState.nameHash == taklingState) || bypass)
			{
				litterInstance.GetComponent<Rigidbody>().isKinematic = true;
				audio.clip = dubs[1];
				audio.Play();		
				state = 4;
			}
			break;				
		case 4://talking
			if (!audio.isPlaying || bypass) 
			{
				animator.SetInteger("LitterCollectingAnimationState", 0);
				state = 5;
			}
			break;		
		case 5://idle
			animating = false;
			break;				
		}
		bypass = false;
	}
}
