using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet2D : NetworkBehaviour
{
    private readonly float _destroyAfter = 5f;
    private Rigidbody2D _rigidBody2D;

    private float _force = 800f;
    private readonly float _damage = 1f;
    
    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), _destroyAfter);
    }

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        Debug.Log("Instante");
        _rigidBody2D.AddForce(transform.right * _force);
    }
    
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out GlobalPlane plane))
        {
            plane.PlaneCollider.OnDamage(_damage);
            NetworkServer.Destroy(gameObject);
        }

    }
}
