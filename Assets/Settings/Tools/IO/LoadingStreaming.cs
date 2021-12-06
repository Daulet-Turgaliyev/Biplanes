using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.IO
{
	public static class LoadingStreaming
	{
		public static readonly string StreamingPath = Application.streamingAssetsPath;
		
		public static async Task<byte[]> LoadFileBytes(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Path.Combine(StreamingPath, path);
#if UNITY_EDITOR || UNITY_IOS
			return await LoadingFile.LoadFileBytesAsync(path);
#elif UNITY_ANDROID
			return await LoadingRequest.LoadFileBytesAsync(path);
#endif
			return null;
		}
		
		public static async Task<string> LoadFileString(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Path.Combine(StreamingPath, path);
#if UNITY_EDITOR || UNITY_IOS
			return await LoadingFile.LoadFileStringAsync(path);
#elif UNITY_ANDROID
			return await LoadingRequest.LoadFileStringAsync(path);
#endif
			return null;
		}
	}
}