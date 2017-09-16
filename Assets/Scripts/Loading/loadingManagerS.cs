using UnityEngine;
using System.Collections;

public class loadingManagerS : MonoBehaviour {

	public loadingBoxManagerS boxM;
	public loadingVisualManagerS visualM;

	private int state = 0;

	private float tutorialTime = 11.0f;
	private float subTaskTime = 30.0f;
	private float startTime;

	private int randomInstanceCount = 0;


	void Awake()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		randomInstanceCount = 0;
	}

	// Use this for initialization
	void Start ()
	{
		//starting point
		//CreateState (LoadingSubTask.Identical_Boxes, Level.Tutorial, 0);
		CreateState ((LoadingSubTask) generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//state checks
		CheckState ();

		loadingVisualManagerS.debugLines[0] = "Skill: " + generalManagerS.ActiveSkill + ", Sub-Tusk: " + generalManagerS.ActiveSubTask + ", Level: " + generalManagerS.ActiveLevel + ", State: " + state;
		loadingVisualManagerS.debugLines[1] = "Time: " + Time.timeSinceLevelLoad.ToString("#");
		loadingVisualManagerS.debugLines[2] = "Last Activity Time: " +timerS.lastActivitiyTime.ToString("#.#");

		//Debug
		if (Input.GetKeyDown(KeyCode.W))
			CreateState((LoadingSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		//Debug
	}

	void CheckState()
	{
		switch (generalManagerS.ActiveSubTask) 
		{
		case (int)LoadingSubTask.Identical_Boxes:
#region CHECK_Identical_Boxes
			switch (generalManagerS.ActiveLevel) 
			{
			case Level.Tutorial:
				switch(state)
				{
				case 0:
					if(visualM.tutorialFinished)
					{
						CreateState(LoadingSubTask.Identical_Boxes, Level.Tutorial, 1);
					}
					break;	
				case 1:
					//info
					visualM.info = "Upcoming Box: " + (boxM.remainingBoxes).ToString();

					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	

					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 0;
						visualM.pictographsOn = true;
						visualM.PlayVoice(10);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Identical_Boxes, Level.Tutorial, 200);
					//if number of dirt = 0 win
					if(!boxM.isPlaying) 
						CreateState(LoadingSubTask.Identical_Boxes, Level.Tutorial, 100);					
					break;	
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Identical_Boxes, Level.Train_without_Distracters, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 0));
					break;							
				}	
				break;
			case Level.Train_without_Distracters:
				switch(state)
				{
				case 0:
					//info
					visualM.info = "Upcoming Box: " + (boxM.remainingBoxes).ToString();

					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 0;
						visualM.pictographsOn = true;
						visualM.PlayVoice(10);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Identical_Boxes, Level.Train_without_Distracters, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Identical_Boxes, Level.Train_without_Distracters, 100);	
					break;		
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Identical_Boxes, Level.Train_with_Distracters, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 0));
					break;							
				}	
				break;
			case Level.Train_with_Distracters:
				switch(state)
				{
				case 0:
					//info
					visualM.info = "Upcoming Box: " + (boxM.remainingBoxes).ToString();

					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 0;
						visualM.pictographsOn = true;
						visualM.PlayVoice(10);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Identical_Boxes, Level.Train_with_Distracters, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Identical_Boxes, Level.Train_with_Distracters, 100);	
					break;		
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 0));
					break;							
				}	
				break;
			}
			break;
#endregion
#region CHECK_Labeled_Boxes
		case (int)LoadingSubTask.Labeled_Boxes:
			switch (generalManagerS.ActiveLevel) 
			{
			case Level.Tutorial:
				switch(state)
				{
				case 0:
					if(visualM.tutorialFinished)
					{
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 1);
					}
					break;	
				case 1:
					//info
					visualM.info = "Upcoming Box: " + (boxM.remainingBoxes).ToString();
					
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 1;
						visualM.pictographsOn = true;
						visualM.PlayVoice(13);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Tutorial, 100);					
					break;	
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Labeled_Boxes, Level.Train_without_Distracters, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Timer, Level.Tutorial, 0));
					break;							
				}	
				break;
			case Level.Train_without_Distracters:
				switch(state)
				{
				case 0:
					//info
					visualM.info = "Upcoming Box: " + (boxM.remainingBoxes).ToString();
					
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 1;
						visualM.pictographsOn = true;
						visualM.PlayVoice(13);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Train_without_Distracters, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Train_without_Distracters, 100);	
					break;		
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Labeled_Boxes, Level.Train_with_Distracters, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Timer, Level.Tutorial, 0));
					break;							
				}	
				break;
			case Level.Train_with_Distracters:
				switch(state)
				{
				case 0:
					//info
					visualM.info = "Upcoming Box: " + (boxM.remainingBoxes).ToString();
					
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 1;
						visualM.pictographsOn = true;
						visualM.PlayVoice(13);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Train_with_Distracters, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Labeled_Boxes, Level.Train_with_Distracters, 100);	
					break;		
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Timer, Level.Tutorial, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Timer, Level.Tutorial, 0));
					break;							
				}	
				break;
			}
			break;
