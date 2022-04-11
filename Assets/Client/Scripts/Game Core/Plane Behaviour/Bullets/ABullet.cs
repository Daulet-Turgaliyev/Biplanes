using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABullet : NetworkBehaviour
{
    [SyncVar] 
    public int OwnerId;
    
    private readonly float _destroyAfter = 5f;
    public readonly int Damage = 1;

    private Rigidbody2D _rigidBody2D;
    
    private NetworkIdentity _networkIdentity;
    
    private readonly float _force = 800f;
    
    
    
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
		if (col.transform.TryGetComponent(out PlaneBehaviour planeBehaviour))
		{
			planeBehaviour.HealPoint -= Damage;
			Debug.Log(planeBehaviour.HealPoint);
		}
		NetworkServer.Destroy(gameObject);
	}
}
