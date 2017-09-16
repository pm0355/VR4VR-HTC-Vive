using UnityEngine;
using System.Collections;

public class moneyManagerS : MonoBehaviour {

	public moneyVisualManagerS visualM;

	public GameObject[] customers;
	public Transform[] customerPositions;
	private Transform creations;

	private int state = 0;

	public GUIStyle warningStyle;

	private int randomInstanceCount = 0;

	void Awake()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		randomInstanceCount = 0;
	}

	// Use this for initialization
	void Start () 
	{
		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;

		//starting point
		//CreateState (MoneyManagementSubTask.Recognizing, Level.Train_without_Distracters, 0);
		CreateState((MoneyManagementSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 0);	
	}
	
	// Update is called once per frame
	void Update () {
		//state checks
		CheckState();

		moneyVisualManagerS.debugLines[0] = "Skill: " + generalManagerS.ActiveSkill + ", Sub-Tusk: " + generalManagerS.ActiveSubTask + ", Level: " + generalManagerS.ActiveLevel + ", State: " + state;
		moneyVisualManagerS.debugLines[1] = "Time: " + Time.timeSinceLevelLoad.ToString("#");
		moneyVisualManagerS.debugLines[2] = "Last Activity Time: " + timerS.lastActivitiyTime.ToString("#.#");


		//Debug
		if(Input.GetKeyDown(KeyCode.W))
			CreateState((MoneyManagementSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		//Debug
	}
	void CheckState()
	{
		switch (generalManagerS.ActiveSubTask)
		{
#region CHECK_Recognizing
			case (int)MoneyManagementSubTask.Recognizing:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(MoneyManagementSubTask.Recognizing, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 0;
									visualM.ActivatePictographs(true);

									visualM.PlayVoice(7);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Recognizing, Level.Tutorial, 200);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Recognizing, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Counting, Level.Tutorial, 0));
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
									visualM.PlayVoice(7);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Recognizing, Level.Train_without_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Recognizing, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Counting, Level.Tutorial, 0));
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
									visualM.PlayVoice(7);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Recognizing, Level.Train_with_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Counting, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Counting, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
				#endregion
#region CHECK_Counting
			case (int)MoneyManagementSubTask.Counting:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(MoneyManagementSubTask.Counting, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 1;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Counting, Level.Tutorial, 200);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Counting, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Change, Level.Tutorial, 0));
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
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Counting, Level.Train_without_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Counting, Level.Train_with_Distracters, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Change, Level.Tutorial, 0));
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
									visualM.PlayVoice(10);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Counting, Level.Train_with_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Change, Level.Tutorial, 0));
								break;
							case 200:	//FAIL
								if (!visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Change, Level.Tutorial, 0));
								break;
						}
						break;
				}
				break;
			#endregion
