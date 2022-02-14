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
    
    private void LocalSubscribe()
    {
        OnPlaneFixedUpdater += _pilotBase.CustomFixedUpdate;
        OnGround += _pilotBase.PilotMovement.ChangeGroundState;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>())
        {
            _pilotBase?.OnCloseParachute();
            OnGround(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>()) OnGround(false);
        
    }
}
