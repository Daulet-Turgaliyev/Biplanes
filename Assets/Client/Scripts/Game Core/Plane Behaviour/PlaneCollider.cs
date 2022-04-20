
    using System;
    using Mirror;
    using UnityEngine;

    public class PlaneCollider: MonoBehaviour
    {
        public Action OnBuildingEnter = () => { };
        public Action<ABullet> OnBulletEnter = (ABullet bullet) => { };
        public Action OnGroundEnter = () => { };

        private bool _isDestroy;
        
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.GetComponent<Building>())
            {
                if (_isDestroy == false)
                {
                    OnBuildingEnter?.Invoke();
                    _isDestroy = true;
                }
            }

            if (other.collider.TryGetComponent(out ABullet bullet))
            {
                OnBulletEnter(bullet);
            }

            if (other.collider.GetComponent<Ground>())
                OnGroundEnter?.Invoke();
        }

        private void OnDestroy()
        {
            OnBuildingEnter = null;
            OnGroundEnter = null;
        }
    }
