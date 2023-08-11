using System;
using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Freeze : CharacterStatusAbility, IAbility, IAbilityInstance
{
	private int _remainHitStack;

	private float _breakableTime;

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

	public float duration => (float)(((double)CharacterStatusSetting.instance.freeze.duration + base.attacker.stat.GetFinal(Stat.Kind.FreezeDuration)) * owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Freeze)) * base.durationMultiplier;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public int hitStack { get; set; }

	public new StatusEffect.FreezeHandler effectHandler { get; set; }

	public override event CharacterStatus.OnTimeDelegate onAttachEvents;

	public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

	public override event CharacterStatus.OnTimeDelegate onDetachEvents;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Freeze(Character owner)
	{
		this.owner = owner;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
		effectHandler.UpdateTime(deltaTime);
	}

	public void Refresh()
	{
		remainTime = duration;
		_breakableTime = remainTime - CharacterStatusSetting.instance.freeze.minimumTime;
		_remainHitStack = hitStack;
		onRefreshEvents?.Invoke(base.attacker, owner);
		onRefreshed?.Invoke(base.attacker, owner);
		effectHandler.HandleOnRefresh(base.attacker, owner);
	}

	public void Attach()
	{
		remainTime = duration;
		_breakableTime = remainTime - CharacterStatusSetting.instance.freeze.minimumTime;
		if ((Object)(object)owner.movement != (Object)null)
		{
			owner.movement.blocked.Attach((object)this);
		}
		((ChronometerBase)owner.chronometer.animation).AttachTimeScale((object)this, 0f);
		owner.blockLook.Attach((object)this);
		_remainHitStack = hitStack;
		owner.health.onTookDamage += OnTookDamage;
		onAttached?.Invoke(base.attacker, owner);
		onAttachEvents?.Invoke(base.attacker, owner);
		effectHandler.HandleOnAttach(base.attacker, owner);
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (tookDamage.motionType == Damage.MotionType.Status || remainTime >= _breakableTime)
		{
			return;
		}
		string[] nonHitCountAttackKeys = CharacterStatusSetting.instance.freeze.nonHitCountAttackKeys;
		foreach (string value in nonHitCountAttackKeys)
		{
			if (tookDamage.key.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}
		effectHandler.HandleOnTakeDamage(base.attacker, owner);
		_remainHitStack--;
		if (_remainHitStack <= 0)
		{
			remainTime = 0f;
		}
	}

	public void Detach()
	{
		remainTime = 0f;
		((ChronometerBase)owner.chronometer.animation).DetachTimeScale((object)this);
		owner.movement.push.Expire();
		owner.blockLook.Detach((object)this);
		if ((Object)(object)owner.movement != (Object)null)
		{
			owner.movement.blocked.Detach((object)this);
		}
		owner.health.onTookDamage -= OnTookDamage;
		onDetachEvents?.Invoke(base.attacker, owner);
		onDetached?.Invoke(base.attacker, owner);
		effectHandler.HandleOnDetach(base.attacker, owner);
	}

	public void Initialize()
	{
		effectHandler = new StatusEffect.Freeze(owner);
	}

	public void AddRemainHitStack()
	{
		AddRemainHitStack(1);
	}

	public void AddRemainHitStack(int count)
	{
		if (!(remainTime <= 0f))
		{
			_remainHitStack += count;
		}
	}
}
