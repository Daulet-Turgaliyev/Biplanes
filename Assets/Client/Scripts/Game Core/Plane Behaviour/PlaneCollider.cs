using System;
using Mirror;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
	public Action<int> OnBulletCollision;
	
	public void BulletHit(int damage)
	{
		OnBulletCollision?.Invoke(damage);
	}

	private void OnDisable() { OnBulletCollision = null; }
}
