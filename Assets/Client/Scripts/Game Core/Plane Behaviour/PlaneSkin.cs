using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public sealed class PlaneSkin : MonoBehaviour
{
    [SerializeField] 
    private NetworkIdentity _networkIdentity;
    
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField] 
    private Sprite[] planeSprite;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    } 

    private void Start()
    {
        EnableRenderer();
    }

    private async void EnableRenderer()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(ReferenceEquals(_networkIdentity, null)) return;
        
        int myId = 0;
        
        if (_networkIdentity.hasAuthority == true)
            myId = 1;
            
        _spriteRenderer.sprite = planeSprite[myId];
    }
}
