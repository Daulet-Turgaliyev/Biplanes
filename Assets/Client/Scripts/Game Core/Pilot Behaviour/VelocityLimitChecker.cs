
	using System;
	using Mirror;
	using UnityEngine;

	public sealed class VelocityLimitChecker: MonoBehaviour
	{
		[SerializeField]
		private PilotBehaviour _pilotBehaviour;
		private float _velocityLimit;
		private static float OFFSET_VELOCITY = -0.1f;
		
		[SerializeField] 
		private NetworkIdentity _networkIdentity;
		
		public Action OnOverLimit = () => { };
		
		public void VelocityLimitCheckerInit(float velocityLimit)
		{
			_velocityLimit = velocityLimit + OFFSET_VELOCITY;
		}

		~VelocityLimitChecker()
		{
			OnOverLimit = null;
		}

		[ClientCallback]
		private void OnTriggerEnter2D(Collider2D col)
		{
			if (_networkIdentity.hasAuthority == false) return;
			
			if (col.GetComponent<Ground>() == false) return;
			
			if (_pilotBehaviour.Velocity.y < _velocityLimit)
				OnOverLimit?.Invoke();
		}
	}
