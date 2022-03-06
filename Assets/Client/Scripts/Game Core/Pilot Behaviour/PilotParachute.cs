
    using Mirror;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PilotParachute: NetworkBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField]
        private SpriteRenderer _parachuteSprite;

        private NetworkIdentity _networkIdentity;
        
        private float _currentGravityScale;

        public void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _networkIdentity = GetComponent<NetworkIdentity>();
            _currentGravityScale = _rigidbody2D.gravityScale;
        }

        
        public void OpenParachute()
        {
            if (_networkIdentity.hasAuthority == false) return;
            
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.gravityScale = .2f;
            CmdSetEnableParachute(true);
        }

        [Command]
        private void CmdSetEnableParachute(bool isActive)
        {
            RpcSetEnableParachute(isActive);
        }

        [ClientRpc]
        private void RpcSetEnableParachute(bool isActive)
        {
            _parachuteSprite.enabled = isActive;
        }
        
        public void CloseParachute()
        {
            if (_networkIdentity.hasAuthority == false) return;
            _rigidbody2D.gravityScale = _currentGravityScale;
            CmdSetEnableParachute(false);
        }
    }
