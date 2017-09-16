using UnityEngine;
using System.Collections;

public class cautiousnessVisualManagerS : MonoBehaviour {

	public GameObject tutorialAnimation;
	private cautiousnessAnimationS activeAnimation;
	public int tutorialSetNo { get; set; }
	public bool tutorialActive { get; set; }
	public bool tutorialFinished { get; set; }
	private int tutorialState = 0;

	public int pictographSetNo { get; set; }
	public Texture[] pictograph1;				//user input
	public Texture[] pictograph2;				//user input
	public Texture[] pictograph3;				//user input
	public float pictographTime = 10.0f;
	private float pictographStartTime;
	public bool pictographsActive { get; set; }
	private int textureNo = 0;

	public int visualNo { get; set; }
	public Texture[] visuals;
	public float visualTime = 3.0f;
	private float visualStartTime;
	public bool visualsActive { get; set; }

	public string info { get; set; }
	public bool infoActive { get; set; }

	public GUIStyle infoStyle;

	public static string[] debugLines = new string[20];
	public GUIStyle debugStyle;

	public bool faded { get; set; }
	private bool fading;
	private float fadeAlpha = 0.0f;

	public AudioClip[] voices;

	private Transform creations;

	private bool bypass = false;

	//DUBS
	//0-  subtask1 tutorial-1 			071
	//1-  subtask1 tutorial-2 			072
	//2-  subtask2 tutorial-1 			077
	//3-  subtask2 tutorial-2			078
	//4-  subtask3 tutorial-1 			081
	//5-  subtask3 tutorial-2 			082
	//6-  subtask1 tutorial-finish		074
	//7-  subtask2 tutorial-finish		074
	//8-  subtask3 tutorial-finish		074
	//9-  subtask1 practice-start		075
	//10- subtask1 pictograph			073
	//11- subtask1 practice-finish		076
	//12- subtask2 practice-start		079
	//13- subtask2 pictograph			073
	//14- subtask2 practice-finish		080
	//15- subtask3 practice-start		083
	//16- subtask3 pictograph			073
	//17- subtask3 practice-finish		084

	void Start()
	{
		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			bypass = true;
			audio.Stop();
		}

		if (pictographsActive && Time.timeSinceLevelLoad > pictographStartTime + pictographTime)
			ActivatePictographs(false);

		if (visualsActive && Time.timeSinceLevelLoad > visualStartTime + visualTime)
			ActivateVisuals(false);

