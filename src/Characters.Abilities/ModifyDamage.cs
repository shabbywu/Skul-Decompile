using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyDamage : Ability
{
	public class Instance : AbilityInstance<ModifyDamage>
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

		internal Instance(Character owner, ModifyDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainCount = ((ability._applyCount == 0) ? int.MaxValue : ability._applyCount);
			owner.onGiveDamage.Add(0, OnOwnerGiveDamage);
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

		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
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
			if (!string.IsNullOrWhiteSpace(ability._attackKey) && !damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			damage.percentMultiplier *= ability._damagePercent;
			damage.multiplier += ability._damagePercentPoint;
			damage.criticalChance += ability._extraCriticalChance;
			damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplier;
			if (ability._guaranteedCritical)
			{
				damage.SetGuaranteedCritical(ability._guaranteedCriticalPriority, critical: true);
			}
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
	private float _extraCriticalDamageMultiplier;

	[SerializeField]
	private int _applyCount;

	[Header("확정 크리티컬 설정")]
	[SerializeField]
	private bool _guaranteedCritical;

	[SerializeField]
	private int _guaranteedCriticalPriority;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
