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
			owner.onGiveDamage.Add(int.MaxValue, HandleOnGiveDamage);
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if ((Object)(object)target.character == (Object)null)
			{
				return false;
			}
			if (!ability._characterType[target.character.type])
			{
				return false;
			}
			if (!ability._attackType[damage.attackType])
			{
				return false;
			}
			if (!ability._motionType[damage.motionType])
			{
				return false;
			}
			((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner, target.character));
			return false;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
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
