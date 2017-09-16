using UnityEngine;
using System.Collections;

public class moneyVisualManagerS : MonoBehaviour {

	public moneySupervisorAnimationS supervisorAnimation;

	public int tutorialSetNo {get;set;}	
	public Texture[] tutorial1;					//user input
	public Texture[] tutorial2;					//user input
	public Texture[] tutorial3;					//user input
	public bool tutorialActive { get; set; }
	private int tutorialTextureNo = 0;
	private int tutorialState = 0;
	private int tutorialPartNo = 0;
	public bool tutorialFinished {get;set;}
	
	public int pictographSetNo {get;set;}
	//public Texture[] pictograph1;				//user input
	//public Texture[] pictograph2;				//user input
	//public Texture[] pictograph3;				//user input
	public float pictographTime = 10.0f;
	private float pictographStartTime;
	public bool pictographsActive { get; set; }
	private int textureNo = 0;
	
	public int visualNo {get;set;}
	public Texture[] visuals;
	public float visualTime = 3.0f;
	private float visualStartTime;
	public bool visualsActive { get; set; }
	
	public string info {get;set;}
	public bool infoActive { get; set; }
	
	public GUIStyle infoStyle;
	
	public static string[] debugLines = new string[20];
	public GUIStyle debugStyle;
	
	public bool faded {get;set;}
	private bool fading;
	private float fadeAlpha = 0.0f;
	
	public AudioClip[] voices;

	public TutorialPart[] tutorialParts;

	private bool bypass = false;

	[System.Serializable]
	public struct TutorialPart
	{
		public AudioClip[] tutorialDubs;
		public Texture[] tutorialTextures;
	}

	//DUBS
	//1-  subtask1 tutorial-your turn 	037
	//2-  subtask2 tutorial-your turn 	043
	//3-  subtask3 tutorial-your turn 	049
	//4-  subtask1 tutorial-finish		039
	//5-  subtask2 tutorial-finish		045
	//6-  subtask3 tutorial-finish		051
	//7-  subtask1 practice-start		040
	//8- subtask1 pictograph			038
	//9- subtask1 practice-finish		041
	//10- subtask2 practice-start		046
	//11- subtask2 pictograph			044
	//12- subtask2 practice-finish		047
	//13- subtask3 practice-start		052
	//14- subtask3 pictograph			050
	//15- subtask3 practice-finish		053
	
	// Use this for initialization
	void Start () 
	{
		tutorialSetNo =0;
		//tutorialOn = false;
		tutorialActive = false;
		tutorialFinished = false;
		
		pictographSetNo =0;
		//pictographsOn = false;
		pictographsActive = false;
		
		visualNo =0;
		//visualOn = false;
		visualsActive = false;
		
		info = "";
		//infoOn = false;
		infoActive = false;
		
		faded = false;
		fading = false;

		supervisorAnimation.Activate(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			bypass = true;
			audio.Stop();
		}

		if(pictographsActive && Time.timeSinceLevelLoad > pictographStartTime + pictographTime)
			ActivatePictographs(false);	

		if(visualsActive && Time.timeSinceLevelLoad > visualStartTime + visualTime)
			ActivateVisuals(false);				
	}

	void FixedUpdate () 
	{
		if(tutorialActive && (tutorialState==2)) 
		{
			if(tutorialSetNo == 0 && tutorialTextureNo < tutorial1.Length) tutorialTextureNo++;
			if(tutorialSetNo == 1 && tutorialTextureNo < tutorial2.Length) tutorialTextureNo++;
			if(tutorialSetNo == 2 && tutorialTextureNo < tutorial3.Length) tutorialTextureNo++;
		}
		
		
		if(pictographsActive) 
		{
			textureNo++;
			if (pictographSetNo == 0 && textureNo == tutorial1.Length-1) textureNo = 0;
			if (pictographSetNo == 1 && textureNo == tutorial2.Length-1) textureNo = 0;
			if (pictographSetNo == 2 && textureNo == tutorial3.Length-1) textureNo = 0;
		}
	}	
	
