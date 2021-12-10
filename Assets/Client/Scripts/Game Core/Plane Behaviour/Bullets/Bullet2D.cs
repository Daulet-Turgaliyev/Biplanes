using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet2D : NetworkBehaviour
{
    private readonly float _destroyAfter = 5f;
    private Rigidbody2D _rigidBody2D;

    private float _force = 800f;
    public readonly float Damage = 1f;

    private void Awake() => _rigidBody2D = GetComponent<Rigidbody2D>();

    public void Start() => _rigidBody2D.AddForce(transform.right * _force);
    
    public override void OnStartServer() => Invoke(nameof(ServerDestroySelf), _destroyAfter);
    
    [Server]
    private void ServerDestroySelf() =>  NetworkServer.Destroy(gameObject);

    public void NetworkDestroySelf() => NetworkServer.Destroy(gameObject);
}
