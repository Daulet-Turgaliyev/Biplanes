﻿
	using System;
	using Mirror;
	using UnityEngine;

	public sealed class PlaneCabin: NetworkBehaviour
	{
		[SerializeField] 
		private PilotBehaviour pilotPrefab;
		
		public Action OnJumpUp = () => { };

		private NetworkIdentity _networkIdentity;

		private void Awake()
		{
			_networkIdentity = GetComponent<NetworkIdentity>();
		}

		private void OnEnable()
		{
			OnJumpUp += JumpOut;
		}

		private void OnDisable() => OnJumpUp = null;
		
		private void JumpOut()
		{
			if (_networkIdentity.hasAuthority)
				CmdJumpOutPlane();
		}
		
		//TODO: Let's see it again it's so bad
		[Command]
		private void CmdJumpOutPlane()
		{
			PilotBehaviour pilot = Instantiate(pilotPrefab, transform.position, Quaternion.identity);
			var conn = _networkIdentity.connectionToClient;
			NetworkServer.Spawn(pilot.gameObject, conn);
		}
		
	}
