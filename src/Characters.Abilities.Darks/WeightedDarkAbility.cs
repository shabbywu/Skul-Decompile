using System;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Darks;

public sealed class WeightedDarkAbility : MonoBehaviour
{
	[Serializable]
	public class Subcomponents : SubcomponentArray<WeightedDarkAbility>
	{
	}

	[SerializeField]
	private string _label;

	[Range(0f, 100f)]
	[SerializeField]
	private int _value;

	[MinMaxSlider(0f, 10f)]
	[SerializeField]
	private Vector2Int _appearanceLevel;

	[SerializeField]
	private DarkAbility _abilityComponent;

	public DarkAbility key => _abilityComponent;

	public int value => _value;

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(_label))
		{
			return _label;
		}
		return ((Object)this).ToString();
	}

	public void Initialize()
	{
		_abilityComponent.Initialize();
	}

	public void ResetValue()
	{
		_value = 0;
	}

	public bool Available(Character owner)
	{
		if (Singleton<HardmodeManager>.Instance.currentLevel < ((Vector2Int)(ref _appearanceLevel)).x || Singleton<HardmodeManager>.Instance.currentLevel > ((Vector2Int)(ref _appearanceLevel)).y)
		{
			return false;
		}
		return key.Available(owner);
	}
}
