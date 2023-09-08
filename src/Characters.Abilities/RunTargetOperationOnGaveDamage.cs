using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class RunTargetOperationOnGaveDamage : Ability
{
	private class Instance : AbilityInstance<RunTargetOperationOnGaveDamage>
	{
		public Instance(Character owner, RunTargetOperationOnGaveDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && ability._characterType[target.character.type] && ability._attackType[gaveDamage.attackType] && ability._motionType[gaveDamage.motionType])
			{
				((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner, target.character));
			}
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
		_operations.Initialize();
		base.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
