using UnityEngine;

namespace Setup {

	internal static class FPSSetup
	{
		[RuntimeInitializeOnLoadMethod]
		private static void Setup()
		{
			// maximize frame rate
			Application.targetFrameRate = 300;
		}
	}

}
