using Mirror;
using UnityEngine;

[RequireComponent(typeof(PilotParachute), typeof(Rigidbody2D))]
public sealed class PilotBehaviour : PlayerNetworkObjectBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    [SerializeField]
    private PilotData _pilotData;
    
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    
    private PilotParachute _pilotParachute;
    
    [SerializeField]
    private ParachuteDamageble _parachuteDamageble;
    
    private PilotBase _pilotBase;

    [SyncVar(hook = nameof(OnUpdateFlipX))] 
    public bool PilotDirection;
    
    [SyncVar] 
    public Vector2 Velocity;
    
    [SerializeField]
    private VelocityLimitChecker _velocityLimitChecker;
    
    #region Unity Events

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _pilotParachute = GetComponent<PilotParachute>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    [ClientCallback]
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>())
        {
            _pilotBase?.OnCloseParachute();
            OnGround(true);
        }
    }

    [ClientCallback]
    private void Update()
    {
        if (hasAuthority == false) return;
        Velocity = _rigidbody2D.velocity;
        CmdSendCurrentVelocity(Velocity);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>()) OnGround(false);
    }

    private void OnUpdateFlipX(bool oldFlipX, bool newFlipX)
    {
        _spriteRenderer.flipX = newFlipX;
    }
    
    #endregion

    #region Methods

    protected override void Initialize()
    {
        _pilotBase = new PilotBase(_rigidbody2D, _pilotData, _pilotParachute);
        _velocityLimitChecker.VelocityLimitCheckerInit(_pilotData.FallSpeedLimit.y);
        _velocityLimitChecker.OnOverLimit += FallingDie;
        base.Initialize();
    }

    protected override void LocalSubscribe()
    {
        OnFixedUpdater += _pilotBase.CustomFixedUpdate;
        OnGround += _pilotBase.PilotMovement.ChangeGroundState;
        _parachuteDamageble.OnParachuteHit += _pilotBase?.OnCloseParachute;
        _pilotBase.OnUpdateFlipX += CmdChangeFlipDirection;

    }

    protected override void GlobalSubscribe()
    {
    }
    

    #endregion

    #region Commands
    
    public void KillPilot()
    {
        RpcCallDieAnimation();
        GameManager.Instance.CloseCurrentWindow();
        OnDie?.Invoke(_networkIdentity, true, true, 1, 4);
    }

    [Command]
    private void FallingDie()
    {
        RpcCallDieAnimation();
        OnDie?.Invoke(_networkIdentity, true, true, 1, 3);
    }
    
    [Command]
    public void CmdRespawnPlane()
    {
        OnDie?.Invoke(_networkIdentity, true, false, 0, 0);
    }
    
    [Command]
    private void CmdChangeFlipDirection(bool newFlipX)
    {
        PilotDirection = newFlipX;
    }

    [Command]
    private void CmdSendCurrentVelocity(Vector2 newVelocity) => Velocity = newVelocity;
    

    [ClientRpc]
    private void RpcCallDieAnimation()
    {
        GetComponentInChildren<Animator>().Play("vanish");
    }

    #endregion
    
    
}
