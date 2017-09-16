using UnityEngine;
using System.Collections;

public class cautiousnessManagerS : MonoBehaviour {

	public cautiousnessVisualManagerS visualM;
	public trafficManagerS trafficM;
	public characterS characterM;

	private int state = 0;
	private int randomInstanceCount = 0;

	public Transform tutorialTargetPos;

	void Awake()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		randomInstanceCount = 0;
	}
	void Start()
	{
		//starting point
		//CreateState (CautiousnessSubTask.Stationary, Level.Train_without_Distracters, 0);
		CreateState((CautiousnessSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 0);
	}
	void Update()
	{
		//state checks
		CheckState();

		cautiousnessVisualManagerS.debugLines[0] = "Skill: " + generalManagerS.ActiveSkill + ", Sub-Tusk: " + generalManagerS.ActiveSubTask + ", Level: " + generalManagerS.ActiveLevel + ", State: " + state;
		cautiousnessVisualManagerS.debugLines[1] = "Time: " + Time.timeSinceLevelLoad.ToString("#");
		cautiousnessVisualManagerS.debugLines[2] = "Last Activity Time: " + timerS.lastActivitiyTime.ToString("#.#");


		//Debug
		if (Input.GetKeyDown(KeyCode.W))
			CreateState((CautiousnessSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		//Debug
	}
	void CheckState()
	{
		switch (generalManagerS.ActiveSubTask)
		{
			#region CHECK_Stationary
			case (int)CautiousnessSubTask.Stationary:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(CautiousnessSubTask.Stationary, Level.Tutorial, 1);
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
									CreateState(CautiousnessSubTask.Stationary, Level.Tutorial, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Stationary, Level.Tutorial, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Stationary, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_People, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(CautiousnessSubTask.Stationary, Level.Train_without_Distracters, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Stationary, Level.Train_without_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Stationary, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_People, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(CautiousnessSubTask.Stationary, Level.Train_with_Distracters, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Stationary, Level.Train_with_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_People, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_People, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
			#endregion
			#region CHECK_Moving_People
			case (int)CautiousnessSubTask.Moving_People:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(CautiousnessSubTask.Moving_People, Level.Tutorial, 1);
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
									CreateState(CautiousnessSubTask.Moving_People, Level.Tutorial, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Moving_People, Level.Tutorial, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_People, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_without_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(CautiousnessSubTask.Moving_People, Level.Train_without_Distracters, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Moving_People, Level.Train_without_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_People, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 0));
								break;
						}
						break;
					case Level.Train_with_Distracters:
						switch (state)
						{
							case 0:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(CautiousnessSubTask.Moving_People, Level.Train_with_Distracters, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Moving_People, Level.Train_with_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
			#endregion
			#region CHECK_Moving_Cars
			case (int)CautiousnessSubTask.Moving_Cars:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 1);
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
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Tutorial, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_Cars, Level.Train_without_Distracters, 0));
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
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(16);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Train_without_Distracters, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Train_without_Distracters, 100);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(CautiousnessSubTask.Moving_Cars, Level.Train_with_Distracters, 0));
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
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(16);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Train_with_Distracters, 200);
								//if all targets OK => win
								if (!trafficM.isPlaying)
									CreateState(CautiousnessSubTask.Moving_Cars, Level.Train_with_Distracters, 100);
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
	void CreateState(CautiousnessSubTask _subTask, Level _level, int _state)
	{
		generalManagerS.ActiveSubTask = (int)_subTask;
		generalManagerS.ActiveLevel = _level;
		state = _state;

		timerS.setStateStartTime();

		Debug.Log(_subTask + ", " + _level + ", " + _state);

		switch (_subTask)
		{
			#region CREATE_Stationary
			case CautiousnessSubTask.Stationary:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//set traffic
								trafficM.ClearItems();
								trafficM.ActivateTraffic(false, false);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								
								//reset and lock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = false;
								characterM.allowRealWalk = false;

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
								
								break;
							case 1:
								//START GAME
								trafficM.CreateTargetAt(tutorialTargetPos.position, false);

								//unlock user
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;	
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

								//reset and unlock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								randomInstanceCount = Random.Range(3, 5);
								//randomInstanceCount = Random.Range(4, 5);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								trafficM.ClearItems();

								//START GAME
								trafficM.ActivateTraffic(false, false);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								trafficM.CreateTargets(randomInstanceCount, false);
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

								//reset and unlock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;

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
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(3, 5);
								//if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 5);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								trafficM.ClearItems();

								//START GAME
								trafficM.ActivateTraffic(false, false);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								trafficM.CreateTargets(randomInstanceCount, false);
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
			#region CREATE_Moving_People
			case CautiousnessSubTask.Moving_People:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//set traffic
								trafficM.ClearItems();
								trafficM.ActivateTraffic(true, false);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);

								//reset and lock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = false;
								characterM.allowRealWalk = false;

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
								saverS.instanceCount = 1;
								timerS.setLevelStartTime();
								
								break;
							case 1:
								//START GAME
								trafficM.CreateTargetAt(tutorialTargetPos.position, false);

								//unlock user
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;
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

								//reset and unlock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								randomInstanceCount = Random.Range(3, 5);
								//randomInstanceCount = Random.Range(4, 5);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								trafficM.ClearItems();

								//START GAME
								trafficM.ActivateTraffic(true, false);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								trafficM.CreateTargets(randomInstanceCount, false);

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

								//reset and unlock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;

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
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(3, 5);
								//if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 5);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								trafficM.ClearItems();

								//START GAME
								trafficM.ActivateTraffic(true, false);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								trafficM.CreateTargets(randomInstanceCount, false);

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
			#region CREATE_Moving_Cars
			case CautiousnessSubTask.Moving_Cars:
				switch (_level)
				{
					case Level.Tutorial:
						switch (_state)
						{
							case 0:
								//timing set
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//set traffic
								trafficM.ClearItems();
								trafficM.ActivateTraffic(true, true);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);

								//reset and lock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = false;
								characterM.allowRealWalk = false;

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
								saverS.instanceCount = 1;
								timerS.setLevelStartTime();

								break;
							case 1:
								//START GAME
								trafficM.CreateTargetAt(tutorialTargetPos.position, true);

								//unlock user
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;
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

								//reset and unlock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(15);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//start save session
								randomInstanceCount = Random.Range(3, 5);
								//randomInstanceCount = Random.Range(4, 5);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								trafficM.ClearItems();

								//START GAME
								trafficM.ActivateTraffic(true, true);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								trafficM.CreateTargets(randomInstanceCount, true);
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

								//reset and unlock the user
								characterM.transform.GetChild(0).position = Vector3.zero;
								characterM.allowWalkInPlace = true;
								characterM.allowRealWalk = false;

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
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(3, 5);
								//if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 5);
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
								timerS.setLevelStartTime();

								//reset game
								trafficM.ClearItems();

								//START GAME
								trafficM.ActivateTraffic(true, true);
								trafficM.PlaceLowLevelObstacles();
								trafficM.PlaceParkingCars(0.2f);
								trafficM.CreateTargets(randomInstanceCount, true);
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

	IEnumerator CreateStateWithFade(CautiousnessSubTask _subTask, Level _level, int _state)
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
