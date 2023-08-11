using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class IgnoreTakingDamageByDirection : Ability
{
	public class Instance : AbilityInstance<IgnoreTakingDamageByDirection>
	{
		public Instance(Character owner, IgnoreTakingDamageByDirection ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MaxValue, (TakeDamageDelegate)CancelDamage);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)CancelDamage);
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
			damage.@null = true;
			return true;
		}
	}

	private enum Direction
	{
		Front,
		Back
	}

	[SerializeField]
	private Direction _from;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
