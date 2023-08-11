using FX.SpriteEffects;
using Singletons;
using UnityEngine;

namespace Characters.Abilities;

public abstract class AbilityInstance : IAbilityInstance
{
	private EffectPoolInstance _loopEffect;

	private GenericSpriteEffect _spriteEffect;

	public readonly Character owner;

	public readonly Ability ability;

	public float remainTime { get; set; }

	public bool attached { get; private set; }

	public virtual Sprite icon => ability.defaultIcon;

	public virtual float iconFillAmount
	{
		get
		{
			if (ability.duration != float.PositiveInfinity)
			{
				return 1f - remainTime / ability.duration;
			}
			return 0f;
		}
	}

	public bool iconFillInversed
	{
		get
		{
			return ability.iconFillInversed;
		}
		set
		{
			ability.iconFillInversed = value;
		}
	}

	public bool iconFillFlipped
	{
		get
		{
			return ability.iconFillFlipped;
		}
		set
		{
			ability.iconFillFlipped = value;
		}
	}

	public virtual int iconStacks => 0;

	public bool expired => remainTime <= 0f;

	Character IAbilityInstance.owner => owner;

	IAbility IAbilityInstance.ability => ability;

	public AbilityInstance(Character owner, Ability ability)
	{
		this.owner = owner;
		this.ability = ability;
		remainTime = ability.duration;
	}

	public virtual void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
	}

	public virtual void Refresh()
	{
		remainTime = ability.duration;
	}

	public void Attach()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		attached = true;
		_loopEffect = ((ability.loopEffect == null) ? null : ability.loopEffect.Spawn(((Component)owner).transform.position, owner));
		if (owner.spriteEffectStack != null && ability.spriteEffect != null && ability.spriteEffect.enabled)
		{
			_spriteEffect = ability.spriteEffect.CreateInstance();
			owner.spriteEffectStack.Add(_spriteEffect);
		}
		ability.effectOnAttach?.Spawn(((Component)owner).transform.position, owner);
		if (ability.soundOnAttach != null && (Object)(object)ability.soundOnAttach.audioClip != (Object)null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability.soundOnAttach, ((Component)owner).transform.position);
		}
		OnAttach();
	}

	public void Detach()
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		attached = false;
		if (!((Object)(object)owner == (Object)null))
		{
			if ((Object)(object)_loopEffect != (Object)null)
			{
				_loopEffect.Stop();
				_loopEffect = null;
			}
			if (owner.spriteEffectStack != null && ability.spriteEffect != null && ability.spriteEffect.enabled)
			{
				owner.spriteEffectStack.Remove(_spriteEffect);
			}
			ability.effectOnDetach?.Spawn(((Component)owner).transform.position, owner);
			if (ability.soundOnAttach != null && (Object)(object)ability.soundOnDetach.audioClip != (Object)null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability.soundOnDetach, ((Component)owner).transform.position);
			}
			OnDetach();
		}
	}

	protected abstract void OnAttach();

	protected abstract void OnDetach();
}
public abstract class AbilityInstance<T> : AbilityInstance where T : Ability
{
	public new readonly T ability;

	public AbilityInstance(Character owner, T ability)
		: base(owner, ability)
	{
		this.ability = ability;
	}
}
