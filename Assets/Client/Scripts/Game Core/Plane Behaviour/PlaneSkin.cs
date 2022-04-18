using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public sealed class PlaneSkin : MonoBehaviour
{
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
        var myId = 0;

        _spriteRenderer.sprite = planeSprite[myId];
    }
}
