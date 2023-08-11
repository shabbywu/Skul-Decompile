using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.Constraints;
using Characters.Operations;
using Level;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Spirits;

public class Spirit : AbilityAttacher, IAbility, IAbilityInstance
{
	private static readonly Vector3 _leftScale = new Vector3(-1f, 1f, 1f);

	private static readonly Vector3 _rightScale = new Vector3(1f, 1f, 1f);

	[SerializeField]
	[Constraint.Subcomponent]
	private Constraint.Subcomponents _constraints;

	[SerializeField]
	private Collider2D _detectRange;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

	[Space]
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite _icon;

	[Space]
	[SerializeField]
	private float _trackSpeed = 2.5f;

	[SerializeField]
	protected float _attackInterval = 6f;

	[Space]
	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	private OperationInfo[] _operationInfos;

	private float _remainTimeToNextAttack;

	private bool _waitForTarget;

	private Vector3 _position;

	[NonSerialized]
	public Transform targetPosition;

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => _icon;

	public virtual float iconFillAmount
	{
		get
		{
			if (!_waitForTarget)
			{
				return _remainTimeToNextAttack / _attackInterval;
			}
			return 0f;
		}
	}

	public bool iconFillInversed => false;

	public bool iconFillFlipped => true;

	public virtual int iconStacks => 0;

	public bool expired => false;

	public float duration => float.PositiveInfinity;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public void Initialize()
	{
	}

	public virtual void UpdateTime(float deltaTime)
	{
		_remainTimeToNextAttack -= deltaTime * (float)base.owner.stat.GetFinal(Stat.Kind.SpiritAttackCooldownSpeed);
		if (_remainTimeToNextAttack <= 0f)
		{
			if (!FindTarget())
			{
				_waitForTarget = true;
				_remainTimeToNextAttack = 0.5f;
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CTimer());
				_waitForTarget = false;
				_remainTimeToNextAttack += _attackInterval;
			}
		}
	}

	protected virtual void Awake()
	{
		_remainTimeToNextAttack = _attackInterval;
		_operationInfos = ((SubcomponentArray<OperationInfo>)_operations).components.OrderBy((OperationInfo operation) => operation.timeToTrigger).ToArray();
		for (int i = 0; i < _operationInfos.Length; i++)
		{
			_operationInfos[i].operation.Initialize();
		}
	}

	private void OnEnable()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded += ResetPosition;
	}

	private void OnDisable()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= ResetPosition;
		}
	}

	private void ResetPosition()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_position = ((Component)targetPosition).transform.position;
		((Component)this).transform.position = _position;
	}

	private void Update()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		_position = Vector3.Lerp(_position, targetPosition.position, ((ChronometerBase)base.owner.chronometer.master).deltaTime * _trackSpeed);
		((Component)this).transform.localScale = ((targetPosition.position.x - _position.x < 0f) ? _leftScale : _rightScale);
	}

	private void LateUpdate()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (attached)
		{
			((Component)this).transform.position = _position;
		}
	}

	private bool FindTarget()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)base.owner).gameObject));
		((Behaviour)_detectRange).enabled = true;
		_overlapper.OverlapCollider(_detectRange);
		if (GetComponentExtension.GetComponents<Collider2D, Target>((IEnumerable<Collider2D>)_overlapper.results, true).Count == 0)
		{
			return false;
		}
		return true;
	}

	private IEnumerator CTimer()
	{
		int operationIndex = 0;
		float time = 0f;
		while (true)
		{
			if (operationIndex < _operationInfos.Length && time >= _operationInfos[operationIndex].timeToTrigger)
			{
				_operationInfos[operationIndex].operation.Run(base.owner);
				operationIndex++;
			}
			else
			{
				yield return null;
				time += ((ChronometerBase)base.owner.chronometer.animation).deltaTime;
			}
		}
	}

	private void OnMapChanged(Map old, Map @new)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)targetPosition != (Object)null)
		{
			_position = targetPosition.position;
		}
	}

	public override void OnIntialize()
	{
	}

	public override void StartAttach()
	{
		_waitForTarget = false;
		if ((Object)(object)base.owner != (Object)null)
		{
			base.owner.ability.Add(this);
		}
	}

	public override void StopAttach()
	{
		if ((Object)(object)base.owner != (Object)null)
		{
			base.owner.ability.Remove(this);
		}
	}

	public void Refresh()
	{
	}

	public void Attach()
	{
		base.owner.playerComponents.inventory.item.AttachSpirit(this);
		ResetPosition();
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += OnMapChanged;
	}

	public void Detach()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		base.owner.playerComponents.inventory.item.DetachSpirit(this);
		((Component)this).transform.localPosition = Vector3.zero;
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= OnMapChanged;
	}
}
