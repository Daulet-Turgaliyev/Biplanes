using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PilotParachute), typeof(Rigidbody2D))]
public sealed class PilotBehaviour : NetworkBehaviour
{
    [SerializeField, FormerlySerializedAs("Pilot Data")]
    private PilotData _pilotData;
    
    private PilotParachute _pilotParachute;
    
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    private PilotBase _pilotBase;

    private Action OnPlaneFixedUpdater = delegate { };
    
    private Action OnDie = delegate { };

    private Action<bool> OnGround = b => { };

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _pilotParachute = GetComponent<PilotParachute>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnStartAuthority()
    {
        Initialize();
    }

    public void Initialize()
    {
        _pilotBase = new PilotBase(_rigidbody2D, _pilotData, _pilotParachute);
        
        LocalSubscribe();
    }

    private void FixedUpdate()
    {
        OnPlaneFixedUpdater?.Invoke();
    }

    private void OnDestroy()
    {
        OnPlaneFixedUpdater = null;
    }

    private void GlobalSubscribe()
    {
    }
    
    private void StartDestroyPilot()
    {
        if (CanSendCommand() == false) return;
        CmdDestroyPlane();
    }

    private bool CanSendCommand()
    {
        // != false
        return _networkIdentity.hasAuthority && isClient;
    }

    [Command]
    private void CmdDestroyPlane() { GameManager.Instance.DestroyPilot(this); }


    private void RespawnMyPlane()
    {
        CmdRespawnMyPlane();
    }
    
    [Command]
    private void CmdRespawnMyPlane()
    {
        GameManager.Instance.RespawnPlaneFromHuman(_networkIdentity.connectionToClient);
    }
    
    private void LocalSubscribe()
    {
        OnPlaneFixedUpdater += _pilotBase.CustomFixedUpdate;
        OnGround += _pilotBase.PilotMovement.ChangeGroundState;
        OnDie += StartDestroyPilot;
    }
    
    [ClientCallback]
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>())
        {
            _pilotBase?.OnCloseParachute();
            OnGround(true);
        }

        if (other.collider.TryGetComponent(out RespawnPoint respawnPoint))
        {
            if (CanSendCommand() == true)
            {
                RespawnMyPlane();
            }
            OnDie();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>()) OnGround(false);
    }
}
