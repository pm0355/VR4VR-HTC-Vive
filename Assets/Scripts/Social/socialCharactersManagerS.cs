using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class socialCharactersManagerS : MonoBehaviour {

	public Transform[] positions;
	public GameObject[] characterObjs;
	public GameObject forkliftObj;
	public RuntimeAnimatorController[] controllers;

	private Transform creations;
	private bool dynamicCharactersActive = false;
	private List<GameObject> activeObjs;

	private float nextDynamicTime;
	public float dynamicFreq = 20.0f;
	public float dynamicDeviation = 5.0f;

	private int st;

	public AudioClip[] audios;

	// Use this for initialization
	void Start () 
	{
		creations = new GameObject().transform;
		creations.name = "Creations";
		creations.parent = transform;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject tempObj = null;
		if(dynamicCharactersActive)
		{
			if(Time.timeSinceLevelLoad > nextDynamicTime)
			{
				if (st == 1)
				{
					int rand4 = Random.Range(0, 4);
					if (rand4 < 2)
					{
						tempObj = Instantiate(activeObjs[Random.Range(0, 2)], positions[rand4 + 8].position, positions[rand4 + 8].rotation) as GameObject;
						tempObj.transform.parent = creations;
						tempObj.GetComponent<Animator>().runtimeAnimatorController = controllers[0];
						tempObj.AddComponent<socialCharacterDestroyerS>();
					}
					else
					{
						List<GameObject> tempChars = new List<GameObject>();
						tempChars.Add(activeObjs[0]);
						tempChars.Add(activeObjs[1]);
						tempChars.Add(activeObjs[2]);

						int rand5 = Random.Range(0, tempChars.Count);
						tempObj = Instantiate(tempChars[rand5], positions[rand4 * 2 + 6].position, positions[rand4 * 2 + 6].rotation) as GameObject;
						tempObj.transform.parent = creations;
						tempObj.GetComponent<Animator>().runtimeAnimatorController = controllers[0];
						tempObj.AddComponent<socialCharacterDestroyerS>();
						tempChars.RemoveAt(rand5);

						rand5 = Random.Range(0, tempChars.Count);
						tempObj = Instantiate(tempChars[rand5], positions[rand4 * 2 + 7].position, positions[rand4 * 2 + 7].rotation) as GameObject;
						tempObj.transform.parent = creations;
						tempObj.GetComponent<Animator>().runtimeAnimatorController = controllers[1];
						tempObj.AddComponent<socialCharacterDestroyerS>();
					}
				}
				if (st == 2)
				{
					int rand3 = Random.Range(0, 3);
					if(rand3==2)
					{
						tempObj = Instantiate(forkliftObj) as GameObject;
						tempObj.transform.parent = creations;
					}
					else
					{
						tempObj = Instantiate(activeObjs[0], positions[rand3 + 8].position, positions[rand3 + 8].rotation) as GameObject;
						tempObj.transform.parent = creations;
						tempObj.GetComponent<Animator>().runtimeAnimatorController = controllers[0];
						tempObj.AddComponent<socialCharacterDestroyerS>();
					}
				}
				CalculateNextTime();
			}
		}
	}

	public void CreateCharacters(int _subtask)
	{
		ClearCharacters();

		st = _subtask;

		GameObject tempObj = null;

		activeObjs = new List<GameObject>();
		activeObjs.Add(characterObjs[st * 3 + 0]);
		activeObjs.Add(characterObjs[st * 3 + 1]);
		activeObjs.Add(characterObjs[st * 3 + 2]);

		if (st == 0 || st == 2)
		{
			int rand1 = Random.Range(0, 4);

			int rand2 = Random.Range(0, activeObjs.Count);
			tempObj = Instantiate(activeObjs[rand2], positions[rand1 * 2 + 0].position, positions[rand1 * 2 + 0].rotation) as GameObject;
			tempObj.transform.parent = creations;
			tempObj.GetComponent<Animator>().runtimeAnimatorController = controllers[Random.Range(4, 6)];
			activeObjs.RemoveAt(rand2);

			rand2 = Random.Range(0, activeObjs.Count);
			tempObj = Instantiate(activeObjs[rand2], positions[rand1 * 2 + 1].position, positions[rand1 * 2 + 1].rotation) as GameObject;
			tempObj.transform.parent = creations;
			tempObj.GetComponent<Animator>().runtimeAnimatorController = controllers[Random.Range(2, 4)];
			activeObjs.RemoveAt(rand2);
		}

		CalculateNextTime();
		dynamicCharactersActive = true;

		audio.Stop();
		audio.clip = audios[Random.Range(0, audios.Length)];
		audio.Play();
	}
	void CalculateNextTime()
	{
		nextDynamicTime = Time.timeSinceLevelLoad + dynamicFreq + Random.Range(-dynamicDeviation, dynamicDeviation);
	}
	void ClearCharacters()
	{
		foreach (Transform child in creations) Destroy(child.gameObject);
		dynamicCharactersActive = false;
	}
}
