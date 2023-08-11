using System;
using Characters.Marks;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnMarkMaxStack : Trigger
{
	[SerializeField]
	private Transform _moveToTargetPosition;

	[SerializeField]
	private MarkInfo _mark;

	[SerializeField]
	private bool _clearMarkOnTriggered;

	private Character _character;

	public override void Attach(Character character)
	{
		MarkInfo mark = _mark;
		mark.onStack = (MarkInfo.OnStackDelegate)Delegate.Combine(mark.onStack, new MarkInfo.OnStackDelegate(OnStack));
	}

	public override void Detach()
	{
		MarkInfo mark = _mark;
		mark.onStack = (MarkInfo.OnStackDelegate)Delegate.Remove(mark.onStack, new MarkInfo.OnStackDelegate(OnStack));
	}

	private void OnStack(Mark mark, float stack)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (!(stack < (float)_mark.maxStack) && base.canBeTriggered)
		{
			if ((Object)(object)_moveToTargetPosition != (Object)null)
			{
				_moveToTargetPosition.position = Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)mark.owner.collider).bounds));
			}
			if (_clearMarkOnTriggered)
			{
				mark.ClearStack(_mark);
			}
			Invoke();
		}
	}
}
