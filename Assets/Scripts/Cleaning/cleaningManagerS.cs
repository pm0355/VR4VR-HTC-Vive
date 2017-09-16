using UnityEngine;
using System.Collections;

public class cleaningManagerS : MonoBehaviour {
	
	public int state {get;set;}
	
	//public distractersS distracterM;
	public hudS hudM;
	public characterS characterM;
	public animationsS animationM;
	//public distracterManagerS distracterM;
	
	public GameObject distributerObj;
	public GameObject vacuumObj;
	public GameObject mopObj;
	public GameObject dryDirtPileObj;
	public GameObject wetDirtPileObj;
	public GameObject randomObj;
	public GameObject trashBinObj;
	public GameObject[] litterObjs;
	
	private GameObject vacuumInstance;
	private GameObject mopInstance;
	
	private distributerS distributer;
	
	public GameObject[] environments;
	public Transform[] positions;
	
	private Transform creations;
	private Transform environment;

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
		
		environment = new GameObject().transform;
		environment.name = "Environment";
		environment.parent = transform;	
		
		distributer = distributerObj.GetComponent<distributerS>();
		
		//starting point
		//CreateSubTask (CleaningSubTask.Vacuuming, Level.Tutorial, 0);
		CreateSubTask ((CleaningSubTask) generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//state checks
		ControlSituation ();

		hudS.debugLines[0] = "Skill: " + generalManagerS.ActiveSkill + ", Sub-Tusk: " + generalManagerS.ActiveSubTask + ", Level: " + generalManagerS.ActiveLevel + ", State: " + state;
		hudS.debugLines[1] = "Time: " + Time.timeSinceLevelLoad.ToString("#");
		hudS.debugLines[2] = "Last Activity Time: " +timerS.lastActivitiyTime.ToString("#.#");

		//Debug
		if (Input.GetKeyDown(KeyCode.W))
			CreateSubTask((CleaningSubTask)generalManagerS.ActiveSubTask, generalManagerS.ActiveLevel, 100);
		//Debug
	}
	
	void ControlSituation ()
	{
		switch(generalManagerS.ActiveSubTask)
		{
#region Vacuuming_CONTROL			
		case (int)CleaningSubTask.Vacuuming:		//Vacuuming
			switch (generalManagerS.ActiveLevel) 
			{
			case Level.Tutorial: 			//Vacuuming - Tutorial
				switch(state)
				{
				case 0:							//Vacuuming - Tutorial - animation
					//if animation ends state = 1
					if(!animationM.IsRunning())
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Tutorial, 1);
					break;
				case 1:							//Vacuuming - Tutorial - user trial
					//if 30 sec passed => indicator
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 0;
						hudM.pictographsOn = true;
						animationM.TalkCurrentAnimation(2);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Tutorial, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Tutorial, 100);					
					break;				
				case 100:	//WIN
					if(!animationM.IsRunning() && !hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Vacuuming, Level.Train_without_Distracters, 0));
					break;
				case 200:	//FAIL
					if(!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Mopping, Level.Tutorial, 0));					
					break;					
				}
				break;
			case Level.Train_without_Distracters:			//Vacuuming - Train_without_Distracters
				switch(state)
				{
				case 0:							//Vacuuming - Train_without_Distracters - user trial
					if(hudM.info != "Remaining Dirt: " + distributer.instanceCount)
						hudM.info = "Remaining Dirt: " + distributer.instanceCount;
					//if 30 sec passed => indicator
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 0;
						hudM.pictographsOn = true;
						animationM.PlayVoice(1);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Train_without_Distracters, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Train_without_Distracters, 100);					
					break;				
				case 100:	//WIN
					if (!animationM.IsVoicePlaying() && !hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Vacuuming, Level.Train_with_Distracters, 0));
					break;
				case 200:	//FAIL
					if(!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Mopping, Level.Tutorial, 0));					
					break;						
				}				
				break;
			case Level.Train_with_Distracters:			//Vacuuming - Train_with_Distracters
				switch(state)
				{
				case 0:							//Vacuuming - Train_without_Distracters - user trial
					if(hudM.info != "Remaining Dirt: " + distributer.instanceCount)
						hudM.info = "Remaining Dirt: " + distributer.instanceCount;
					//if 30 sec passed => indicator
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 0;
						hudM.pictographsOn = true;
						animationM.PlayVoice(1);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Train_with_Distracters, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Vacuuming, Level.Train_with_Distracters, 100);					
					break;				
				case 100:	//WIN
					if(!hudM.visualOn)
					{
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Mopping, Level.Tutorial, 0));
					}
					break;	
				case 200:	//FAIL
					if(!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Mopping, Level.Tutorial, 0));					
					break;						
				}				
				break;
			}
			break;				
