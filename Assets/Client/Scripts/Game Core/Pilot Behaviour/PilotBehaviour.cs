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
    public Action<PilotBehaviour> OnDestroyedPilot = delegate {};

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
    private void CmdDestroyPlane() { OnDestroyedPilot(this); }
    
    private void LocalSubscribe()
    {
        OnPlaneFixedUpdater += _pilotBase.CustomFixedUpdate;
        OnGround += _pilotBase.PilotMovement.ChangeGroundState;
        OnDie += StartDestroyPilot;
        OnDestroyedPilot += GameManager.Instance.DestroyPilot;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>())
        {
            _pilotBase?.OnCloseParachute();
            OnGround(true);
        }

        if (other.collider.GetComponent<RespawnPoint>())
        {
            OnDie();
            GameManager.Instance.RespawnPlaneFromHuman(_networkIdentity.connectionToClient);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>()) OnGround(false);
    }
}
