using System;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class MotionConstraint : Constraint
{
	[Serializable]
	private class Info
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<Info>
		{
		}

		[SerializeField]
		internal Action action;

		[SerializeField]
		internal Motion motion;

		[SerializeField]
		[MinMaxSlider(0f, 1f)]
		internal Vector2 animationRange;
	}

	[SerializeField]
	private Info.Reorderable _infos;

	public override void Initilaize(Action action)
	{
		base.Initilaize(action);
		Info[] values = ((ReorderableArray<Info>)_infos).values;
		foreach (Info info in values)
		{
			if (info.animationRange.y == 1f)
			{
				info.animationRange.y = float.PositiveInfinity;
			}
		}
	}

	public override bool Pass()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Motion runningMotion = _action.owner.runningMotion;
		if ((Object)(object)runningMotion == (Object)null)
		{
			return true;
		}
		Info[] values = ((ReorderableArray<Info>)_infos).values;
		foreach (Info info in values)
		{
			if ((Object)(object)runningMotion == (Object)(object)info.motion)
			{
				return MMMaths.Range(runningMotion.normalizedTime, info.animationRange);
			}
		}
		return true;
	}
}
