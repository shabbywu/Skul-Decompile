using System;
using System.Linq;
using Characters.Actions;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class OperationOnGuardMotion : Ability
{
	public class Instance : AbilityInstance<OperationOnGuardMotion>
	{
		private bool _guarding;

		private int count;

		public Instance(Character owner, OperationOnGuardMotion ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			count = 0;
			_guarding = false;
			owner.health.onTakeDamage.Add(int.MaxValue, Guard);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove(Guard);
			ability._operationOnGuard.Stop();
		}

		private bool Guard(ref Damage damage)
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			Motion runningMotion = owner.runningMotion;
			if ((Object)(object)runningMotion == (Object)null)
			{
				return false;
			}
			if (ability._motions.Length != 0 && !ability._motions.Contains(runningMotion))
			{
				return false;
			}
			if (damage.attackType == Damage.AttackType.Additional)
			{
				return false;
			}
			if (!owner.invulnerable.value && !damage.@null && damage.amount < 1.0)
			{
				return false;
			}
			if (!ability._attackType[damage.attackType])
			{
				return false;
			}
			Vector3 position = ((Component)owner).transform.position;
			Vector3 position2 = damage.attacker.transform.position;
			if (ability._onlyFront)
			{
				if (owner.lookingDirection == Character.LookingDirection.Right && position.x > position2.x)
				{
					return false;
				}
				if (owner.lookingDirection == Character.LookingDirection.Left && position.x < position2.x)
				{
					return false;
				}
			}
			damage.@null = true;
			if (ability._operationOnGuard.components.Length == 0)
			{
				return false;
			}
			if (count > ability._operationOnGuardMaxCount)
			{
				return false;
			}
			Vector3 position3 = ((damage.attacker.projectile != null) ? Vector2.op_Implicit(((Collider2D)ability._operationRange).ClosestPoint(damage.hitPoint)) : Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)ability._operationRange).bounds)));
			count++;
			ability._operationRunPosition.position = position3;
			ability._operationOnGuard.Run(owner);
			if (!_guarding)
			{
				ability._operationOnGuardStart.Run(owner);
				_guarding = true;
			}
			return false;
		}
	}

	[SerializeField]
	private bool _onlyFront = true;

	[SerializeField]
	private AttackTypeBoolArray _attackType = new AttackTypeBoolArray(false, true, true, true, false);

	[SerializeField]
	private Motion[] _motions;

	[SerializeField]
	private BoxCollider2D _operationRange;

	[SerializeField]
	private Transform _operationRunPosition;

	[SerializeField]
	private int _operationOnGuardMaxCount;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _operationOnGuardStart;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operationOnGuard;

	public override void Initialize()
	{
		base.Initialize();
		_operationOnGuardStart.Initialize();
		_operationOnGuard.Initialize();
		if (_operationOnGuardMaxCount == 0)
		{
			_operationOnGuardMaxCount = int.MaxValue;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
