
	using System;
	using UnityEngine;

	public class ParachuteDamageble: MonoBehaviour
	{
		public Action OnParachuteHit = () => { };
		private void OnDestroy()
		{
			OnParachuteHit = null;
		}
	}
