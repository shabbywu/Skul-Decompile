using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class AttachToOneTargetOnGaveDamage : Ability, IAbilityInstance
{
	[Header("데미지 변경")]
	[SerializeField]
	private bool _onGiveDamage;

	[SerializeField]
	private float _damagePercent = 1f;

	[SerializeField]
	private float _damagePercentPoint;

	[SerializeField]
	private float _extraCriticalChance;

	[SerializeField]
	private float _extraCriticalDamageMultiplier;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	private Character _lastTarget;

	public Character owner { get; private set; }

	IAbility IAbilityInstance.ability => this;

	public Sprite icon => null;

	public float iconFillAmount => 0f;

	public float remainTime { get; set; }

	public bool attached => true;

	public int iconStacks => 0;

	public bool expired => false;

	public void Attach()
	{
		if (_onGiveDamage)
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)TryAttachAbility);
			return;
		}
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(TryAttachAbility));
	}

	private bool TryAttachAbility(ITarget target, ref Damage damage)
	{
		Character character = target.character;
		if ((Object)(object)character == (Object)null)
		{
			return false;
		}
		if ((Object)(object)character == (Object)(object)owner)
		{
			return false;
		}
		if ((Object)(object)_lastTarget == (Object)null)
		{
			_lastTarget = character;
		}
		if (_lastTarget.health.dead || !((Component)_lastTarget).gameObject.activeInHierarchy)
		{
			_lastTarget = null;
			return false;
		}
		if ((Object)(object)_lastTarget != (Object)(object)character)
		{
			return false;
		}
		damage.percentMultiplier *= _damagePercent;
		damage.multiplier += _damagePercentPoint;
		damage.criticalChance += _extraCriticalChance;
		damage.criticalDamageMultiplier += _extraCriticalDamageMultiplier;
		_lastTarget.ability.Add(_abilityComponent.ability);
		return false;
	}

	private void TryAttachAbility(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		Character character = target.character;
		if (!((Object)(object)character == (Object)null))
		{
			if ((Object)(object)_lastTarget == (Object)null)
			{
				_lastTarget = character;
			}
			if (_lastTarget.health.dead || !((Component)_lastTarget).gameObject.activeInHierarchy)
			{
				_lastTarget = null;
			}
			else if (!((Object)(object)_lastTarget != (Object)(object)character))
			{
				_lastTarget.ability.Add(_abilityComponent.ability);
			}
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		_abilityComponent.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void Detach()
	{
		if (_onGiveDamage)
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)TryAttachAbility);
		}
		else
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(TryAttachAbility));
		}
		if ((Object)(object)_lastTarget != (Object)null && !_lastTarget.health.dead)
		{
			_lastTarget.ability.Remove(_abilityComponent.ability);
		}
	}

	public void Refresh()
	{
	}

	public void UpdateTime(float deltaTime)
	{
	}
}
