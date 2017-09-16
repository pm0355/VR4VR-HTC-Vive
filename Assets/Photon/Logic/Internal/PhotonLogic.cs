namespace Photon.Logic.Internal
{
	using System;
	using System.Threading;
	using ExitGames.Client.Photon;
	using ExitGames.Client.Photon.LoadBalancing;
	using Photon.Logic.Event;
	using GameObject = UnityEngine.GameObject;

	public class PhotonLogic : LoadBalancingClient
	{
		public string ErrorMessage { get; set; }
		public event EventHandler<PhotonEventArgs> PhotonEvent;
		public event EventHandler<PhotonPropertyEventArgs> PhotonPropertyChange;
		public event EventHandler<PhotonPropertyEventArgs> PhotonGameJoined;

		private Timer outgoingTimer;
		private GameObject dispatch;

		public PhotonLogic()
		{
			AppId = PhotonSettings.AppId;
			AppVersion = PhotonSettings.AppVersion;
			PlayerName = PhotonSettings.PlayerName;
			AutoJoinLobby = false;
		}

		public void ConnectToRegion()
		{
			DebugReturn(DebugLevel.ALL, "ConnectToRegion");
			outgoingTimer = new Timer(SendOutgoing, null, 0, PhotonSettings.SendOutgoingInterval);
			CreateDispatch();
			Service();
			ConnectToRegionMaster(PhotonSettings.Region);
			Service();
		}

		public new void Disconnect()
		{
			Service();
			base.Disconnect();
			Service();
			outgoingTimer.Dispose();
			GameObject.DestroyImmediate(dispatch);
		}

		private void CreateDispatch()
		{
			dispatch = new UnityEngine.GameObject("PhotonDispatch");
			Lifecycle lifecycle = dispatch.AddComponent<Lifecycle>();
			lifecycle.UpdateObject += DispatchIncoming;
			dispatch.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
		}

		private void OnConnectedToMaster()
		{
			OpJoinOrCreateRoom(PhotonSettings.DefaultRoom, 0, null);
		}

		private void SendOutgoing(Object state)
		{
			loadBalancingPeer.SendOutgoingCommands();
		}

		void DispatchIncoming(object sender, EventArgs e)
		{
			loadBalancingPeer.DispatchIncomingCommands();
		}

		public override void OnOperationResponse(OperationResponse operationResponse)
		{
			base.OnOperationResponse(operationResponse);
			DebugReturn(DebugLevel.ALL, "OnOperationResponse: " + operationResponse.ToString());

			switch (operationResponse.OperationCode)
			{
				case (byte)OperationCode.Authenticate:
					ConnectReturn(operationResponse);
					break;
				case (byte)OperationCode.CreateGame:
					CreateRoomReturn(operationResponse);
					break;
				case (byte)OperationCode.JoinGame:
					JoinRoomReturn(operationResponse);
					break;
				case (byte)OperationCode.SetProperties:
					break;
			}
		}

		public override void OnStatusChanged(StatusCode statusCode)
		{
			base.OnStatusChanged(statusCode);
			DebugReturn(DebugLevel.ALL, "OnStatusChanged: " + statusCode.ToString());
		}

		public override void OnEvent(EventData photonEvent)
		{
			base.OnEvent(photonEvent);
			DebugReturn(DebugLevel.ALL, "OnEvent: " + photonEvent.ToString());

			if (photonEvent.Code < 200)
				OnPhotonEvent(new PhotonEventArgs(photonEvent));
			else if (photonEvent.Code == EventCode.PropertiesChanged)
				OnPropertyChange(new PhotonPropertyEventArgs(photonEvent));
		}

		protected virtual void OnPhotonEvent(PhotonEventArgs e)
		{
			EventHandler<PhotonEventArgs> handler = PhotonEvent;

			if (handler != null)
				handler(this, e);	// Raise the event
		}

		protected virtual void OnPropertyChange(PhotonPropertyEventArgs e)
		{
			EventHandler<PhotonPropertyEventArgs> handler = PhotonPropertyChange;

			if (handler != null)
				handler(this, e);	// Raise the event
		}

		protected virtual void OnGameJoined(PhotonPropertyEventArgs e)
		{
			EventHandler<PhotonPropertyEventArgs> handler = PhotonGameJoined;

			if (handler != null)
				handler(this, e);	// Raise the event
		}

		private void ConnectReturn(OperationResponse operationResponse)
		{
			DebugReturn(DebugLevel.ALL, "ConnectReturn.");
			if (operationResponse.ReturnCode != ErrorCode.None) {
				ConnectionErrorReturn(operationResponse); return; }

			if (Server == ServerConnection.MasterServer)
				OnConnectedToMaster();
		}

		private void CreateRoomReturn(OperationResponse operationResponse)
		{
			DebugReturn(DebugLevel.ALL, "CreateRoomReturn.");
			if (operationResponse.ReturnCode != ErrorCode.None)
			{
				ErrorMessage = string.Format("CreateRoom failed. Client will stay on masterserver.\nResponse: {0}", operationResponse.ToStringFull());
				DebugReturn(DebugLevel.ERROR, ErrorMessage);
				return;
			}
		}

		private void JoinRoomReturn(OperationResponse operationResponse)
		{
			DebugReturn(DebugLevel.ALL, "JoinRoomReturn.");
			if (operationResponse.ReturnCode != ErrorCode.None)
			{
				ErrorMessage = string.Format("JoinRoom failed. Client will stay on masterserver.\nResponse: {0}.", operationResponse.ToStringFull());
				DebugReturn(DebugLevel.ERROR, ErrorMessage);
				return;
			}

			if (IsConnectedAndReady)
				OnGameJoined(new PhotonPropertyEventArgs(CurrentRoom.CustomProperties));
		}

		public override void DebugReturn(DebugLevel level, string message)
		{
			base.DebugReturn(level, message);
			PhotonLocator.PhotonDebug.Log(level, "[PhotonLogic]\t\t " + message);
		}

		#region Error Handling

		private void ConnectionErrorReturn(OperationResponse operationResponse)
		{
			switch (operationResponse.ReturnCode)
			{
				case ErrorCode.InvalidAuthentication:
					ErrorMessage = string.Format("Authentication failed. Invalid AppId: {0}.\nResponse: {1}", AppId, operationResponse.ToStringFull());
					DebugReturn(DebugLevel.ERROR, ErrorMessage);
					break;
				case ErrorCode.InvalidOperationCode:
				case ErrorCode.InternalServerError:
					ErrorMessage = string.Format("Authentication failed. Successfully connected to the server ({0}) but it was unable to 'authenticate'. \nResponse: {1}", MasterServerAddress, operationResponse.ToStringFull());
					DebugReturn(DebugLevel.ERROR, ErrorMessage);
					break;
				default:
					ErrorMessage = string.Format("Authentication failed. Unable to connect to Photon. \nResponse: {1}", MasterServerAddress, operationResponse.ToStringFull());
					DebugReturn(DebugLevel.ERROR, ErrorMessage);
					break;
			}

			Disconnect();
		}

		#endregion
	}
}