#region CHECK_Change
			case (int)MoneyManagementSubTask.Change:
				switch (generalManagerS.ActiveLevel)
				{
					case Level.Tutorial:
						switch (state)
						{
							case 0:
								if (visualM.tutorialFinished)
								{
									CreateState(MoneyManagementSubTask.Change, Level.Tutorial, 1);
								}
								break;
							case 1:
								//if 1 min passed => sound, animation, pictograph
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
								{
									visualM.pictographSetNo = 2;
									visualM.ActivatePictographs(true);
									visualM.PlayVoice(13);
								}

								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Change, Level.Tutorial, 200);
								break;
							case 100:	//WIN
								//if(visualM.tutorialFinished && !visualM.visualsActive)
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Change, Level.Train_without_Distracters, 0));
								break;
							case 200:	//FAIL
								if(!visualM.visualsActive)
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
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Change, Level.Train_without_Distracters, 200);
								break;
							case 100:	//WIN
								if (visualM.IsAudioStoped() && !visualM.visualsActive)
									StartCoroutine(CreateStateWithFade(MoneyManagementSubTask.Change, Level.Train_with_Distracters, 0));
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
									visualM.PlayVoice(13);
								}
								//if 5 min passed => fail
								if (Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime)
									CreateState(MoneyManagementSubTask.Change, Level.Train_with_Distracters, 200);
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
	void CreateState(MoneyManagementSubTask _subTask, Level _level, int _state)
	{
		generalManagerS.ActiveSubTask = (int)_subTask;
		generalManagerS.ActiveLevel = _level;
		state = _state;

		GameObject tempObj = null;
		timerS.setStateStartTime();

		Debug.Log(_subTask + ", " + _level + ", " + _state);

		switch (_subTask)
		{
#region CREATE_Recognizing
			case MoneyManagementSubTask.Recognizing:
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

								//create customer
								CleanObjects();
								tempObj = Instantiate(customers[Random.Range(0, customers.Length)], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = 1;
								timerS.setLevelStartTime();
								break;
							case 1:
								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 0, 1);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(6);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//create customer
								CleanObjects();
								int customerIndex = Random.Range(0, customers.Length);
								tempObj = Instantiate(customers[customerIndex], customerPositions[0].position, customerPositions[0].rotation) as GameObject;
								tempObj.transform.parent = creations;
								
								int newCustomerIndex = -1;
								do
								   newCustomerIndex = Random.Range(0, customers.Length);
								while (newCustomerIndex == customerIndex);
								tempObj = Instantiate(customers[newCustomerIndex], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//START GAME
								randomInstanceCount = Random.Range(5, 8);
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 0, randomInstanceCount);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(6);

								//distracters on
								//generalManagerS.DistracterM.SetActiveDistracters(130048);
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//create customer
								CleanObjects();
								int customerIndex = Random.Range(0, customers.Length);
								tempObj = Instantiate(customers[customerIndex], customerPositions[0].position, customerPositions[0].rotation) as GameObject;
								tempObj.transform.parent = creations;
								
								int newCustomerIndex = -1;
								do
								   newCustomerIndex = Random.Range(0, customers.Length);
								while (newCustomerIndex == customerIndex);
								tempObj = Instantiate(customers[newCustomerIndex], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//START GAME
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(5, 8);
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 0, randomInstanceCount);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
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
#region CREATE_Counting
			case MoneyManagementSubTask.Counting:
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

								//create customer
								CleanObjects();
								tempObj = Instantiate(customers[Random.Range(0, customers.Length)], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = 1;
								timerS.setLevelStartTime();
								break;
							case 1:
								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 1, 1);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//create customer
								CleanObjects();
								int customerIndex = Random.Range(0, customers.Length);
								tempObj = Instantiate(customers[customerIndex], customerPositions[0].position, customerPositions[0].rotation) as GameObject;
								tempObj.transform.parent = creations;
								
								int newCustomerIndex = -1;
								do
								   newCustomerIndex = Random.Range(0, customers.Length);
								while (newCustomerIndex == customerIndex);
								tempObj = Instantiate(customers[newCustomerIndex], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//START GAME
								randomInstanceCount = Random.Range(4, 7);
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 1, randomInstanceCount);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(9);

								//distracters on
								//generalManagerS.DistracterM.SetActiveDistracters(130048);
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//create customer
								CleanObjects();
								int customerIndex = Random.Range(0, customers.Length);
								tempObj = Instantiate(customers[customerIndex], customerPositions[0].position, customerPositions[0].rotation) as GameObject;
								tempObj.transform.parent = creations;
								
								int newCustomerIndex = -1;
								do
								   newCustomerIndex = Random.Range(0, customers.Length);
								while (newCustomerIndex == customerIndex);
								tempObj = Instantiate(customers[newCustomerIndex], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//START GAME
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 7);
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 1, randomInstanceCount);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
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
#region CREATE_Change
			case MoneyManagementSubTask.Change:
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

								//create customer
								CleanObjects();
								tempObj = Instantiate(customers[Random.Range(0, customers.Length)], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = 1;
								timerS.setLevelStartTime();
								break;
							case 1:
								//START GAME
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 2, 1);
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters off
								generalManagerS.DistracterM.ActiveteDistracters(false);

								//create customer
								CleanObjects();
								int customerIndex = Random.Range(0, customers.Length);
								tempObj = Instantiate(customers[customerIndex], customerPositions[0].position, customerPositions[0].rotation) as GameObject;
								tempObj.transform.parent = creations;
								
								int newCustomerIndex = -1;
								do
								   newCustomerIndex = Random.Range(0, customers.Length);
								while (newCustomerIndex == customerIndex);
								tempObj = Instantiate(customers[newCustomerIndex], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//START GAME
								randomInstanceCount = Random.Range(4, 7);
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 2, randomInstanceCount);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
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
								timerS.initializeTimes(0.0f, 60.0f, 300.0f);

								//visuals
								visualM.ActivateTutorial(false);
								visualM.ActivatePictographs(false);
								visualM.ActivateVisuals(false);
								visualM.ActivateInfo(false);
								visualM.PlayVoice(12);

								//distracters on
								//generalManagerS.DistracterM.SetActiveDistracters(130048);
								generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
								generalManagerS.DistracterM.ActiveteDistracters(true);

								//create customer
								CleanObjects();
								int customerIndex = Random.Range(0, customers.Length);
								tempObj = Instantiate(customers[customerIndex], customerPositions[0].position, customerPositions[0].rotation) as GameObject;
								tempObj.transform.parent = creations;
								
								int newCustomerIndex = -1;
								do
								   newCustomerIndex = Random.Range(0, customers.Length);
								while (newCustomerIndex == customerIndex);
								tempObj = Instantiate(customers[newCustomerIndex], customerPositions[1].position, customerPositions[1].rotation) as GameObject;
								tempObj.transform.parent = creations;

								//START GAME
								if (randomInstanceCount == 0) randomInstanceCount = Random.Range(4, 7);
								networkView.RPC("StartSubTask", RPCMode.OthersBuffered, 2, randomInstanceCount);

								//start save session
								saverS.StartSaveSession();
								saverS.instanceCount = randomInstanceCount;
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

	IEnumerator CreateStateWithFade(MoneyManagementSubTask _subTask, Level _level, int _state)
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
	void StartSubTask(int _subtask, int _cycle)
	{
		throw new System.NotImplementedException();
	}
	[RPC]
	void StopTask()
	{
		throw new System.NotImplementedException();
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
		}
		else
		{
			saverS.failCount++;
			saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

			//Debug.Log("Wrong");
			visualM.visualNo = 4;
			visualM.ActivateVisuals(true);
		}
		//PHOTON CODE
		generalManagerS.PhotonProgressUpdate();
		//PHOTON CODE
	}
	[RPC]
	void SubTaskFinished(bool _isWin)
	{	
		if (_isWin)
		{
			saverS.successCount++;
			saverS.successTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

			//Debug.Log("WIN");
			CreateState((MoneyManagementSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		}
		else
		{
			saverS.failCount++;
			saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

			//Debug.Log("LOSE");
			CreateState((MoneyManagementSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 200);
		}
		if (visualM.visualsActive) visualM.ActivateVisuals(false);	
		timerS.setLastActivitiyTime();
	}
	[RPC]
	void SaveLog(string _text)
	{
		saverS.SaveText(_text);
	}
}
