using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Actions;
using Characters.Movements;
using Hardmode;
using Level;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class DarkAideAI : AIController
{
	private enum Pattern
	{
		GoldenMeteor,
		MeteorAir,
		Rush,
		DimensionPierce,
		Homing,
		MeteorGround,
		Idle,
		SkippableIdle
	}

	[SerializeField]
	private Transform _minHeightTransform;

	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	private Transform _body;

	[SerializeField]
	private Transform _teleportDestination;

	[SerializeField]
	[Space]
	[Header("GoldmaneMeteor")]
	private Characters.Actions.Action _goldenMeteorJump;

	[SerializeField]
	private Characters.Actions.Action _goldenMeteorReady;

	[SerializeField]
	private Characters.Actions.Action _goldenMeteorAttack;

	[SerializeField]
	private Characters.Actions.Action _goldenMeteorLanding;

	private int _countOfGoldenMeteor = 3;

	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2Int _countOfGoldenMeteorRange;

	[SerializeField]
	private float _heightOfGoldenMeteor;

	[SerializeField]
	private float _durationOfGoldenMeteor;

	[Header("MeteorInTheAir")]
	[Space]
	[SerializeField]
	private Characters.Actions.Action _meteorInAirJumpAndReady;

	[SerializeField]
	private Characters.Actions.Action _meteorInAirAttack;

	[SerializeField]
	private Characters.Actions.Action _meteorInAirLandingAndStanding;

	[MinMaxSlider(0f, 10f)]
	[SerializeField]
	private Vector2Int _countOfMeteorInAirRange;

	private int _countOfMeteorInAir = 2;

	[SerializeField]
	private float _durationOfMeteorInAir;

	[SerializeField]
	private float _durationOfMeteorInAirHardmode;

	[SerializeField]
	private float _distanceToPlayerOfMeteorInAir;

	[MinMaxSlider(-180f, 180f)]
	[SerializeField]
	private Vector2 _angleOfMeteorInAir;

	[SerializeField]
	[Subcomponent(typeof(Teleport))]
	private Teleport _teleportForAir;

	[Header("MeteorInTheGround2")]
	[SerializeField]
	[Space]
	private Characters.Actions.Action _meteorInGround2Ready;

	[SerializeField]
	private Characters.Actions.Action _meteorInGround2Attack;

	[SerializeField]
	private Characters.Actions.Action _meteorInGround2Landing;

	[SerializeField]
	private Characters.Actions.Action _meteorInGround2Standing;

	[SerializeField]
	private float _durationOfMeteorInGround2;

	[SerializeField]
	[Subcomponent(typeof(ThunderAttack))]
	private ThunderAttack _thunderAttack;

	[SerializeField]
	[Subcomponent(typeof(RangeAttack))]
	[Header("RangeAttackHoming")]
	[Space]
	private RangeAttack _rangeAttack;

	[Subcomponent(typeof(Teleport))]
	[SerializeField]
	[Space]
	[Header("BackStep")]
	private Teleport _backStepTeleport;

	[Header("Rush")]
	[SerializeField]
	[Space]
	[Subcomponent(typeof(DarkRush))]
	private DarkRush _darkRush;

	[SerializeField]
	private float _darkRushPredelay = 15f;

	private bool _darkRushPredelayEnd;

	[Header("Dimension Piece")]
	[SerializeField]
	private Characters.Actions.Action _dimensionPierce;

	[SerializeField]
	private Characters.Actions.Action _dimensionPierceCoolTimeAction;

	[SerializeField]
	private Transform _dimensionPiercePoint;

	[Space]
	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2Int _dimensionPierceCountRange;

	private int _dimensionPierceCount;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	[Space]
	[Header("Idle")]
	private Idle _idle;

	[SerializeField]
	[Subcomponent(typeof(SkipableIdle))]
	private SkipableIdle _skippableIdle;

	[SerializeField]
	[Header("Tools")]
	private Collider2D _meleeAttackTrigger;

	private float platformWidth;

	private CharacterController2D.CollisionState _collisionState;

	[Range(0f, 10f)]
	[SerializeField]
	[Header("Pattern Proportion")]
	private int _goldenMeteorPercent;

	[SerializeField]
	[Range(0f, 10f)]
	private int _meteorAirPercent;

	[SerializeField]
	[Range(0f, 10f)]
	private int _meteorGroundPercent;

	[Range(0f, 10f)]
	[SerializeField]
	private int _homingPercent;

	private List<Pattern> _patterns;

	private new void Start()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		_collisionState = character.movement.controller.collisionState;
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		platformWidth = ((Bounds)(ref bounds)).size.x;
		_patterns = new List<Pattern>(_goldenMeteorPercent + _meteorAirPercent + _meteorGroundPercent + _homingPercent);
		for (int i = 0; i < _goldenMeteorPercent; i++)
		{
			_patterns.Add(Pattern.GoldenMeteor);
		}
		for (int j = 0; j < _meteorAirPercent; j++)
		{
			_patterns.Add(Pattern.MeteorAir);
		}
		for (int k = 0; k < _meteorGroundPercent; k++)
		{
			_patterns.Add(Pattern.MeteorGround);
		}
		for (int l = 0; l < _homingPercent; l++)
		{
			_patterns.Add(Pattern.Homing);
		}
		character.health.onDiedTryCatch += delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_body.rotation = Quaternion.identity;
		};
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return Chronometer.global.WaitForSeconds(1f);
		character.movement.config.type = Movement.Config.Type.Walking;
		((MonoBehaviour)this).StartCoroutine(CStartPredelay());
		while (!base.dead)
		{
			yield return Combat();
		}
	}

	public void ApplyHealth(Character healthOwner)
	{
		if (healthOwner.health.currentHealth > 0.0)
		{
			character.health.SetCurrentHealth(healthOwner.health.currentHealth);
			character.health.PercentHeal(0.4f);
		}
		else
		{
			character.health.SetCurrentHealth(healthOwner.health.maximumHealth);
		}
	}

	private IEnumerator RunPattern(Pattern pattern)
	{
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		switch (pattern)
		{
		case Pattern.GoldenMeteor:
			yield return CastGoldenMeteor();
			break;
		case Pattern.MeteorAir:
			yield return CastMeteorInAir();
			break;
		case Pattern.DimensionPierce:
			yield return CastDimensionPierce();
			break;
		case Pattern.Rush:
			yield return CastRush();
			break;
		case Pattern.MeteorGround:
			yield return CastBackstep();
			yield return CastMeteorInGround2();
			break;
		case Pattern.Homing:
			yield return CastRangeAttackHoming();
			break;
		case Pattern.Idle:
			yield return CastIdle();
			break;
		case Pattern.SkippableIdle:
			yield return CastSkippableIdle();
			break;
		}
	}

	private IEnumerator Combat()
	{
		if (CanUseDarkDimensionRush())
		{
			yield return RunPattern(Pattern.Rush);
			yield return RunPattern(Pattern.Idle);
			yield break;
		}
		if (CanUseDarkDimensionPierce())
		{
			yield return RunPattern(Pattern.DimensionPierce);
			yield return RunPattern(Pattern.SkippableIdle);
			yield break;
		}
		Pattern pattern = _patterns.Random();
		yield return RunPattern(pattern);
		switch (pattern)
		{
		case Pattern.GoldenMeteor:
			yield return RunPattern(Pattern.Idle);
			break;
		case Pattern.MeteorAir:
			yield return RunPattern(Pattern.Idle);
			break;
		case Pattern.MeteorGround:
			yield return RunPattern(Pattern.SkippableIdle);
			break;
		case Pattern.Homing:
			yield return RunPattern(Pattern.SkippableIdle);
			break;
		}
	}

	private IEnumerator CastIdle()
	{
		yield return _idle.CRun(this);
	}

	private IEnumerator CastSkippableIdle()
	{
		yield return _skippableIdle.CRun(this);
	}

	public IEnumerator CastGoldenMeteor()
	{
		Bounds platform = _collisionState.lastStandingCollider.bounds;
		_countOfGoldenMeteor = Random.Range(((Vector2Int)(ref _countOfGoldenMeteorRange)).x, ((Vector2Int)(ref _countOfGoldenMeteorRange)).y);
		Vector2 val = default(Vector2);
		Vector2 val3 = default(Vector2);
		for (int i = 0; i < _countOfGoldenMeteor; i++)
		{
			((Vector2)(ref val))._002Ector(((Component)base.target).transform.position.x, ((Bounds)(ref platform)).max.y + _heightOfGoldenMeteor);
			((Component)_teleportDestination).transform.position = Vector2.op_Implicit(val);
			yield return _teleportForAir.CRun(this);
			_goldenMeteorJump.TryStart();
			while (_goldenMeteorJump.running)
			{
				yield return null;
			}
			_goldenMeteorReady.TryStart();
			while (_goldenMeteorReady.running)
			{
				yield return null;
			}
			_goldenMeteorAttack.TryStart();
			Vector2 val2 = Vector2.op_Implicit(((Component)character).transform.position);
			((Vector2)(ref val3))._002Ector(((Component)character).transform.position.x, ((Bounds)(ref platform)).max.y);
			yield return MoveToDestination(Vector2.op_Implicit(val2), Vector2.op_Implicit(val3), _goldenMeteorAttack, _durationOfGoldenMeteor);
			_goldenMeteorLanding.TryStart();
			while (_goldenMeteorLanding.running)
			{
				yield return null;
			}
		}
	}

	public IEnumerator CastMeteorInAir()
	{
		Bounds platform = _collisionState.lastStandingCollider.bounds;
		float duration = (Singleton<HardmodeManager>.Instance.hardmode ? _durationOfMeteorInAirHardmode : _durationOfMeteorInAir);
		_countOfMeteorInAir = Random.Range(((Vector2Int)(ref _countOfMeteorInAirRange)).x, ((Vector2Int)(ref _countOfMeteorInAirRange)).y);
		for (int i = 0; i < _countOfMeteorInAir; i++)
		{
			Vector2 position = GetMeteorAirStartPosition();
			while (character.stunedOrFreezed)
			{
				yield return null;
			}
			((Component)_teleportDestination).transform.position = Vector2.op_Implicit(position);
			yield return _teleportForAir.CRun(this);
			Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
			Vector2 dest = new Vector2(((Component)base.target).transform.position.x, ((Bounds)(ref platform)).max.y);
			character.ForceToLookAt(dest.x);
			while (character.stunedOrFreezed)
			{
				yield return null;
			}
			_meteorInAirJumpAndReady.TryStart();
			while (_meteorInAirJumpAndReady.running)
			{
				yield return null;
			}
			while (character.stunedOrFreezed)
			{
				yield return null;
			}
			_meteorInAirAttack.TryStart();
			yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _meteorInAirAttack, duration, rotate: true, interporate: false);
			while (_meteorInAirAttack.running)
			{
				yield return null;
			}
			while (character.stunedOrFreezed)
			{
				yield return null;
			}
			_meteorInAirLandingAndStanding.TryStart();
			while (_meteorInAirLandingAndStanding.running)
			{
				yield return null;
			}
		}
	}

	private Vector2 GetMeteorAirStartPosition()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		float distanceToPlayerOfMeteorInAir = _distanceToPlayerOfMeteorInAir;
		Vector2 val = Clamp();
		float angle = Random.Range(val.x, val.y);
		Vector2 val2 = RotateVector(Vector2.right, angle) * distanceToPlayerOfMeteorInAir;
		Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
		return new Vector2(((Component)base.target).transform.position.x, ((Bounds)(ref bounds)).max.y) + val2;
	}

	private Vector2 Clamp()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Vector2 angleOfMeteorInAir = _angleOfMeteorInAir;
		angleOfMeteorInAir = MinClamp(angleOfMeteorInAir);
		return MaxClamp(angleOfMeteorInAir);
	}

	private Vector2 MinClamp(Vector2 angle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		float distanceToPlayerOfMeteorInAir = _distanceToPlayerOfMeteorInAir;
		Vector2 val = RotateVector(Vector2.right, _angleOfMeteorInAir.x) * distanceToPlayerOfMeteorInAir;
		float x = (Vector2.op_Implicit(((Component)base.target).transform.position) + val).x;
		Bounds bounds = Map.Instance.bounds;
		if (x >= ((Bounds)(ref bounds)).max.x)
		{
			return new Vector2(90f, _angleOfMeteorInAir.y);
		}
		return angle;
	}

	private Vector2 MaxClamp(Vector2 angle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		float distanceToPlayerOfMeteorInAir = _distanceToPlayerOfMeteorInAir;
		Vector2 val = RotateVector(Vector2.right, _angleOfMeteorInAir.y) * distanceToPlayerOfMeteorInAir;
		float x = (Vector2.op_Implicit(((Component)base.target).transform.position) + val).x;
		Bounds bounds = Map.Instance.bounds;
		if (x <= ((Bounds)(ref bounds)).min.x)
		{
			return new Vector2(_angleOfMeteorInAir.x, 90f);
		}
		return angle;
	}

	private Vector2 RotateVector(Vector2 v, float angle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		float num = angle * ((float)Math.PI / 180f);
		float num2 = v.x * Mathf.Cos(num) - v.y * Mathf.Sin(num);
		float num3 = v.x * Mathf.Sin(num) + v.y * Mathf.Cos(num);
		return new Vector2(num2, num3);
	}

	public IEnumerator CastMeteorInGround2()
	{
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
		float num = ((source.x > ((Bounds)(ref bounds)).center.x) ? (((Bounds)(ref bounds)).min.x + 3f) : (((Bounds)(ref bounds)).max.x - 3f));
		Vector2 dest = new Vector2(num, ((Component)character).transform.position.y);
		character.ForceToLookAt(num);
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Ready.TryStart();
		while (_meteorInGround2Ready.running)
		{
			yield return null;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Attack.TryStart();
		yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _meteorInGround2Attack, _durationOfMeteorInGround2);
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Landing.TryStart();
		yield return _thunderAttack.CRun(this);
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		while (_meteorInGround2Landing.running)
		{
			yield return null;
		}
		_meteorInGround2Standing.TryStart();
		while (_meteorInGround2Standing.running)
		{
			yield return null;
		}
	}

	public IEnumerator CastRush()
	{
		yield return _darkRush.CRun(this);
	}

	public IEnumerator CastRangeAttackHoming()
	{
		yield return _rangeAttack.CRun(this);
	}

	public IEnumerator CastBackstep()
	{
		Bounds platformBounds = _collisionState.lastStandingCollider.bounds;
		float destinationX = ((((Bounds)(ref platformBounds)).center.x > ((Component)character).transform.position.x) ? (((Bounds)(ref platformBounds)).max.x - 1f) : (((Bounds)(ref platformBounds)).min.x + 1f));
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(destinationX, ((Bounds)(ref platformBounds)).max.y);
		((Component)_teleportDestination).transform.position = Vector2.op_Implicit(val);
		character.ForceToLookAt((destinationX > ((Bounds)(ref platformBounds)).center.x) ? ((Bounds)(ref platformBounds)).max.x : ((Bounds)(ref platformBounds)).min.x);
		yield return _backStepTeleport.CRun(this);
		character.ForceToLookAt((destinationX > ((Bounds)(ref platformBounds)).center.x) ? ((Bounds)(ref platformBounds)).max.x : ((Bounds)(ref platformBounds)).min.x);
	}

	public IEnumerator CastDimensionPierce()
	{
		_dimensionPierceCount = Random.Range(((Vector2Int)(ref _dimensionPierceCountRange)).x, ((Vector2Int)(ref _dimensionPierceCountRange)).y);
		for (int i = 0; i < _dimensionPierceCount; i++)
		{
			_dimensionPierce.TryStart();
			while (_dimensionPierce.running)
			{
				yield return null;
			}
		}
		_dimensionPierceCoolTimeAction.TryStart();
	}

	private IEnumerator MoveToDestination(Vector3 source, Vector3 dest, Characters.Actions.Action action, float duration, bool rotate = false, bool interporate = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float elapsed = 0f;
		ClampDestinationY(ref dest);
		Vector3 val;
		if (interporate)
		{
			val = source - dest;
			float num = ((Vector3)(ref val)).magnitude / platformWidth;
			duration *= num;
		}
		Character.LookingDirection direction = character.lookingDirection;
		if (rotate)
		{
			Vector3 val2 = dest - source;
			float num2 = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
			if (character.lookingDirection == Character.LookingDirection.Left)
			{
				num2 += 180f;
			}
			_body.rotation = Quaternion.AngleAxis(num2, Vector3.forward);
		}
		while (action.running)
		{
			if (character.stunedOrFreezed)
			{
				yield return null;
				continue;
			}
			Vector2 val3 = Vector2.Lerp(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), elapsed / duration);
			((Component)character).transform.position = Vector2.op_Implicit(val3);
			elapsed += character.chronometer.master.deltaTime;
			if (elapsed > duration)
			{
				character.CancelAction();
				break;
			}
			val = source - dest;
			if (((Vector3)(ref val)).magnitude < 0.1f)
			{
				character.CancelAction();
				break;
			}
			yield return null;
		}
		((Component)character).transform.position = dest;
		character.lookingDirection = direction;
		if (rotate)
		{
			_body.rotation = Quaternion.identity;
		}
	}

	private bool CanUseDarkDimensionPierce()
	{
		return _dimensionPierceCoolTimeAction.canUse;
	}

	private bool CanUseDarkDimensionRush()
	{
		if (_darkRush.CanUse())
		{
			return _darkRushPredelayEnd;
		}
		return false;
	}

	private void ClampDestinationY(ref Vector3 dest)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (dest.y <= ((Component)_minHeightTransform).transform.position.y)
		{
			dest.y = ((Component)_minHeightTransform).transform.position.y;
		}
	}

	private IEnumerator CStartPredelay()
	{
		_darkRushPredelayEnd = false;
		yield return character.chronometer.master.WaitForSeconds(_darkRushPredelay);
		_darkRushPredelayEnd = true;
	}
}
