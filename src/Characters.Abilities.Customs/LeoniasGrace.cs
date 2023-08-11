using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class LeoniasGrace : Ability
{
	public class Instance : AbilityInstance<LeoniasGrace>
	{
		public override int iconStacks => (int)ability._remainHealth;

		public Instance(Character owner, LeoniasGrace ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MinValue, (TakeDamageDelegate)OnOwnerTakeDamage);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)OnOwnerTakeDamage);
		}

		private bool OnOwnerTakeDamage(ref Damage damage)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			ability._remainHealth -= damage.amount;
			if ((Object)(object)ability._hitPoint != (Object)null)
			{
				ability._hitPoint.position = Vector2.op_Implicit(damage.hitPoint);
			}
			ability._operationsOnHit.Run(owner);
			if (ability._remainHealth < 0.0)
			{
				ability._operationsOnBreak.Run(owner);
				ability._operationInfos.Stop();
			}
			return true;
		}
	}

	[SerializeField]
	private OperationInfos _operationInfos;

	[SerializeField]
	private double _health;

	[SerializeField]
	private Transform _hitPoint;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operationsOnHit;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operationsOnBreak;

	private double _remainHealth;

	public override void Initialize()
	{
		base.Initialize();
		_operationsOnHit.Initialize();
		_operationsOnBreak.Initialize();
		_remainHealth = _health;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
