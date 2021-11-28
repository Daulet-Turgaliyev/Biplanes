using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Tools.IO
{
	public static class LoadingRequest
	{
		private static void LoadWebRequest(string url, Action<DownloadHandler> callback)
		{
			if (string.IsNullOrEmpty(url))
			{
				callback(null);
				return;
			}
			
			var request = UnityWebRequest.Get(url);
			var operation = request.SendWebRequest();
			operation.completed += asyncOperation => {
				callback(request.downloadHandler);
				request.Dispose();
			};
		}
		
		public static async Task<byte[]> LoadFileBytesAsync(string url)
		{
			if (string.IsNullOrEmpty(url)) return null;
			bool isDone = false;
			byte[] result = null;
			LoadWebRequest(url, handler => {
				if (handler != null)
				{
					result = handler.data;
				}
				isDone = true;
			});
			
			while (!isDone)
			{
				await Task.Delay(10);
			}

			return result;
		}
		
		public static async Task<string> LoadFileStringAsync(string url)
		{
			if (string.IsNullOrEmpty(url)) return null;
			bool isDone = false;
			string result = null;
			LoadWebRequest(url, handler => {
				if (handler != null)
				{
					result = handler.text;
				}
				isDone = true;
			});
			
			while (!isDone)
			{
				await Task.Delay(10);
			}

			return result;
		}
	}
}