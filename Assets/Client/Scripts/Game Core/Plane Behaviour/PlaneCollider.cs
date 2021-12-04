using System;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
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
}