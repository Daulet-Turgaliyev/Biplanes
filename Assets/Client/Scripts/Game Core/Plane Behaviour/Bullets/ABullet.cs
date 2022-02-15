using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABullet : NetworkBehaviour
{
    private readonly float _destroyAfter = 5f;
    private Rigidbody2D _rigidBody2D;

    private float _force = 800f;
    public int Damage = 1;

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
      /*  if (col.collider.TryGetComponent(out PlaneBehaviour planeBehaviour))
            planeBehaviour.RpcChangeCondition(Damage);
*/
        NetworkServer.Destroy(gameObject);
    }
}
