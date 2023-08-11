using System;
using System.Collections;
using System.Linq;
using Characters.Actions.Constraints;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class Motion : MonoBehaviour
{
	public enum SpeedMultiplierSource
	{
		Default,
		ForceBasic,
		ForceSkill,
		ForceMovement,
		ForceCharging,
		ForceBasicAndCharging,
		ForceSkillAndCharging
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<Motion>
	{
		public void EndBehaviour()
		{
			Motion[] components = base.components;
			for (int i = 0; i < components.Length; i++)
			{
				components[i].EndBehaviour();
			}
		}
	}

	[SerializeField]
	[GetComponentInParent(true)]
	private Action _action;

	[SerializeField]
	private string _key;

	[SerializeField]
	private CharacterAnimationController.AnimationInfo _animationInfo;

	[SerializeField]
	[Constraint.Subcomponent]
	private Constraint.Subcomponents _constraints;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private bool _blockMovement = true;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private bool _blockLook = true;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private bool _stay;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _length;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _speed = 1f;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private SpeedMultiplierSource _speedMultiplierSource;

	[SerializeField]
	[Range(0f, 1f)]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _speedMultiplierFactor = 1f;

	private float _runSpeed;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	private OperationInfo[] _operationInfos;

	private CoroutineReference _cRunOperations;

	public Character owner => action.owner;

	public Action action { get; set; }

	public string key => _key;

	public CharacterAnimationController.AnimationInfo animationInfo
	{
		get
		{
			return _animationInfo;
		}
		set
		{
			_animationInfo = animationInfo;
		}
	}

	public bool blockMovement => _blockMovement;

	public bool blockLook => _blockLook;

	public bool stay => _stay;

	public float length { get; set; }

	public float speed => _speed;

	public float time { get; protected set; }

	public float normalizedTime => time / length;

	public SpeedMultiplierSource speedMultiplierSource => _speedMultiplierSource;

	public float speedMultiplierFactor => _speedMultiplierFactor;

	public bool running { get; set; }

	public event System.Action onStart;

	public event System.Action onApply;

	public event System.Action onEnd;

	public event System.Action onCancel;

	public static Motion AddComponent(GameObject gameObject, Action action, bool blockLook, bool blockMovement)
	{
		Motion motion = gameObject.AddComponent<Motion>();
		motion.action = action;
		motion._blockLook = blockLook;
		motion._blockMovement = blockMovement;
		motion._constraints = new Constraint.Subcomponents();
		motion.Initialize(action);
		return motion;
	}

	private void Awake()
	{
		if (_animationInfo == null)
		{
			_animationInfo = new CharacterAnimationController.AnimationInfo();
		}
		if (_operations == null)
		{
			_operationInfos = new OperationInfo[0];
		}
		else if ((Object)(object)_action == (Object)null)
		{
			_operationInfos = ((SubcomponentArray<OperationInfo>)_operations).components.OrderBy((OperationInfo operation) => operation.timeToTrigger).ToArray();
		}
		else
		{
			_operationInfos = (from operation in _action.operations.Concat(((SubcomponentArray<OperationInfo>)_operations).components)
				orderby operation.timeToTrigger
				select operation).ToArray();
		}
		length = CaculateRealLength();
		for (int i = 0; i < _operationInfos.Length; i++)
		{
			_operationInfos[i].operation.Initialize();
		}
	}

	private void OnDestroy()
	{
		_animationInfo.Dispose();
		_action = null;
		for (int i = 0; i < _operationInfos.Length; i++)
		{
			_operationInfos[i] = null;
		}
		_operationInfos = null;
		_operations = null;
	}

	private void OnDisable()
	{
		CancelBehaviour();
	}

	private void StopAllOperations()
	{
		for (int i = 0; i < _operationInfos.Length; i++)
		{
			if (!_operationInfos[i].stay)
			{
				_operationInfos[i].operation.Stop();
			}
		}
	}

	public void Initialize(Action action)
	{
		this.action = action;
		for (int i = 0; i < ((SubcomponentArray<Constraint>)_constraints).components.Length; i++)
		{
			((SubcomponentArray<Constraint>)_constraints).components[i].Initilaize(action);
		}
	}

	public float CaculateRealLength()
	{
		if (_length > 0f)
		{
			return _length;
		}
		float num = 0f;
		for (int i = 0; i < ((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values.Length; i++)
		{
			if ((Object)(object)((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values[i].clip != (Object)null && ((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values[i].clip.length > num)
			{
				num = ((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values[i].clip.length;
			}
		}
		return num;
	}

	public void StartBehaviour(float speed)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (!running)
		{
			_runSpeed = speed;
			running = true;
			this.onStart?.Invoke();
			_cRunOperations = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRunOperations());
		}
	}

	public void EndBehaviour()
	{
		if (running)
		{
			time = length;
			StopAllOperations();
			running = false;
			((CoroutineReference)(ref _cRunOperations)).Stop();
			this.onEnd?.Invoke();
		}
	}

	public void CancelBehaviour()
	{
		if (running)
		{
			StopAllOperations();
			running = false;
			((CoroutineReference)(ref _cRunOperations)).Stop();
			this.onCancel?.Invoke();
		}
	}

	public override string ToString()
	{
		if (_animationInfo == null || ((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values == null || ((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values.Length == 0 || (Object)(object)((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values[0].clip == (Object)null)
		{
			return ((Object)this).ToString();
		}
		return "Motion (" + ((Object)((ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>)_animationInfo).values[0].clip).name + ")";
	}

	public IEnumerator CWaitForEndOfRunning()
	{
		while (running)
		{
			yield return null;
		}
	}

	private IEnumerator CRunOperations()
	{
		int operationIndex = 0;
		time = 0f;
		while (true)
		{
			if (operationIndex < _operationInfos.Length && time >= _operationInfos[operationIndex].timeToTrigger)
			{
				_operationInfos[operationIndex].operation.runSpeed = _runSpeed;
				_operationInfos[operationIndex].operation.Run(owner);
				operationIndex++;
			}
			else
			{
				yield return (object)new WaitForEndOfFrame();
				time += ((ChronometerBase)owner.chronometer.animation).deltaTime * _runSpeed;
			}
		}
	}

	internal bool PassConstraints()
	{
		return ((SubcomponentArray<Constraint>)_constraints).components.Pass();
	}

	internal void ConsumeConstraints()
	{
		action.ConsumeConstraints();
		((SubcomponentArray<Constraint>)_constraints).components.Consume();
	}
}
