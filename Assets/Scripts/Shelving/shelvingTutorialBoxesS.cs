using UnityEngine;
using System.Collections;

public class shelvingTutorialBoxesS : MonoBehaviour {

	private Animator animator;
	private AnimatorStateInfo currentState;

	static int tutorialState = Animator.StringToHash("Base Layer.Tutorial");

	public GameObject[] boxObjs;
	public GameObject[] monitorObjs;

	private int boxState = 0;
	private int monitorState = 0;

	public Material magicBoxTreadMill;
	private float treadMillSpeed = 1.0f;
 

	void Awake()
	{
		animator = GetComponent<Animator>();

		boxObjs[0].SetActive(true);
		boxObjs[1].SetActive(false);
		boxObjs[2].SetActive(false);
		boxObjs[3].SetActive(false);

		monitorObjs[0].SetActive(true);
		monitorObjs[1].SetActive(false);
		monitorObjs[2].SetActive(false);
	}

	void Update () 
	{
		currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.nameHash == tutorialState)
		{
			if (boxState==0 && currentState.normalizedTime > 0.057f)
			{
				boxObjs[0].SetActive(false);
				boxObjs[1].SetActive(true);
				boxObjs[2].SetActive(false);
				boxObjs[3].SetActive(false);
				boxState++;
			}
			if (boxState == 1 && currentState.normalizedTime > 0.174f)
			{
				boxObjs[0].SetActive(true);
				boxObjs[1].SetActive(false);
				boxObjs[2].SetActive(false);
				boxObjs[3].SetActive(false);
				boxState++;
			}
			if (boxState == 2 && currentState.normalizedTime > 0.705f)
			{
				boxObjs[0].SetActive(false);
				boxObjs[1].SetActive(false);
				boxObjs[2].SetActive(true);
				boxObjs[3].SetActive(false);
				boxState++;
			}
			if (boxState == 3 && currentState.normalizedTime > 0.823f)
			{
				boxObjs[0].SetActive(false);
				boxObjs[1].SetActive(false);
				boxObjs[2].SetActive(false);
				boxObjs[3].SetActive(true);
				boxState++;
			}


			if (monitorState == 0 && currentState.normalizedTime > 0.499f)
			{
				monitorObjs[0].SetActive(false);
				monitorObjs[1].SetActive(true);
				monitorObjs[2].SetActive(false);
				monitorState++;
			}
			if (monitorState == 1 && currentState.normalizedTime > 0.941f)
			{
				monitorObjs[0].SetActive(false);
				monitorObjs[1].SetActive(false);
				monitorObjs[2].SetActive(true);
				monitorState++;
			}

			if (currentState.normalizedTime > 0.351f && currentState.normalizedTime < 0.499f)
			{
				magicBoxTreadMill.mainTextureOffset = new Vector2(magicBoxTreadMill.mainTextureOffset.x + treadMillSpeed * Time.deltaTime, 0.0f);
			}
			if (currentState.normalizedTime > 0.499f && currentState.normalizedTime < 0.646f)
			{
				magicBoxTreadMill.mainTextureOffset = new Vector2(magicBoxTreadMill.mainTextureOffset.x - treadMillSpeed * Time.deltaTime, 0.0f);
			}
		}
	}
}