#endregion
#region Mopping_CONTROL			
		case (int)CleaningSubTask.Mopping:			//Mopping
			switch (generalManagerS.ActiveLevel) 
			{
			case Level.Tutorial: 			//Mopping - Tutorial
				switch(state)
				{
				case 0:							//Mopping - Tutorial - animation
					//if animation ends state = 1
					if(!animationM.IsRunning())
						CreateSubTask(CleaningSubTask.Mopping, Level.Tutorial, 1);
					break;
				case 1:							//Mopping - Tutorial - user trial
					//if 30 sec passed => indicator
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 1;
						hudM.pictographsOn = true;
						animationM.TalkCurrentAnimation(2);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Mopping, Level.Tutorial, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Mopping, Level.Tutorial, 100);					
					break;				
				case 100:	//WIN
					if(!animationM.IsRunning() && !hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Mopping, Level.Train_without_Distracters, 0));
					break;
				case 200:	//FAIL
					if(!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Litter_Collection, Level.Tutorial, 0));					
					break;					
				}
				break;
			case Level.Train_without_Distracters:			//Vacuuming - Train_without_Distracters
				switch(state)
				{
				case 0:							//Vacuuming - Train_without_Distracters - user trial
					if(hudM.info != "Remaining Dirt: " + distributer.instanceCount)
						hudM.info = "Remaining Dirt: " + distributer.instanceCount;
					//if 30 sec passed => indicator
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 1;
						hudM.pictographsOn = true;
						animationM.PlayVoice(4);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Mopping, Level.Train_without_Distracters, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Mopping, Level.Train_without_Distracters, 100);					
					break;				
				case 100:	//WIN
					if (!animationM.IsVoicePlaying() && !hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Mopping, Level.Train_with_Distracters, 0));
					break;
				case 200:	//FAIL
					if(!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Litter_Collection, Level.Tutorial, 0));					
					break;						
				}				
				break;	
			case Level.Train_with_Distracters:			//Vacuuming - Train_withDistracters
				switch(state)
				{
				case 0:							//Vacuuming - Train_without_Distracters - user trial
					if(hudM.info != "Remaining Dirt: " + distributer.instanceCount)
						hudM.info = "Remaining Dirt: " + distributer.instanceCount;
					//if 30 sec passed => indicator
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 1;
						hudM.pictographsOn = true;
						animationM.PlayVoice(4);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Mopping, Level.Train_with_Distracters, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Mopping, Level.Train_with_Distracters, 100);					
					break;					
				case 100:	//WIN
					if (!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Litter_Collection, Level.Tutorial, 0));
					break;	
				case 200:	//FAIL
					if(!hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Litter_Collection, Level.Tutorial, 0));					
					break;		
				}				
				break;
			}
			break;				
