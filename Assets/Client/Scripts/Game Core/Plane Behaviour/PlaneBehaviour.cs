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

    public Action<NetworkIdentity, bool> OnPlaneDie;

    #region UnityEvents

    private void Awake()
    {
        HealPoint = 4;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnDestroy() { OnFixedUpdater = null;
        OnPlaneDie = null;
    }

    private void OnUpdateHP(int oldHp, int newHp)
    {
        Debug.Log("New HP: " + newHp);
        if (newHp > 0)
        {
            _planeCondition.TrySetCondition(newHp, hasAuthority);
        }
        else
        {
            if(_networkIdentity.hasAuthority)
            {
                CmdDestroyPlane();
                GameManager.Instance.CloseCurrentWindow();
            }
            _planeCondition.DestroyAnimation();
        }
    }

    [Command]
    private void CmdDestroyPlane()
    {
        OnPlaneDie?.Invoke(_networkIdentity, true);
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

    protected override void LocalSubscribe() { OnFixedUpdater += _planeBase.CustomFixedUpdate; }

    #endregion

    #region Commands


    #endregion

}
