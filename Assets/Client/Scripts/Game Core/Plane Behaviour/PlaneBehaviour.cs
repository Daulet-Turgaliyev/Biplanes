using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlaneBehaviour: PlayerNetworkObjectBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [Space(3)] [Header("Plane Parts"), SerializeField]
    private PlaneWeapon _planeWeapon;

    [SerializeField] NetworkIdentity _networkIdentity;

    [SerializeField] private PlaneCabin _planeCabin;

    [SerializeField] private PlaneCondition _planeCondition;

    [SerializeField] private PlaneSkin _planeSkin;

    [SerializeField] private PlaneCollider _planeCollider;

    [SerializeField] private PlaneData _planeData;

    [Space(3)] [Header("Mirror")] [SyncVar(hook = nameof(OnUpdateHP))]
    public int HealPoint;
    

    private PlaneBase _planeBase;

    #region UnityEvents

    private void Awake()
    {
        HealPoint = 4;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnDestroy() { OnFixedUpdater = null; }

    private void OnUpdateHP(int oldHp, int newHp)
    {
        Debug.Log("New HP: " + newHp);
        if (newHp > 0)
        {
            _planeCondition.TrySetCondition(newHp, hasAuthority);
        }
        else
        {
            DestroyPlane();
        }
    }

    public void DestroyPlane(int destroyTimer = 0)
    {
        if(_networkIdentity.hasAuthority)
            CmdDelayedDestroyPlane(destroyTimer);
            
        _planeCondition.DestroyAnimation();
    }
    
    [Command]
    private async void CmdDelayedDestroyPlane(int destroyDelay)
    {
        int destroyMillisecondsDelay = destroyDelay * 1000;
        await Task.Delay(destroyMillisecondsDelay);
        OnDie?.Invoke(_networkIdentity, _planeCabin.HasPilotInCabin, _planeCabin.HasPilotInCabin, 2);
    }

    #endregion

    #region Methods

    protected override void Initialize()
    {
        _planeBase = new PlaneBase(_rigidbody2D, _planeWeapon, _planeCabin, _planeData);

        GameManager.Instance.OpenGameWindow(_planeBase);

        base.Initialize();
    }


    protected override void GlobalSubscribe() { }

    protected override void LocalSubscribe()
    {
        OnFixedUpdater += _planeBase.CustomFixedUpdate;
        _planeCabin.OnJumped += DestroyPlane;
    }

    #endregion

    #region Commands


    
    #endregion

}
