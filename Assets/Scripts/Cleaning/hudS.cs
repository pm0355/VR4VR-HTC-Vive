using UnityEngine;
using System.Collections;

/*   _______________________
	|				 |_INFO_|
	|         _____         |
	|        |     |        |
	|        |_VIS_|        |
	|				 _______|
	|				|		|
	|				| PICT. |
	|_______________|_______|
*/

struct PictographTextures
{
	public Texture[] textures;
}

public class hudS : MonoBehaviour { 
	
	public bool pictographsOn {get;set;}		//script control
	public int pictographSetNo {get;set;}		//script control
	
	public Texture[] pictographs0;				//user input
	public Texture[] pictographs1;				//user input
	public Texture[] pictographs2;				//user input
	
	private PictographTextures[] pictographs;	//private all set
	public float pictographTime = 10.0f;
	private float pictographStartTime;
	private bool pictographsActive = false;
	private int textureNo = 0;
	
	public bool visualOn {get;set;}
	public int visualNo {get;set;}
	public Texture[] visuals;
	public float visualTime = 3.0f;
	private float visualStartTime;
	private bool visualsActive = false;
	
	public bool infoOn {get;set;}
	public string info {get;set;}
	private bool infoActive = false;
	
	public GUIStyle infoStyle;

	public GUIStyle debugStyle;
	
	public bool faded {get;set;}
	private bool fading;
	private float fadeAlpha = 0.0f;
	
	public static string[] debugLines = new string[20];
	
	// Use this for initialization
	void Start () {
		pictographs = new PictographTextures[3];

		pictographs[0].textures = new Texture[pictographs0.Length];
		for(int i=0; i<pictographs0.Length; i++) pictographs[0].textures[i] = pictographs0[i];
		
		pictographs[1].textures = new Texture[pictographs1.Length];
		for(int i=0; i<pictographs1.Length; i++) pictographs[1].textures[i] = pictographs1[i];		

		pictographs[2].textures = new Texture[pictographs2.Length];
		for(int i=0; i<pictographs2.Length; i++) pictographs[2].textures[i] = pictographs2[i];			
		//pictographsOn = true;
		pictographSetNo =0;
		
		//visualOn = true;
		visualNo =0;	
		
		info = "";
		//infoOn = true;
		
		faded = false;
		fading = false;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if(pictographsOn && !pictographsActive) 
		{
			pictographsActive = true;
			pictographStartTime = Time.timeSinceLevelLoad;
			timerS.setLastPictographTime();

			saverS.promptCount++;
			saverS.promptTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";
		}
		if(!pictographsOn && pictographsActive) 
		{
			pictographsActive = false;
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
		if(pictographsActive) 
		{
			textureNo++;
			if(textureNo == pictographs[pictographSetNo].textures.Length) textureNo = 0;
		}
	}	
	
	void OnGUI()
	{
		if(pictographsActive) userInterfaceS.drawTex(new Vector2(0.58f, 0.58f), new Vector2(0.4f,0.4f), pictographs[pictographSetNo].textures[textureNo]);
		if(visualsActive) userInterfaceS.drawTex(new Vector2(0.3f, 0.15f), new Vector2(0.4f,0.4f), visuals[visualNo]);
		if(infoActive) userInterfaceS.drawText(new Vector2(0.75f, 0.0f), new Vector2(0.25f, 0.15f), info, infoStyle);
		
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
}
