using System.IO;
using UnityEngine;

namespace Tools.Extensions
{
	public static class FileExtensions
	{
		public static void CreateDirectoryIfNull(string path, bool getDirectoryName = false)
		{
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogWarning("Trying creating folder with empty path!");
				return;
			}

			if (getDirectoryName)
			{
				path = Path.GetDirectoryName(path);
				
				if (string.IsNullOrEmpty(path))
				{
					Debug.LogWarning($"Incorrect folder path \"{path}\"!");
					return;
				}
			}
			
			DirectoryInfo info = new DirectoryInfo(path);
			if (!info.Exists)
			{
				info.Create();
			}
		}
	}
}