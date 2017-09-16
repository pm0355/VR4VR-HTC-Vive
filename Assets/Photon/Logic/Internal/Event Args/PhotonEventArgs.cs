namespace Photon.Logic.Event
{
	using System;
	using ExitGames.Client.Photon;
	using ExitGames.Client.Photon.LoadBalancing;

	public class PhotonEventArgs : EventArgs
	{
		private EventData photonEvent;

		public PhotonEventArgs(EventData photonEvent) { this.photonEvent = photonEvent; }

		public byte eventCode { get { return photonEvent.Code; } }
		public object content { get { return photonEvent[ParameterCode.Data]; } }
		public int sender
		{
			get
			{
				object actor;
				photonEvent.Parameters.TryGetValue(ParameterCode.ActorNr, out actor);
				return (int)actor;
			}
		}
	}
}