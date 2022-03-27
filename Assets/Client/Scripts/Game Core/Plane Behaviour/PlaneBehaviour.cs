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
    private PlaneCollider _planeCollider;
    
    [SerializeField]
    private PlaneData _planeData;

    [Space(3)] [Header("Mirror")] [SyncVar]
    private int _healPoint = 3;

    private PlaneBase _planeBase;

    #region UnityEvents

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        OnFixedUpdater = null;
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
        OnFixedUpdater += _planeBase.CustomFixedUpdate;
        
        _planeCabin.OnJumped += CmdDestroyPlane;
        _planeCollider.OnBuildingEnter += CmdGetFatalDamage;
        _planeCollider.OnBulletEnter += CmdGetDamage;
        
        _planeCondition.OnDestroy += CmdDestroyPlane;
        _planeCondition.OnRespawnPlane += CmdRespawnPlane;
    }
    

    #endregion

    #region Commands
    
    [Command(requiresAuthority = false)]
    private void CmdGetDamage(ABullet bullet)
    {
        MatchController.Instance.GetDamage(this, bullet.Damage);
    }
    
    [Command(requiresAuthority = false)]
    private void CmdGetFatalDamage()
    {
        if(_planeCabin.HasPilotInCabin == false) return;
        MatchController.Instance.GetFatalDamage(this);
    }

    [Command(requiresAuthority = false)]
    private async void CmdRespawnPlane()
    {
        await MatchController.Instance.RespawnPlane(_networkIdentity, 5);
    }

    [Command(requiresAuthority = false)]
    private async void CmdDestroyPlane(int destroyTime)
    {
        await MatchController.Instance.NetworkDestroy(gameObject, destroyTime);
    }
    
    #endregion  

    

    [ClientRpc]
    public void RpcFastDestroyPlane()
    {
        _healPoint = 0;
        _planeCondition.TrySetCondition(0, _networkIdentity.hasAuthority);
    }

    [ClientRpc]
    public void RpcChangeCondition(int damage)
    {
        Debug.Log($"DAMAGE: {damage} Name: {gameObject.name} HP: {_healPoint} ");
        _healPoint -= damage;

        _planeCondition.TrySetCondition(_healPoint, _networkIdentity.hasAuthority);
    }
}
