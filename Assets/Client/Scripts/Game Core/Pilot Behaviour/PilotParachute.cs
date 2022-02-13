
    using UnityEngine;

    public class PilotParachute
    {
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _parachuteSprite;

        public PilotParachute(Rigidbody2D rigidbody2D, SpriteRenderer parachuteSprite)
        {
            _rigidbody2D = rigidbody2D;
            _parachuteSprite = parachuteSprite;
        }

        public void OpenParachute()
        {
            _rigidbody2D.gravityScale = .1f;
            _parachuteSprite.enabled = true;
        }
    }
