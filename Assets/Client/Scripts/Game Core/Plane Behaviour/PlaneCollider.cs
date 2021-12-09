using System;
using Mirror;
using UnityEngine;


public class PlaneCollider : MonoBehaviour
{
    [SerializeField] 
    private new Rigidbody2D rigidbody2D;

    [SerializeField] 
    private NetworkIdentity networkIdentity;

    public Action<float> OnDamage;

    private void Start()
    {
        if (networkIdentity.isLocalPlayer == false)
            gameObject.AddComponent<GlobalPlane>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.TryGetComponent(out UpperBorder _))
        {
            rigidbody2D.rotation += 1f;
        }
    }

    private void OnDisable()
    {
        OnDamage = null;
    }
}