	void OnGUI()
	{
		if(tutorialActive) 
		{
			switch (tutorialState) {
			case 0:			// play initial talk
				tutorialFinished = false;
				supervisorAnimation.Activate(true);
				supervisorAnimation.Talk(true);

				//if (tutorialSetNo == 0) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorial1[0]);
				//if (tutorialSetNo == 1) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorial2[0]);
				//if (tutorialSetNo == 2) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorial3[0]);

				tutorialState++;
				break;
			case 1:
				if (!audio.isPlaying || bypass)
				{
					tutorialPartNo++;

					if(tutorialPartNo > tutorialParts[tutorialSetNo].tutorialTextures.Length)
					{
						tutorialState++;
						tutorialTextureNo = 0;
						supervisorAnimation.Talk(false);
					}
					else
					{
						audio.clip = tutorialParts[tutorialSetNo].tutorialDubs[tutorialPartNo-1];
						audio.Play();
					}
				}

				if (tutorialPartNo <= tutorialParts[tutorialSetNo].tutorialTextures.Length) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorialParts[tutorialSetNo].tutorialTextures[tutorialPartNo - 1]);
				break;
			case 2:			// main tutorial

				if (tutorialSetNo == 0) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorial1[tutorialTextureNo]);
				if (tutorialSetNo == 1) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorial2[tutorialTextureNo]);
				if (tutorialSetNo == 2) userInterfaceS.drawTex(new Vector2(0.06f, 0.05f), new Vector2(0.6f, 0.8f), tutorial3[tutorialTextureNo]);

				if (tutorialSetNo == 0) if (tutorialTextureNo == tutorial1.Length - 1 || bypass) tutorialState++;
				if (tutorialSetNo == 1) if (tutorialTextureNo == tutorial2.Length - 1 || bypass) tutorialState++;
				if (tutorialSetNo == 2) if (tutorialTextureNo == tutorial3.Length - 1 || bypass) tutorialState++;
				break;
			case 3:			// play next talk
				supervisorAnimation.Talk(true);
				
				audio.clip = voices[tutorialSetNo];
				audio.Play();
				tutorialState++;
				break;
			case 4:			// forman animation	
				if (!audio.isPlaying || bypass)
				{
					tutorialState++;
					tutorialTextureNo = 0;
				}
				break;
			case 5:			//ended
				supervisorAnimation.Talk(false);
				tutorialFinished = true;
				break;
			case 100:		//just forman animation
				supervisorAnimation.Talk(true);
			
				if(!audio.isPlaying)
				{
					tutorialState = 5;
					tutorialTextureNo = 0;
				}
				break;
			}
			bypass = false;
		}
		
		if(pictographsActive) 
		{
			if (pictographSetNo == 0) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f, 0.4f), tutorial1[textureNo]);
			if (pictographSetNo == 1) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f, 0.4f), tutorial2[textureNo]);
			if (pictographSetNo == 2) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f, 0.4f), tutorial3[textureNo]);
		}
		if(visualsActive) userInterfaceS.drawTex(new Vector2(0.3f, 0.15f), new Vector2(0.4f,0.4f), visuals[visualNo]);
		if(infoActive) userInterfaceS.drawText(new Vector2(0.59f, 0.052f), new Vector2(0.25f, 0.1f), info, infoStyle);
		
		if(fading)	userInterfaceS.drawTexEverywhere(visuals[3],fadeAlpha);
		
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
			tutorialTextureNo = 0;
			tutorialState = 0;
			tutorialPartNo = 0;
			tutorialActive = true;
		}
		if (!_toActive && tutorialActive)
		{
			if (audio.clip == voices[0] || audio.clip == voices[1] || audio.clip == voices[2] || audio.clip == voices[3] || audio.clip == voices[4] || audio.clip == voices[5]) audio.Stop();
			supervisorAnimation.Activate(false);
			tutorialActive = false;
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
		if(!fading)
		{
			StartCoroutine(Fader(1.0f, true));
			//Debug.Log("fade(true)");
		}
	}
	
	public void FadeIn()
	{	
		if(faded)
		{
			StartCoroutine(Fader(1.0f, false));
			//Debug.Log("fade(false)");
		}		
	}
	
	IEnumerator Fader(float _time, bool _isFadeOut) 
	{
		if(_isFadeOut)
		{
			fading = true;
			for (float f=0.0f; f < _time; f += Time.deltaTime) 
			{
				fadeAlpha = f/_time;
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
			for (float f=_time; f > 0.0f; f -= Time.deltaTime) 
			{
				fadeAlpha = f/_time;
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
		
		if(tutorialActive)
		{
			tutorialFinished = false;
			tutorialState = 100;
		}
	}
	public void StopVoice()
	{
		if(audio.clip != null)
			audio.Stop();
	}
	public bool IsAudioStoped()
	{
		if(audio.isPlaying) return false;
		else return true;
	}
}
