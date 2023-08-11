using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OmenArms : Ability
{
	public class Instance : AbilityInstance<OmenArms>
	{
		private float _remainCooldownTime;

		public override float iconFillAmount => _remainCooldownTime / ability._cooldownTime;

		public Instance(Character owner, OmenArms ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += HandleOnStartAction;
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= HandleOnStartAction;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if ((action.type == Characters.Actions.Action.Type.BasicAttack || action.type == Characters.Actions.Action.Type.JumpAttack) && !(_remainCooldownTime > 0f))
			{
				OperationInfo.Subcomponents subcomponents = (owner.movement.isGrounded ? ability._additionalAttackOnGround : ability._additionalAttackOnAir);
				((MonoBehaviour)owner).StartCoroutine(subcomponents.CRun(owner));
				_remainCooldownTime = ability._cooldownTime;
			}
		}
	}

	[SerializeField]
	private float _cooldownTime;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _additionalAttackOnGround;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _additionalAttackOnAir;

	public override void Initialize()
	{
		base.Initialize();
		_additionalAttackOnGround.Initialize();
		_additionalAttackOnAir.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
