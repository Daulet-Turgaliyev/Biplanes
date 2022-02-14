using Mirror;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public sealed class PlaneSkin : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] 
    private Sprite[] planeSprite;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
    
    public void Initialize(bool hasAuthority)
    {/*
        int myId = LevelInitializer.Instance.GetSkinId();

        if (hasAuthority == false)
            myId = myId == 0 ? 1 : 0;
        
        Debug.Log($"Skin Id {myId}");
        _spriteRenderer.sprite = planeSprite[myId];*/
    }
}
