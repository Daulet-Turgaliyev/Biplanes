using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PilotBehaviour : NetworkBehaviour
{
    [SerializeField, FormerlySerializedAs("Pilot Data")]
    private PilotData _pilotData;

    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    private PilotBase _pilotBase;
    
    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnStartAuthority()
    {
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("Включаем");

        _pilotBase = new PilotBase(_rigidbody2D, _pilotData);
    }
}
