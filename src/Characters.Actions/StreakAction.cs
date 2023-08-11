using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class StreakAction : Action
{
	[Subcomponent(true, typeof(Motion))]
	[HideInInspector]
	[SerializeField]
	private Motion _startMotion;

	[MinMaxSlider(0f, 1f)]
	[HideInInspector]
	[SerializeField]
	private Vector2 _mainInput = new Vector2(0f, 1f);

	[MinMaxSlider(0f, 1f)]
	[HideInInspector]
	[SerializeField]
	private Vector2 _mainCancel = new Vector2(0.9f, 1f);

	[HideInInspector]
	[SerializeField]
	private bool _blockLook = true;

	[HideInInspector]
	[Subcomponent(typeof(Motion))]
	[SerializeField]
	private Motion _motion;

	[HideInInspector]
	[SerializeField]
	private bool _cancelToEnd;

	[HideInInspector]
	[Subcomponent(true, typeof(Motion))]
	[SerializeField]
	private Motion _endMotion;

	[SerializeField]
	[HideInInspector]
	[Subcomponent(true, typeof(Motion))]
	private Motion _fullStreakEndMotion;

	private bool _startReserved;

	private bool _endReserved;

	private Character.LookingDirection _lookingDirection;

	public override Motion[] motions => new Motion[3] { _startMotion, _motion, _endMotion };

	public Motion motion => _motion;

	public override bool canUse
	{
		get
		{
			if (!base.cooldown.canUse)
			{
				return false;
			}
			if (_owner.stunedOrFreezed)
			{
				return false;
			}
			if (!PassAllConstraints(((Object)(object)_startMotion != (Object)null) ? _startMotion : _motion))
			{
				return false;
			}
			Motion runningMotion = base.owner.runningMotion;
			if ((Object)(object)runningMotion != (Object)null)
			{
				if ((Object)(object)runningMotion == (Object)(object)_endMotion)
				{
					return false;
				}
				if ((Object)(object)runningMotion == (Object)(object)_fullStreakEndMotion)
				{
					return false;
				}
			}
			return true;
		}
	}

	private void CacheLookingDirection()
	{
		_lookingDirection = base.owner.lookingDirection;
	}

	private void RestoreLookingDirection()
	{
		base.owner.lookingDirection = _lookingDirection;
	}

	private void Expire()
	{
		base.cooldown.streak.Expire();
		_onEnd?.Invoke();
	}

	protected override void Awake()
	{
		base.Awake();
		_motion.Initialize(this);
		base.cooldown.Serialize();
		if (_motion.blockLook && _blockLook)
		{
			_motion.onStart += RestoreLookingDirection;
		}
		else if ((Object)(object)_startMotion != (Object)null)
		{
			_motion.onStart += delegate
			{
				if ((Object)(object)base.owner.runningMotion == (Object)(object)_startMotion)
				{
					RestoreLookingDirection();
				}
			};
		}
		_motion.onEnd += OnMotionEnd;
		if ((Object)(object)_startMotion != (Object)null)
		{
			_startMotion.Initialize(this);
			_startMotion.onEnd += delegate
			{
				DoMotion(_motion);
			};
		}
		if ((Object)(object)_endMotion != (Object)null)
		{
			_endMotion.Initialize(this);
			if (_endMotion.blockLook && _blockLook)
			{
				_endMotion.onStart += RestoreLookingDirection;
			}
			_endMotion.onEnd += Expire;
		}
		if ((Object)(object)_fullStreakEndMotion != (Object)null)
		{
			_fullStreakEndMotion.Initialize(this);
			if (_fullStreakEndMotion.blockLook && _blockLook)
			{
				_fullStreakEndMotion.onStart += RestoreLookingDirection;
			}
			_fullStreakEndMotion.onEnd += Expire;
		}
		else if ((Object)(object)_endMotion != (Object)null)
		{
			_fullStreakEndMotion = _endMotion;
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
	}

	private void OnMotionEnd()
	{
		if ((Object)(object)_fullStreakEndMotion != (Object)null && base.cooldown.streak.count > 0 && base.cooldown.streak.remains == 0)
		{
			DoMotion(_fullStreakEndMotion);
		}
		else if (_endReserved || _inputMethod != 0)
		{
			if ((Object)(object)_endMotion != (Object)null)
			{
				DoMotion(_endMotion);
			}
			else
			{
				Expire();
			}
		}
		else if (_inputMethod == InputMethod.TryStartIsPressed && base.cooldown.streak.remains > 0 && ConsumeCooldownIfNeeded())
		{
			DoMotion(_motion);
		}
	}

	public override bool TryStart()
	{
		Motion runningMotion = _owner.runningMotion;
		if ((Object)(object)runningMotion != (Object)null && ((Object)(object)runningMotion == (Object)(object)_endMotion || (Object)(object)runningMotion == (Object)(object)_fullStreakEndMotion))
		{
			return false;
		}
		if (HandleWasPressed())
		{
			return true;
		}
		if (((Object)(object)_startMotion != (Object)null && (Object)(object)runningMotion == (Object)(object)_startMotion) || (Object)(object)runningMotion == (Object)(object)_motion)
		{
			return false;
		}
		if (base.cooldown.streak.remains > 0)
		{
			return false;
		}
		if (!canUse)
		{
			return false;
		}
		if (!ConsumeCooldownIfNeeded())
		{
			return false;
		}
		CacheLookingDirection();
		if ((Object)(object)_startMotion != (Object)null)
		{
			DoAction(_startMotion);
		}
		else
		{
			DoAction(_motion);
		}
		_startReserved = false;
		_endReserved = false;
		return true;
	}

	private bool HandleWasPressed()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (_inputMethod != InputMethod.TryStartWasPressed)
		{
			return false;
		}
		if (_startReserved)
		{
			return false;
		}
		if (!base.cooldown.canUse)
		{
			return false;
		}
		if (base.cooldown.streak.remains == 0)
		{
			return false;
		}
		Motion runningMotion = _owner.runningMotion;
		if ((Object)(object)runningMotion == (Object)null)
		{
			return false;
		}
		if ((Object)(object)runningMotion != (Object)(object)_motion)
		{
			return false;
		}
		if (_mainInput.x == _mainInput.y)
		{
			return false;
		}
		if (!MMMaths.Range(runningMotion.normalizedTime, _mainInput))
		{
			return false;
		}
		if (MMMaths.Range(runningMotion.normalizedTime, _mainCancel))
		{
			if (ConsumeCooldownIfNeeded())
			{
				DoMotion(_motion);
			}
		}
		else
		{
			_startReserved = true;
			((MonoBehaviour)this).StartCoroutine(CReservedAttack());
		}
		return true;
	}

	private IEnumerator CReservedAttack()
	{
		while ((Object)(object)_owner.runningMotion == (Object)(object)_motion && _startReserved)
		{
			yield return null;
			if (MMMaths.Range(_owner.runningMotion.normalizedTime, _mainCancel))
			{
				if (ConsumeCooldownIfNeeded())
				{
					DoMotion(_motion);
				}
				break;
			}
		}
		_startReserved = false;
	}

	protected override void ForceEndProcess()
	{
		_needForceEnd = false;
		Motion[] array = motions;
		foreach (Motion obj in array)
		{
			if (((object)base.owner.motion).Equals((object?)obj))
			{
				base.cooldown.streak.Expire();
				base.owner.CancelAction();
				_onEnd?.Invoke();
				break;
			}
		}
	}

	public override bool TryEnd()
	{
		if (_inputMethod != 0)
		{
			return false;
		}
		_endReserved = true;
		if (_cancelToEnd)
		{
			if ((Object)(object)_endMotion == (Object)null)
			{
				base.cooldown.streak.Expire();
				base.owner.CancelAction();
				_onEnd?.Invoke();
				return false;
			}
			DoMotion(_endMotion);
			return true;
		}
		return false;
	}
}
