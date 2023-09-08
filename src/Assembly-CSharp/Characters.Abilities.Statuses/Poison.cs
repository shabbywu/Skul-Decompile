using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Poison : CharacterStatusAbility, IAbility, IAbilityInstance
{
	private double _remainTimeToNextTick;

	public bool stoppingPower;

	public CharacterStatus.OnTimeDelegate onTookPoisonTickDamage;

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

	public float duration => CharacterStatusSetting.instance.poison.duration * base.durationMultiplier;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public double tickInterval => (double)CharacterStatusSetting.instance.poison.tickFrequency - base.attacker.stat.GetFinal(Stat.Kind.PoisonTickFrequency);

	public new StatusEffect.PoisonHandler effectHandler { get; set; }

	public override event CharacterStatus.OnTimeDelegate onAttachEvents;

	public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

	public override event CharacterStatus.OnTimeDelegate onDetachEvents;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Poison(Character owner)
	{
		this.owner = owner;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
		_remainTimeToNextTick -= deltaTime;
		effectHandler.UpdateTime(deltaTime);
		if (_remainTimeToNextTick <= 0.0)
		{
			_remainTimeToNextTick += tickInterval;
			GiveDamage(base.attacker);
		}
	}

	private void GiveDamage(Character attacker)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)attacker == (Object)null))
		{
			Damage damage = attacker.stat.GetDamage(CharacterStatusSetting.instance.poison.baseTickDamage, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), CharacterStatusSetting.instance.poison.hitInfo);
			damage.canCritical = false;
			damage.stoppingPower = (stoppingPower ? 1f : 0f);
			damage.multiplier -= damage.multiplier * (1.0 - owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Poison));
			attacker.Attack(owner, ref damage);
			onTookPoisonTickDamage?.Invoke(attacker, owner);
			effectHandler.HandleOnTookPoisonTickDamage(attacker, owner);
		}
	}

	public void Refresh()
	{
		remainTime = duration;
		onRefreshed?.Invoke(base.attacker, owner);
		effectHandler.HandleOnRefresh(base.attacker, owner);
	}

	public void Attach()
	{
		remainTime = duration;
		onAttached?.Invoke(base.attacker, owner);
		effectHandler.HandleOnAttach(base.attacker, owner);
	}

	public void Detach()
	{
		onDetached?.Invoke(base.attacker, owner);
		effectHandler.HandleOnDetach(base.attacker, owner);
		base.attacker = null;
	}

	public void Initialize()
	{
		effectHandler = StatusEffect.GetDefaultPoisonEffectHanlder(owner);
	}
}
