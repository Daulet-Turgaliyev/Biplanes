
    using System;
    using Mirror;
    using UnityEngine;
    using UnityEngine.PlayerLoop;

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PilotParachute: NetworkBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        public bool IsParachuteActive { get; private set; }

        [SerializeField]
        private SpriteRenderer _parachuteSprite;

        [SerializeField]
        private CircleCollider2D _parachuteCollider;

        private NetworkIdentity _networkIdentity;
        
        private float _currentGravityScale;

        public void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _networkIdentity = GetComponent<NetworkIdentity>();
            _currentGravityScale = _rigidbody2D.gravityScale;
        }
        
        //TODO: Найди причину включения парашёюта
        private void Update()
        {
            _parachuteSprite.enabled = IsParachuteActive;
        }

        public void OpenParachute()
        {
            if (_networkIdentity.hasAuthority == false) return;
            
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
            _parachuteCollider.enabled = isActive;
            IsParachuteActive = isActive;
        }
        
        public void CloseParachute()
        {
            if (_networkIdentity.hasAuthority == false) return;
            _rigidbody2D.gravityScale = _currentGravityScale;
            CmdSetEnableParachute(false);
        }
    }
