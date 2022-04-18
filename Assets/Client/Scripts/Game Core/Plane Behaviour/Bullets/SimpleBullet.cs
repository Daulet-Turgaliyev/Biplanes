
    using UnityEngine;

    public sealed class SimpleBullet: ABullet
    {
	    [SerializeField]
	    private SpriteRenderer _spriteRenderer;

	    private void Awake()
	    {
		    _spriteRenderer.enabled = true;
	    }

	    private void Start() => base.OnBulletInit();
		
	}
