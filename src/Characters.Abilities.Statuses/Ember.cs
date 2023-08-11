using System.Collections.Generic;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Statuses;

public sealed class Ember : IAbility, IAbilityInstance
{
	private const float _tickInterval = 0.33f;

	private static readonly ColorBlend _colorBlend = new ColorBlend(100, new Color(8f / 15f, 2f / 15f, 0f, 1f), 0f);

	private const string _floatingTextKey = "floating/status/burn";

	private const string _floatingTextColor = "#DD4900";

	private const Damage.Attribute _damageAttribute = Damage.Attribute.Magic;

	private static Stat.Values _stat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.100000023841858));

	private readonly List<Character> _attackers = new List<Character>();

	private readonly List<double> _damages = new List<double>();

	private readonly List<float> _remainTimes = new List<float>();

	private readonly double _damageMultiplier;

	private float _remainTimeToNextTick;

	private double _currentDamage;

	public Character owner { get; private set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => null;

	public float iconFillAmount => remainTime / duration;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => remainTime <= 0f;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Ember(Character owner)
	{
		this.owner = owner;
		_damageMultiplier = 1.0;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
		_remainTimeToNextTick -= deltaTime;
		if (_remainTimeToNextTick <= 0f)
		{
			_remainTimeToNextTick += 0.33f;
			GiveDamage();
		}
		bool flag = false;
		for (int num = _remainTimes.Count - 1; num >= 0; num--)
		{
			if ((_remainTimes[num] -= deltaTime) <= 0f)
			{
				_damages.RemoveAt(num);
				_remainTimes.RemoveAt(num);
				flag = true;
			}
		}
		if (flag)
		{
			UpdateDamage();
		}
	}

	private void GiveDamage()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Character character = _attackers[_attackers.Count - 1];
		if (!((Object)(object)character == (Object)null))
		{
			Damage damage = new Damage(character, _currentDamage * _damageMultiplier, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), Damage.Attribute.Magic, Damage.AttackType.Additional, Damage.MotionType.Status, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: true, @null: false, 0.0, 0.0, 0);
			character.Attack(owner, ref damage);
		}
	}

	private void UpdateDamage()
	{
		_currentDamage = 0.0;
		for (int i = 0; i < _damages.Count; i++)
		{
			double num = _damages[i];
			if (_currentDamage < num)
			{
				_currentDamage = num;
			}
		}
	}

	public void Add(Character attacker, float duration)
	{
		if (remainTime < duration)
		{
			remainTime = duration;
			this.duration = duration;
		}
		_attackers.Add(attacker);
		_damages.Add(3.3000001907348633);
		_remainTimes.Add(duration);
		UpdateDamage();
	}

	public void Refresh()
	{
	}

	public void Attach()
	{
		remainTime = duration;
		_remainTimeToNextTick = 0f;
		owner.spriteEffectStack.Add(_colorBlend);
		owner.stat.AttachValues(_stat);
		SpawnFloatingText();
	}

	public void Detach()
	{
		owner.spriteEffectStack.Remove(_colorBlend);
		owner.stat.DetachValues(_stat);
	}

	public void Initialize()
	{
	}

	private void SpawnFloatingText()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds);
		Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString("floating/status/burn"), Vector2.op_Implicit(val), "#DD4900");
	}
}
