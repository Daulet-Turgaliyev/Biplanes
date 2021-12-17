using System;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class PlaneSkin : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] 
    private Sprite[] planeSprite;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
    public void Init(bool isLocal)
    {
        _spriteRenderer.sprite = isLocal ? planeSprite[1] : planeSprite[0];
    }
}
