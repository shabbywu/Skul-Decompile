using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class FlyChase : Behaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public new static readonly Type[] types = new Type[1] { typeof(FlyChase) };

		public SubcomponentAttribute(bool allowCustom = true)
			: base(allowCustom, types)
		{
		}
	}

	[UnityEditor.Subcomponent(typeof(MoveToTargetWithFly))]
	[SerializeField]
	private MoveToTargetWithFly _moveToTarget;

	[Wander.Subcomponent(true)]
	[SerializeField]
	private Wander _wanderWhenChaseFail;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(Idle))]
	private Idle _beforeRangeWander;

	private void Start()
	{
		_childs = new List<Behaviour> { _moveToTarget, _wanderWhenChaseFail, _beforeRangeWander };
	}

	public override IEnumerator CRun(AIController controller)
	{
		_ = controller.character;
		Character target = controller.target;
		base.result = Result.Doing;
		if ((Object)(object)target == (Object)null)
		{
			base.result = Result.Fail;
			yield break;
		}
		yield return _moveToTarget.CRun(controller);
		if (_moveToTarget.result == Result.Fail)
		{
			yield return _beforeRangeWander.CRun(controller);
			base.result = Result.Fail;
			yield return _wanderWhenChaseFail.CRun(controller);
		}
		else
		{
			base.result = Result.Success;
		}
	}
}
