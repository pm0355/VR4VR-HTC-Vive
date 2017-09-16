using UnityEngine;
using System.Collections;

public class recorderManagerS : MonoBehaviour {

	private Transform recorderPos;
	public Camera recorderCam;
	private AVProMovieCaptureFromCamera recorderScript;

	// Use this for initialization
	void Start () 
	{
		recorderScript = recorderCam.GetComponent<AVProMovieCaptureFromCamera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (recorderPos != null)
		{
			transform.position = recorderPos.position;
			transform.rotation = recorderPos.rotation;
		}
	}
	public void StartRecording(string _name)
	{
		GetCamera();
		recorderScript._forceFilename = _name+".avi";
		recorderScript.StartCapture();
	}
	public void StopRecording()
	{
		recorderScript.StopCapture();
	}
	public void GetCamera()
	{
		if (Application.isEditor)
		{
			recorderScript._outputFolderPath = "../Videos/";
		}
		else
		{
			recorderScript._outputFolderPath = "Videos";
		}

		recorderPos = Camera.main.transform;
		transform.position = recorderPos.position;
		transform.rotation = recorderPos.rotation;

		recorderCam.orthographic = Camera.main.orthographic;
		recorderCam.nearClipPlane = Camera.main.nearClipPlane;
		recorderCam.farClipPlane = Camera.main.farClipPlane;

		if (Camera.main.orthographic)
		{
			recorderCam.orthographicSize = Camera.main.orthographicSize;

			recorderCam.aspect = 2.0f;
			if (Application.isEditor)
			{
				recorderScript._maxVideoSize.x = 400;
				recorderScript._maxVideoSize.y = 200;
			}
			else
			{
				//recorderScript._maxVideoSize.x = 1024;
				//recorderScript._maxVideoSize.y = 512;
				recorderScript._maxVideoSize.x = 600;
				recorderScript._maxVideoSize.y = 300;
			}
		}
		else
		{
			recorderCam.fieldOfView = Camera.main.fieldOfView;

			recorderCam.aspect = 1.33333333f;
			if (Application.isEditor)
			{
				recorderScript._maxVideoSize.x = 160;
				recorderScript._maxVideoSize.y = 120;
			}
			else
			{
				//recorderScript._maxVideoSize.x = 1024;
				//recorderScript._maxVideoSize.y = 768;

				//recorderScript._maxVideoSize.x = 600;
				//recorderScript._maxVideoSize.y = 450;

				recorderScript._maxVideoSize.x = 320;
				recorderScript._maxVideoSize.y = 240;
			}
		}

		recorderScript._audioCapture = Camera.main.GetComponent<AVProUnityAudioCapture>();
	}
}
