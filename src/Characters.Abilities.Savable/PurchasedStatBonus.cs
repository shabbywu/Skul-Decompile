using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class PurchasedStatBonus : IAbility, IAbilityInstance, ISavableAbility
{
	private readonly Stat.Values _buff = new Stat.Values(new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 0.0));

	private Character _owner;

	private static float _currentValue;

	Character IAbilityInstance.owner => _owner;

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => null;

	public float iconFillAmount => 0f;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => false;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public float stack
	{
		get
		{
			return _currentValue;
		}
		set
		{
			if (_currentValue == 0f)
			{
				_currentValue = value;
			}
			else
			{
				_currentValue += value;
			}
		}
	}

	public IAbilityInstance CreateInstance(Character owner)
	{
		_owner = owner;
		return this;
	}

	public void Initialize()
	{
	}

	public void UpdateTime(float deltaTime)
	{
	}

	public void Refresh()
	{
	}

	void IAbilityInstance.Attach()
	{
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_buff).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_buff).values[i].value = _currentValue;
		}
		_owner.stat.AttachValues(_buff);
	}

	void IAbilityInstance.Refresh()
	{
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_buff).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_buff).values[i].value = _currentValue;
		}
		_owner.stat.SetNeedUpdate();
	}

	void IAbilityInstance.Detach()
	{
		_currentValue = 0f;
		_owner.stat.DetachValues(_buff);
	}
}
