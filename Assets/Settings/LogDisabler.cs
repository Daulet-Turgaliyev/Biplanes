using UnityEngine;

namespace Setup {

	internal static class LogDisabler
	{
		[RuntimeInitializeOnLoadMethod]
		private static void DisableLogs()
		{
#if !UNITY_EDITOR
			Debug.unityLogger.logEnabled = Debug.isDebugBuild;
#endif
		}
	}

}