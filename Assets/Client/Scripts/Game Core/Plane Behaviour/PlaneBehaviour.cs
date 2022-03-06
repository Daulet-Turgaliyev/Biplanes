using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlaneBehaviour : PlayerNetworkObjectBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    [Space(3)] [Header("Plane Parts"), SerializeField]
    private PlaneWeapon _planeWeapon;

    [SerializeField]
    private PlaneCabin _planeCabin;

    [SerializeField]
    private PlaneCondition _planeCondition;

    [SerializeField]
    private PlaneSkin _planeSkin;

    [SerializeField]
    private PlaneData _planeData;

    [Space(3)] [Header("Mirror")] [SyncVar]
    private int _healPoint = 3;

    private PlaneBase _planeBase;

    public Action<PlaneBehaviour> OnDestroyPlane = delegate { };
    
    #region UnityEvents

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        _planeSkin.Initialize(hasAuthority);
    }

    private void OnDestroy()
    {
        OnFixedUpdater = null;
        OnDestroyPlane = null;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Building>())
            FastDestroyPlane();

        if (other.collider.TryGetComponent(out ABullet bullet))
        {
            if (_networkIdentity.connectionToClient.connectionId != bullet.OwnerId)
                RpcChangeCondition(bullet.Damage);
        }

        if (other.collider.GetComponent<Ground>())
        {
            /*if(ReferenceEquals(_planeBase.VelocityLimitChecker, null)) return;
                
            if (_planeBase.VelocityLimitChecker.CheckLimit())
                OnBuildingCollision2D();*/
        }
    }
    
    #endregion

    #region Methods

    protected override void Initialize()
    {
        var gameManager = GameManager.Instance;

        _planeBase = new PlaneBase(_rigidbody2D, _planeWeapon, _planeCabin, _planeData);

        gameManager.OpenGameWindow(_planeBase);
        gameManager.SetPlaneBehaviour(this);

        base.Initialize();
    }
    

    protected override void GlobalSubscribe() { }

    protected override void LocalSubscribe()
    {
        _planeCondition.OnDestroy += StartDestroyPlane;
        OnFixedUpdater += _planeBase.CustomFixedUpdate;
        _planeBase.OnFastDestroyPlane += FastDestroyPlane;
        _planeCondition.OnRespawnPlane += StartRespawnPlane;
    }
    
    private void StartDestroyPlane()
    {
        if (CanSendCommand(_networkIdentity) == false) return;
        CmdDestroyPlane();
    }
    
    private void FastDestroyPlane()
    {
        if (CanSendCommand(_networkIdentity) == false) return;
        CmdFastDestroyPlane();
    }
    
    private async void StartRespawnPlane()
    {
        if (CanSendCommand(_networkIdentity) == false) return;

        await Task.Delay(3000);

        CmdRespawnPlane();
    }

    #endregion

    #region Commands

    
    [Command]
    private void CmdFastDestroyPlane() => RpcFastDestroyPlane();
    
    [Command]
    private void CmdDestroyPlane() => OnDestroyPlane(this);
    
    [Command]
    private void CmdRespawnPlane() => GameManager.Instance.RespawnPlaneFromHuman(_networkIdentity.connectionToClient);
    
    
    #endregion  


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
}
