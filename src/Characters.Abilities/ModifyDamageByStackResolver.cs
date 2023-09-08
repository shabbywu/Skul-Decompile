using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ModifyDamageByStackResolver : Ability
{
	public sealed class Instance : AbilityInstance<ModifyDamageByStackResolver>
	{
		private float _remainCooldownTime;

		public Instance(Character owner, ModifyDamageByStackResolver ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(ability._priority, HandleOnGiveDamage);
			ability._stackResolver.Attach(owner);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			ability._stackResolver.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!Filter(target, damage))
			{
				return false;
			}
			int stack = ability._stackResolver.GetStack(ref damage);
			damage.percentMultiplier *= 1f + ability._damagePercentPerStack * (float)stack;
			damage.multiplier += ability._damagePercentPointPerStack * (float)stack;
			damage.criticalChance += ability._extraCriticalChancePerStack * (float)stack;
			damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplierPerStack * (float)stack;
			_remainCooldownTime = ability._cooldownTime;
			return false;
		}

		private bool Filter(ITarget target, Damage damage)
		{
			if (_remainCooldownTime > 0f)
			{
				return false;
			}
			if ((Object)(object)target.character != (Object)null && !ability._characterTypes[target.character.type])
			{
				return false;
			}
			if (!ability._attackTypes[damage.motionType])
			{
				return false;
			}
			if (!ability._damageTypes[damage.attackType])
			{
				return false;
			}
			if (ability._attackKeys.Length != 0)
			{
				bool flag = false;
				string[] attackKeys = ability._attackKeys;
				foreach (string value in attackKeys)
				{
					if (damage.key.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
			ability._stackResolver.Detach(owner);
		}
	}

	[SerializeField]
	private int _priority;

	[SubclassSelector]
	[SerializeReference]
	private IStackResolver _stackResolver;

	[Header("공격 대상 필터")]
	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[Header("설정")]
	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private string[] _attackKeys;

	[SerializeField]
	private float _damagePercentPerStack;

	[SerializeField]
	private float _damagePercentPointPerStack;

	[SerializeField]
	private float _extraCriticalChancePerStack;

	[SerializeField]
	private float _extraCriticalDamageMultiplierPerStack;

	public override void Initialize()
	{
		base.Initialize();
		_stackResolver.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
