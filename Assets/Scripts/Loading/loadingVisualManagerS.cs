using UnityEngine;
using System.Collections;

public class loadingVisualManagerS : MonoBehaviour {
	
	public bool tutorialOn {get;set;}		//script control
	public int tutorialSetNo {get;set;}		//script control
	public Texture[] foreman;					//user input
	public Texture[] tutorial1;					//user input
	public Texture[] tutorial2;					//user input
	public Texture[] tutorial3;					//user input
	private bool tutorialActive = false;
	private int foremanTextureNo = 0;
	private int tutorialTextureNo = 0;
	private int tutorialState = 0;
	public bool tutorialFinished {get;set;}
	
	public bool pictographsOn {get;set;}		//script control
	public int pictographSetNo {get;set;}		//script control
	public Texture[] pictograph1;				//user input
	public Texture[] pictograph2;				//user input
	public Texture[] pictograph3;				//user input
	public float pictographTime = 10.0f;
	private float pictographStartTime;
	private bool pictographsActive = false;
	private int textureNo = 0;
	
	public bool visualOn {get;set;}
	public int visualNo {get;set;}
	public Texture[] visuals;
	public float visualTime = 3.0f;
	public float visualStartTime { get; set; }
	private bool visualsActive = false;
	
	public bool infoOn {get;set;}
	public string info {get;set;}
	private bool infoActive = false;
	
	public GUIStyle infoStyle;
	
	public static string[] debugLines = new string[20];
	public GUIStyle debugStyle;
	
	public bool faded {get;set;}
	private bool fading;
	private float fadeAlpha = 0.0f;
	
	public AudioClip[] voices;
	
	//DUBS
	//0-  subtask1 tutorial-1 			018
	//1-  subtask1 tutorial-2 			019
	//2-  subtask2 tutorial-1 			024
	//3-  subtask2 tutorial-2			025
	//4-  subtask3 tutorial-1 			030
	//5-  subtask3 tutorial-2 			031
	//6-  subtask1 tutorial-finish		021
	//7-  subtask2 tutorial-finish		027
	//8-  subtask3 tutorial-finish		033
	//9-  subtask1 practice-start		022
	//10- subtask1 pictograph			020
	//11- subtask1 practice-finish		023
	//12- subtask2 practice-start		028
	//13- subtask2 pictograph			026
	//14- subtask2 practice-finish		029
	//15- subtask3 practice-start		034
	//16- subtask3 pictograph			032
	//17- subtask3 practice-finish		035
	
	private bool bypass = false;

	// Use this for initialization
	void Start () 
	{
		tutorialSetNo =0;
		tutorialOn = false;
		tutorialActive = false;
		tutorialFinished = false;
		
		pictographSetNo =0;
		pictographsOn = false;
		pictographsActive = false;
		
		visualNo =0;
		visualOn = false;
		visualsActive = false;
		
		info = "";
		infoOn = false;
		infoActive = false;
		
		faded = false;
		fading = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			bypass = true;
			audio.Stop();
		}

		if(tutorialOn && !tutorialActive) 
		{
			tutorialTextureNo = 0;
			tutorialActive = true;
			tutorialState = 0;
		}
		if(!tutorialOn && tutorialActive) 
		{
			tutorialActive = false;
			if(audio.clip==voices[0] || audio.clip==voices[1] || audio.clip==voices[2] || audio.clip==voices[3] || audio.clip==voices[4] || audio.clip==voices[5]) audio.Stop();
		}		
		
