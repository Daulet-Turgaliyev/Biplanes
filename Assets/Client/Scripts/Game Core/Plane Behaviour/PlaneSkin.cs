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
    
    public void Start()
    {
        int myId = 0;
        
        if (_networkIdentity.hasAuthority == true)
        {
            myId = GameManager.Instance.IsOwner ? 0 : 1;
        }
        else
        {
            myId = GameManager.Instance.IsOwner ? 1 : 0;
        }
        _spriteRenderer.sprite = planeSprite[myId];
    }
}
