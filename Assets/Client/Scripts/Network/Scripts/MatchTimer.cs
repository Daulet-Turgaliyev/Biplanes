
	using System;
	using UnityEngine;

	public class MatchTimer
	{
		private float _timeLeft;
		private bool _timerOn;

		public Action OnTimeOver;
		

		public float GetTimeLeftMinutes {
			get {
				if (_timeLeft <= 0) return 0;
				return Mathf.FloorToInt(_timeLeft / 60f);
			}
		}

		public float GetTimeLeftSeconds {
			get {
				if (_timeLeft <= 0) return 0;
				return Mathf.FloorToInt(_timeLeft % 60f);
			}
		}

		public void StartTimer(float timeToCount)
		{
			_timeLeft = timeToCount;
			_timerOn = true;
		}

		public void Update()
		{
			if (_timerOn == false) return;

			if (_timeLeft > 0)
			{
				_timeLeft -= Time.deltaTime;
				Debug.Log($"{GetTimeLeftMinutes}:{GetTimeLeftSeconds}");
			}
			else
			{
				OnTimeOver?.Invoke();
				_timerOn = false;
			}
		}
	}
