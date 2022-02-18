
	using UnityEngine;

	public sealed class VelocityLimitChecker
	{
		private Rigidbody2D _rigidbody2D;
		private float _velocityLimit;
		
		public VelocityLimitChecker(Rigidbody2D rigidbody2D, float velocityLimit)
		{
			_rigidbody2D = rigidbody2D;
			_velocityLimit = velocityLimit;
		}

		public bool CheckLimit()
		{
			Debug.Log($"{_rigidbody2D.velocity.magnitude} >= {_velocityLimit} {_rigidbody2D.velocity.magnitude >= _velocityLimit}");
			if (_rigidbody2D.velocity.magnitude >= _velocityLimit) return true;
			return false;
		}
	}