#endregion
#region LITTERCOLLECTING_CONTROL			
		case (int)CleaningSubTask.Litter_Collection:	//Litter_Collection
			switch (generalManagerS.ActiveLevel) 
			{
			case Level.Tutorial: 			//Litter_Collection - Tutorial
				switch(state)
				{
				case 0:							//Litter_Collection - Tutorial - animation
					//if animation ends state = 1
					if(!animationM.IsRunning())
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Tutorial, 1);
					break;
				case 1:							//Litter_Collection - Tutorial - user trial
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 2;
						hudM.pictographsOn = true;
						animationM.TalkCurrentAnimation(2);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Tutorial, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Tutorial, 100);					
					break;				
				case 100:	//WIN
					if(!animationM.IsRunning() && !hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Litter_Collection, Level.Train_without_Distracters, 0));
					break;
				case 200:	//FAIL
					if (!hudM.visualOn)
						generalManagerS.EndLevel();
					break;					
				}
				break;
			case Level.Train_without_Distracters:			//Litter_Collection - Train_without_Distracters
				switch(state)
				{
				case 0:							//Litter_Collection - Train_without_Distracters - user trial
					if(hudM.info != "Remaining Litter: " + distributer.instanceCount)
						hudM.info = "Remaining Litter: " + distributer.instanceCount;
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 2;
						hudM.pictographsOn = true;
						animationM.PlayVoice(7);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Train_without_Distracters, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Train_without_Distracters, 100);					
					break;				
				case 100:	//WIN
					if (!animationM.IsVoicePlaying() && !hudM.visualOn)
						StartCoroutine(CreateSubTaskWithFade(CleaningSubTask.Litter_Collection, Level.Train_with_Distracters, 0));
					break;
				case 200:	//FAIL
					if (!hudM.visualOn)
						generalManagerS.EndLevel();
					break;						
				}				
				break;	
			case Level.Train_with_Distracters:			//Litter_Collection - Train_with_Distracters
				switch(state)
				{
				case 0:							//Litter_Collection - Train_without_Distracters - user trial
					if(hudM.info != "Remaining Litter: " + distributer.instanceCount)
						hudM.info = "Remaining Litter: " + distributer.instanceCount;
					//if 30 sec passed => indicator
					//if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.indicationTime) distributer.IndicateOne();					
					//if 1 min passed => sound, animation, pictograph
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.pictographTime && Time.timeSinceLevelLoad > timerS.lastPictographTime + timerS.pictographTime)
					{
						hudM.pictographSetNo = 2;
						hudM.pictographsOn = true;
						animationM.PlayVoice(7);
					}
					//if 5 min passed => fail
					if(Time.timeSinceLevelLoad > timerS.lastActivitiyTime + timerS.failTime) 
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Train_with_Distracters, 200);
					//if number of dirt = 0 win
					if(distributer.instanceCount == 0 ) 
						CreateSubTask(CleaningSubTask.Litter_Collection, Level.Train_with_Distracters, 100);					
					break;					
				case 100:	//WIN
					if (!hudM.visualOn)
						generalManagerS.EndLevel();
					break;
				case 200:	//FAIL
					if (!hudM.visualOn)
						generalManagerS.EndLevel();
					break;					
				}				
				break;
			}
			break;				
