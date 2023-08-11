using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class WindMail : Ability
{
	public class Instance : AbilityInstance<WindMail>
	{
		private float _remainCooldownTime;

		private float _remainTime;

		private bool _canUse;

		public Instance(Character owner, WindMail ability)
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
			if (_remainTime < 0f)
			{
				_canUse = false;
			}
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if (action.type == Characters.Actions.Action.Type.Dash && !(_remainCooldownTime > 0f))
			{
				if (!_canUse)
				{
					_remainTime = ability._doubleDashTime;
					_canUse = true;
				}
				else
				{
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
	private float _doubleDashTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
