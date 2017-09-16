namespace Photon.Logic
{
	using System;
	using System.Linq;
	using ExitGames.Client.Photon;
	using ExitGames.Client.Photon.LoadBalancing;
	using Photon.Logic.Internal;
	using Photon.Logic.Event;

	public class PhotonNetwork
	{
		public bool connected { get { return client.IsConnectedAndReady; } }
		public string error { get { return client.ErrorMessage; } }
		public Room CurrentRoom { get { return client.CurrentRoom; } }
		public string roomName { get { return CurrentRoom.Name; } }
		public bool inRoom { get { return (CurrentRoom != null); } }

		private PhotonLogic client;

		public PhotonNetwork() { client = new PhotonLogic(); }

		public void Connect() { client.ConnectToRegion(); }

		public void Disconnect() { client.Disconnect(); }

		public void AddPhotonEventCall(EventHandler<PhotonEventArgs> callback) { client.PhotonEvent += callback; }
		public void RemovePhotonEventCall(EventHandler<PhotonEventArgs> callback) { client.PhotonEvent -= callback; }
		public void AddPhotonPropertyChangeCall(EventHandler<PhotonPropertyEventArgs> callback) { client.PhotonPropertyChange += callback; }
		public void RemovePhotonPropertyChangeCall(EventHandler<PhotonPropertyEventArgs> callback) { client.PhotonPropertyChange -= callback; }
		public void AddPhotonGameJoinedCall(EventHandler<PhotonPropertyEventArgs> callback) { client.PhotonGameJoined += callback; }
		public void RemovePhotonGameJoinedCall(EventHandler<PhotonPropertyEventArgs> callback) { client.PhotonGameJoined -= callback; }

		public bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
		{
			return client.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
		}

		public string[] PlayerList()
		{
			if (CurrentRoom == null)
				return new string[0];

			return CurrentRoom.Players.Values.Select(player => player.Name).ToArray();
		}
	}
}