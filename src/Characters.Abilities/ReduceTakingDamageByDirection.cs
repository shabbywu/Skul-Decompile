using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ReduceTakingDamageByDirection : Ability
{
	public class Instance : AbilityInstance<ReduceTakingDamageByDirection>
	{
		public Instance(Character owner, ReduceTakingDamageByDirection ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MaxValue, CancelDamage);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove(CancelDamage);
		}

		private bool CancelDamage(ref Damage damage)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			if (damage.attackType == Damage.AttackType.Additional)
			{
				return false;
			}
			Vector3 position = ((Component)owner).transform.position;
			Vector3 val = damage.attacker.transform.position;
			if (damage.attackType == Damage.AttackType.Ranged || damage.attackType == Damage.AttackType.Projectile)
			{
				val = Vector2.op_Implicit(damage.hitPoint);
			}
			bool flag = ability._from == Direction.Front && owner.lookingDirection == Character.LookingDirection.Right;
			flag |= ability._from == Direction.Back && owner.lookingDirection == Character.LookingDirection.Left;
			bool flag2 = position.x < val.x;
			if ((flag && !flag2) || (!flag && flag2))
			{
				return false;
			}
			damage.multiplier -= ability._takingDamageReducePercent;
			return false;
		}
	}

	private enum Direction
	{
		Front,
		Back
	}

	[SerializeField]
	private Direction _from;

	[Range(0f, 1f)]
	[SerializeField]
	private float _takingDamageReducePercent;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
