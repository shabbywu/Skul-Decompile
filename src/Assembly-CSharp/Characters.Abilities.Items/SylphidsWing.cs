using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class SylphidsWing : Ability
{
	public sealed class Instance : AbilityInstance<SylphidsWing>
	{
		private int _stack;

		public override Sprite icon
		{
			get
			{
				if (_stack >= 1)
				{
					return base.icon;
				}
				return null;
			}
		}

		public override float iconFillAmount => owner.movement.isGrounded ? 1 : 0;

		public override int iconStacks => _stack;

		public Instance(Character owner, SylphidsWing ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += HandleOnStartAction;
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if (!owner.movement.isGrounded && action.type == Characters.Actions.Action.Type.Dash)
			{
				_stack++;
				if (_stack >= ability._cycle)
				{
					((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner));
					_stack = 0;
				}
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= HandleOnStartAction;
		}
	}

	[SerializeField]
	private int _cycle;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

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
