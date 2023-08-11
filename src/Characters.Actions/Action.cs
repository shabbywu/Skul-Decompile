using System;
using System.Collections;
using Characters.Actions.Constraints;
using Characters.Controllers;
using Characters.Cooldowns;
using Characters.Operations;
using Data;
using InControl;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public abstract class Action : MonoBehaviour
{
	public enum Type
	{
		Dash,
		BasicAttack,
		JumpAttack,
		Jump,
		Skill,
		Swap,
		Custom
	}

	protected enum InputMethod
	{
		TryStartIsPressed,
		TryStartWasPressed,
		TryStartWasReleased,
		NotUsed
	}

	protected System.Action _onStart;

	protected System.Action _onEnd;

	protected System.Action _onCancel;

	protected Character _owner;

	protected PlayerInput _input;

	protected bool _needForceEnd;

	[Tooltip("숫자가 작을수록 우선순위가 높음")]
	[SerializeField]
	private int _priority;

	[SerializeField]
	private Type _type = Type.Skill;

	[SerializeField]
	[Button.StringPopup]
	private int _button;

	[SerializeField]
	protected InputMethod _inputMethod;

	[SerializeField]
	private bool _cancelOnGround;

	[SerializeField]
	[Tooltip("액션의 시작 구간을 수동으로 설정합니다. 액션이 시작될 때 관련 사용효과가 발동되고 쿨다운이 감소합니다.")]
	private bool _triggerStartManually;

	[SerializeField]
	private CooldownSerializer _cooldown;

	[Constraint.Subcomponent]
	[SerializeField]
	protected Constraint.Subcomponents _constraints;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	public Character owner => _owner;

	public int priority => _priority;

	public Button button
	{
		get
		{
			return Button.values[_button];
		}
		set
		{
			_button = value.index;
		}
	}

	protected PlayerAction defaultButton => _input[_button];

	internal OperationInfo[] operations => ((SubcomponentArray<OperationInfo>)_operations).components;

	public bool running
	{
		get
		{
			if ((Object)(object)_owner.runningMotion != (Object)null)
			{
				return (Object)(object)_owner.runningMotion.action == (Object)(object)this;
			}
			return false;
		}
	}

	public abstract bool canUse { get; }

	public Type type => _type;

	public float extraSpeedMultiplier { get; set; } = 1f;


	public abstract Motion[] motions { get; }

	public CooldownSerializer cooldown => _cooldown;

	protected bool consumeCooldownManually => _triggerStartManually;

	public event System.Action onStart
	{
		add
		{
			_onStart = (System.Action)Delegate.Combine(_onStart, value);
		}
		remove
		{
			_onStart = (System.Action)Delegate.Remove(_onStart, value);
		}
	}

	public event System.Action onEnd
	{
		add
		{
			_onEnd = (System.Action)Delegate.Combine(_onEnd, value);
		}
		remove
		{
			_onEnd = (System.Action)Delegate.Remove(_onEnd, value);
		}
	}

	private void OnDestroy()
	{
		if (_cancelOnGround)
		{
			_owner.movement.onGrounded -= OnGrounded;
		}
		_owner = null;
		_onStart = null;
		_onEnd = null;
		_onCancel = null;
	}

	protected virtual void Awake()
	{
		Array.Sort(((SubcomponentArray<OperationInfo>)_operations).components, (OperationInfo x, OperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
	}

	public virtual void Initialize(Character owner)
	{
		_cooldown.Serialize();
		if (_cooldown.type == CooldownSerializer.Type.Time)
		{
			switch (_type)
			{
			case Type.Dash:
				_cooldown.time.GetCooldownSpeed = owner.stat.GetDashCooldownSpeed;
				break;
			case Type.Skill:
				_cooldown.time.GetCooldownSpeed = owner.stat.GetSkillCooldownSpeed;
				break;
			}
		}
		_owner = owner;
		_input = ((Component)_owner).GetComponent<PlayerInput>();
		for (int i = 0; i < ((SubcomponentArray<Constraint>)_constraints).components.Length; i++)
		{
			((SubcomponentArray<Constraint>)_constraints).components[i].Initilaize(this);
		}
		if (_cancelOnGround)
		{
			_owner.movement.onGrounded -= OnGrounded;
			_owner.movement.onGrounded += OnGrounded;
		}
		if (_cooldown.streak.count > 0)
		{
			_owner.playerComponents.inventory.weapon.onSwap -= _cooldown.streak.Expire;
			_owner.playerComponents.inventory.weapon.onSwap += _cooldown.streak.Expire;
		}
	}

	private void OnGrounded()
	{
		if ((Object)(object)_owner.runningMotion != (Object)null && (Object)(object)_owner.runningMotion.action == (Object)(object)this)
		{
			_owner.CancelAction();
		}
	}

	public virtual bool Process()
	{
		if (!((Component)this).gameObject.activeInHierarchy || defaultButton == null)
		{
			return false;
		}
		if (_needForceEnd)
		{
			ForceEndProcess();
			return false;
		}
		if (type == Type.Skill && _owner.silence.value)
		{
			return false;
		}
		if (GameData.Settings.arrowDashEnabled && type == Type.Dash && (_input.left.IsDoublePressed || _input.right.IsDoublePressed) && TryStart())
		{
			return true;
		}
		if ((_inputMethod == InputMethod.TryStartIsPressed && ((OneAxisInputControl)defaultButton).IsPressed) || (_inputMethod == InputMethod.TryStartWasPressed && ((OneAxisInputControl)defaultButton).WasPressed) || (_inputMethod == InputMethod.TryStartWasReleased && ((OneAxisInputControl)defaultButton).WasReleased))
		{
			return TryStart();
		}
		if ((Object)(object)_owner.motion != (Object)null && (Object)(object)_owner.motion.action == (Object)(object)this && (_inputMethod == InputMethod.TryStartIsPressed || _inputMethod == InputMethod.TryStartWasPressed) && ((OneAxisInputControl)defaultButton).WasReleased)
		{
			return TryEnd();
		}
		return false;
	}

	public abstract bool TryStart();

	public virtual bool TryEnd()
	{
		_needForceEnd = false;
		return false;
	}

	public void ForceEnd()
	{
		_needForceEnd = true;
	}

	protected virtual void ForceEndProcess()
	{
		_needForceEnd = false;
		Motion[] array = motions;
		foreach (Motion obj in array)
		{
			if (((object)owner.motion).Equals((object?)obj))
			{
				owner.CancelAction();
				_owner.motion.action = null;
				break;
			}
		}
	}

	protected float GetSpeedMultiplier(Motion motion)
	{
		float num = 1f;
		switch (motion.speedMultiplierSource)
		{
		case Motion.SpeedMultiplierSource.Default:
			switch (type)
			{
			case Type.BasicAttack:
			case Type.JumpAttack:
				num = _owner.stat.GetInterpolatedBasicAttackSpeed();
				break;
			case Type.Skill:
				num = _owner.stat.GetInterpolatedSkillAttackSpeed();
				break;
			}
			break;
		case Motion.SpeedMultiplierSource.ForceBasic:
			num = _owner.stat.GetInterpolatedBasicAttackSpeed();
			break;
		case Motion.SpeedMultiplierSource.ForceSkill:
			num = _owner.stat.GetInterpolatedSkillAttackSpeed();
			break;
		case Motion.SpeedMultiplierSource.ForceMovement:
			num = _owner.stat.GetInterpolatedMovementSpeed();
			break;
		case Motion.SpeedMultiplierSource.ForceCharging:
			num = _owner.stat.GetInterpolatedChargingSpeed();
			break;
		case Motion.SpeedMultiplierSource.ForceBasicAndCharging:
			num = _owner.stat.GetInterpolatedBasicAttackChargingSpeed();
			break;
		case Motion.SpeedMultiplierSource.ForceSkillAndCharging:
			num = _owner.stat.GetInterpolatedSkillAttackChargingSpeed();
			break;
		}
		return extraSpeedMultiplier * ((num - 1f) * motion.speedMultiplierFactor + 1f);
	}

	protected void DoAction(Motion motion)
	{
		motion.action = this;
		_owner.DoAction(motion, GetSpeedMultiplier(motion), !_triggerStartManually);
		motion.ConsumeConstraints();
		if (!_triggerStartManually)
		{
			_onStart?.Invoke();
		}
	}

	protected void DoActionNonBlock(Motion motion)
	{
		motion.action = this;
		_owner.DoActionNonBlock(motion);
		motion.ConsumeConstraints();
		_onStart?.Invoke();
	}

	protected void DoMotion(Motion motion)
	{
		motion.action = this;
		_owner.DoMotion(motion, GetSpeedMultiplier(motion));
		motion.ConsumeConstraints();
	}

	public IEnumerator CWaitForEndOfRunning()
	{
		if (!((Object)(object)_owner.runningMotion == (Object)null) && !((Object)(object)_owner.runningMotion.action != (Object)(object)this))
		{
			yield return _owner.runningMotion.CWaitForEndOfRunning();
		}
	}

	public void TriggerStartManually()
	{
		_owner.TriggerOnStartActionManually(this);
		_cooldown.Consume();
		_onStart?.Invoke();
	}

	internal bool PassAllConstraints(Motion motion)
	{
		if (((SubcomponentArray<Constraint>)_constraints).components.Pass())
		{
			return motion.PassConstraints();
		}
		return false;
	}

	internal bool PassConstraints(Motion motion)
	{
		return ((SubcomponentArray<Constraint>)_constraints).components.Pass();
	}

	internal void ConsumeConstraints()
	{
		((SubcomponentArray<Constraint>)_constraints).components.Consume();
	}

	protected bool ConsumeCooldownIfNeeded()
	{
		if (_triggerStartManually)
		{
			return true;
		}
		return _cooldown.Consume();
	}
}
