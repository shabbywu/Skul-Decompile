using System;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class CriticalChanceByDistance : Ability
{
	public class Instance : AbilityInstance<CriticalChanceByDistance>
	{
		private int _remainCount;

		internal Instance(Character owner, CriticalChanceByDistance ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(0, OnOwnerGiveDamage);
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnOwnerGiveDamage);
		}

		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (!ability._motionFilter[damage.motionType] || !ability._attackFilter[damage.attackType])
			{
				return false;
			}
			Vector2 val = MMMaths.Vector3ToVector2(target.transform.position);
			Vector2 val2 = MMMaths.Vector3ToVector2(((Component)owner).transform.position);
			float distance = Vector2.Distance(val, val2);
			damage.criticalChance += ability.GetBonusCriticalChance(distance);
			return false;
		}
	}

	[SerializeField]
	[Range(0f, 100f)]
	private int _maxBonusCriticalChance;

	[Header("Filter")]
	[SerializeField]
	private MotionTypeBoolArray _motionFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackFilter;

	[SerializeField]
	[Header("Distance")]
	private float _minBonusDistance = 2f;

	[SerializeField]
	private float _maxBonusDistance = 7f;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	private float GetBonusCriticalChance(float distance)
	{
		return Mathf.Clamp01((distance - _minBonusDistance) / _maxBonusDistance) * (float)_maxBonusCriticalChance * 0.01f;
	}
}
