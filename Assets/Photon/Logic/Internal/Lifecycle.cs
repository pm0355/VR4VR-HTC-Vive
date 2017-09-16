namespace Photon.Logic.Internal
{
	using UnityEngine;
	using System;

	public class Lifecycle : MonoBehaviour
	{
		public event EventHandler StartObject;
		public event EventHandler UpdateObject;

		void Awake() { DontDestroyOnLoad(this.gameObject); }

		void Start() { OnStart(); }

		void Update() { OnUpdate(); }

		protected virtual void OnStart()
		{
			EventHandler handler = StartObject;

			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		protected virtual void OnUpdate()
		{
			EventHandler handler = UpdateObject;

			if (handler != null)
				handler(this, EventArgs.Empty);
		}
	}
}