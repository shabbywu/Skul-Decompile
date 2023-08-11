using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class CowardlyCloak : Ability
{
	public class Instance : AbilityInstance<CowardlyCloak>
	{
		private float _remainCooldownTime;

		private float _remainTime;

		private bool _canUse;

		public Instance(Character owner, CowardlyCloak ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += HandleOnStartAction;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainTime -= deltaTime;
			_remainCooldownTime -= deltaTime;
			if (_remainTime <= 0f)
			{
				_canUse = false;
			}
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if ((action.type == Characters.Actions.Action.Type.Dash || action.type == Characters.Actions.Action.Type.Jump) && !(_remainCooldownTime > 0f))
			{
				if (action.type == Characters.Actions.Action.Type.Dash)
				{
					_remainTime = ability._jumpTime;
					_canUse = true;
				}
				else if (action.type == Characters.Actions.Action.Type.Jump && _canUse)
				{
					_remainTime = 0f;
					_canUse = false;
					_remainCooldownTime = ability._cooldownTime;
					((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner));
				}
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= HandleOnStartAction;
		}
	}

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private float _jumpTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
