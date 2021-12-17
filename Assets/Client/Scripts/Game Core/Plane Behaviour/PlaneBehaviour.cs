using System;
using Mirror;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBehaviour : NetworkBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [Space(3)]
        [Header("Plane Part"), SerializeField] 
        private PlaneWeapon planeWeapon;
        
        [SerializeField] 
        private PlaneCollider planeCollider;

        [SerializeField] 
        private PlaneCondition planeCondition;

        [SerializeField] 
        private PlaneSkin planeSkin;
        
        [Space(3)]
        [Header("Mirror")]
        [SyncVar]
        private int _healPoint = 4;

        private PlaneBase _planeBase;
        private Action OnPlaneFixedUpdater = delegate {  };

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Start()
        {
            planeSkin.Init(isLocalPlayer);
            GlobalSubscribe();
            
            if (isLocalPlayer != true) return;
            
            _planeBase = new PlaneBase(_rigidbody2D, planeWeapon);
            LevelInitializer.Instance.planeBase = _planeBase;
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
            planeCollider.OnBulletCollision += RpcChangeCondition;
        }
        
        private void LocalSubscribe()
        {
            planeCondition.OnDie += DiePlane;
            OnPlaneFixedUpdater += _planeBase.CustomFixedUpdate;
        }

        private void DiePlane()
        {
            OnPlaneFixedUpdater = null;
        }

        [ClientRpc]
        private void RpcChangeCondition(int damage)
        {
            Debug.Log($"DAMAGE: {damage} Name: {gameObject.name} HP: {_healPoint} ");
            _healPoint -= damage;

            planeCondition.TrySetCondition(_healPoint);
        }
    }
