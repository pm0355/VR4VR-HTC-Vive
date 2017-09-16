using UnityEngine;
using System.Collections;

public class socialManagerS : MonoBehaviour {

	public socialVisualManagerS visualM;
	public socialCharactersManagerS charactersM;
	private Transform creations;

	private socialCharacterAnimatorS characterAnimation;
	public GameObject[] trainingCharacters;
	public AudioClip[] questionsSubtask0;
	public int[] moodSubtask0;
	public AudioClip[] questionsSubtask1;
	public int[] moodSubtask1;
	public AudioClip[] questionsSubtask2;
	public int[] moodSubtask2;

	public Texture[] backgroundsSubtask0;
	public Texture[] backgroundsSubtask1;
	public Texture[] backgroundsSubtask2;
	public Renderer background;

	public socialBackgroundAudioS bgAudioM;

	private bool characterSpeaking = false;

	private int state = 0;
	public GUIStyle warningStyle;

	private bool questionState = true;

	void Awake()
	{
		Random.seed = System.DateTime.Now.Millisecond;
	}

	// Use this for initialization
	void Start()
	{
		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;

		//starting point
		//CreateState (SocialnessSubTask.Basic_Personal, Level.Train_without_Distracters, 0);
		CreateState((SocialnessSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 0);
	}

	// Update is called once per frame
	void Update()
	{

		//debug
		/*
		if (Input.GetKeyDown(KeyCode.Alpha0)) AskQuestion(0);
		if (Input.GetKeyDown(KeyCode.Alpha1)) AskQuestion(1);
		if (Input.GetKeyDown(KeyCode.Alpha2)) AskQuestion(2);
		if (Input.GetKeyDown(KeyCode.Alpha3)) AskQuestion(3);
		if (Input.GetKeyDown(KeyCode.Alpha4)) AskQuestion(4);
		if (Input.GetKeyDown(KeyCode.Alpha5)) AskQuestion(5);
		if (Input.GetKeyDown(KeyCode.Alpha6)) AskQuestion(6);
		if (Input.GetKeyDown(KeyCode.Alpha7)) AskQuestion(7);
		if (Input.GetKeyDown(KeyCode.Alpha8)) AskQuestion(8);
		if (Input.GetKeyDown(KeyCode.Alpha9)) AskQuestion(9);
		*/
		//debug




		//state checks
		CheckState();

		socialVisualManagerS.debugLines[0] = "Skill: " + generalManagerS.ActiveSkill + ", Sub-Tusk: " + generalManagerS.ActiveSubTask + ", Level: " + generalManagerS.ActiveLevel + ", State: " + state;
		socialVisualManagerS.debugLines[1] = "Time: " + Time.timeSinceLevelLoad.ToString("#");
		socialVisualManagerS.debugLines[2] = "Last Activity Time: " + timerS.lastActivitiyTime.ToString("#.#");


		//Debug
		if (Input.GetKeyDown(KeyCode.W))
			CreateState((SocialnessSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		//Debug

		if (characterSpeaking && !audio.isPlaying)
		{
			characterAnimation.SetTalk(false, 0);
			characterSpeaking = false;
		}
	}
	void CheckState()
	{
		switch (generalManagerS.ActiveSubTask)
		{
			#region CHECK_Basic_Personal
			case (int)SocialnessSubTask.Basic_Personal:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(SocialnessSubTask.Basic_Personal, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);

									visualM.PlayVoice(7);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Basic_Personal, Level.Tutorial, 200);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Basic_Personal, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Longer_Vocational, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(7);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Basic_Personal, Level.Train_without_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Basic_Personal, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Longer_Vocational, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(7);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Basic_Personal, Level.Train_with_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Longer_Vocational, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Longer_Vocational, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
			#endregion
			#region CHECK_Longer_Vocational
			case (int)SocialnessSubTask.Longer_Vocational:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(SocialnessSubTask.Longer_Vocational, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Longer_Vocational, Level.Tutorial, 200);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Longer_Vocational, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Moody_Hard, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Longer_Vocational, Level.Train_without_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Longer_Vocational, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Moody_Hard, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Longer_Vocational, Level.Train_with_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Moody_Hard, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Moody_Hard, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
			#endregion
			#region CHECK_Moody_Hard
			case (int)SocialnessSubTask.Moody_Hard:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(SocialnessSubTask.Moody_Hard, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}

								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Moody_Hard, Level.Tutorial, 200);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Moody_Hard, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									generalManagerS.EndLevel();
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Moody_Hard, Level.Train_without_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(SocialnessSubTask.Moody_Hard, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									generalManagerS.EndLevel();
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (!questionState && Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(SocialnessSubTask.Moody_Hard, Level.Train_with_Distracters, 200);
								break;
							case 100:	//WIN
								if (!visualM.visualsActive)
									generalManagerS.EndLevel();
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									generalManagerS.EndLevel();
								break;
						}
						break;
				}
				break;
			#endregion
		}
	}
	void CreateState(SocialnessSubTask _subTask, Level _level, int _state)
	{
		generalManagerS.ActiveSubTask = (int)_subTask;
		generalManagerS.ActiveLevel = _level;
		state = _state;

		GameObject tempObj = null;
		timerS.setStateStartTime();

		Debug.Log(_subTask + ", " + _level + ", " + _state);

		questionState = true;

		switch (_subTask)
		{
			#region CREATE_Basic_Personal
			case SocialnessSubTask.Basic_Personal:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//tutorial
								CleanObjects();
								visualM.tutorialSetNo = 0;
								visualM.ActivateTutorial(true);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//background
								background.material.mainTexture = backgroundsSubtask0[Random.Range(0, backgroundsSubtask0.Length)];

								//other characters
								charactersM.CreateCharacters(0);

								//background sound
								bgAudioM.PlayAudio(0);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 1:
								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 0, true);
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(3);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(6);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//background
								background.material.mainTexture = backgroundsSubtask0[Random.Range(0, backgroundsSubtask0.Length)];

								//other characters
								charactersM.CreateCharacters(0);

								//create character
								CleanObjects();
								tempObj = Instantiate(trainingCharacters[0]) as GameObject;
								tempObj.transform.parent = creations;
								characterAnimation = tempObj.GetComponentInChildren<socialCharacterAnimatorS>();

								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 0, false);

								//background sound
								bgAudioM.PlayAudio(0);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(8);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(6);

								//distracters on
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//background
								background.material.mainTexture = backgroundsSubtask0[Random.Range(0, backgroundsSubtask0.Length)];

								//other characters
								charactersM.CreateCharacters(0);

								//create character
								CleanObjects();
								tempObj = Instantiate(trainingCharacters[1]) as GameObject;
								tempObj.transform.parent = creations;
								characterAnimation = tempObj.GetComponentInChildren<socialCharacterAnimatorS>();

								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 0, false);

