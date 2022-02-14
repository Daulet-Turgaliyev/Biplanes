
    using Mirror;
    using UnityEngine;
    using UnityEngine.Serialization;

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PilotParachute: NetworkBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField, FormerlySerializedAs("SpriteRenderer")]
        private SpriteRenderer _parachuteSprite;

        private float _currentGravityScale;

        public void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _currentGravityScale = _rigidbody2D.gravityScale;
        }

        [Command]
        public void OpenParachute()
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.gravityScale = .5f;
            _parachuteSprite.enabled = true;
        }
        
        public void CloseParachute()
        {
            _rigidbody2D.gravityScale = _currentGravityScale;
            _parachuteSprite.enabled = false;
        }
    }
