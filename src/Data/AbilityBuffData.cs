using System.Collections.Generic;
using Platforms;
using Singletons;

namespace Data;

public class AbilityBuffData
{
	public readonly string key;

	public readonly int count;

	private readonly string _defaultValue;

	private readonly IntData _count;

	private List<string> _stringDataBuffer;

	public string this[int index]
	{
		get
		{
			return _stringDataBuffer[index];
		}
		set
		{
			_stringDataBuffer[index] = value;
		}
	}

	public AbilityBuffData(string key, int count, string defaultValue)
	{
		this.key = key;
		this.count = count;
		_defaultValue = defaultValue;
		_count = new IntData(key + "/count");
		_stringDataBuffer = new List<string>(count);
		for (int i = 0; i < count; i++)
		{
			_stringDataBuffer.Add(PersistentSingleton<PlatformManager>.Instance.platform.data.GetString($"{key}/{i}", ""));
		}
	}

	public void Save()
	{
		for (int i = 0; i < count; i++)
		{
			PersistentSingleton<PlatformManager>.Instance.platform.data.SetString($"{key}/{i}", _stringDataBuffer[i]);
		}
	}

	public void Reset()
	{
		for (int i = 0; i < count; i++)
		{
			_stringDataBuffer[i] = _defaultValue;
		}
	}
}
