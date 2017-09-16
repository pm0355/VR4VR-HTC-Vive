namespace Photon.Logic
{
	using ExitGames.Client.Photon;
	using PhotonSettings = Photon.Logic.Internal.PhotonSettings;

	public class PhotonDebug
	{
		private DebugLevel globalLevel;

		public PhotonDebug(DebugLevel globalLevel)
		{
			this.globalLevel = globalLevel;
		}

		public void Log(DebugLevel level, string message)
		{
			if (globalLevel == DebugLevel.OFF || level > globalLevel)
				return;

			const string preface = "[Photon] ";
			message = preface + message;

			if (level == DebugLevel.ERROR)
				UnityEngine.Debug.LogError(message);
			else if (level == DebugLevel.WARNING)
				UnityEngine.Debug.LogWarning(message);
			//else
				//UnityEngine.Debug.Log(message);
		}
	}
}