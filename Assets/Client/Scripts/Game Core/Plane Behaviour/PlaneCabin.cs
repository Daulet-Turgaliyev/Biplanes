
	using System;
	using Mirror;
	using UnityEngine;

	public sealed class PlaneCabin: NetworkBehaviour
	{
		[SerializeField] 
		private PilotBehaviour _pilotPrefab;
		
		public Action OnJumpUp = () => { };
		public Action<int> OnJumped = (int t) => { };
		
		private NetworkIdentity _networkIdentity;

		public bool HasPilotInCabin { get; private set; }
		
		private void Awake()
		{
			_networkIdentity = GetComponent<NetworkIdentity>();
			HasPilotInCabin = true;
		}

		private void OnEnable() => OnJumpUp += JumpOut;
		private void OnDisable() => OnJumpUp = null;
		
		private void JumpOut()
		{
			if (_networkIdentity.hasAuthority)
				CmdJumpOutPlane();

			OnJumped?.Invoke(3);
		}
	
		[Command]
		private void CmdJumpOutPlane()
		{
			HasPilotInCabin = false;
			PilotBehaviour pilot = Instantiate(_pilotPrefab, transform.position, Quaternion.identity);
			var conn = _networkIdentity.connectionToClient;
			pilot.GetComponent<NetworkMatch>().matchId = MatchController.Instance.GetNetworkMath;
			NetworkServer.Spawn(pilot.gameObject, conn);
		}
		
	}
