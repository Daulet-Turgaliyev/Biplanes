using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PilotParachute), typeof(Rigidbody2D))]
public sealed class PilotBehaviour : PlayerNetworkObjectBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;
    
    [SerializeField]
    private PilotData _pilotData;
    
    private PilotParachute _pilotParachute;

    private PilotBase _pilotBase;
    
    private Action OnDie = delegate { };

    private Action<bool> OnGround = b => { };

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

        if (other.collider.GetComponent<RespawnPoint>() == false) return;
        
        if (CanSendCommand(_networkIdentity) == true)
        {
            RespawnMyPlane();
        }
        
        OnDie();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>()) OnGround(false);
    }
    
    #endregion

    #region Methods

    protected override void Initialize()
    {
        _pilotBase = new PilotBase(_rigidbody2D, _pilotData, _pilotParachute);
        base.Initialize();
    }
    
    
    protected override void LocalSubscribe()
    {
        OnFixedUpdater += _pilotBase.CustomFixedUpdate;
        OnGround += _pilotBase.PilotMovement.ChangeGroundState;
        OnDie += StartDestroyPilot;
    }

    protected override void GlobalSubscribe() { }
    
    private void StartDestroyPilot()
    {
        if (CanSendCommand(_networkIdentity) == false) return;
        CmdDestroyPlane();
    }
    
    private void RespawnMyPlane()
    {
        CmdRespawnMyPlane();
    }
    
    #endregion

    #region Commands

    [Command]
    private void CmdDestroyPlane() => GameManager.Instance.DestroyPilot(this);

    [Command]
    private void CmdRespawnMyPlane() => GameManager.Instance.RespawnPlaneFromHuman(_networkIdentity.connectionToClient);
    

    #endregion
    
    
}
