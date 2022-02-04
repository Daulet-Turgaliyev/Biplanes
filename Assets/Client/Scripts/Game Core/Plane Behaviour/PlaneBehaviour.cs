using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBehaviour : NetworkBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private NetworkIdentity _networkIdentity;
        
        [FormerlySerializedAs("PlaneWeapon")]
        [Space(3)]
        [Header("Plane Parts"), SerializeField] 
        private PlaneWeapon _planeWeapon;
        
        [FormerlySerializedAs("PlaneCollider")] [SerializeField] 
        private PlaneCollider _planeCollider;

        [FormerlySerializedAs("PlaneCondition")] [SerializeField] 
        private PlaneCondition _planeCondition;

        [field: SerializeField] 
        public PlaneSkin PlaneSkin { get; private set; }
        
        [field:SerializeField]
        public PlaneData PlaneData { get; private set; }
        
        [Space(3)]
        [Header("Mirror")]
        [SyncVar]
        private int _healPoint = 3;
        
        public PlaneBase planeBase { get; private set; }

        public Action<PlaneBehaviour> OnDestroyedPlane = delegate {  };
        private Action OnPlaneFixedUpdater = delegate {  };

        private void Awake()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Start()
        {
            PlaneSkin.Initialize(hasAuthority);
            GlobalSubscribe();
        }

        [ClientRpc]
        public void Initialize()
        {
            if (_networkIdentity.hasAuthority == false)
            {
                Debug.LogWarning("НЕТ АВТОРИЗАЦИИ");
                return;
            }
            planeBase = new PlaneBase(_rigidbody2D, _planeWeapon, PlaneData);

            if (ReferenceEquals(LevelInitializer.Instance, null))
            {
                Debug.Log("Блять где Level Init?!");
                return;
            }

            LevelInitializer.Instance.StartPlane(planeBase);
            LocalSubscribe();
        }

        private void OnDisable()
        {
            OnPlaneFixedUpdater = null;
            OnDestroyedPlane = null;
        }

        private void FixedUpdate()
        {
            OnPlaneFixedUpdater?.Invoke();
        }

        private void GlobalSubscribe()
        {
            _planeCollider.OnBulletCollision += RpcChangeCondition;
        }
        
        private void LocalSubscribe()
        {
            _planeCondition.OnDie += DiePlane;
            _planeCondition.OnDestroy += StartDestroyPlane;
            OnPlaneFixedUpdater += planeBase.CustomFixedUpdate;
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

            _planeCondition.TrySetCondition(_healPoint);
        }

        private void StartDestroyPlane()
        {
            if (CanSendCommand() == false) return;
            LevelInitializer.Instance.DestroyActiveWindow();
            CmdDestroyPlane();
        }
        
        [Command]
        private void CmdDestroyPlane()
        {
            OnDestroyedPlane(this);
        }

        private bool CanSendCommand()
        {
            // != false
            return _networkIdentity.hasAuthority && isClient;
        }
        
        public void JumpOnPlane()
        {
            LevelInitializer.Instance.DestroyActiveWindow();
        
        }
    }
