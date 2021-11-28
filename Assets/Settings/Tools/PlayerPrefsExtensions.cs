using System;
using UnityEngine;

public static class PlayerPrefsExtensions {
	public static DateTime GetDate(string key, DateTime defaultValue = default) {
		if (PlayerPrefs.HasKey(key)) {
			long dateLong = long.Parse(PlayerPrefs.GetString(key));
			return DateTime.FromBinary(dateLong);
		}

		SetDate(key, defaultValue);
		return defaultValue;
	}
	
	public static void SetDate(string key, DateTime value) {
		PlayerPrefs.SetString(key, value.ToBinary().ToString());
	}

	public static bool GetBool(string key, bool defaultValue = false) {
		if (PlayerPrefs.HasKey(key)) {
			bool value = PlayerPrefs.GetInt(key) == 1;
			return value;
		}

		SetBool(key, defaultValue);
		return defaultValue;
	}

	public static void SetBool(string key, bool value) {
		PlayerPrefs.SetInt(key, value ? 1 : 0);
	}
}

public abstract class PrefsValue<T>
{
	public readonly string Key;
	public T Value => getter();

	private T value;

	private delegate T ValueGetter();

	private ValueGetter getter;

	public PrefsValue(string key, T defaultValue = default)
	{
		Key = key;
		getter = () => {
			value = Load(defaultValue);
			getter = () => value;
			return value;
		};
	}

	public void Set(T value)
	{
		this.value = value;
		Save();
	}

	protected abstract void Save();
	protected abstract T Load(T defaultValue = default);
}

public class PrefsString: PrefsValue<string>
{
	public PrefsString(string key, string defaultValue = default): base(key, defaultValue) { }

	protected override void Save() => PlayerPrefs.SetString(Key, Value);

	protected override string Load(string defaultValue = default) => PlayerPrefs.GetString(Key, defaultValue);
}

public class PrefsInt: PrefsValue<int>
{
	public PrefsInt(string key, int defaultValue = default): base(key, defaultValue) { }

	protected override void Save() => PlayerPrefs.SetInt(Key, Value);

	protected override int Load(int defaultValue = default) => PlayerPrefs.GetInt(Key, defaultValue);
}

public class PrefsBool: PrefsValue<bool>
{
	public PrefsBool(string key, bool defaultValue = default): base(key, defaultValue) { }

	protected override void Save() => PlayerPrefs.SetInt(Key, Value ? 1 : 0);

	protected override bool Load(bool defaultValue = default) => PlayerPrefs.GetInt(Key, defaultValue ? 1 : 0) == 1;
}

public class PrefsDateTime: PrefsValue<DateTime>
{
	public PrefsDateTime(string key, DateTime defaultValue = default): base(key, defaultValue) { }

	protected override void Save()
	{
		PlayerPrefs.SetString(Key, Value.ToBinary().ToString());
	}

	protected override DateTime Load(DateTime defaultValue = default)
	{
		if (PlayerPrefs.HasKey(Key)) {
			long dateLong = long.Parse(PlayerPrefs.GetString(Key));
			return DateTime.FromBinary(dateLong);
		}
		
		return defaultValue;
	}
}