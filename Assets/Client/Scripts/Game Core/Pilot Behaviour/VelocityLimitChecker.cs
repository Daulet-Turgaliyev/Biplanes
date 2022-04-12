
	using System;
	using UnityEngine;

	public sealed class VelocityLimitChecker: MonoBehaviour
	{
		[SerializeField]
		private  Rigidbody2D _rigidbody2D;
		private  float _velocityLimit;

		public Action OnOverLimit = () => { };
		
		public void VelocityLimitCheckerInit(float velocityLimit)
		{
			_velocityLimit = velocityLimit;
		}

		~VelocityLimitChecker()
		{
			OnOverLimit = null;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<Ground>())
			{
				Debug.Log($"{_rigidbody2D.velocity.y} < {_velocityLimit}");
				if (_rigidbody2D.velocity.y < _velocityLimit) OnOverLimit?.Invoke();
			}
		}
	}
