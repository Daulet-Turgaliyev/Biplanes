using System;
using Mirror;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
	public Action<float> OnBulletCollision;
	
	public void BulletHit(float damage)
	{
		OnBulletCollision?.Invoke(damage);
	}

	private void OnDisable() { OnBulletCollision = null; }
}
