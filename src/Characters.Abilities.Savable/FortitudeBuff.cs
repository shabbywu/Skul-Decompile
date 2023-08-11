using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class FortitudeBuff : IAbility, IAbilityInstance, ISavableAbility
{
	private readonly Stat.Values _buff = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.10000000149011612), new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.10000000149011612));

	private Stat.Values _attached;

	private Character _owner;

	private int _level = 1;

	Character IAbilityInstance.owner => _owner;

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => Singleton<DarktechManager>.Instance.resource.fortitudeBuffIcon;

	public float iconFillAmount => 0f;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => _level;

	public bool expired => false;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public float stack
	{
		get
		{
			return _level;
		}
		set
		{
			_level = (int)value;
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

	void IAbilityInstance.Attach()
	{
		_attached = _buff.Clone();
		remainTime = Singleton<DarktechManager>.Instance.setting.품목순환장치버프맵카운트;
		_owner.stat.AttachValues(_attached);
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_attached).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_attached).values[i].value = ((ReorderableArray<Stat.Value>)_buff).values[i].GetStackedValue(stack);
		}
	}

	void IAbilityInstance.Refresh()
	{
		remainTime = Singleton<DarktechManager>.Instance.setting.품목순환장치버프맵카운트;
		stack++;
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_attached).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_attached).values[i].value = ((ReorderableArray<Stat.Value>)_buff).values[i].GetStackedValue(stack);
		}
		_owner.stat.SetNeedUpdate();
	}

	void IAbilityInstance.Detach()
	{
		_owner.stat.DetachValues(_buff);
	}
}
