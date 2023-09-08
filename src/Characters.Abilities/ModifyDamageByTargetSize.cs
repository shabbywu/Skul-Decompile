using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyDamageByTargetSize : Ability
{
	public class Instance : AbilityInstance<ModifyDamageByTargetSize>
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

		internal Instance(Character owner, ModifyDamageByTargetSize ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainCount = ability._applyCount;
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
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)target.character == (Object)null)
			{
				return false;
			}
			Bounds bounds;
			Vector3 size;
			switch (ability._targetSize)
			{
			case TargetSize.Bigger:
			{
				bounds = target.collider.bounds;
				size = ((Bounds)(ref bounds)).size;
				float sqrMagnitude2 = ((Vector3)(ref size)).sqrMagnitude;
				bounds = ((Collider2D)owner.collider).bounds;
				size = ((Bounds)(ref bounds)).size;
				if (sqrMagnitude2 < ((Vector3)(ref size)).sqrMagnitude)
				{
					return false;
				}
				break;
			}
			case TargetSize.Smaller:
			{
				bounds = target.collider.bounds;
				size = ((Bounds)(ref bounds)).size;
				float sqrMagnitude = ((Vector3)(ref size)).sqrMagnitude;
				bounds = ((Collider2D)owner.collider).bounds;
				size = ((Bounds)(ref bounds)).size;
				if (sqrMagnitude > ((Vector3)(ref size)).sqrMagnitude)
				{
					return false;
				}
				break;
			}
			}
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
			damage.percentMultiplier *= ability._damagePercent;
			damage.multiplier += ability._damagePercentPoint;
			damage.criticalChance += ability._extraCriticalChance;
			damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplier;
			_remainCooldownTime = ability._cooldownTime;
			_remainCount--;
			if (_remainCount == 0)
			{
				owner.ability.Remove(this);
			}
			return false;
		}
	}

	private enum TargetSize
	{
		Bigger,
		Smaller
	}

	[SerializeField]
	private TargetSize _targetSize;

	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	[SerializeField]
	private string _attackKey;

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

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
