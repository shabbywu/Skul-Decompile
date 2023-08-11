using System.Collections.Generic;
using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Burn : CharacterStatusAbility, IAbility, IAbilityInstance
{
	private readonly TargetLayer _targetLayer;

	private float _remainTimeToNextTick;

	private List<Target> _targets;

	public CharacterStatus.OnTimeDelegate onTookBurnDamage;

	public CharacterStatus.OnTimeDelegate onTookEmberDamage;

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

	public float duration => CharacterStatusSetting.instance.burn.duration * base.durationMultiplier;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public new StatusEffect.BurnHandler effectHandler { get; set; }

	public override event CharacterStatus.OnTimeDelegate onAttachEvents;

	public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

	public override event CharacterStatus.OnTimeDelegate onDetachEvents;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Burn(Character owner)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		_targets = new List<Target>(128);
		_targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: true, foeBody: false, allyProjectile: false, foeProjectile: false);
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
		_remainTimeToNextTick -= deltaTime;
		effectHandler.UpdateTime(deltaTime);
		if (_remainTimeToNextTick <= 0f)
		{
			_remainTimeToNextTick += CharacterStatusSetting.instance.burn.tickInterval;
			GiveDamage();
		}
	}

	private void GiveDamage()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)base.attacker == (Object)null || owner.health.dead)
		{
			return;
		}
		LayerMask val = _targetLayer.Evaluate(((Component)owner).gameObject);
		if (owner.type == Character.Type.Player)
		{
			val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x400);
		}
		Damage damage = base.attacker.stat.GetDamage(CharacterStatusSetting.instance.burn.baseTargetTickDamage, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), CharacterStatusSetting.instance.burn.hitInfo);
		damage.canCritical = false;
		damage.multiplier -= damage.multiplier * (1.0 - owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Burn));
		base.attacker.Attack(owner, ref damage);
		onTookBurnDamage?.Invoke(base.attacker, owner);
		effectHandler.HandleOnTookBurnDamage(base.attacker, owner);
		if ((Object)(object)base.attacker == (Object)null)
		{
			return;
		}
		float radius = CharacterStatusSetting.instance.burn.rangeRadius * (float)base.attacker.stat.GetFinal(Stat.Kind.EmberDamage);
		TargetFinder.FindTargetInRange(Vector2.op_Implicit(((Component)owner).transform.position), radius, val, _targets);
		foreach (Target target in _targets)
		{
			if ((Object)(object)base.attacker == (Object)null)
			{
				break;
			}
			if (!((Object)(object)target.character == (Object)null) && !target.character.health.dead && !((Object)(object)target.character == (Object)(object)owner))
			{
				damage = base.attacker.stat.GetDamage(CharacterStatusSetting.instance.burn.baseRangeTickDamage, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), CharacterStatusSetting.instance.burn.rangeHitInfo);
				damage.canCritical = false;
				damage.multiplier *= base.attacker.stat.GetFinal(Stat.Kind.EmberDamage);
				base.attacker.Attack(target, ref damage);
				onTookEmberDamage?.Invoke(base.attacker, target.character);
				effectHandler.HandleOnTookEmberDamage(base.attacker, target.character);
			}
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
		_remainTimeToNextTick = 0f;
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
		effectHandler = StatusEffect.GetDefaultBurnEffectHanlder(owner);
	}
}
