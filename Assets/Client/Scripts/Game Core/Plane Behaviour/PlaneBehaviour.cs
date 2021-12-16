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
            GlobalSubscribe();
            
            if (isLocalPlayer != true) return;
            
            PlaneBase = new PlaneBase(rigidbody2D, planeWeapon);
            LevelInitializer.Instance.planeBase = PlaneBase;
            OnDie += () => Debug.Log("Die");
            LocalSubscribe();
        }

        private void OnDisable()
        {
            OnPlaneFixedUpdater = null;
        }

        private void FixedUpdate()
        {
            OnPlaneFixedUpdater?.Invoke();
        }

        private void GlobalSubscribe()
        {
            planeCollider.OnBulletCollision += DealDamage;
        }
        
        private void LocalSubscribe()
        {
            OnPlaneFixedUpdater += PlaneBase.CustomFixedUpdate;
        }

        private void DealDamage(float damage)
        {
            Debug.Log($"DAMAGE: {damage} Name: {gameObject.name} HP: {_healPoint} ");
            _healPoint -= damage;
            
            if (_healPoint <= 0f)
                OnDie?.Invoke();
        }
    }
