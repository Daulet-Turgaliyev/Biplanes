using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlaneBehaviour: NetworkBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    [Space(3)] [FormerlySerializedAs("PlaneWeapon")] [Header("Plane Parts"), SerializeField]
    private PlaneWeapon _planeWeapon;

    [FormerlySerializedAs("PlaneCabin")] [Header("Plane Parts"), SerializeField]
    private PlaneCabin _planeCabin;

    [FormerlySerializedAs("PlaneCollider")] [Header("Plane Parts"), SerializeField]
    private PlaneCollider _planeCollider;

    [FormerlySerializedAs("PlaneCondition")] [SerializeField]
    private PlaneCondition _planeCondition;

    [field: SerializeField] public PlaneSkin PlaneSkin { get; private set; }

    [field: SerializeField] public PlaneData PlaneData { get; private set; }

    [Space(3)] [Header("Mirror")] [SyncVar]
    private int _healPoint = 3;

    public PlaneBase planeBase { get; private set; }

    public Action<PlaneBehaviour> OnDestroyPlane = delegate {};

    private Action OnPlaneFixedUpdater = delegate { };

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnStartAuthority() { Initialize(); }

    public void Start()
    {
        PlaneSkin.Initialize(hasAuthority);
    }

    public void Initialize()
    {
        GameManager gameManager = GameManager.Instance;

        planeBase = new PlaneBase(_rigidbody2D, _planeWeapon, _planeCabin, PlaneData);

        gameManager.OpenGameWindow(planeBase);
        gameManager.SetPlaneBehaviour(this);
        
        LocalSubscribe();
    }

    private void OnDisable()
    {
        OnPlaneFixedUpdater = null;
        OnDestroyPlane = null;
    }

    private void FixedUpdate() { OnPlaneFixedUpdater?.Invoke(); }

    private void LocalSubscribe()
    {
        _planeCondition.OnDestroy += StartDestroyPlane;
        OnPlaneFixedUpdater += planeBase.CustomFixedUpdate;
        planeBase.OnFastDestroyPlane += FastDestroyPlane;
        _planeCondition.OnRespawnPlane += StartRespawnPlane;
    }
    
    private void OnDestroy() { OnPlaneFixedUpdater = null; }

    private void FastDestroyPlane()
    {
        if (CanSendCommand() == false) return;
        CmdFastDestroyPlane();
    }
    
    [Command]
    private void CmdFastDestroyPlane()
    {
        RpcFastDestroyPlane();
    }
    
    [ClientRpc]
    private void RpcFastDestroyPlane()
    {
        _healPoint = 0;
        _planeCondition.DiePlane(_networkIdentity.hasAuthority, true);
    }

    [ClientRpc]
    private void RpcChangeCondition(int damage)
    {
        Debug.Log($"DAMAGE: {damage} Name: {gameObject.name} HP: {_healPoint} ");
        _healPoint -= damage;

        _planeCondition.TrySetCondition(_healPoint, _networkIdentity.hasAuthority);
    }

    private void StartDestroyPlane()
    {
        if (CanSendCommand() == false) return;
        CmdDestroyPlane();
    }
    
    [Command]
    private void CmdDestroyPlane() { OnDestroyPlane(this); }

    private async void StartRespawnPlane()
    {
        if (CanSendCommand() == false) return;

        await Task.Delay(3000);
        
        CmdRespawnPlane();
    }
    
    [Command]
    private void CmdRespawnPlane() { GameManager.Instance.RespawnPlaneFromHuman(_networkIdentity.connectionToClient); }
    
    private bool CanSendCommand()
    {
        // != false
        return _networkIdentity.hasAuthority && isClient;
    }
    
    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Building>())
            FastDestroyPlane();

        if (other.collider.TryGetComponent(out ABullet bullet))
        {
            if(_networkIdentity.connectionToClient.connectionId != bullet.ownerId)
                RpcChangeCondition(bullet.Damage);
        }
        
        if (other.collider.GetComponent<Ground>())
        {
            /*if(ReferenceEquals(_planeBase.VelocityLimitChecker, null)) return;
                
            if (_planeBase.VelocityLimitChecker.CheckLimit())
                OnBuildingCollision2D();*/
        }
    }
}
