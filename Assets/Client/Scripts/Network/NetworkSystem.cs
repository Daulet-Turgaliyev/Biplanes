using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkMatch))]
public sealed class NetworkSystem : NetworkBehaviour
{
        private NetworkIdentity _networkIdentity;
        
        
        private void Awake()
        {
                _networkIdentity = GetComponent<NetworkIdentity>();
        }
}
