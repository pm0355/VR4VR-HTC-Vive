using Photon;
using UnityEngine;
using PhotonDebug = Photon.Logic.PhotonDebug;
using DebugLevel = ExitGames.Client.Photon.DebugLevel;
using PhotonSettings = Photon.Logic.Internal.PhotonSettings;

public static class PhotonLocator
{
	public static PhotonConnect PhotonConnect { get; private set; }
	public static PhotonDebug PhotonDebug { get; private set; }

	static PhotonLocator()
	{
		Locate();
		PhotonDebug = new PhotonDebug(PhotonSettings.DebugLevel);
	}

	public static void Locate()
	{
		PhotonConnect = Locate<PhotonConnect>();
	}

	private static T Locate<T>() where T : Component
	{
		GameObject gameobject = SafeFind(typeof(T).Name);
		if (gameobject != null)
			return gameobject.SafeGetComponent<T>();
		return null;
	}

	private static GameObject SafeFind(string name)
	{
		GameObject gameobject = GameObject.Find(name);
		if (gameobject == null)
			Log(DebugLevel.ERROR, "GameObject: '" + name + "' was not found.");
		return gameobject;
	}

	private static T SafeGetComponent<T>(this GameObject gameobject) where T : Component
	{
		T component = gameobject.GetComponent<T>();
		if (component == null)
			Log(DebugLevel.ERROR, "Component: '" + typeof(T) + "' was not found on the GameObject '" + gameobject.name + "'");
		return component;
	}

	private static void Log(DebugLevel level, string message)
	{
		PhotonDebug.Log(level, "[PhotonLocator]\t\t " + message);
	}
}