		if (tutorialActive)
		{
			switch (tutorialState)
			{
				case 0:
					GameObject tempObj = Instantiate(tutorialAnimation) as GameObject;
					tempObj.transform.parent = creations;
					activeAnimation = tempObj.GetComponentInChildren<cautiousnessAnimationS>();

					activeAnimation.SetTalk(true);
					audio.clip = voices[tutorialSetNo * 2];
					audio.Play();
					tutorialState++;
					break;
				case 1:
					if (!audio.isPlaying || bypass)
					{
						activeAnimation.SetTalk(false);
						tutorialState++;
					}
					break;
				case 2:
					if (activeAnimation.tutorialAnimationCompleted || bypass)
					{
						activeAnimation.SetTalk(true);
						audio.clip = voices[tutorialSetNo * 2 + 1];
						audio.Play();
						tutorialState++;
					}
					break;
				case 3:
					if (!audio.isPlaying || bypass)
					{
						activeAnimation.SetTalk(false);
						tutorialFinished = true;
						tutorialState++;
					}
					break;
			}
			bypass = false;
		}
	}
	void FixedUpdate()
	{
		if (pictographsActive)
		{
			textureNo++;
			if (pictographSetNo == 0 && textureNo == pictograph1.Length) textureNo = 0;
			if (pictographSetNo == 1 && textureNo == pictograph2.Length) textureNo = 0;
			if (pictographSetNo == 2 && textureNo == pictograph3.Length) textureNo = 0;
		}
	}
	void OnGUI()
	{
		if (pictographsActive)
		{
			if (pictographSetNo == 0) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f, 0.4f), pictograph1[textureNo]);
			if (pictographSetNo == 1) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f, 0.4f), pictograph2[textureNo]);
			if (pictographSetNo == 2) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f, 0.4f), pictograph3[textureNo]);
		}
		if (visualsActive) userInterfaceS.drawTex(new Vector2(0.3f, 0.15f), new Vector2(0.4f, 0.4f), visuals[visualNo]);
		if (infoActive) userInterfaceS.drawText(new Vector2(0.59f, 0.052f), new Vector2(0.25f, 0.1f), info, infoStyle);

		if (fading) userInterfaceS.drawTexEverywhere(visuals[3], fadeAlpha);

		//DEBUG Lines
		GUILayout.BeginVertical();
		foreach (string item in debugLines)
		{
			GUILayout.Label(item, debugStyle);
		}
		GUILayout.EndVertical();
		//DEBUG Lines		
	}

	public void ActivateTutorial(bool _toActive)
	{
		if (_toActive && !tutorialActive)
		{
			tutorialActive = true;
			tutorialFinished = false;
			tutorialState = 0;
		}
		if (!_toActive && tutorialActive)
		{
			if (audio.clip == voices[0] || audio.clip == voices[1] || audio.clip == voices[2] || audio.clip == voices[3] || audio.clip == voices[4] || audio.clip == voices[5]) audio.Stop();
			tutorialActive = false;
			CleanObjects();
		}
	}
	public void ActivatePictographs(bool _toActive)
	{
		if (_toActive && !pictographsActive)
		{
			textureNo = 0;
			pictographStartTime = Time.timeSinceLevelLoad;
			timerS.setLastPictographTime();
			pictographsActive = true;

			saverS.promptCount++;
			saverS.promptTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";
		}
		if (!_toActive && pictographsActive)
		{
			pictographsActive = false;
		}
	}
	public void ActivateVisuals(bool _toActive)
	{
		if (_toActive && !visualsActive)
		{
			visualStartTime = Time.timeSinceLevelLoad;
			visualsActive = true;
		}
		if (!_toActive && visualsActive)
		{
			visualsActive = false;
		}
	}
	public void ActivateInfo(bool _toActive)
	{
		if (_toActive && !infoActive)
		{
			infoActive = true;
		}
		if (!_toActive && infoActive)
		{
			infoActive = false;
		}
	}

	public void FadeOut()
	{
		if (!fading)
		{
			StartCoroutine(Fader(1.0f, true));
			//Debug.Log("fade(true)");
		}
	}
	public void FadeIn()
	{
		if (faded)
		{
			StartCoroutine(Fader(1.0f, false));
			//Debug.Log("fade(false)");
		}
	}
	IEnumerator Fader(float _time, bool _isFadeOut)
	{
		if (_isFadeOut)
		{
			fading = true;
			for (float f = 0.0f; f < _time; f += Time.deltaTime)
			{
				fadeAlpha = f / _time;
				//Debug.Log(fadeAlpha);
				yield return null;
			}
			fadeAlpha = 1.0f;
			//Debug.Log(fadeAlpha);

			faded = true;
		}
		else
		{
			faded = false;
			for (float f = _time; f > 0.0f; f -= Time.deltaTime)
			{
				fadeAlpha = f / _time;
				//Debug.Log(fadeAlpha);
				yield return null;
			}
			fadeAlpha = 0.0f;
			//Debug.Log(fadeAlpha);

			fading = false;
		}
	}

	public void PlayVoice(int _index)
	{
		audio.clip = voices[_index];
		audio.Play();

		if (tutorialActive)
		{
			tutorialFinished = false;
			activeAnimation.SetTalk(true);
			tutorialState = 3;
		}
	}
	public void StopVoice()
	{
		if (audio.clip != null)
			audio.Stop();
	}
	public bool IsAudioStoped()
	{
		if (audio.isPlaying) return false;
		else return true;
	}

	private void CleanObjects()
	{
		foreach (Transform child in creations) Destroy(child.gameObject);
	}
}
