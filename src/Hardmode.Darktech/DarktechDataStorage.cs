using System;
using System.Collections.Generic;
using Data;

namespace Hardmode.Darktech;

public sealed class DarktechDataStorage
{
	private List<DarktechData.Type> _types;

	public DarktechDataStorage()
	{
		Array values = Enum.GetValues(typeof(DarktechData.Type));
		_types = new List<DarktechData.Type>(values.Length);
		foreach (DarktechData.Type item in values)
		{
			_types.Add(item);
		}
	}

	public void LockAll()
	{
		Array values = Enum.GetValues(typeof(DarktechData.Type));
		foreach (DarktechData.Type item in values)
		{
			GameData.HardmodeProgress.unlocked.SetData(item, value: false);
		}
		foreach (DarktechData.Type item2 in values)
		{
			GameData.HardmodeProgress.activated.SetData(item2, value: false);
		}
	}

	public void Lock(DarktechData.Type type)
	{
		GameData.HardmodeProgress.unlocked.SetData(type, value: false);
		GameData.HardmodeProgress.activated.SetData(type, value: false);
	}

	public void UnlockAll()
	{
		foreach (DarktechData.Type type in _types)
		{
			Unlock(type);
		}
	}

	public void ActivateAll()
	{
		foreach (DarktechData.Type type in _types)
		{
			Activate(type);
		}
	}

	public void Unlock(DarktechData.Type type)
	{
		if (!GameData.HardmodeProgress.unlocked.GetData(type))
		{
			GameData.HardmodeProgress.unlocked.SetData(type, value: true);
		}
	}

	public void Activate(DarktechData.Type type)
	{
		if (!GameData.HardmodeProgress.activated.GetData(type))
		{
			GameData.HardmodeProgress.activated.SetData(type, value: true);
		}
	}

	public bool IsActivated(DarktechData.Type type)
	{
		return GameData.HardmodeProgress.activated.GetData(type);
	}

	public bool IsUnlocked(DarktechData.Type type)
	{
		return GameData.HardmodeProgress.unlocked.GetData(type);
	}
}
