using UnityEngine;
using System.Collections;

public class shelvingManagerS : MonoBehaviour {

	public shelvingVisualManagerS visualM;
	public boxManagerS boxM;

	private int state = 0;
	public GUIStyle warningStyle;

	private int randomInstanceCount = 0;

	void Awake()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		randomInstanceCount = 0;
	}
	void Start()
	{
		//starting point
		//CreateState (ShelvingSubTask.Alignment, Level.Train_without_Distracters, 0);
		CreateState((ShelvingSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 0);
	}
	void Update()
	{
		//state checks
		CheckState();

		shelvingVisualManagerS.debugLines[0] = "Skill: " + generalManagerS.ActiveSkill + ", Sub-Tusk: " + generalManagerS.ActiveSubTask + ", Level: " + generalManagerS.ActiveLevel + ", State: " + state;
		shelvingVisualManagerS.debugLines[1] = "Time: " + Time.timeSinceLevelLoad.ToString("#");
		shelvingVisualManagerS.debugLines[2] = "Last Activity Time: " + timerS.lastActivitiyTime.ToString("#.#");


		//Debug
		if (Input.GetKeyDown(KeyCode.W))
			CreateState((ShelvingSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		//Debug
	}
	void CheckState()
	{
		switch (generalManagerS.ActiveSubTask)
		{
#region CHECK_Alignment
			case (int)ShelvingSubTask.Alignment:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(ShelvingSubTask.Alignment, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);

									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Alignment, Level.Tutorial, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Alignment, Level.Tutorial, 100);	
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Alignment, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Textured, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								if (visualM.IsAudioStoped())
								{
									CreateState(ShelvingSubTask.Alignment, Level.Train_without_Distracters, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Alignment, Level.Train_without_Distracters, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Alignment, Level.Train_without_Distracters, 100);	
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Alignment, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Textured, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								if (visualM.IsAudioStoped())
								{
									CreateState(ShelvingSubTask.Alignment, Level.Train_with_Distracters, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Alignment, Level.Train_with_Distracters, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Alignment, Level.Train_with_Distracters, 100);	
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Textured, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Textured, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
#endregion
#region CHECK_Supply_Textured
			case (int)ShelvingSubTask.Supply_Textured:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(ShelvingSubTask.Supply_Textured, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Supply_Textured, Level.Tutorial, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Supply_Textured, Level.Tutorial, 100);	
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Textured, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								if (visualM.IsAudioStoped())
								{
									CreateState(ShelvingSubTask.Supply_Textured, Level.Train_without_Distracters, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Supply_Textured, Level.Train_without_Distracters, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Supply_Textured, Level.Train_without_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Textured, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								if (visualM.IsAudioStoped())
								{
									CreateState(ShelvingSubTask.Supply_Textured, Level.Train_with_Distracters, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Supply_Textured, Level.Train_with_Distracters, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Supply_Textured, Level.Train_with_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
#endregion
#region CHECK_Supply_Labeled
			case (int)ShelvingSubTask.Supply_Labeled:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(16);
								}

								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Tutorial, 100);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Labeled, Level.Train_without_Distracters, 0));
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
								if (visualM.IsAudioStoped())
								{
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Train_without_Distracters, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(16);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Train_without_Distracters, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Train_without_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(ShelvingSubTask.Supply_Labeled, Level.Train_with_Distracters, 0));
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
								if (visualM.IsAudioStoped())
								{
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Train_with_Distracters, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(16);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Train_with_Distracters, 200);
								//if all boxes OK => win
								if (!boxM.isActive)
									CreateState(ShelvingSubTask.Supply_Labeled, Level.Train_with_Distracters, 100);
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
	void CreateState(ShelvingSubTask _subTask, Level _level, int _state)
	{
		generalManagerS.ActiveSubTask = (int)_subTask;
		generalManagerS.ActiveLevel = _level;
		state = _state;

		timerS.setStateStartTime();

		Debug.Log(_subTask + ", " + _level + ", " + _state);

		switch (_subTask)
		{
#region CREATE_Alignment
			case ShelvingSubTask.Alignment:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//tutorial
								visualM.tutorialSetNo = 0;
								visualM.ActivateTutorial(true);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = 1;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(0, 1);
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(6);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								randomInstanceCount = Random.Range(3, 6);
								//randomInstanceCount = Random.Range(5, 8);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(0, randomInstanceCount);
								//visuals
								visualM.ActivateInfo(true);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters on
								//generalManagerS.DistracterM.SetActiveDistracters(917522);
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//start save session
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(3, 6);
								//if (randomInstanceCount == 0) randomInstanceCount = Random.Range(5, 8);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(0, randomInstanceCount);
								//visuals
								visualM.ActivateInfo(true);
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
#region CREATE_Supply_Textured
			case ShelvingSubTask.Supply_Textured:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//tutorial
								visualM.tutorialSetNo = 1;
								visualM.ActivateTutorial(true);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = 2;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(1, 1);
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(7);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								randomInstanceCount = Random.Range(3, 6);
								//randomInstanceCount = Random.Range(4, 7);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount * 2;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(1, randomInstanceCount);
								//visuals
								visualM.ActivateInfo(true);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters on
								//generalManagerS.DistracterM.SetActiveDistracters(917522);
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//start save session
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(3, 6);
								//if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 7);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount * 2;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(1, randomInstanceCount);
								//visuals
								visualM.ActivateInfo(true);
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
#region CREATE_Supply_Labeled
			case ShelvingSubTask.Supply_Labeled:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//tutorial
								visualM.tutorialSetNo = 2;
								visualM.ActivateTutorial(true);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = 2;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(2, 1);
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
					case Level.Train_without_Distracters:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(15);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								randomInstanceCount = Random.Range(3, 6);
								//randomInstanceCount = Random.Range(4, 7);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount * 2;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(2, randomInstanceCount);
								//visuals
								visualM.ActivateInfo(true);
								break;
							case 100:	//WIN
								//forman voice and visual
								visualM.ActivatePictographs(false);
								visualM.PlayVoice(17);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(15);

								//distracters on
								//generalManagerS.DistracterM.SetActiveDistracters(917522);
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//start save session
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(3, 6);
								//if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 7);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount * 2;
								timerS.setLevelStartTime();

								//reset game
								boxM.ResetLevel();
								break;
							case 1:
								//START GAME
								boxM.StartLevel(2, randomInstanceCount);
								//visuals
								visualM.ActivateInfo(true);
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

	IEnumerator CreateStateWithFade(ShelvingSubTask _subTask, Level _level, int _state)
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
	void OnDestroy()
	{
		//
	}
}
