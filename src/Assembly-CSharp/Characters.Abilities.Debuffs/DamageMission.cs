using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Debuffs;

[Serializable]
public sealed class DamageMission : Ability
{
	public class Instance : AbilityInstance<DamageMission>
	{
		private bool _success;

		public Instance(Character owner, DamageMission ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		public override void Refresh()
		{
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ability._motionType[gaveDamage.motionType] && ability._attackType[gaveDamage.attackType] && ability._targetType[character.type] && !gaveDamage.@null && damageDealt != 0.0)
			{
				_success = true;
				owner.ability.Remove(this);
			}
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			if (_success)
			{
				((MonoBehaviour)owner).StartCoroutine(ability._onSuccess.CRun(owner));
			}
			else
			{
				((MonoBehaviour)owner).StartCoroutine(ability._onFailed.CRun(owner));
			}
		}
	}

	[SerializeField]
	private AttackTypeBoolArray _attackType;

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	[SerializeField]
	private CharacterTypeBoolArray _targetType;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onSuccess;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onFailed;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	public override void Initialize()
	{
		base.Initialize();
		_onSuccess.Initialize();
		_onFailed.Initialize();
	}
}
