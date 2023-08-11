using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class MultiChargeAction : Action
{
	[Serializable]
	protected class ChargeMotions
	{
		[Serializable]
		public class Reorderable : ReorderableArray<ChargeMotions>
		{
		}

		[Space]
		[Subcomponent(typeof(Motion))]
		public Motion charging;

		[Subcomponent(true, typeof(Motion))]
		public Motion charged;

		[Subcomponent(typeof(Motion))]
		public Motion finish;
	}

	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion _anticipation;

	[SerializeField]
	[Subcomponent(true, typeof(Motion))]
	protected Motion _prepare;

	[SerializeField]
	[Subcomponent(true, typeof(Motion))]
	protected Motion _earlyFinish;

	[Space]
	[SerializeField]
	protected ChargeMotions.Reorderable _chargeMotions;

	private Character.LookingDirection _lookingDirection;

	protected Motion[] _motions;

	private bool _earlyFinishReserved;

	public override Motion[] motions => _motions;

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(_anticipation);
			}
			return false;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		InitializeMotions();
	}

	private void InvokeStartCharging()
	{
		if (!_earlyFinishReserved)
		{
			_owner.onStartCharging?.Invoke(this);
		}
	}

	private void InvokeEndCharging()
	{
		_owner.onStopCharging?.Invoke(this);
	}

	private void InvokeCancelCharging()
	{
		_owner.onCancelCharging?.Invoke(this);
	}

	private void InitializeMotions()
	{
		List<Motion> list = new List<Motion>(8);
		list.Add(_anticipation);
		if ((Object)(object)_prepare != (Object)null)
		{
			list.Add(_prepare);
		}
		ChargeMotions[] values = ((ReorderableArray<ChargeMotions>)_chargeMotions).values;
		foreach (ChargeMotions chargeMotions in values)
		{
			list.Add(chargeMotions.charging);
			if ((Object)(object)chargeMotions.charged != (Object)null)
			{
				list.Add(chargeMotions.charged);
			}
		}
		list.Add(((ReorderableArray<ChargeMotions>)_chargeMotions).values.Last().finish);
		for (int j = 0; j < list.Count - 1; j++)
		{
			Motion nextMotion = list[j + 1];
			list[j].onEnd += delegate
			{
				DoMotion(nextMotion);
			};
		}
		if ((Object)(object)_earlyFinish != (Object)null)
		{
			list.Add(_earlyFinish);
		}
		foreach (Motion item in list)
		{
			item.Initialize(this);
		}
		if (_anticipation.blockLook)
		{
			_anticipation.onStart += delegate
			{
				_lookingDirection = _owner.lookingDirection;
			};
			if ((Object)(object)_prepare != (Object)null)
			{
				_prepare.onStart += RepositLookingDirection;
			}
			if ((Object)(object)_earlyFinish != (Object)null)
			{
				_earlyFinish.onStart += RepositLookingDirection;
			}
			values = ((ReorderableArray<ChargeMotions>)_chargeMotions).values;
			foreach (ChargeMotions chargeMotions2 in values)
			{
				chargeMotions2.charging.onStart += RepositLookingDirection;
				if ((Object)(object)chargeMotions2.charged != (Object)null)
				{
					chargeMotions2.charged.onStart += RepositLookingDirection;
				}
				chargeMotions2.finish.onStart += RepositLookingDirection;
				list.Add(chargeMotions2.finish);
				chargeMotions2.finish.Initialize(this);
			}
		}
		for (int k = 0; k < ((ReorderableArray<ChargeMotions>)_chargeMotions).values.Length; k++)
		{
			ChargeMotions chargeMotions3 = ((ReorderableArray<ChargeMotions>)_chargeMotions).values[k];
			Motion motion = null;
			if (k + 1 < ((ReorderableArray<ChargeMotions>)_chargeMotions).values.Length)
			{
				motion = ((ReorderableArray<ChargeMotions>)_chargeMotions).values[k + 1].charging;
			}
			if (k == 0)
			{
				chargeMotions3.charging.onStart += InvokeStartCharging;
			}
			chargeMotions3.charging.onCancel += InvokeCancelCharging;
			if ((Object)(object)chargeMotions3.charged == (Object)null)
			{
				if ((Object)(object)motion == (Object)null)
				{
					chargeMotions3.charging.onEnd += InvokeEndCharging;
				}
			}
			else
			{
				chargeMotions3.charged.onEnd += InvokeEndCharging;
				chargeMotions3.charged.onCancel += InvokeCancelCharging;
			}
		}
		_motions = list.ToArray();
		void RepositLookingDirection()
		{
			_owner.ForceToLookAt(_lookingDirection);
		}
	}

	public override bool TryStart()
	{
		if (!((Component)this).gameObject.activeSelf || !canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		DoAction(_anticipation);
		return true;
	}

	private void EarlyFinish()
	{
		_earlyFinishReserved = false;
		DoMotion(_earlyFinish);
		_anticipation.onEnd -= EarlyFinish;
		if ((Object)(object)_prepare != (Object)null)
		{
			_prepare.onEnd -= EarlyFinish;
		}
	}

	public void ReserveEarlyFinish()
	{
		_earlyFinishReserved = true;
		_anticipation.onEnd -= EarlyFinish;
		_anticipation.onEnd += EarlyFinish;
		if ((Object)(object)_prepare != (Object)null)
		{
			_prepare.onEnd -= EarlyFinish;
			_prepare.onEnd += EarlyFinish;
		}
	}

	public override bool TryEnd()
	{
		Motion runningMotion = base.owner.runningMotion;
		if ((Object)(object)runningMotion == (Object)null || (Object)(object)runningMotion.action != (Object)(object)this)
		{
			return false;
		}
		if ((Object)(object)runningMotion == (Object)(object)_earlyFinish)
		{
			return false;
		}
		if ((Object)(object)_earlyFinish != (Object)null && ((Object)(object)runningMotion == (Object)(object)_anticipation || (Object)(object)runningMotion == (Object)(object)_prepare))
		{
			ReserveEarlyFinish();
			return false;
		}
		for (int i = 0; i < ((ReorderableArray<ChargeMotions>)_chargeMotions).values.Length; i++)
		{
			ChargeMotions chargeMotions = ((ReorderableArray<ChargeMotions>)_chargeMotions).values[i];
			if ((Object)(object)runningMotion == (Object)(object)chargeMotions.finish)
			{
				return false;
			}
			if ((Object)(object)runningMotion == (Object)(object)chargeMotions.charging)
			{
				if (i == 0)
				{
					if ((Object)(object)_earlyFinish == (Object)null)
					{
						return false;
					}
					DoMotion(_earlyFinish);
				}
				else
				{
					DoMotion(((ReorderableArray<ChargeMotions>)_chargeMotions).values[i - 1].finish);
				}
				return true;
			}
			if ((Object)(object)runningMotion == (Object)(object)chargeMotions.charged)
			{
				DoMotion(chargeMotions.finish);
				return true;
			}
		}
		base.owner.CancelAction();
		return false;
	}
}
