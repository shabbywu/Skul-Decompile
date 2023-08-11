using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class LifeChange : IAbility, IAbilityInstance, ISavableAbility
{
	private readonly Stat.Values _buff = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.03999999910593033), new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.03999999910593033));

	private Character _owner;

	private const int maxstack = 75;

	private int _stack;

	private Stat.Values _buffClone;

	Character IAbilityInstance.owner => _owner;

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => SavableAbilityResource.instance.fogWolfBuffIcons[0];

	public float iconFillAmount => 0f;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => (int)stack;

	public bool expired => false;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public float stack
	{
		get
		{
			return _stack;
		}
		set
		{
			_stack = Mathf.Min((int)value, 75);
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
		_buffClone = _buff.Clone();
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_buff).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_buffClone).values[i].value = ((ReorderableArray<Stat.Value>)_buff).values[i].GetStackedValue(stack);
		}
		_owner.stat.AttachValues(_buffClone);
	}

	void IAbilityInstance.Refresh()
	{
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_buff).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_buffClone).values[i].value = ((ReorderableArray<Stat.Value>)_buff).values[i].GetStackedValue(stack);
		}
		_owner.stat.SetNeedUpdate();
	}

	void IAbilityInstance.Detach()
	{
		_owner.stat.DetachValues(_buff);
	}
}