#endregion
#region CHECK_Timed_Boxes
		case (int)LoadingSubTask.Timer:
			switch (generalManagerS.ActiveLevel) 
			{
			case Level.Tutorial:
				switch(state)
				{
				case 0:
					if(visualM.tutorialFinished)
					{
						CreateState(LoadingSubTask.Timer, Level.Tutorial, 1);
					}
					break;	
				case 1:
					//info
					visualM.info = "Box: " + (boxM.remainingBoxes).ToString() + " Time: " + ((int)(startTime + tutorialTime - Time.timeSinceLevelLoad)).ToString();
					
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > startTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 2;
						visualM.pictographsOn = true;
						visualM.PlayVoice(16);
						startTime = Time.timeSinceLevelLoad;
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateState(LoadingSubTask.Timer, Level.Tutorial, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Timer, Level.Tutorial, 100);					
					break;	
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Timer, Level.Train_without_Distracters, 0));
					break;	
				case 200:	//FAIL
					if(!visualM.visualOn)
						generalManagerS.EndLevel();
					break;							
				}	
				break;
			case Level.Train_without_Distracters:
				switch(state)
				{
				case 0:
					//info
					visualM.info = "Box: " + (boxM.remainingBoxes).ToString() + " Time: " + ((int)(startTime + subTaskTime - Time.timeSinceLevelLoad)).ToString();
					
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 2;
						visualM.pictographsOn = true;
						visualM.PlayVoice(16);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > startTime + timerS.failTime) 
						CreateState(LoadingSubTask.Timer, Level.Train_without_Distracters, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Timer, Level.Train_without_Distracters, 100);	
					break;		
				case 100:	//WIN
					if(visualM.IsAudioStoped() && !visualM.visualOn)
						StartCoroutine(CreateStateWithFade(LoadingSubTask.Timer, Level.Train_with_Distracters, 0));
					break;	
				case 200:	//FAIL
					if (!visualM.visualOn)
						generalManagerS.EndLevel();
					break;							
				}	
				break;
			case Level.Train_with_Distracters:
				switch(state)
				{
				case 0:
					//info
					visualM.info = "Box: " + (boxM.remainingBoxes).ToString() + " Time: " + ((int)(startTime + subTaskTime - Time.timeSinceLevelLoad)).ToString();
					
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();	
					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						visualM.pictographSetNo = 2;
						visualM.pictographsOn = true;
						visualM.PlayVoice(16);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > startTime + timerS.failTime) 
						CreateState(LoadingSubTask.Timer, Level.Train_with_Distracters, 200);
					//if number of dirt = 0 win
					if (!boxM.isPlaying) 
						CreateState(LoadingSubTask.Timer, Level.Train_with_Distracters, 100);	
					break;		
				case 100:	//WIN
					if (!visualM.visualOn)
						generalManagerS.EndLevel();
					break;	
				case 200:	//FAIL
					if (!visualM.visualOn)
						generalManagerS.EndLevel();
					break;							
				}	
				break;
			}
			break;
#endregion
		}
	}

	void CreateState(LoadingSubTask _subTask, Level _level, int _state )
	{
		generalManagerS.ActiveSubTask = (int)_subTask;
		generalManagerS.ActiveLevel = _level;
		state = _state;

		//GameObject tempObj = null;
		timerS.setStateStartTime();
		
		Debug.Log(_subTask + ", " + _level + ", " + _state);

		switch (_subTask) 
		{
#region CREATE_Identical_Boxes
		case LoadingSubTask.Identical_Boxes:
			switch (_level) 
			{
			case Level.Tutorial:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, 300.0f);

					//clear items
					boxM.ClearAll();

					//tutorial
					visualM.tutorialSetNo = 0;
					visualM.tutorialOn = true;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = 1;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;	
				case 1:
					//create one box
					boxM.CreateTutorial(false);

					//info on
					visualM.infoOn = true;

					break;
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.PlayVoice(6);
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;							
				}	
				break;
			case Level.Train_without_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, 300.0f);
					
					//visuals
					visualM.tutorialOn = false;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					visualM.PlayVoice(9);
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//create boxes
					boxM.ClearAll();
					randomInstanceCount = Random.Range(5, 8);
					boxM.DivideArea(randomInstanceCount, 0.0f, false);

					//info on
					visualM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = randomInstanceCount;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;		
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.PlayVoice(11);
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;							
				}	
				break;
			case Level.Train_with_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, 300.0f);
					
					//visuals
					visualM.tutorialOn = false;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					visualM.PlayVoice(9);
					
					//distracters on
					//generalManagerS.DistracterM.SetActiveDistracters(992);
					generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
					generalManagerS.DistracterM.ActiveteDistracters(true);

					//create boxes
					boxM.ClearAll();
					if (randomInstanceCount == 0) randomInstanceCount = Random.Range(5, 8);
					boxM.DivideArea(randomInstanceCount, 0.0f, false);

					//info on
					visualM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = randomInstanceCount;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;		
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;								
				}	
				break;
			}
			break;
