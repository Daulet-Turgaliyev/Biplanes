using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet2D : NetworkBehaviour
{
    private readonly float _destroyAfter = 5f;
    private Rigidbody2D _rigidBody2D;

    private float _force = 500f;

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), _destroyAfter);
    }
    
    public void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.AddForce(transform.right * _force);
    }
    
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D _)
    {
        NetworkServer.Destroy(gameObject);
    }
}
