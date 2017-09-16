namespace Photon.Logic.Event
{
	using System;
	using ExitGames.Client.Photon;
	using ExitGames.Client.Photon.LoadBalancing;

	public class PhotonPropertyEventArgs : EventArgs
	{
		public Hashtable content { get; private set; }

		public PhotonPropertyEventArgs(Hashtable content) { this.content = content; }
		public PhotonPropertyEventArgs(EventData photonEvent) { content = (Hashtable)photonEvent[ParameterCode.Properties]; }
	}
}