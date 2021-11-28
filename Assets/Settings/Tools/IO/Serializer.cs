using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Tools.IO
{
	/// <summary>
	/// Exceptions can be.
	/// </summary>
	public static class Serializer
	{
		private static BinaryFormatter _formatter;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Setup() { _formatter = new BinaryFormatter(); }

		public static void Serialize<T>(Stream stream, T t) { _formatter.Serialize(stream, t); }

		public static T Deserialize<T>(Stream stream)
		{
			T result = (T)_formatter.Deserialize(stream);
			return result;
		}

		public static void Serialize<T>(string path, T t)
		{
			var stream = File.OpenWrite(path);
			try
			{
				Serialize(stream, t);
			}
			finally
			{
				stream.Close();
			}
		}

		public static T Deserialize<T>(string path, T _default = default)
		{
			T result = _default;

			if (File.Exists(path))
			{
				var stream = File.OpenRead(path);
				try
				{
					result = Deserialize<T>(stream);
				}
				finally
				{
					stream.Close();
				}
			}

			return result;
		}
	}
}