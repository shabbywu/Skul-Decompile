using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class ChargeAction : Action
{
	[SerializeField]
	[Subcomponent(typeof(Motion))]
	protected Motion _anticipation;

	[Subcomponent(true, typeof(Motion))]
	[SerializeField]
	protected Motion _prepare;

	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion _charging;

	[SerializeField]
	[Subcomponent(true, typeof(Motion))]
	protected Motion _charged;

	[Subcomponent(true, typeof(Motion))]
	[SerializeField]
	protected Motion _earlyFinish;

	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion _finish;

	private Character.LookingDirection _lookingDirection;

	protected Motion[] _motions;

	private bool _earlyFinishReserved;

	private bool _isCharging;

	public override Motion[] motions => new Motion[6] { _anticipation, _prepare, _charging, _charged, _earlyFinish, _finish };

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

	public float chargedPercent => _charging.normalizedTime;

	public float chargingPercent
	{
		get
		{
			if (!_charging.running)
			{
				return 0f;
			}
			return _charging.normalizedTime;
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
		List<Motion> list = new List<Motion>(6);
		list.Add(_anticipation);
		if ((Object)(object)_prepare != (Object)null)
		{
			list.Add(_prepare);
		}
		list.Add(_charging);
		if ((Object)(object)_charged != (Object)null)
		{
			list.Add(_charged);
		}
		list.Add(_finish);
		for (int i = 0; i < list.Count - 1; i++)
		{
			Motion nextMotion = list[i + 1];
			list[i].onEnd += delegate
			{
				DoMotion(nextMotion);
			};
		}
		if ((Object)(object)_earlyFinish != (Object)null)
		{
			list.Add(_earlyFinish);
		}
		_motions = list.ToArray();
		Motion[] array = _motions;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Initialize(this);
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
			_charging.onStart += RepositLookingDirection;
			if ((Object)(object)_charged != (Object)null)
			{
				_charged.onStart += RepositLookingDirection;
			}
			if ((Object)(object)_earlyFinish != (Object)null)
			{
				_earlyFinish.onStart += RepositLookingDirection;
			}
			_finish.onStart += RepositLookingDirection;
		}
		_charging.onStart += InvokeStartCharging;
		_charging.onCancel += InvokeCancelCharging;
		if ((Object)(object)_charged == (Object)null)
		{
			_charging.onEnd += InvokeEndCharging;
			return;
		}
		_charged.onEnd += InvokeEndCharging;
		_charged.onCancel += InvokeCancelCharging;
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
		if ((Object)(object)base.owner.motion == (Object)(object)_finish || (Object)(object)base.owner.motion == (Object)(object)_earlyFinish)
		{
			return false;
		}
		if ((Object)(object)_charged != (Object)null && (Object)(object)base.owner.motion == (Object)(object)_charged)
		{
			DoMotion(_finish);
			return true;
		}
		if ((Object)(object)_earlyFinish != (Object)null && (Object)(object)base.owner.motion != (Object)(object)_earlyFinish && (Object)(object)base.owner.motion != (Object)(object)_finish)
		{
			if ((Object)(object)base.owner.motion == (Object)(object)_anticipation || (Object)(object)base.owner.motion == (Object)(object)_prepare)
			{
				ReserveEarlyFinish();
				return false;
			}
			DoMotion(_earlyFinish);
			return true;
		}
		base.owner.CancelAction();
		return false;
	}
}
