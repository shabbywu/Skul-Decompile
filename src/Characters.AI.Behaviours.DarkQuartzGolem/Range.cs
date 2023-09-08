using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem;

public class Range : Behaviour, IPattern
{
	[SerializeField]
	internal Collider2D trigger;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(ActionAttack))]
	private ActionAttack _attack;

	public bool CanUse()
	{
		throw new NotImplementedException();
	}

	public bool CanUse(AIController controller)
	{
		return (Object)(object)controller.FindClosestPlayerBody(trigger) != (Object)null;
	}

	public override IEnumerator CRun(AIController controller)
	{
		yield return _attack.CRun(controller);
	}
}
