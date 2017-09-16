using UnityEngine;
using System.Collections;

public class distracterManagerS : MonoBehaviour {

	private bool distractersActive;
	public bool DistractersActive {get{return distractersActive;}}

	private Transform distracters;
	private int numberOfActiveDistracters;

	private bool[] activeDistracters;
	public GameObject[] distracterObjs;

	private float nextDistracterTime;
	private int nextDistracterIndex;
	private float distracterPeriod = 30.0f;
	private float distracterDeviation = 5.0f;

	private int[] defaultDistracters = {
										   31, 31, 31,
										   992, 992, 992,
										   130048, 130048, 130048,
										   917522, 917522, 917522,
										   32505856, 32505856, 32505856,
										   1040191488, 1040191488, 1040191488
									   };
	
	public GUIStyle distracterStyle;

	void Awake () 
	{
		distracters = new GameObject().transform;
		distracters.name = "Distracters";
		distracters.parent = transform;

		activeDistracters = new bool[30];
	}
	void Update () 
	{
		if(distractersActive)
		{
			if(Time.timeSinceLevelLoad > nextDistracterTime)
			{
				StartDistracter(nextDistracterIndex);
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) ManuelDistracter(1);
		if (Input.GetKeyDown(KeyCode.Alpha2)) ManuelDistracter(2);
		if (Input.GetKeyDown(KeyCode.Alpha3)) ManuelDistracter(3);
		if (Input.GetKeyDown(KeyCode.Alpha4)) ManuelDistracter(4);
		if (Input.GetKeyDown(KeyCode.Alpha5)) ManuelDistracter(5);
		if (Input.GetKeyDown(KeyCode.Alpha6)) ManuelDistracter(6);
		if (Input.GetKeyDown(KeyCode.Alpha7)) ManuelDistracter(7);
		if (Input.GetKeyDown(KeyCode.Alpha8)) ManuelDistracter(8);
		if (Input.GetKeyDown(KeyCode.Alpha9)) ManuelDistracter(9);
		if (Input.GetKeyDown(KeyCode.Alpha0)) ManuelDistracter(10);		
	}

	public void LoadDefaultDistracters()
	{
		SetActiveDistracters(defaultDistracters[((int)generalManagerS.ActiveSkill - 1) * 3 + generalManagerS.ActiveSubTask]);
		//Debug.Log("Active distracters: " + GetActiveDistracters());
	}
	public void ActiveteDistracters(bool _active)
	{
		if (_active)
		{
			if (distractersActive == false && numberOfActiveDistracters > 0)
			{
				distractersActive = true;
				ChooseNextDistracter();
			}
		}
		else
		{
			distractersActive = false;
			ClearDistracters();
		}
	}
	public void SetDistracterTiming(float _period, float _deviation)
	{
		distracterPeriod = _period;
		distracterDeviation = _deviation;

		ChooseNextDistracter();
	}

	public void StartDistracter(int _index)
	{
		if(distractersActive)
		{
			if(activeDistracters[_index])
			{
				ClearDistracters();

				GameObject tempObj = Instantiate(distracterObjs[_index], Vector3.zero, Quaternion.identity) as GameObject;
				tempObj.transform.parent = distracters;
				tempObj.name = "Distracter"+_index;

				saverS.distracterCount++;
				saverS.distracterOrder += _index.ToString() + "-";
				saverS.distracterTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

				ChooseNextDistracter();
			}
		}
	}

	public void SetActiveDistracters( int _distacters )
	{
		int value = _distacters;
		int count = 0;
		for (int i = activeDistracters.Length-1; i >= 0; i--) 
		{
			if(value >= Mathf.Pow(2.0f,i))
			{
				activeDistracters[i] = true;
				value -= (int) Mathf.Pow(2.0f,i);
				count++;
			}
			else
			{
				activeDistracters[i] = false;
			}
		}
		numberOfActiveDistracters = count;

		if(numberOfActiveDistracters < 1) ActiveteDistracters(false);
		else ChooseNextDistracter();
	}
	public void ToggleDistracter (int _distracterIndex)
	{
		activeDistracters[_distracterIndex] = !activeDistracters[_distracterIndex];
	}

	public int GetActiveDistracters( )
	{
		int value = 0;
		for (int i = 0; i < activeDistracters.Length; i++) 
		{
			if(activeDistracters[i]) 
				value += (int) Mathf.Pow(2.0f,i);
		}
		return value;
	}
	public bool GetDistracter(int _distracterIndex)
	{
		return activeDistracters[_distracterIndex];
	}
	public int GetNumberOfActiveDistracters( )
	{
		return numberOfActiveDistracters;
	}

	void ManuelDistracter(int _keyInput)
	{
		//Debug.Log("active:" + numberOfActiveDistracters + "   input" + _keyInput);
		if (numberOfActiveDistracters >= _keyInput)
		{
			int manuelDistracterIndex = 0;
			int correspondingDistracter = 0;
			for (int i = 0; i < activeDistracters.Length; i++)
			{
				if (activeDistracters[i])
				{
					manuelDistracterIndex++;
					if (manuelDistracterIndex == _keyInput)
					{
						correspondingDistracter = i;
						break;
					}
				}

			}
			StartDistracter(correspondingDistracter);
		}
	}
	void ChooseNextDistracter()
	{
		if (numberOfActiveDistracters > 0)
		{
			int distIndex = Random.Range(0, numberOfActiveDistracters);
			int count = 0;
			for (int i = 0; i < activeDistracters.Length; i++)
			{
				if (activeDistracters[i])
				{
					if (distIndex == count)
					{
						nextDistracterIndex = i;
						break;
					}
					else count++;
				}
			}
			nextDistracterTime = Time.timeSinceLevelLoad + distracterPeriod + Random.Range(-distracterDeviation, distracterDeviation);
		}
	}
	void ClearDistracters()
	{
		foreach (Transform child in distracters) Destroy(child.gameObject);
	}
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(960, 0, 960, 1080));
		GUILayout.BeginVertical();
		GUILayout.Label("DISTRACTERS", distracterStyle);
		GUILayout.Label("-----------------------", distracterStyle);
		if (distractersActive) GUILayout.Label("Distracters ON", distracterStyle);
		else GUILayout.Label("Distracters OFF", distracterStyle);
		if(distractersActive)
		{
			GUILayout.Label("Distracter period is " + distracterPeriod + " deviation is " + distracterDeviation, distracterStyle);
			GUILayout.Label("Next Distracter is " + (Distracter)nextDistracterIndex, distracterStyle);
			GUILayout.Label("Next Distracter in " + ((int)(nextDistracterTime - Time.timeSinceLevelLoad)).ToString(), distracterStyle);
		
			GUILayout.Label("", distracterStyle);

			for (int i = 0; i < activeDistracters.Length; i++) 
			{
				if (activeDistracters[i]) GUILayout.Label("Distracter: (ON) " + (Distracter)i, distracterStyle);
				else GUILayout.Label("Distracter: (OFF) " + (Distracter)i , distracterStyle);				
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
