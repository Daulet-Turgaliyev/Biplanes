using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.IO
{
	public static class LoadingLocal
	{
		public static string LocalPath { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Setup()
		{
#if UNITY_EDITOR
			LocalPath = Application.dataPath.Replace("Assets", "Local");
#else
			LocalPath = Application.persistentDataPath;
#endif
		}
		
		public static byte[] LoadFileBytes(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Path.Combine(LocalPath, path);
			return LoadingFile.LoadFileBytes(path);
		}

		public static string LoadFileString(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Path.Combine(LocalPath, path);
			return LoadingFile.LoadFileString(path);
		}

		public static async Task<byte[]> LoadFileBytesAsync(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Path.Combine(LocalPath, path);
			return await LoadingFile.LoadFileBytesAsync(path);
		}

		public static async Task<string> LoadFileStringAsync(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Path.Combine(LocalPath, path);
			return await LoadingFile.LoadFileStringAsync(path);
		}
	}
}