#endregion
#region CREATE_Labeled_Boxes
		case LoadingSubTask.Labeled_Boxes:
			switch (_level) 
			{
			case Level.Tutorial:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, 300.0f);
					
					//clear items
					boxM.ClearAll();
					
					//tutorial
					visualM.tutorialSetNo = 1;
					visualM.tutorialOn = true;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = 1;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;	
				case 1:			
					//create one box
					boxM.ClearAll();
					boxM.CreateTutorial(true);
					
					//info on
					visualM.infoOn = true;

					break;
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.PlayVoice(7);
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;							
				}	
				break;
			case Level.Train_without_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, 300.0f);
					
					//visuals
					visualM.tutorialOn = false;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					visualM.PlayVoice(12);
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//create boxes
					boxM.ClearAll();
					randomInstanceCount = Random.Range(5, 8);
					boxM.DivideArea(randomInstanceCount, 0.15f, true);
					
					//info on
					visualM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = randomInstanceCount;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;		
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.PlayVoice(14);
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;							
				}	
				break;
			case Level.Train_with_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, 300.0f);
					
					//visuals
					visualM.tutorialOn = false;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					visualM.PlayVoice(12);
										
					//distracters on
					//generalManagerS.DistracterM.SetActiveDistracters(992);
					generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
					generalManagerS.DistracterM.ActiveteDistracters(true);

					//create boxes
					boxM.ClearAll();
					if (randomInstanceCount == 0) randomInstanceCount = Random.Range(5, 8);				
					boxM.DivideArea(randomInstanceCount, 0.15f, true);
					
					//info on
					visualM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = randomInstanceCount;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;		
				case 100:	//WIN
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;								
				}	
				break;			
			}
			break;
#endregion
#region CREATE_Timed_Boxes
		case LoadingSubTask.Timer:
			switch (_level) 
			{
			case Level.Tutorial:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(0.0f, tutorialTime, tutorialTime*5.95f);
					
					//clear items
					boxM.ClearAll();
					
					//tutorial
					visualM.tutorialSetNo = 2;
					visualM.tutorialOn = true;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = 1;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;	
				case 1:					
					//create one box
					boxM.ClearAll();
					boxM.CreateTutorial(true);
					startTime = Time.timeSinceLevelLoad;
					
					//info on();
					visualM.infoOn = true;

					break;
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.PlayVoice(8);
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;							
				}	
				break;
			case Level.Train_without_Distracters:
				switch(_state)
				{
				case 0:
					//visuals
					visualM.tutorialOn = false;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					visualM.PlayVoice(15);
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//create boxes
					boxM.ClearAll();
					randomInstanceCount = Random.Range(5, 8);
					boxM.DivideArea(randomInstanceCount, 0.15f, true);
					startTime = Time.timeSinceLevelLoad;
					subTaskTime = randomInstanceCount * (20.0f + Random.Range(0.0f, 5.0f));

					//timing set
					timerS.initializeTimes(0.0f, 60.0f, subTaskTime);
				
					//info on
					visualM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = randomInstanceCount;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;		
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.PlayVoice(17);
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;							
				}	
				break;
			case Level.Train_with_Distracters:
				switch(_state)
				{
				case 0:
					//visuals
					visualM.tutorialOn = false;
					visualM.pictographsOn = false;
					visualM.visualOn = false;
					visualM.infoOn = false;
					visualM.PlayVoice(15);
					
					//distracters on
					//generalManagerS.DistracterM.SetActiveDistracters(992);
					generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
					generalManagerS.DistracterM.ActiveteDistracters(true);

					//create boxes
					boxM.ClearAll();
					if (randomInstanceCount == 0) randomInstanceCount = Random.Range(5, 8);				
					boxM.DivideArea(randomInstanceCount, 0.15f, true);
					startTime = Time.timeSinceLevelLoad;
					subTaskTime = randomInstanceCount * (20.0f + Random.Range(0.0f, 5.0f));
					
					//timing set
					timerS.initializeTimes(0.0f, 60.0f, subTaskTime);

					//info on
					visualM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = randomInstanceCount;
					timerS.setLevelStartTime();

					//resume Omni
					boxM.ResumeHepticDevice();
					break;		
				case 100:	//WIN
					//forman voice and visual
					visualM.pictographsOn = false;
					visualM.visualNo = 0;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
					break;		
				case 200:	//FAIL
					//visual
					visualM.pictographsOn = false;
					visualM.visualNo = 1;
					if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
					visualM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();

					//pause Omni
					boxM.PauseHepticDevice();
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

	IEnumerator CreateStateWithFade (LoadingSubTask _subTask, Level _level, int _state )
	{	
		visualM.FadeOut();
		for(;;)
		{
			if(visualM.faded) break;
			yield return null;
		}
		
		CreateState (_subTask, _level, _state);
		visualM.FadeIn();		
	}
}	