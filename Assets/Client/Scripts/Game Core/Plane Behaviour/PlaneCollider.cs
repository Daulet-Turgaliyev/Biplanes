using UnityEngine;


public class PlaneCollider : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.TryGetComponent(out UpperBorder _))
        {
            rigidbody2D.rotation += 1f;
        }
    }
}
