using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Wound : CharacterStatusAbility, IAbility, IAbilityInstance
{
	public Character owner { get; private set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached { get; set; }

	public Sprite icon => null;

	public float iconFillAmount => remainTime / duration;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => remainTime <= 0f;

	public float duration => 2.1474836E+09f;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public bool critical { get; set; }

	public override event CharacterStatus.OnTimeDelegate onAttachEvents;

	public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

	public override event CharacterStatus.OnTimeDelegate onDetachEvents;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Wound(Character owner)
	{
		this.owner = owner;
	}

	public void UpdateTime(float deltaTime)
	{
		attached = true;
		remainTime -= deltaTime;
		base.effectHandler.UpdateTime(deltaTime);
	}

	private void GiveDamage()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)base.attacker == (Object)null) && !((Object)(object)owner == (Object)null))
		{
			Damage damage = base.attacker.stat.GetDamage(CharacterStatusSetting.instance.bleed.baseDamage, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), CharacterStatusSetting.instance.bleed.hitInfo);
			damage.criticalChance = base.attacker.stat.GetFinal(Stat.Kind.CriticalChance) - 1.0;
			damage.criticalDamageMultiplier = base.attacker.stat.GetFinal(Stat.Kind.CriticalDamage);
			damage.canCritical = critical;
			damage.multiplier *= base.attacker.stat.GetFinal(Stat.Kind.BleedDamage);
			damage.multiplier -= damage.multiplier * (1.0 - owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Wound));
			base.attacker.Attack(owner, ref damage);
		}
	}

	public void Refresh()
	{
		GiveDamage();
		remainTime = 0f;
		attached = false;
		onDetached?.Invoke(base.attacker, owner);
		base.effectHandler.HandleOnDetach(base.attacker, owner);
		owner.ability.Remove(this);
	}

	public void Attach()
	{
		remainTime = duration;
		onAttached?.Invoke(base.attacker, owner);
		base.effectHandler.HandleOnAttach(base.attacker, owner);
	}

	public void Detach()
	{
		remainTime = 0f;
		attached = false;
	}

	public void Initialize()
	{
		base.effectHandler = StatusEffect.GetDefaultWoundEffectHanlder(owner);
	}
}