								//background sound
								bgAudioM.PlayAudio(0);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
				}
				break;
			#endregion
			#region CREATE_Longer_Vocational
			case SocialnessSubTask.Longer_Vocational:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//tutorial
								CleanObjects();
								visualM.tutorialSetNo = 1;
								visualM.ActivateTutorial(true);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//background
								background.material.mainTexture = backgroundsSubtask1[Random.Range(0, backgroundsSubtask1.Length)];

								//other characters
								charactersM.CreateCharacters(1);

								//background sound
								bgAudioM.PlayAudio(1);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 1:
								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 1, true);
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(4);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//background
								background.material.mainTexture = backgroundsSubtask1[Random.Range(0, backgroundsSubtask1.Length)];

								//other characters
								charactersM.CreateCharacters(1);

								//create character
								CleanObjects();
								tempObj = Instantiate(trainingCharacters[2]) as GameObject;
								tempObj.transform.parent = creations;
								characterAnimation = tempObj.GetComponentInChildren<socialCharacterAnimatorS>();

								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 1, false);

								//background sound
								bgAudioM.PlayAudio(1);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(11);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters on
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//background
								background.material.mainTexture = backgroundsSubtask1[Random.Range(0, backgroundsSubtask1.Length)];

								//other characters
								charactersM.CreateCharacters(1);

								//create character
								CleanObjects();
								tempObj = Instantiate(trainingCharacters[3]) as GameObject;
								tempObj.transform.parent = creations;
								characterAnimation = tempObj.GetComponentInChildren<socialCharacterAnimatorS>();

								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 1, false);

								//background sound
								bgAudioM.PlayAudio(1);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 100:	//WIN
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
				}
				break;
			#endregion
			#region CREATE_Moody_Hard
			case SocialnessSubTask.Moody_Hard:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//tutorial
								CleanObjects();
								visualM.tutorialSetNo = 2;
								visualM.ActivateTutorial(true);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//background
								background.material.mainTexture = backgroundsSubtask2[Random.Range(0, backgroundsSubtask2.Length)];

								//other characters
								charactersM.CreateCharacters(2);

								//background sound
								bgAudioM.PlayAudio(2);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 1:
								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 2, true);
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(5);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//background
								background.material.mainTexture = backgroundsSubtask2[Random.Range(0, backgroundsSubtask2.Length)];

								//other characters
								charactersM.CreateCharacters(2);

								//create character
								CleanObjects();
								tempObj = Instantiate(trainingCharacters[4]) as GameObject;
								tempObj.transform.parent = creations;
								characterAnimation = tempObj.GetComponentInChildren<socialCharacterAnimatorS>();

								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 2, false);

								//background sound
								bgAudioM.PlayAudio(2);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(14);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 240.0f, 600.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters on
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//background
								background.material.mainTexture = backgroundsSubtask2[Random.Range(0, backgroundsSubtask2.Length)];

								//other characters
								charactersM.CreateCharacters(2);

								//create character
								CleanObjects();
								tempObj = Instantiate(trainingCharacters[5]) as GameObject;
								tempObj.transform.parent = creations;
								characterAnimation = tempObj.GetComponentInChildren<socialCharacterAnimatorS>();

								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 2, false);

								//background sound
								bgAudioM.PlayAudio(2);

								//start save session
								saverS.StartSaveSession();
								timerS.setLevelStartTime();
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 0;
								visualM.ActivateVisuals(true);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//save session
								saverS.result = 1;
								saverS.SaveDataBase();
								break;
							case 200:	//FAIL
								//visual
								visualM.ActivatePictographs(false);
								visualM.visualNo = 1;
								visualM.ActivateVisuals(true);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//save session
								saverS.result = 2;
								saverS.SaveDataBase();
								break;
						}
						break;
				}
				break;
			#endregion
		}

		//PHOTON CODE
		generalManagerS.PhotonUpdate();
		//PHOTON CODE
	}
	void OnGUI()
	{
		if (!networkManagerS.isConnected) userInterfaceS.drawText(new Vector2(0.5f, 0.0f), new Vector2(0.5f, 0.1f), "Please Launch the Application", warningStyle);
	}

	IEnumerator CreateStateWithFade(SocialnessSubTask _subTask, Level _level, int _state)
	{
		visualM.FadeOut();
		for (; ; )
		{
			if (visualM.faded) break;
			yield return null;
		}

		CreateState(_subTask, _level, _state);
		visualM.FadeIn();
	}
	void CleanObjects()
	{
		foreach (Transform child in creations) Destroy(child.gameObject);
	}
	void OnDestroy()
	{
		//networkView.RPC("StopTask", RPCMode.OthersBuffered);
	}

	[RPC]
	void StartSubTask(int _subtask, bool _isTutorial)
	{
		throw new System.NotImplementedException();
	}
	[RPC]
	void StopTask()
	{
		throw new System.NotImplementedException();
	}
	[RPC]
	void AskQuestion(int _index)
	{
		if (generalManagerS.ActiveLevel == Level.Tutorial)
		{
			if (generalManagerS.ActiveSubTask == 0) visualM.PlayVoice(15);
			if (generalManagerS.ActiveSubTask == 1) visualM.PlayVoice(16);
			if (generalManagerS.ActiveSubTask == 2) visualM.PlayVoice(17);
		}
		else
		{
			if(_index<100)
			{
				if (generalManagerS.ActiveSubTask == 0)
				{
					characterAnimation.SetTalk(true, moodSubtask0[_index]);
					audio.clip = questionsSubtask0[_index];
				}
				if (generalManagerS.ActiveSubTask == 1)
				{
					characterAnimation.SetTalk(true, moodSubtask1[_index]);
					audio.clip = questionsSubtask1[_index];
				}
				if (generalManagerS.ActiveSubTask == 2)
				{
					characterAnimation.SetTalk(true, moodSubtask2[_index]);
					audio.clip = questionsSubtask2[_index];
				}
				audio.Play();
				characterSpeaking = true;
			}
		}

		timerS.setLastActivitiyTime();
		questionState = false;
		saverS.instanceCount++;
	}
	[RPC]
	void ShowVisual(bool _isCorrect)
	{
		if (visualM.visualsActive) visualM.ActivateVisuals(false);
		if (_isCorrect)
		{
			saverS.successCount++;
			saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

			//Debug.Log("Correct");
			visualM.visualNo = 2;
			visualM.ActivateVisuals(true);

			timerS.setLastActivitiyTime();

			saverS.SaveText("Correct");
		}
		else
		{
			saverS.failCount++;
			saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";
			saverS.SaveText("Wrong");

			//Debug.Log("Wrong");
			visualM.visualNo = 4;
			visualM.ActivateVisuals(true);
		}

		//PHOTON CODE
		generalManagerS.PhotonProgressUpdate();
		//PHOTON CODE

		questionState = true;
	}
	[RPC]
	void SubTaskFinished(bool _isWin)
	{
		if (visualM.visualsActive) visualM.ActivateVisuals(false);
		if (_isWin)
		{
			//Debug.Log("WIN");
			CreateState((SocialnessSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		}
		else
		{
			//Debug.Log("LOSE");
			CreateState((SocialnessSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 200);
		}
		timerS.setLastActivitiyTime();
	}
	[RPC]
	void SaveLog(string _text)
	{
		saverS.SaveText(_text);
	}
}
