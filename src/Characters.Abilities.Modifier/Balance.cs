using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.Modifier;

[Serializable]
public sealed class Balance : IStackResolver
{
	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private int _stackPerBalance;

	[SerializeField]
	private int _maxStack;

	public void Initialize()
	{
	}

	public void Attach(Character owner)
	{
	}

	public void Detach(Character owner)
	{
	}

	public int GetStack(ref Damage damage)
	{
		if (_stackPerBalance == 0)
		{
			return 0;
		}
		int num = GameData.Currency.currencies[_type].balance / _stackPerBalance;
		if (_maxStack <= 0)
		{
			return num;
		}
		return Mathf.Min(num, _maxStack);
	}

	public void UpdateTime(float deltaTime)
	{
	}
}
