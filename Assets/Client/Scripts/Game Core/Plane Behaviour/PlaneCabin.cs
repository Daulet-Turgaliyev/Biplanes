
	using System;
	using Mirror;
	using UnityEngine;

	public class PlaneCabin: NetworkBehaviour
	{
		[SerializeField] 
		private GameObject pilotPrefab;
		
		public Action OnJumpUp = () => { };

		private NetworkIdentity _networkIdentity;
		private void Awake()
		{
			_networkIdentity = GetComponent<NetworkIdentity>();
		}
		
		public void Init()
		{

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
		

		[Command]
		private void CmdJumpOutPlane()
		{
			GameObject pilot = Instantiate(pilotPrefab, transform.position, transform.rotation);
			NetworkServer.Spawn(pilot, _networkIdentity.connectionToClient);
		}
		
	}
