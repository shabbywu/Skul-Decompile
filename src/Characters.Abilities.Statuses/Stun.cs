using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Stun : CharacterStatusAbility, IAbility, IAbilityInstance
{
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

	public float duration => (float)(((double)CharacterStatusSetting.instance.stun.duration + base.attacker.stat.GetFinal(Stat.Kind.StunDuration)) * owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Stun)) * base.durationMultiplier;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public override event CharacterStatus.OnTimeDelegate onAttachEvents;

	public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

	public override event CharacterStatus.OnTimeDelegate onDetachEvents;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Stun(Character owner)
	{
		this.owner = owner;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
	}

	public void Refresh()
	{
		remainTime = duration;
		onRefreshEvents?.Invoke(base.attacker, owner);
		onRefreshed?.Invoke(base.attacker, owner);
		base.effectHandler.HandleOnRefresh(base.attacker, owner);
	}

	public void Attach()
	{
		remainTime = duration;
		if (owner.type == Character.Type.Boss)
		{
			((ChronometerBase)owner.chronometer.animation).AttachTimeScale((object)this, 0f);
			owner.blockLook.Attach((object)this);
			if ((Object)(object)owner.movement != (Object)null)
			{
				owner.movement.blocked.Attach((object)this);
			}
		}
		else
		{
			owner.CancelAction();
			owner.animationController.Stun();
		}
		owner.blockLook.Attach((object)this);
		if ((Object)(object)owner.movement != (Object)null)
		{
			owner.movement.blocked.Attach((object)this);
		}
		onAttachEvents?.Invoke(base.attacker, owner);
		onAttached?.Invoke(base.attacker, owner);
		base.effectHandler.HandleOnAttach(base.attacker, owner);
	}

	public void Detach()
	{
		remainTime = 0f;
		if (owner.type == Character.Type.Boss)
		{
			((ChronometerBase)owner.chronometer.animation).DetachTimeScale((object)this);
			owner.blockLook.Detach((object)this);
			if ((Object)(object)owner.movement != (Object)null)
			{
				owner.movement.blocked.Detach((object)this);
			}
		}
		else
		{
			owner.animationController.StopAll();
		}
		owner.blockLook.Detach((object)this);
		if ((Object)(object)owner.movement != (Object)null)
		{
			owner.movement.blocked.Detach((object)this);
		}
		onDetachEvents?.Invoke(base.attacker, owner);
		onDetached?.Invoke(base.attacker, owner);
		base.effectHandler.HandleOnDetach(base.attacker, owner);
	}

	public void Initialize()
	{
		base.effectHandler = StatusEffect.GetDefaultStunEffectHandler(owner);
	}
}
