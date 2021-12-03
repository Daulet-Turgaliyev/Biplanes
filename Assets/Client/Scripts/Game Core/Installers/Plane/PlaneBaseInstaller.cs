using UnityEngine;
using Zenject;

public class PlaneBaseInstaller : MonoInstaller
{
    [SerializeField] 
    private Rigidbody2D rigidbody2D;

    [SerializeField] 
    private SpriteRenderer spriteRenderer;

    [SerializeField] 
    private Transform collidersTransform;
    
    public override void InstallBindings()
    {
        Debug.Log("Install");
        Container.Bind<Rigidbody2D>().FromInstance(rigidbody2D).NonLazy();
        Container.Bind<SpriteRenderer>().FromInstance(spriteRenderer).NonLazy();
        Container.Bind<Transform>().FromInstance(collidersTransform).NonLazy();
    }
}