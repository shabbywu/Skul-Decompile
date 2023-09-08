using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyTakingDamage : Ability
{
	public class Instance : AbilityInstance<ModifyTakingDamage>
	{
		private float _remainCooldownTime;

		private int _remainCount;

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

		internal Instance(Character owner, ModifyTakingDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if (ability._applyCount == 0)
			{
				_remainCount = int.MaxValue;
			}
			else
			{
				_remainCount = ability._applyCount;
			}
			owner.health.onTakeDamage.Add(0, HandleOnTakeDamage);
		}

		private bool HandleOnTakeDamage(ref Damage damage)
		{
			if (_remainCooldownTime > 0f)
			{
				return false;
			}
			if ((Object)(object)damage.attacker.character != (Object)null && !ability._characterTypes[damage.attacker.character.type])
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
			damage.percentMultiplier *= ability._damagePercent;
			damage.multiplier += ability._damagePercentPoint;
			damage.criticalChance += ability._extraCriticalChance;
			damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplier;
			damage.criticalDamagePercentMultiplier += ability._extraCriticalDamagePercentMultiplier;
			_remainCooldownTime = ability._cooldownTime;
			_remainCount--;
			if (_remainCount == 0)
			{
				owner.ability.Remove(this);
			}
			return false;
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove(HandleOnTakeDamage);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}
	}

	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	[SerializeField]
	private string _attackKey;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(true, true, true, true, true, true, true, true, true);

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private float _damagePercent = 1f;

	[SerializeField]
	private float _damagePercentPoint;

	[SerializeField]
	private float _extraCriticalChance;

	[SerializeField]
	private float _extraCriticalDamagePercentMultiplier;

	[SerializeField]
	private float _extraCriticalDamageMultiplier;

	[SerializeField]
	private int _applyCount;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