		if(pictographsOn && !pictographsActive) 
		{
			textureNo = 0;
			pictographsActive = true;
			pictographStartTime = Time.timeSinceLevelLoad;
			timerS.setLastPictographTime();

			saverS.promptCount++;
			saverS.promptTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";
		}
		if(!pictographsOn && pictographsActive) 
		{
			pictographsActive = false;
			if (audio.clip == voices[10] || audio.clip == voices[13] || audio.clip == voices[16]) audio.Stop();
		}		
		if(pictographsActive && Time.timeSinceLevelLoad > pictographStartTime + pictographTime)
		{
			pictographsOn = false;
			pictographsActive = false;
		}
		
		
		if(visualOn && !visualsActive) 
		{
			visualsActive = true;
			visualStartTime = Time.timeSinceLevelLoad;
		}	
		if(!visualOn && visualsActive) 
		{
			visualsActive = false;
		}	
		if(visualsActive && Time.timeSinceLevelLoad > visualStartTime + visualTime)
		{
			visualOn = false;
			visualsActive = false;
		}
		
		
		if(infoOn && !infoActive) 
		{
			infoActive = true;
		}		
		if(!infoOn && infoActive) 
		{
			infoActive = false;
		}			
	}
	
	void FixedUpdate () 
	{
		if(tutorialActive && (tutorialState==1 || tutorialState==4 || tutorialState==100)) 
		{
			foremanTextureNo++;
			if(foremanTextureNo == foreman.Length) foremanTextureNo = 0;
		}
		if(tutorialActive && (tutorialState==2)) 
		{
			if(tutorialSetNo == 0 && tutorialTextureNo < tutorial1.Length) tutorialTextureNo++;
			if(tutorialSetNo == 1 && tutorialTextureNo < tutorial2.Length) tutorialTextureNo++;
			if(tutorialSetNo == 2 && tutorialTextureNo < tutorial3.Length) tutorialTextureNo++;
		}
		
		
		if(pictographsActive) 
		{
			textureNo++;
			if(pictographSetNo == 0 && textureNo == pictograph1.Length) textureNo = 0;
			if(pictographSetNo == 1 && textureNo == pictograph2.Length) textureNo = 0;
			if(pictographSetNo == 2 && textureNo == pictograph3.Length) textureNo = 0;
		}
	}	
	
	void OnGUI()
	{
		if(tutorialActive) 
		{
			switch (tutorialState) {
			case 0:
				tutorialFinished = false;
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[0]);
				
				if(tutorialSetNo == 0 ) userInterfaceS.drawTexEverywhere(tutorial1[0], 1.0f);
				if(tutorialSetNo == 1 ) userInterfaceS.drawTexEverywhere(tutorial2[0], 1.0f);
				if(tutorialSetNo == 2 ) userInterfaceS.drawTexEverywhere(tutorial3[0], 1.0f);
				
				audio.clip = voices[tutorialSetNo*2];
				audio.Play();
				tutorialState++;
				break;
			case 1:
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[foremanTextureNo]);
				
				if(tutorialSetNo == 0 ) userInterfaceS.drawTexEverywhere(tutorial1[0], 1.0f);
				if(tutorialSetNo == 1 ) userInterfaceS.drawTexEverywhere(tutorial2[0], 1.0f);
				if(tutorialSetNo == 2 ) userInterfaceS.drawTexEverywhere(tutorial3[0], 1.0f);

				if (!audio.isPlaying || bypass)
				{
					tutorialState++;
					tutorialTextureNo = 0;
				}
				break;
			case 2:
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[0]);
				
				if(tutorialSetNo == 0 ) userInterfaceS.drawTexEverywhere(tutorial1[tutorialTextureNo], 1.0f);
				if(tutorialSetNo == 1 ) userInterfaceS.drawTexEverywhere(tutorial2[tutorialTextureNo], 1.0f);
				if(tutorialSetNo == 2 ) userInterfaceS.drawTexEverywhere(tutorial3[tutorialTextureNo], 1.0f);

				if (tutorialSetNo == 0) if (tutorialTextureNo == tutorial1.Length - 1 || bypass) { tutorialState++; }
				if (tutorialSetNo == 1) if (tutorialTextureNo == tutorial2.Length - 1 || bypass) { tutorialState++; }
				if (tutorialSetNo == 2) if (tutorialTextureNo == tutorial3.Length - 1 || bypass) { tutorialState++; }
				break;
			case 3:
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[0]);
				
				if(tutorialSetNo == 0 ) userInterfaceS.drawTexEverywhere(tutorial1[tutorial1.Length - 1], 1.0f);
				if(tutorialSetNo == 1 ) userInterfaceS.drawTexEverywhere(tutorial2[tutorial2.Length - 1], 1.0f);
				if(tutorialSetNo == 2 ) userInterfaceS.drawTexEverywhere(tutorial3[tutorial3.Length - 1], 1.0f);
				
				audio.clip = voices[tutorialSetNo*2 + 1];
				audio.Play();
				tutorialState++;
				break;
			case 4:
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[foremanTextureNo]);
				
				if(tutorialSetNo == 0 ) userInterfaceS.drawTexEverywhere(tutorial1[tutorial1.Length - 1], 1.0f);
				if(tutorialSetNo == 1 ) userInterfaceS.drawTexEverywhere(tutorial2[tutorial2.Length - 1], 1.0f);
				if(tutorialSetNo == 2 ) userInterfaceS.drawTexEverywhere(tutorial3[tutorial3.Length - 1], 1.0f);

				if (!audio.isPlaying || bypass)
				{
					tutorialState++;
					tutorialTextureNo = 0;
				}
				break;
			case 5:
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[0]);
				tutorialFinished = true;
				break;
			case 100:
				userInterfaceS.drawTex(new Vector2(0.85f, 0.25f), new Vector2(0.15f,0.75f), foreman[foremanTextureNo]);
				
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
			if(pictographSetNo == 0 ) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f,0.4f), pictograph1[textureNo]);
			if(pictographSetNo == 1 ) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f,0.4f), pictograph2[textureNo]);
			if(pictographSetNo == 2 ) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f,0.4f), pictograph3[textureNo]);
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
