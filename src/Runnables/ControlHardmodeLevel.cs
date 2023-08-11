using Data;
using UnityEngine;

namespace Runnables;

public sealed class ControlHardmodeLevel : Runnable
{
	private enum ValueType
	{
		Current,
		Cleared
	}

	private enum Method
	{
		Add,
		Remove,
		Set
	}

	[SerializeField]
	private ValueType _valueType;

	[SerializeField]
	private Method _method;

	[SerializeField]
	private int _value;

	public override void Run()
	{
		if (CheckCondition())
		{
			switch (_method)
			{
			case Method.Add:
				Add();
				break;
			case Method.Remove:
				Remove();
				break;
			case Method.Set:
				Set();
				break;
			}
		}
	}

	private bool CheckCondition()
	{
		if (GameData.HardmodeProgress.hardmodeLevel <= GameData.HardmodeProgress.clearedLevel)
		{
			return false;
		}
		return true;
	}

	private void Add()
	{
		switch (_valueType)
		{
		case ValueType.Current:
			GameData.HardmodeProgress.hardmodeLevel = Mathf.Min(GameData.HardmodeProgress.hardmodeLevel + _value, GameData.HardmodeProgress.maxLevel);
			break;
		case ValueType.Cleared:
			GameData.HardmodeProgress.clearedLevel = Mathf.Min(GameData.HardmodeProgress.clearedLevel + _value, GameData.HardmodeProgress.maxLevel);
			GameData.HardmodeProgress.SaveAll();
			break;
		}
	}

	private void Remove()
	{
		switch (_valueType)
		{
		case ValueType.Current:
			GameData.HardmodeProgress.hardmodeLevel = Mathf.Max(GameData.HardmodeProgress.hardmodeLevel - _value, 0);
			break;
		case ValueType.Cleared:
			GameData.HardmodeProgress.clearedLevel = Mathf.Max(GameData.HardmodeProgress.clearedLevel - _value, 0);
			GameData.HardmodeProgress.SaveAll();
			break;
		}
	}

	private void Set()
	{
		switch (_valueType)
		{
		case ValueType.Current:
			GameData.HardmodeProgress.hardmodeLevel = Mathf.Clamp(_value, 0, GameData.HardmodeProgress.maxLevel);
			break;
		case ValueType.Cleared:
			GameData.HardmodeProgress.clearedLevel = Mathf.Clamp(_value, 0, GameData.HardmodeProgress.maxLevel);
			GameData.HardmodeProgress.SaveAll();
			break;
		}
	}
}
