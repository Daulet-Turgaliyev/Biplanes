using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudMove: MonoBehaviour
{
	[SerializeField]
	private float _speed;

	private void Start()
	{
		_speed = Random.Range(.01f, .03f);
	}

	private void FixedUpdate()
	{
		transform.Translate(Vector3.left * _speed);
	}
}