#endregion		
		}	
	}
	
	void CreateSubTask (CleaningSubTask _subTask, Level _level, int _state)
	{
		generalManagerS.ActiveSubTask = (int)_subTask;
		generalManagerS.ActiveLevel = _level;
		state = _state;

		GameObject tempObj = null;
		timerS.setStateStartTime();

		Debug.Log(_subTask + ", " + _level + ", " + _state);
		
		switch(_subTask)
		{
#region Vacuuming_CREATE
		case CleaningSubTask.Vacuuming:		//Vacuuming
			switch (_level) 
			{
			case Level.Tutorial: 			//Vacuuming - Tutorial
				switch(_state)
				{
				case 0:							//Vacuuming - Tutorial - animation
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);
					
					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[0]) as GameObject;
					tempObj.transform.parent = environment;	
					
					//lock the user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = false;
					characterM.allowRealWalk = false;

					//create vacuum and attach to user
					CleanObjects();
					if (vacuumInstance == null)
					{
						vacuumInstance = Instantiate(vacuumObj) as GameObject;
						characterM.ChangeHandModel(true, 1);
						characterM.ChangeHandModel(false, 1);
					}
					
					//create formen, required prefabs and start animation 
					animationM.CreateAnimation(0);
					animationM.PlayCurrentAnimation();
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = false;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = 1;
					timerS.setLevelStartTime();
					break;
				case 1:	
					//create dirt
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();	
					distributer.CreateSingleObject(dryDirtPileObj, positions[0], false);
					tempObj.transform.parent = creations;		
					break;	
				case 100:	//WIN
					//forman voice and visual
					animationM.TalkCurrentAnimation(3);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();
					break;						
				}
				break;
			case Level.Train_without_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);

					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[0]) as GameObject;
					tempObj.transform.parent = environment;	

					//unlock user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = true;
					characterM.allowRealWalk = false;	
					
					//create vacuum and attach to user
					CleanObjects();
					if(vacuumInstance==null) 
					{
						vacuumInstance = Instantiate(vacuumObj) as GameObject;
						characterM.ChangeHandModel(true, 1);
						characterM.ChangeHandModel(false, 1);
					}
					
					//create dirts
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();
					randomInstanceCount = Random.Range(3, 6);
					//randomInstanceCount = Random.Range(5, 10);
					distributer.count = randomInstanceCount;
					distributer.CreateObjects(dryDirtPileObj);
					tempObj.transform.parent = creations;
					
					//delete but talk formen
					animationM.DestroyAnimations();
					animationM.PlayVoice(0);

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = distributer.instanceCount;
					timerS.setLevelStartTime();
					break;	
				case 100:	//WIN
					//forman voice and visual
					animationM.PlayVoice(2);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					if(vacuumInstance != null)
					{
						Destroy(vacuumInstance); 
						characterM.ChangeHandModel(true, 0);
						characterM.ChangeHandModel(false, 0);
					}	
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();
					break;							
				}				
				break;	
			case Level.Train_with_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);

					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[0]) as GameObject;
					tempObj.transform.parent = environment;	

					//unlock user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = true;
					characterM.allowRealWalk = false;	
					
					//create vacuum and attach to user
					CleanObjects();
					if(vacuumInstance==null) 
					{
						vacuumInstance = Instantiate(vacuumObj) as GameObject;
						characterM.ChangeHandModel(true, 1);
						characterM.ChangeHandModel(false, 1);
					}
					
					//create dirts
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();
					if(randomInstanceCount==0)	randomInstanceCount = Random.Range(3, 6);
					//if(randomInstanceCount==0)	randomInstanceCount = Random.Range(5, 10);
					distributer.count = randomInstanceCount;
					distributer.CreateObjects(dryDirtPileObj);
					tempObj.transform.parent = creations;
					
					//talk formen
					animationM.PlayVoice(0);

					//distracters on
					//generalManagerS.DistracterM.SetActiveDistracters(31);
					generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
					generalManagerS.DistracterM.ActiveteDistracters(true);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = distributer.instanceCount;
					timerS.setLevelStartTime();
					break;
				case 100:	//WIN
					//forman voice and visual	
					if(vacuumInstance != null)
					{
						Destroy(vacuumInstance); 
						characterM.ChangeHandModel(true, 0);
						characterM.ChangeHandModel(false, 0);
					}	
					
					//animationM.PlayVoice(2);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					if(vacuumInstance != null)
					{
						Destroy(vacuumInstance); 
						characterM.ChangeHandModel(true, 0);
						characterM.ChangeHandModel(false, 0);
					}	
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

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
#region Mopping_CREATE
		case CleaningSubTask.Mopping:			//Mopping
			switch (_level) 
			{
			case Level.Tutorial: 			//Mopping - Tutorial
				switch(_state)
				{
				case 0:							//Mopping - Tutorial - animation
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);
					
					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[0]) as GameObject;
					tempObj.transform.parent = environment;	
					
					//lock the user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = false;
					characterM.allowRealWalk = false;

					//create mop and attach to user
					CleanObjects();
					if (mopInstance == null)
					{
						mopInstance = Instantiate(mopObj) as GameObject;
						characterM.ChangeHandModel(true, 1);
						characterM.ChangeHandModel(false, 1);
					}
					
					//create formen, required prefabs and start animation 
					animationM.DestroyAnimations();
					animationM.CreateAnimation(1);
					animationM.PlayCurrentAnimation();
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = false;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = 1;
					timerS.setLevelStartTime();
					break;
				case 1:	
					//create dirt
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();	
					distributer.CreateSingleObject(wetDirtPileObj, positions[0], false);
					tempObj.transform.parent = creations;		
					break;	
				case 100:	//WIN
					//forman voice and visual
					animationM.TalkCurrentAnimation(3);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();
					break;						
				}
				break;
			case Level.Train_without_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);

					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[0]) as GameObject;
					tempObj.transform.parent = environment;	

					//unlock user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = true;
					characterM.allowRealWalk = false;	
					
					//create mop and attach to user
					CleanObjects();
					if(mopInstance==null) 
					{
						mopInstance = Instantiate(mopObj) as GameObject;
						characterM.ChangeHandModel(true, 1);
						characterM.ChangeHandModel(false, 1);
					}					
					
					//create dirts
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();
					randomInstanceCount = Random.Range(3, 6);
					//randomInstanceCount = Random.Range(5, 10);
					distributer.count = randomInstanceCount;
					distributer.CreateObjects(wetDirtPileObj);
					tempObj.transform.parent = creations;
					
					//delete but talk formen
					animationM.DestroyAnimations();
					animationM.PlayVoice(3);

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = distributer.instanceCount;
					timerS.setLevelStartTime();
					break;	
				case 100:	//WIN
					//forman voice and visual
					animationM.PlayVoice(5);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					if(mopInstance != null)
					{
						Destroy(mopInstance); 
						characterM.ChangeHandModel(true, 0);
						characterM.ChangeHandModel(false, 0);
					}	
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();
					break;
				}				
				break;		
			case Level.Train_with_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);

					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[0]) as GameObject;
					tempObj.transform.parent = environment;	

					//unlock user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = true;
					characterM.allowRealWalk = false;	
					
					//create mop and attach to user
					CleanObjects();
					if(mopInstance==null) 
					{
						mopInstance = Instantiate(mopObj) as GameObject;
						characterM.ChangeHandModel(true, 1);
						characterM.ChangeHandModel(false, 1);
					}		
					
					//create dirts
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();
					if(randomInstanceCount==0)	randomInstanceCount = Random.Range(3, 6);
					//if(randomInstanceCount==0)	randomInstanceCount = Random.Range(5, 10);
					distributer.count = randomInstanceCount;
					distributer.CreateObjects(wetDirtPileObj);
					tempObj.transform.parent = creations;
					
					//talk formen
					animationM.PlayVoice(3);

					//distracters on
					//generalManagerS.DistracterM.SetActiveDistracters(31);
					generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
					generalManagerS.DistracterM.ActiveteDistracters(true);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = distributer.instanceCount;
					timerS.setLevelStartTime();
					break;	
				case 100:	//WIN
					//forman voice and visual
					if(mopInstance != null)
					{
						Destroy(mopInstance); 
						characterM.ChangeHandModel(true, 0);
						characterM.ChangeHandModel(false, 0);
					}						
					
					//animationM.PlayVoice(5);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					if(mopInstance != null)
					{
						Destroy(mopInstance); 
						characterM.ChangeHandModel(true, 0);
						characterM.ChangeHandModel(false, 0);
					}	
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

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
#region LITTERCOLLECTING_CREATE
		case CleaningSubTask.Litter_Collection:	//Litter_Collection
			switch (_level) 
			{
			case Level.Tutorial: 			//Litter_Collection - Tutorial
				switch(_state)
				{
				case 0:							//Litter_Collection - Tutorial - animation
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);
					
					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[1]) as GameObject;
					tempObj.transform.parent = environment;	
					
					//lock the user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = false;
					characterM.allowRealWalk = false;	

					CleanObjects();
										
					//create formen, required prefabs and start animation 
					animationM.DestroyAnimations();
					animationM.CreateAnimation(2);
					animationM.PlayCurrentAnimation();
					
					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = false;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = 1;
					timerS.setLevelStartTime();
					break;
				case 1:	
					//create litter
					CleanObjects();
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();	
					distributer.CreateSingleObject(trashBinObj, positions[1], false);				
										
					distributer.CreateSingleObject(litterObjs[Random.Range(0,litterObjs.Length)], positions[0], false);
					tempObj.transform.parent = creations;		
					break;	
				case 100:	//WIN
					//forman voice and visual
					animationM.TalkCurrentAnimation(3);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();
					break;						
				}
				break;
			case Level.Train_without_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);

					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[1]) as GameObject;
					tempObj.transform.parent = environment;	

					//unlock user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = true;
					characterM.allowRealWalk = false;						
										
					//create trash bins and litters
					CleanObjects();
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();
					distributer.CreateSingleObject(trashBinObj, positions[2], false);
					distributer.CreateSingleObject(trashBinObj, positions[3], false);
					distributer.CreateSingleObject(trashBinObj, positions[4], false);
					distributer.CreateSingleObject(trashBinObj, positions[5], false);

					randomInstanceCount = Random.Range(3, 6);
					//randomInstanceCount = Random.Range(5, 10);
					distributer.count = randomInstanceCount;
					distributer.CreateRandomObjects(litterObjs);
					tempObj.transform.parent = creations;
					
					//delete but talk formen
					animationM.DestroyAnimations();
					animationM.PlayVoice(6);

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = distributer.instanceCount;
					timerS.setLevelStartTime();
					break;	
				case 100:	//WIN
					//forman voice and visual
					animationM.PlayVoice(8);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

					//save session
					saverS.result = 2;
					saverS.SaveDataBase();
					break;							
				}				
				break;	
			case Level.Train_with_Distracters:
				switch(_state)
				{
				case 0:
					//timing set
					timerS.initializeTimes(30.0f, 60.0f, 300.0f);

					//create warehouse
					CleanEnvironment();
					tempObj = Instantiate(environments[1]) as GameObject;
					tempObj.transform.parent = environment;	

					//unlock user
					characterM.transform.GetChild(0).position = Vector3.zero;
					characterM.allowWalkInPlace = true;
					characterM.allowRealWalk = false;	
					
					//create litters
					CleanObjects();
					tempObj = Instantiate(distributerObj) as GameObject;
					distributer = tempObj.GetComponent<distributerS>();
					distributer.CreateSingleObject(trashBinObj, positions[2], false);
					distributer.CreateSingleObject(trashBinObj, positions[3], false);
					distributer.CreateSingleObject(trashBinObj, positions[4], false);
					distributer.CreateSingleObject(trashBinObj, positions[5], false);

					if(randomInstanceCount==0)	randomInstanceCount = Random.Range(3, 6);
					//if(randomInstanceCount==0)	randomInstanceCount = Random.Range(5, 10);
					distributer.count = randomInstanceCount;
					distributer.CreateRandomObjects(litterObjs);
					tempObj.transform.parent = creations;
					
					//talk formen
					animationM.PlayVoice(6);

					//distracters on
					//generalManagerS.DistracterM.SetActiveDistracters(31);
					generalManagerS.DistracterM.SetDistracterTiming(30.0f, 5.0f);
					generalManagerS.DistracterM.ActiveteDistracters(true);
					
					//info
					hudM.pictographsOn = false;
					hudM.visualOn = false;
					hudM.infoOn = true;

					//start save session
					saverS.StartSaveSession();
					saverS.instanceCount = distributer.instanceCount;
					timerS.setLevelStartTime();
					break;		
				case 100:	//WIN
					//forman voice and visual
					//animationM.PlayVoice(8);
					hudM.pictographsOn = false;
					hudM.visualNo = 0;
					hudM.visualOn = true;

					//distracters off
					generalManagerS.DistracterM.ActiveteDistracters(false);

					//save session
					saverS.result = 1;
					saverS.SaveDataBase();
					break;		
				case 200:	//FAIL
					//visual
					hudM.pictographsOn = false;
					hudM.visualNo = 1;
					hudM.visualOn = true;

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
	IEnumerator CreateSubTaskWithFade (CleaningSubTask _subTask, Level _level, int _state)
	{	
		hudM.FadeOut();
		for(;;)
		{
			if(hudM.faded) break;
			yield return null;
		}
		
		CreateSubTask (_subTask, _level, _state);
		hudM.FadeIn();		
	}	
	void CleanObjects()
	{
		foreach (Transform child in creations) Destroy(child.gameObject);
	}
	void CleanEnvironment()
	{
		foreach (Transform child in environment) Destroy(child.gameObject);
	}	
}
