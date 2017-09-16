namespace Photon.Logic.Internal
{
	public class PhotonSettings
	{
		public static string DefaultRoom { get { return "Home"; } }

		public static string AppId { get { return "23c49f04-45df-4fac-b1bb-1a3a33e4ac7a"; } }
		public static string AppVersion { get { return "1.0"; } }
		public static string PlayerName { get { return "Windows"; } }
		public static string Region { get { return "us"; } }

		public const ExitGames.Client.Photon.DebugLevel DebugLevel = ExitGames.Client.Photon.DebugLevel.INFO;

		// Send Outgoing Commands Callback Interval
		public static int SendOutgoingInterval { get { return 100; } } // Time in milliseconds
	}
}