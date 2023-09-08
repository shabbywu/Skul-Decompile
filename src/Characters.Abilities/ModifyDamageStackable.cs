using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyDamageStackable : Ability
{
	public class Instance : AbilityInstance<ModifyDamageStackable>
	{
		private float _remainCooldownTime;

		private int _remainCount;

		private int _stack;

		public override float iconFillAmount
		{
			get
			{
				if (ability._cooldownTime != 0f)
				{
					return _remainCooldownTime / ability._cooldownTime;
				}
				return base.iconFillAmount;
			}
		}

		public override int iconStacks => _stack;

		internal Instance(Character owner, ModifyDamageStackable ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainCount = ability._applyCount;
			owner.onGiveDamage.Add(0, OnOwnerGiveDamage);
			if (ability._maxStack == 0)
			{
				ability._maxStack = int.MaxValue;
			}
			_stack = 1;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnOwnerGiveDamage);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		public override void Refresh()
		{
			base.Refresh();
			_stack = Mathf.Clamp(_stack + 1, 0, ability._maxStack);
		}

		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			if (_remainCooldownTime > 0f)
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
			if (!string.IsNullOrWhiteSpace(ability._attackKey) && !damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			damage.percentMultiplier *= 1f + ability._damagePercentByStack * (float)_stack;
			damage.multiplier += ability._damagePercentPoint * (float)_stack;
			damage.criticalChance += ability._extraCriticalChance * (float)_stack;
			damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplier * (float)_stack;
			_remainCooldownTime = ability._cooldownTime;
			_remainCount--;
			if (_remainCount == 0)
			{
				owner.ability.Remove(this);
			}
			return false;
		}
	}

	[SerializeField]
	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	private string _attackKey;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	[Information("base *= (1 + damagePercent * stack)", InformationAttribute.InformationType.Info, false)]
	private float _damagePercentByStack = 0.1f;

	[SerializeField]
	[Information(" multiplier += damagePercentPoint * stack", InformationAttribute.InformationType.Info, false)]
	private float _damagePercentPoint;

	[SerializeField]
	private float _extraCriticalChance;

	[SerializeField]
	private float _extraCriticalDamageMultiplier;

	[SerializeField]
	private int _applyCount;

	[SerializeField]
	private int _maxStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
