
    using Mirror;
    using UnityEngine;
    using UnityEngine.Serialization;

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PilotParachute: NetworkBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField, FormerlySerializedAs("SpriteRenderer")]
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
        public void CmdSetEnableParachute(bool isActive)
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
