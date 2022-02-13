using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PilotBehaviour : NetworkBehaviour
{
    [SerializeField, FormerlySerializedAs("Pilot Data")]
    private PilotData _pilotData;

    [SerializeField, FormerlySerializedAs("Parachute Sprite")]
    private SpriteRenderer _parachuteSprite;
    
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    private PilotBase _pilotBase;

    private Action OnPlaneFixedUpdater = delegate { };
    private Action<bool> OnGround = b => { };

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnStartAuthority()
    {
        Initialize();
        LocalSubscribe();
    }

    private void Initialize()
    {
        _pilotBase = new PilotBase(_rigidbody2D, _pilotData, _parachuteSprite);
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
        if (other.collider.GetComponent<Ground>()) OnGround(true);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Ground>()) OnGround(false);
    }
}
