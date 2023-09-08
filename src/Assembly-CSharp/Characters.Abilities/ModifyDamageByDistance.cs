using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ModifyDamageByDistance : Ability
{
	public class Instance : AbilityInstance<ModifyDamageByDistance>
	{
		internal Instance(Character owner, ModifyDamageByDistance ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(0, OnGiveDamage);
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (!ability._motionType[damage.motionType])
			{
				return false;
			}
			Vector2 val = MMMaths.Vector3ToVector2(target.transform.position);
			Vector2 val2 = (((Object)(object)ability._center == (Object)null) ? MMMaths.Vector3ToVector2(((Component)owner).transform.position) : MMMaths.Vector3ToVector2(((Component)ability._center).transform.position));
			float num = Mathf.Abs(val.x - val2.x);
			if (ability._skipMinUnder && num < ability._minBonusDistance)
			{
				return false;
			}
			if (ability._skipMaxOver && num > ability._maxBonusDistance)
			{
				return false;
			}
			float num2 = ability._damagePercent;
			float num3 = ability.additionalDamageMultiplier;
			float num4 = ability.additionalCriticalChance;
			if (!ability._discrete)
			{
				num2 = Mathf.Lerp(1f, num2, num / ability._maxBonusDistance);
				num3 = Mathf.Lerp(0f, num3, num / ability._maxBonusDistance);
				num4 = Mathf.Lerp(0f, num4, num / ability._maxBonusDistance);
			}
			damage.percentMultiplier *= num2;
			damage.multiplier += num3;
			damage.criticalChance += num4;
			return false;
		}
	}

	[SerializeField]
	private Transform _center;

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	[SerializeField]
	private float _minBonusDistance = 2f;

	[SerializeField]
	private float _maxBonusDistance = 7f;

	[SerializeField]
	private float additionalDamageMultiplier = 1f;

	[SerializeField]
	private float _damagePercent = 1f;

	[SerializeField]
	private float additionalCriticalChance;

	[SerializeField]
	private bool _skipMaxOver;

	[SerializeField]
	private bool _skipMinUnder = true;

	[SerializeField]
	private bool _discrete = true;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
