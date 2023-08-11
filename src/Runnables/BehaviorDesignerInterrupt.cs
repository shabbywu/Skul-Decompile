using System;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Runnables;

[Serializable]
public class BehaviorDesignerInterrupt : IStatusEvent
{
	void IStatusEvent.Apply(Character owner, Character target)
	{
		BehaviorTree component = ((Component)target).GetComponent<BehaviorTree>();
		if ((Object)(object)component != (Object)null)
		{
			((Behavior)component).DisableBehavior();
		}
	}

	void IStatusEvent.Release(Character owner, Character target)
	{
		BehaviorTree component = ((Component)target).GetComponent<BehaviorTree>();
		if ((Object)(object)component != (Object)null)
		{
			((Behavior)component).EnableBehavior();
		}
	}
}
