using UnityEngine;
using System.Collections;

public enum Skill
{
	None = 0,
	Cleaning = 1,
	Loading,
	Money_Management,
	Shelving, 
	Cautiousness,
	Socialness
}

public enum CleaningSubTask
{
	Vacuuming,
	Mopping,
	Litter_Collection
}
public enum LoadingSubTask
{
	Identical_Boxes,
	Labeled_Boxes,
	Timer
}
public enum MoneyManagementSubTask
{
	Recognizing,
	Counting,
	Change
}
public enum ShelvingSubTask
{
	Alignment,
	Supply_Textured,
	Supply_Labeled
}
public enum CautiousnessSubTask
{
	Stationary,
	Moving_People,
	Moving_Cars
}
public enum SocialnessSubTask
{
	Basic_Personal,
	Longer_Vocational,
	Moody_Hard
}

public enum Level
{
	Tutorial = 0,
	Train_without_Distracters,
	Train_with_Distracters
}

public enum Distracter
{
	Loud_Vacuum = 0,
	Object_Falls,
	Object_Breaks,
	Announcement,
	Lightning,
	Dog_Barking,
	Plane_Passing,
	Night_Time,
	Truck_Alarm,
	Traffic_Noise,
	Grocery_Alarms,
	Running_Child,
	Phone_Ringing,
	Grocery_Music,
	Angry_Customer,
	Grocery_Announcement,
	Baby_Crying,
	Forklift,
	Coworkers,
	Light_Change,
	Fire_Truck,
	Helicopter,
	Christmas_Bells,
	Fireworks,
	Rain,
	Fan_Operating,
	Butterfly,
	Coughing,
	Laughing,
	Whistling
}

public class generalManagerS : MonoBehaviour {

	private static bool isCreated = false;

	public static Skill ActiveSkill;
	public static int ActiveSubTask;
	public static Level ActiveLevel;

	public static int jobCoachID;
	public static int userID;

	public static networkManagerS NetworkM;
	public static distracterManagerS DistracterM;
	public static recorderManagerS RecorderM;

	public static float generalVariable = 0.0f;

	void Awake () 
	{
		generalVariable = 0.0f;
		Screen.showCursor = true;

		if (!isCreated)
		{
			DontDestroyOnLoad(this.gameObject);
			isCreated = true;

			NetworkM = transform.GetComponent<networkManagerS>();
			DistracterM = transform.GetComponent<distracterManagerS>();
			RecorderM = transform.GetComponentInChildren<recorderManagerS>();

			ActiveSkill = 0;
			ActiveSubTask = 0;
			ActiveLevel = 0;
		}
		else
			Destroy(this.gameObject);
	}
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Backspace) && Application.loadedLevel != 0) EndLevel();

		if(Input.GetKey(KeyCode.UpArrow) && generalVariable<1.0f )
		{
			generalVariable += Time.deltaTime * 0.4f;
			if (generalVariable > 1.0f) generalVariable = 1.0f;
		}
		if (Input.GetKey(KeyCode.DownArrow) && generalVariable > 0.0f)
		{
			generalVariable -= Time.deltaTime * 0.4f;
			if (generalVariable < 0.0f) generalVariable = 0.0f;
		}
	}

	void OnGUI()
	{
		if (Application.loadedLevel == 0)
		{
			GUILayout.BeginArea(new Rect(0, 0, 700, 700));
			for (int i = 1; i < 7; i++)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label(((Skill)i).ToString(), GUILayout.Width(130));

				GUILayout.BeginVertical();
				for (int j = 0; j < 3; j++)
				{
					GUILayout.BeginHorizontal();
					switch (i)
					{
						case 1: GUILayout.Label(((CleaningSubTask)j).ToString(), GUILayout.Width(110)); break;
						case 2: GUILayout.Label(((LoadingSubTask)j).ToString(), GUILayout.Width(110)); break;
						case 3: GUILayout.Label(((MoneyManagementSubTask)j).ToString(), GUILayout.Width(110)); break;
						case 4: GUILayout.Label(((ShelvingSubTask)j).ToString(), GUILayout.Width(110)); break;
						case 5: GUILayout.Label(((CautiousnessSubTask)j).ToString(), GUILayout.Width(110)); break;
						case 6: GUILayout.Label(((SocialnessSubTask)j).ToString(), GUILayout.Width(110)); break;
					}
					for (int k = 0; k < 3; k++)
						if (GUILayout.Button(((Level)k).ToString()))
						{
							SetLevel((Skill)i, j, (Level)k);
							StartLevel();
						}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}

		GUI.Label(new Rect(720, 0, 250, 20), "General Variable: " + generalVariable.ToString("f2"));
	}

	public static void SetJobCoachAndUser(int _jobCoachID, int _userID)		// 1- When jobCoach and user is selected
	{
		jobCoachID = _jobCoachID;
		userID = _userID;
	}
	public static void SetLevel(Skill _skill, int _subTask, Level _level)	// 2- When skill, subtask and level selected
	{
		if(Application.loadedLevel==0)
		{
			ActiveSkill = _skill;
			ActiveSubTask = _subTask;
			ActiveLevel = _level;

			DistracterM.LoadDefaultDistracters();
		}
	}
	public static void StartLevel()											// 3- When level started
	{
		if (Application.loadedLevel == 0)
		{
			Application.LoadLevel((int)ActiveSkill);
		}
	}
	public static void EndLevel()											// 4- When level ended
	{
		if (Application.loadedLevel > 0)
		{
			Application.LoadLevel(0);
			DistracterM.ActiveteDistracters(false);
			saverS.result = 3;
			saverS.SaveDataBase();

			//PHOTON CODE
			PhotonLocator.PhotonConnect.Outgoing.BeginTask(0, 0, 0, 0);
			PhotonLocator.PhotonConnect.Outgoing.UpdateProgress(0, 0, 0, 0);
			//PHOTON CODE
		}
	}
	public static void PhotonUpdate()
	{
		int ActiveMask = (ActiveLevel == Level.Train_with_Distracters) ? DistracterM.GetActiveDistracters() : 0;
		PhotonLocator.PhotonConnect.Outgoing.BeginTask(ActiveSkill, ActiveSubTask, ActiveLevel, ActiveMask);

		int instanceCount = saverS.instanceCount;
		PhotonLocator.PhotonConnect.Outgoing.UpdateProgress(0, instanceCount, 0, 0);
	}
	public static void PhotonProgressUpdate()
	{
		int time = (int)(Time.timeSinceLevelLoad - timerS.levelStartTime);
		int instanceCount = saverS.instanceCount;
		int successCount = saverS.successCount;
		int failCount = saverS.failCount;
		PhotonLocator.PhotonConnect.Outgoing.UpdateProgress(time, instanceCount,successCount, failCount);
	}
}
