namespace Photon.Logic.Event
{
	using System;
	using ExtensionSystem;
	using ExtensionSystem.Collections.Generic;
	using DebugLevel = ExitGames.Client.Photon.DebugLevel;

	public class PhotonIncoming
	{
		/* Received Events */
		private const byte evLoadLevel = (byte)'a';
		private const byte evToggleDistraction = (byte)'b';
		private const byte evForceQuit = (byte)'c';

		/* Properties */
		private const string propertyUser = "user";

		public void PhotonEventHandler(object sender, PhotonEventArgs e)
		{
			switch (e.eventCode)
			{
				case evLoadLevel:
					LoadLevel(e.content);
					break;
				case evToggleDistraction:
					ToggleDistraction(e.content);
					break;
				case evForceQuit:
					ForceQuit();
					break;
				default:
					Log(DebugLevel.ERROR, "Error: Unhandled eventCode: " + e.eventCode);
					break;
			}
		}

		public void PhotonPropertyChangeHandler(object sender, PhotonPropertyEventArgs e)
		{
			UpdateProperties(e.content);
		}

		public void PhotonGameJoinedHandler(object sender, PhotonPropertyEventArgs e)
		{
			UpdateProperties(e.content);
		}

		private void UpdateProperties(ExitGames.Client.Photon.Hashtable properties)
		{
			object user;

			if (properties.TryGetValue(propertyUser, out user))
				UpdateUser(user);
		}

		private void LoadLevel(object content)
		{
			Log(DebugLevel.INFO, "Load Level");

			int[] module = content as int[];
			Skill skill; int subtask; Level level;

			if (EArray.TryGetEnumElement(module, 0, out skill) && EArray.TryGetElement(module, 1, out subtask) && EArray.TryGetEnumElement(module, 2, out level))
			{
				generalManagerS.SetLevel(skill, subtask, level);
				generalManagerS.StartLevel();
			}
			else
				InvalidData("Load Level", content);
		}

		private void ToggleDistraction(object content)
		{
			Log(DebugLevel.INFO, "Toggle Distraction");

			Nullable<int> distraction = content as Nullable<int>;

			if (distraction != null)
				generalManagerS.DistracterM.StartDistracter(distraction.GetValueOrDefault());
			else
				InvalidData("Toggle Distraction", content);
		}

		private void ForceQuit()
		{
			Log(DebugLevel.INFO, "End Simulation");
			generalManagerS.EndLevel();
		}

		private void UpdateUser(object content)
		{
			Log(DebugLevel.INFO, "Update User");

			int[] user = content as int[];
			int jobCoachID, userID;

			if (EArray.TryGetElement(user, 0, out jobCoachID) && EArray.TryGetElement(user, 1, out userID))
				generalManagerS.SetJobCoachAndUser(jobCoachID, userID);
			else
				InvalidData("Update User", content);
		}

		private void InvalidData(string eventName, object content)
		{
			Log(DebugLevel.ERROR, "Error: " + eventName + " Data Invalid. Unable to parse event content.\nEvent Content: " + content);
		}

		private void Log(DebugLevel level, string message)
		{
			PhotonLocator.PhotonDebug.Log(level, "[PhotonIncoming]\t\t " + message);
		}
	}
}