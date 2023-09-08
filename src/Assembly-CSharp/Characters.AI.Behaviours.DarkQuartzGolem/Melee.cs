using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem;

public class Melee : Behaviour, IPattern
{
	[SerializeField]
	internal Collider2D trigger;

	[UnityEditor.Subcomponent(typeof(ActionAttack))]
	[SerializeField]
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
