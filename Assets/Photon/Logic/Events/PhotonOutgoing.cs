namespace Photon.Logic.Event
{
	using DebugLevel = ExitGames.Client.Photon.DebugLevel;

	public class PhotonOutgoing
	{
		/* Send Events */
		private const byte evSessionData = (byte)'A';

		private PhotonNetwork photon;

		public PhotonOutgoing(PhotonNetwork photon)
		{
			this.photon = photon;
		}

		public void BeginTask(Skill skill, int subtask, Level level, int distractorMask)
		{
			Log(DebugLevel.INFO, "Update Task: " + skill + ", " + subtask + ", " + level + ", " + distractorMask);
			int[] task = { (int)skill, subtask, (int)level, distractorMask };
			var properties = new ExitGames.Client.Photon.Hashtable() { { "task", task } };
			UpdateRoomProperties(properties);
		}

		public void UpdateProgress(int elaspedTime, int instanceCount, int successCount, int failCount)
		{
			Log(DebugLevel.INFO, "Update progress: " + elaspedTime + ", " + instanceCount + ", " + successCount + ", " + failCount);
			int[] progress = { elaspedTime, instanceCount, successCount, failCount };
			var properties = new ExitGames.Client.Photon.Hashtable() { { "progress", progress } };
			UpdateRoomProperties(properties);
		}

		public void SendSessionData(int sessionID)
		{
			Log(DebugLevel.INFO, "Raise event Session Data with sessionID: " + sessionID);
			RaiseEvent(evSessionData, sessionID, true, null);
		}

		public bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, ExitGames.Client.Photon.LoadBalancing.RaiseEventOptions options)
		{
			if (photon == null) {
				Log(DebugLevel.ERROR, "Event Send Failed: Photon instance is null."); return false; }

			return photon.RaiseEvent(eventCode, eventContent, sendReliable, options);
		}

		public void UpdateRoomProperties(ExitGames.Client.Photon.Hashtable properties)
		{
			if (photon == null) {
				Log(DebugLevel.ERROR, "Set Custom Room Properties Failed: Photon instance is null."); return; }

			photon.CurrentRoom.SetCustomProperties(properties);
		}

		private void Log(DebugLevel level, string message)
		{
			PhotonLocator.PhotonDebug.Log(level, "[PhotonOutgoing]\t\t " + message);
		}
	}
}