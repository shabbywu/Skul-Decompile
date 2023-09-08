using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class HealthAuxiliarySpeed : IAbility, IAbilityInstance, ISavableAbility
{
	private readonly Stat.Values _buff = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 1.5), new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 1.5), new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 1.5));

	private Character _owner;

	private int _level;

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

	public void Refresh()
	{
	}

	void IAbilityInstance.Attach()
	{
		for (int i = 0; i < _buff.values.Length; i++)
		{
			float num = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟[_level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
			_buff.values[i].value = num;
		}
		_owner.stat.AttachValues(_buff);
	}

	void IAbilityInstance.Refresh()
	{
		for (int i = 0; i < _buff.values.Length; i++)
		{
			float num = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟[_level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
			_buff.values[i].value = num;
		}
		_owner.stat.SetNeedUpdate();
	}

	void IAbilityInstance.Detach()
	{
		_owner.stat.DetachValues(_buff);
	}
}
