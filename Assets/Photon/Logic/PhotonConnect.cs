namespace Photon
{
	using UnityEngine;
	using Photon.Logic;
	using Photon.Logic.Event;

	public class PhotonConnect : MonoBehaviour
	{
		public PhotonOutgoing Outgoing { get; private set; }

		private PhotonNetwork photon;
		private PhotonIncoming incoming;

		void Start()
		{
			photon = new PhotonNetwork();
			photon.Connect();
			incoming = new PhotonIncoming();
			AddPhotonCallbacks();
			Outgoing = new PhotonOutgoing(photon);
			PhotonLocator.Locate();
		}

		void OnDestroy()
		{
			if (photon == null)
				return;

			RemovePhotonCallbacks();
			photon.Disconnect();
		}

		void AddPhotonCallbacks()
		{
			photon.AddPhotonEventCall(incoming.PhotonEventHandler);
			photon.AddPhotonPropertyChangeCall(incoming.PhotonPropertyChangeHandler);
			photon.AddPhotonGameJoinedCall(incoming.PhotonGameJoinedHandler);
		}

		void RemovePhotonCallbacks()
		{
			photon.RemovePhotonEventCall(incoming.PhotonEventHandler);
			photon.RemovePhotonPropertyChangeCall(incoming.PhotonPropertyChangeHandler);
			photon.RemovePhotonGameJoinedCall(incoming.PhotonGameJoinedHandler);
		}

		#region GUI Display
		void OnGUI()
		{
			GUILayout.BeginArea(new Rect(720, 100, 250, 200));
			if (photon == null) {
				GUILayout.Label("ERROR: Photon is null."); return; }

			if (photon.connected)
				GUILayout.Label("Photon Connected.");

			if (photon.inRoom)
			{
				GUILayout.Space(10);
				GUILayout.Label("In Room '" + photon.roomName + "'.");
				foreach (string player in photon.PlayerList())
					GUILayout.Label("\t" + player);
			}

			if (!string.IsNullOrEmpty(photon.error))
				GUILayout.Label(photon.error);
			GUILayout.EndArea();
		}
		#endregion
	}
}