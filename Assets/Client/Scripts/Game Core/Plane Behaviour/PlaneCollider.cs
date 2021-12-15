using System;
using Mirror;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
	public Action<Collision2D> OnCollision;
	private void OnCollisionEnter2D(Collision2D other)
	{
		OnCollision?.Invoke(other);
	}

	private void OnDisable() { OnCollision = null; }
}
