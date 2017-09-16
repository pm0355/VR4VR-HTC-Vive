using UnityEngine;
using System.Collections;

public class networkManagerS : MonoBehaviour
{

	public int initialPortNo = 25003;
	public int portWindow = 10;
	private string message;
	private NetworkConnectionError networkError;

	private int portNo;
	private bool isInitializing = false;

	public static bool isConnected = false;

	private int fps;
	private int fpsCount;
	private float fpsSum;

	// Use this for initialization
	void Start()
	{
		portNo = initialPortNo;
		isInitializing = true;
		Initialize();
		StartCoroutine(UpdateFrameRate());
	}

	void Update()
	{
		fpsSum += Time.deltaTime;
		fpsCount++;
	}

	void LateUpdate()
	{
		if (!isInitializing && networkError == NetworkConnectionError.CreateSocketOrThreadFailure)
		{
			portNo++;
			if (portNo > initialPortNo + portWindow) portNo = initialPortNo;
			isInitializing = true;
			Initialize();
		}
	}

	void OnServerInitialized()
	{
		message = "Initialized. Port No:" + portNo;
		//Debug.Log("Server initialized and ready");
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		//message = "Player connected from " + player.ipAddress + ":" + player.port;
		isConnected = true;
		StartCoroutine(ConnectedMessage());
	}
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		message = "Player " + player + " disconnected!";
		isConnected = false;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(720, 40, 250, 20), message);
		GUI.Label(new Rect(720, 60, 250, 20), "fps: " + fps.ToString());
	}

	void Initialize()
	{
		networkError = Network.InitializeServer(32, portNo, !Network.HavePublicAddress());
		message = networkError.ToString();
		Network.InitializeServer(32, portNo, !Network.HavePublicAddress());
		isInitializing = false;
	}

	IEnumerator ConnectedMessage()
	{
		message = "Connected!";
		yield return new WaitForSeconds(3.0f);
		if (message == "Connected!") message = "";
	}

	IEnumerator UpdateFrameRate()
	{
		while (true)
		{
			if(fpsCount > 0)
			{
				fps = (int)((float)fpsCount / fpsSum);
				fpsSum = 0.0f;
				fpsCount = 0;
			}
			else
				fps = 0;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
