using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class FogWolfBuff : IAbility, IAbilityInstance, ISavableAbility
{
	public static int buffCount = 5;

	private readonly Stat.Values buff1 = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.2));

	private readonly Stat.Values buff2 = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.3));

	private readonly Stat.Values buff3 = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.3), new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.3));

	private readonly Stat.Values buff4 = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.15));

	private readonly Stat.Values buff5 = new Stat.Values(new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 50.0));

	private Character _owner;

	private Stat.Values[] _buffs;

	private int _buffIndex;

	Character IAbilityInstance.owner => _owner;

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => SavableAbilityResource.instance.fogWolfBuffIcons[_buffIndex];

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
			return _buffIndex;
		}
		set
		{
			_buffIndex = (int)value;
		}
	}

	public IAbilityInstance CreateInstance(Character owner)
	{
		_owner = owner;
		return this;
	}

	public FogWolfBuff()
	{
		_buffs = new Stat.Values[5] { buff1, buff2, buff3, buff4, buff5 };
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
		_owner.stat.AttachValues(_buffs[_buffIndex]);
	}

	void IAbilityInstance.Detach()
	{
		_owner.stat.DetachValues(_buffs[_buffIndex]);
	}
}
