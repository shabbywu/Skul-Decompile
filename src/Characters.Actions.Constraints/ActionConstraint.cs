using System;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class ActionConstraint : Constraint
{
	[Serializable]
	public class Exception
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<Exception>
		{
		}

		[SerializeField]
		private Motion _motion;

		[SerializeField]
		[MinMaxSlider(0f, 1f)]
		private Vector2 _range;

		public Motion motion => _motion;

		public Vector2 range => _range;
	}

	[SerializeField]
	private ActionTypeBoolArray _canCancel;

	[SerializeField]
	private Exception.Reorderable _exceptions;

	public override bool Pass()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Motion runningMotion = _action.owner.runningMotion;
		if ((Object)(object)runningMotion == (Object)null)
		{
			return true;
		}
		for (int i = 0; i < ((ReorderableArray<Exception>)_exceptions).values.Length; i++)
		{
			Exception ex = ((ReorderableArray<Exception>)_exceptions).values[i];
			if ((Object)(object)ex.motion == (Object)(object)runningMotion)
			{
				if (ex.range.x == ex.range.y)
				{
					return false;
				}
				return MMMaths.Range(runningMotion.normalizedTime, ex.range);
			}
		}
		return ((EnumArray<Action.Type, bool>)_canCancel).GetOrDefault(runningMotion.action.type);
	}
}
