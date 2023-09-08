using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Actions;

public class ComboAction : Action
{
	[Serializable]
	internal class ActionInfo
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<ActionInfo>
		{
		}

		[SerializeField]
		private Vector2 _input;

		[SerializeField]
		private Vector2 _cancel;

		[SerializeField]
		private Motion _motion;

		internal Vector2 input => _input;

		internal Vector2 cancel => _cancel;

		internal Motion motion => _motion;
	}

	[SerializeField]
	private ActionInfo.Reorderable _actionInfo;

	[SerializeField]
	private int _cycleOffset;

	protected bool _cancelReserved;

	protected int _current;

	internal ActionInfo current => _actionInfo.values[_current];

	public override Motion[] motions => _actionInfo.values.Select((ActionInfo m) => m.motion).ToArray();

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(current.motion);
			}
			return false;
		}
	}

	private IEnumerator CReservedAttack()
	{
		while (_cancelReserved && (Object)(object)_owner.runningMotion == (Object)(object)current.motion)
		{
			if (MMMaths.Range(current.motion.time, current.cancel) && (_cancelReserved || MMMaths.Range(current.motion.time, current.input)))
			{
				_cancelReserved = false;
				int num = _current + 1;
				if (num >= _actionInfo.values.Length)
				{
					num = _cycleOffset;
				}
				_current = num;
				DoAction(_actionInfo.values[num].motion);
			}
			yield return null;
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		for (int i = 0; i < _actionInfo.values.Length; i++)
		{
			_actionInfo.values[i].motion.Initialize(this);
		}
	}

	public override bool TryStart()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		if (!canUse)
		{
			return false;
		}
		if ((Object)(object)_owner.runningMotion != (Object)null && (Object)(object)_owner.runningMotion.action == (Object)(object)this)
		{
			if ((Object)(object)_owner.runningMotion != (Object)(object)current.motion)
			{
				return false;
			}
			if (_cancelReserved)
			{
				return false;
			}
			if (current.input.x == current.input.y)
			{
				return false;
			}
			if (!MMMaths.Range(_owner.runningMotion.time, current.input))
			{
				return false;
			}
			_cancelReserved = true;
			((MonoBehaviour)this).StartCoroutine(CReservedAttack());
			return true;
		}
		_current = 0;
		_cancelReserved = false;
		if (!ConsumeCooldownIfNeeded())
		{
			return false;
		}
		DoAction(current.motion);
		return true;
	}
}
