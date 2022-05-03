
	using System;
	using UnityEngine;

	public class MatchTimer
	{
		public float TimeLeft { get; private set; }
		private bool _timerOn;

		public Action OnUpdatedTimer;
		public Action OnTimeOver;
		

		public float GetTimeLeftMinutes {
			get {
				if (TimeLeft <= 0) return 0;
				return Mathf.FloorToInt(TimeLeft / 60f);
			}
		}

		public float GetTimeLeftSeconds {
			get {
				if (TimeLeft <= 0) return 0;
				return Mathf.FloorToInt(TimeLeft % 60f);
			}
		}

		public void StartTimer(float timeToCount)
		{
			TimeLeft = timeToCount;
			_timerOn = true;
		}

		public void Update()
		{
			if (_timerOn == false) return;

			if (TimeLeft > 0)
			{
				TimeLeft -= Time.deltaTime;
				OnUpdatedTimer?.Invoke();
			}
			else
			{
				OnTimeOver?.Invoke();
				_timerOn = false;
			}
		}

		~MatchTimer()
		{
			OnUpdatedTimer = null;
			OnTimeOver = null;
		}
	}
