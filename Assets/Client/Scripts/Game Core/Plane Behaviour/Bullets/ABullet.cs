using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABullet : NetworkBehaviour
{
    protected readonly float _destroyAfter = 5f;
    protected Rigidbody2D _rigidBody2D;

    protected float _force = 800f;
    public readonly float Damage = 1f;

    protected void OnBulletInit()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        if (ReferenceEquals(_rigidBody2D, null) == false)
            _rigidBody2D.AddForce(transform.right * _force);
    }

    public override void OnStartServer() => Invoke(nameof(ServerDestroySelf), _destroyAfter);
    
    [Server]
    private void ServerDestroySelf() =>  NetworkServer.Destroy(gameObject);
    
    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.collider.TryGetComponent(out PlaneCollider x));
        if (col.collider.TryGetComponent(out PlaneCollider planeCollider))
            planeCollider.BulletHit(Damage);
            
        NetworkServer.Destroy(gameObject);
    }
}
