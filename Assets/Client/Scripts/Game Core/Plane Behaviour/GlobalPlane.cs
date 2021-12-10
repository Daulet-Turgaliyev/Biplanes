using System;
using UnityEngine;

[RequireComponent(typeof(PlaneCollider))]
    public class GlobalPlane : MonoBehaviour
    {
        public Action<float> OnDealDamage;
        public PlaneCollider PlaneCollider { get; private set; }
        private void Awake() => PlaneCollider = GetComponent<PlaneCollider>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet2D bullet))
            {
                /*OnDealDamage?.Invoke(bullet.Damage);
                bullet.NetworkDestroySelf();*/
            }
        }
    }