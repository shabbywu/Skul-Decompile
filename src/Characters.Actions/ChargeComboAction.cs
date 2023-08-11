using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class ChargeComboAction : Action
{
	[Serializable]
	internal class ActionInfo
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<ActionInfo>
		{
		}

		[MinMaxSlider(0f, 1f)]
		[Tooltip("fnish 모션을 캔슬하기 위한 입력 구간, 사용자의 입력이 input 범위 내이지만 cancel 보다 빠를 경우 선입력으로 예약됨")]
		public Vector2 input;

		[Tooltip("finish 모션이 캔슬될 수 있는 구간")]
		[MinMaxSlider(0f, 1f)]
		public Vector2 cancel;

		[MinMaxSlider(0f, 1f)]
		[Tooltip("earlyFinish 모션을 캔슬하기 위한 입력 구간, 사용자의 입력이 input 범위 내이지만 earlyCancel 보다 빠를 경우 선입력으로 예약됨")]
		public Vector2 earlyInput;

		[Tooltip("earlyFinish 모션이 캔슬될 수 있는 구간")]
		[MinMaxSlider(0f, 1f)]
		public Vector2 earlyCancel;

		[Space]
		[Subcomponent(typeof(Motion))]
		public Motion anticipation;

		[Subcomponent(true, typeof(Motion))]
		public Motion prepare;

		[Subcomponent(typeof(Motion))]
		public Motion charging;

		[Subcomponent(true, typeof(Motion))]
		public Motion charged;

		[Subcomponent(true, typeof(Motion))]
		public Motion earlyFinish;

		[Subcomponent(typeof(Motion))]
		public Motion finish;

		private bool _earlyFinishReserved;

		private ChargeComboAction _action;

		private Motion[] _motions;

		public Motion[] motions
		{
			get
			{
				Motion[] array = _motions;
				if (array == null)
				{
					Motion[] obj = new Motion[6] { anticipation, prepare, charging, charged, earlyFinish, finish };
					Motion[] array2 = obj;
					_motions = obj;
					array = array2;
				}
				return array;
			}
		}

		public void InitializeMotions(ChargeComboAction action)
		{
			_action = action;
			List<Motion> list = new List<Motion>(6);
			list.Add(anticipation);
			if ((Object)(object)prepare != (Object)null)
			{
				list.Add(prepare);
			}
			list.Add(charging);
			if ((Object)(object)charged != (Object)null)
			{
				list.Add(charged);
			}
			list.Add(finish);
			for (int i = 0; i < list.Count - 1; i++)
			{
				Motion nextMotion = list[i + 1];
				list[i].onEnd += delegate
				{
					action.DoMotion(nextMotion);
				};
			}
			if ((Object)(object)earlyFinish != (Object)null)
			{
				list.Add(earlyFinish);
			}
			_motions = list.ToArray();
			Motion[] array = _motions;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Initialize(action);
			}
			if (anticipation.blockLook)
			{
				anticipation.onStart += delegate
				{
					action._lookingDirection = action._owner.lookingDirection;
				};
				if ((Object)(object)prepare != (Object)null)
				{
					prepare.onStart += RepositLookingDirection;
				}
				charging.onStart += RepositLookingDirection;
				if ((Object)(object)charged != (Object)null)
				{
					charged.onStart += RepositLookingDirection;
				}
				if ((Object)(object)earlyFinish != (Object)null)
				{
					earlyFinish.onStart += RepositLookingDirection;
				}
				finish.onStart += RepositLookingDirection;
			}
			charging.onStart += delegate
			{
				if (!_earlyFinishReserved)
				{
					action.InvokeStartCharging();
				}
			};
			charging.onCancel += action.InvokeCancelCharging;
			if ((Object)(object)charged == (Object)null)
			{
				charging.onEnd += action.InvokeEndCharging;
				return;
			}
			charged.onEnd += action.InvokeEndCharging;
			charged.onCancel += action.InvokeCancelCharging;
			void RepositLookingDirection()
			{
				action._owner.ForceToLookAt(action._lookingDirection);
			}
		}

		private void EarlyFinish()
		{
			_earlyFinishReserved = false;
			_action.DoMotion(earlyFinish);
			anticipation.onEnd -= EarlyFinish;
			if ((Object)(object)prepare != (Object)null)
			{
				prepare.onEnd -= EarlyFinish;
			}
		}

		public void ReserveEarlyFinish()
		{
			_earlyFinishReserved = true;
			anticipation.onEnd -= EarlyFinish;
			anticipation.onEnd += EarlyFinish;
			if ((Object)(object)prepare != (Object)null)
			{
				prepare.onEnd -= EarlyFinish;
				prepare.onEnd += EarlyFinish;
			}
		}
	}

	[SerializeField]
	private ActionInfo.Reorderable _actionInfo;

	[SerializeField]
	private int _cycleOffset;

	protected bool _cancelReserved;

	protected bool _endReserved;

	protected int _current;

	private Character.LookingDirection _lookingDirection;

	private Motion[] _motions;

	internal ActionInfo current => ((ReorderableArray<ActionInfo>)_actionInfo).values[_current];

	public override Motion[] motions
	{
		get
		{
			if (_motions == null)
			{
				_motions = ((ReorderableArray<ActionInfo>)_actionInfo).values.SelectMany((ActionInfo m) => m.motions).ToArray();
			}
			return _motions;
		}
	}

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(current.anticipation);
			}
			return false;
		}
	}

	private IEnumerator CReservedAttack()
	{
		while (_cancelReserved)
		{
			Vector2 val;
			Vector2 val2;
			if ((Object)(object)_owner.runningMotion == (Object)(object)current.earlyFinish)
			{
				val = current.earlyInput;
				val2 = current.earlyCancel;
			}
			else
			{
				if (!((Object)(object)_owner.runningMotion == (Object)(object)current.finish))
				{
					break;
				}
				val = current.input;
				val2 = current.cancel;
			}
			if (MMMaths.Range(_owner.runningMotion.normalizedTime, val2) && (_cancelReserved || MMMaths.Range(_owner.runningMotion.normalizedTime, val)))
			{
				_cancelReserved = false;
				int num = _current + 1;
				if (num >= ((ReorderableArray<ActionInfo>)_actionInfo).values.Length)
				{
					num = _cycleOffset;
				}
				ActionInfo actionInfo = ((ReorderableArray<ActionInfo>)_actionInfo).values[num];
				if (_endReserved)
				{
					_endReserved = false;
					actionInfo.ReserveEarlyFinish();
				}
				_current = num;
				DoAction(actionInfo.anticipation);
			}
			yield return null;
		}
	}

	protected override void Awake()
	{
		for (int i = 0; i < ((ReorderableArray<ActionInfo>)_actionInfo).values.Length; i++)
		{
			((ReorderableArray<ActionInfo>)_actionInfo).values[i].InitializeMotions(this);
		}
	}

	private void InvokeStartCharging()
	{
		_owner.onStartCharging?.Invoke(this);
	}

	private void InvokeEndCharging()
	{
		_owner.onStopCharging?.Invoke(this);
	}

	private void InvokeCancelCharging()
	{
		_owner.onCancelCharging?.Invoke(this);
	}

	public override bool TryStart()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (!((Component)this).gameObject.activeSelf || !canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		Motion runningMotion = _owner.runningMotion;
		if ((Object)(object)runningMotion != (Object)null && (Object)(object)runningMotion.action == (Object)(object)this)
		{
			if (_cancelReserved)
			{
				return false;
			}
			Vector2 val;
			if ((Object)(object)_owner.runningMotion == (Object)(object)current.earlyFinish)
			{
				val = current.earlyInput;
			}
			else
			{
				if (!((Object)(object)_owner.runningMotion == (Object)(object)current.finish))
				{
					return false;
				}
				val = current.input;
			}
			if (val.x == val.y)
			{
				return false;
			}
			if (!MMMaths.Range(runningMotion.normalizedTime, val))
			{
				return false;
			}
			_cancelReserved = true;
			((MonoBehaviour)this).StartCoroutine(CReservedAttack());
			return true;
		}
		_current = 0;
		_cancelReserved = false;
		_endReserved = false;
		DoAction(current.anticipation);
		return true;
	}

	public override bool TryEnd()
	{
		Motion runningMotion = _owner.runningMotion;
		if ((Object)(object)runningMotion == (Object)null || (Object)(object)runningMotion.action != (Object)(object)this)
		{
			return false;
		}
		if ((Object)(object)runningMotion == (Object)(object)current.earlyFinish || (Object)(object)runningMotion == (Object)(object)current.finish)
		{
			if (_cancelReserved)
			{
				_endReserved = true;
			}
			return false;
		}
		if ((Object)(object)current.charged != (Object)null && (Object)(object)runningMotion == (Object)(object)current.charged)
		{
			DoMotion(current.finish);
			return true;
		}
		if ((Object)(object)current.earlyFinish != (Object)null && (Object)(object)runningMotion != (Object)(object)current.earlyFinish && (Object)(object)runningMotion != (Object)(object)current.finish)
		{
			if ((Object)(object)base.owner.motion == (Object)(object)current.anticipation || (Object)(object)base.owner.motion == (Object)(object)current.prepare)
			{
				current.ReserveEarlyFinish();
				return false;
			}
			DoMotion(current.earlyFinish);
			return true;
		}
		base.owner.CancelAction();
		return false;
	}
}
