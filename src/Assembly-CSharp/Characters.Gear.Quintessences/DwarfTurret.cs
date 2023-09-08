using System.Collections;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using Level;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences;

public class DwarfTurret : DestructibleObject
{
	[SerializeField]
	private Minion _minion;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private Collider2D _sight;

	[Header("Intro")]
	[SerializeField]
	private Action _intro;

	[Header("Attack")]
	[SerializeField]
	private float _attackInterval = 1f;

	[SerializeField]
	private Action _action;

	[SerializeField]
	private Action _fastAction;

	[Header("Laser Attack")]
	[SerializeField]
	private float _laserAttackInterval = 5f;

	[SerializeField]
	private Action _laserAction;

	[Header("Attack Speed")]
	[SerializeField]
	private float _attackSpeedMultiplier;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _attackSpeedAbility;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onHitOperations;

	private float _remainAttackTime;

	private float _remainLaserAttackTime;

	private IAbilityInstance _instance;

	private NonAllocOverlapper _overlapper;

	public override Collider2D collider => _collider;

	public Minion minion => _minion;

	public override void Hit(Character from, ref Damage damage, Vector2 force)
	{
		if ((damage.motionType == Damage.MotionType.Basic || damage.motionType == Damage.MotionType.Dash) && from.type == Character.Type.Player)
		{
			_instance = _minion.character.ability.Add(_attackSpeedAbility.ability);
			((MonoBehaviour)this).StartCoroutine(_onHitOperations.CRun(_minion.character));
		}
	}

	private void Awake()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(31);
		minion.onSummon += delegate
		{
			_remainAttackTime = _attackInterval;
			_remainLaserAttackTime = _laserAttackInterval;
			((MonoBehaviour)this).StartCoroutine(CUpdate());
		};
	}

	private IEnumerator CUpdate()
	{
		_intro.TryStart();
		while (_intro.running)
		{
			yield return null;
		}
		while (true)
		{
			float num = _minion.character.chronometer.master.deltaTime;
			bool hasTarget = true;
			HandleLaserAttack(num, hasTarget);
			if (_instance != null && _instance.attached)
			{
				num *= _attackSpeedMultiplier;
			}
			HandleAttack(num, hasTarget);
			yield return null;
		}
	}

	private void HandleAttack(float deltaTime, bool hasTarget)
	{
		_remainAttackTime -= deltaTime;
		if (_remainAttackTime > 0f)
		{
			return;
		}
		_remainAttackTime += _attackInterval;
		if (hasTarget)
		{
			if (_minion.character.ability.Contains(_attackSpeedAbility.ability))
			{
				_fastAction.TryStart();
			}
			else
			{
				_action.TryStart();
			}
		}
	}

	private void HandleLaserAttack(float deltaTime, bool hasTarget)
	{
		if ((Object)(object)_laserAction == (Object)null)
		{
			return;
		}
		_remainLaserAttackTime -= deltaTime;
		if (!(_remainLaserAttackTime > 0f))
		{
			_remainLaserAttackTime += _laserAttackInterval;
			if (hasTarget)
			{
				_laserAction.TryStart();
			}
		}
	}

	private Character FindTarget()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
		return TargetFinder.FindClosestTarget(_overlapper, _sight, _collider);
	}
}
