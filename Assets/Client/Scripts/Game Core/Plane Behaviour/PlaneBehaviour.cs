using System;
using Mirror;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBehaviour : NetworkBehaviour
    {
        [SerializeField] 
        private new Rigidbody2D rigidbody2D;

        [SerializeField] 
        private PlaneWeapon planeWeapon;
        
        [SerializeField] 
        private PlaneCollider planeCollider;
        
        [SyncVar]
        private float _healPoint = 4f;
        
        public PlaneBase PlaneBase { get; private set; }
        
        public Action OnPlaneFixedUpdater = delegate {  };

        public Action OnDie;
        
        public void Start()
        {
            if (isLocalPlayer == true)
            {
                PlaneBase = new PlaneBase(rigidbody2D, planeWeapon);
                LevelInitializer.Instance.planeBase = PlaneBase;
                OnDie += () => Debug.Log("Die");
                Subscribe();
                return;
            }

            Destroy(this);
        }

        private void OnDisable()
        {
            OnPlaneFixedUpdater = null;
        }

        private void FixedUpdate()
        {
            OnPlaneFixedUpdater?.Invoke();
        }

        private void Subscribe()
        {
            OnPlaneFixedUpdater += PlaneBase.CustomFixedUpdate;
            planeCollider.OnCollision += ColiisionAction;
        }

        private void ColiisionAction(Collision2D col)
        {
            col.transform.TryGetComponent(out ABullet bullet);
            
            if(ReferenceEquals(bullet, null)) return;
            DealDamage(bullet);
        }
        
        private void DealDamage(ABullet bullet)
        {
            Debug.Log("DAMAGE");
            _healPoint -= bullet.Damage;
            
            if (_healPoint <= 0f)
                OnDie?.Invoke();
        }
    }
