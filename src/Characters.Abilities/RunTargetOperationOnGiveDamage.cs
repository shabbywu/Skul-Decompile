using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class RunTargetOperationOnGiveDamage : Ability
{
	private class Instance : AbilityInstance<RunTargetOperationOnGiveDamage>
	{
		public Instance(Character owner, RunTargetOperationOnGiveDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)HandleOnGiveDamage);
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if ((Object)(object)target.character == (Object)null)
			{
				return false;
			}
			if (!((EnumArray<Character.Type, bool>)ability._characterType)[target.character.type])
			{
				return false;
			}
			if (!((EnumArray<Damage.AttackType, bool>)ability._attackType)[damage.attackType])
			{
				return false;
			}
			if (!((EnumArray<Damage.MotionType, bool>)ability._motionType)[damage.motionType])
			{
				return false;
			}
			((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner, target.character));
			return false;
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)HandleOnGiveDamage);
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _characterType;

	[SerializeField]
	private AttackTypeBoolArray _attackType;

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operations;

	public override void Initialize()
	{
		base.Initialize();
		_operations.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
