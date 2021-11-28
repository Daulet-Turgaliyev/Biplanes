using System.IO;
using System.Threading.Tasks;

namespace Tools.IO
{
	public static class LoadingFile
	{
		public static byte[] LoadFileBytes(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			if (File.Exists(path) == false) return null;
			return File.ReadAllBytes(path);
		}

		public static string LoadFileString(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			if (File.Exists(path) == false) return null;
			return File.ReadAllText(path);
		}

		public static async Task<byte[]> LoadFileBytesAsync(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			byte[] result = null;
			await Task.Factory.StartNew(() => result = LoadFileBytes(path));
			return result;
		}

		public static async Task<string> LoadFileStringAsync(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			string result = null;
			await Task.Factory.StartNew(() => result = LoadFileString(path));
			return result;
		}
	}
}