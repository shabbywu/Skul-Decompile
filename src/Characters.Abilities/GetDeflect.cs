using System;
using Characters.Actions;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class GetDeflect : Ability
{
	public sealed class Instance : AbilityInstance<GetDeflect>
	{
		public Instance(Character owner, GetDeflect ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MaxValue, (TakeDamageDelegate)ability.OnOwnerTakeDamage);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)ability.OnOwnerTakeDamage);
		}
	}

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private Characters.Actions.Action _deflectAction;

	[SerializeField]
	private DeflectedProjectile _deflectedProjectile;

	[SerializeField]
	private Transform _deflectedTransformValue;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	private bool OnOwnerTakeDamage(ref Damage damage)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		if (damage.attackType == Damage.AttackType.Projectile)
		{
			if ((Object)(object)_deflectAction != (Object)null)
			{
				_deflectAction.TryStart();
			}
			IProjectile projectile = damage.attacker.projectile;
			if (projectile != null)
			{
				if ((Object)(object)projectile.GetComponent<SkulHeadToTeleport>() != (Object)null)
				{
					return true;
				}
				if ((Object)(object)_deflectedProjectile != (Object)null)
				{
					((Component)_deflectedProjectile.reusable.Spawn(_deflectedTransformValue.position, true)).GetComponent<DeflectedProjectile>().Deflect((_owner.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left, projectile.GetComponentInChildren<SpriteRenderer>().sprite, Vector2.op_Implicit(projectile.transform.localScale), projectile.speed);
				}
				projectile.Despawn();
			}
			return true;
		}
		return false;
	}
}
