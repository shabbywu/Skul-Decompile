using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class FlyWander : Behaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public new static readonly Type[] types = new Type[1] { typeof(FlyWander) };

		public SubcomponentAttribute(bool allowCustom = true)
			: base(allowCustom, types)
		{
		}
	}

	[Move.Subcomponent(true)]
	[SerializeField]
	protected Move _move;

	[UnityEditor.Subcomponent(typeof(Idle))]
	[SerializeField]
	protected Idle _idleWhenEndWander;

	[SerializeField]
	protected Collider2D _sightRange;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			yield return null;
			if ((Object)(object)controller.target != (Object)null)
			{
				base.result = Result.Success;
				break;
			}
			if (character.movement.controller.isGrounded)
			{
				_move.direction = (MMMaths.RandomBool() ? Vector2.left : Vector2.right);
				yield return _move.CRun(controller);
			}
		}
		yield return _idleWhenEndWander.CRun(controller);
	}
}
