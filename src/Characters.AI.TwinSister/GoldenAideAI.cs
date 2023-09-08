using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using Characters.Movements;
using Characters.Operations;
using Data;
using Hardmode;
using Level;
using PhysicsUtils;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.AI.TwinSister;

public class GoldenAideAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	private Action _dash;

	[SerializeField]
	private Transform _body;

	[Header("Intro")]
	[SerializeField]
	private Action _introFall;

	[SerializeField]
	private Action _introLanding;

	[SerializeField]
	private float _introFallHeight;

	[Space]
	[SerializeField]
	private Transform _landingPoint;

	[SerializeField]
	[Space]
	[Header("Awaken")]
	private ChainAction _awakening;

	[SerializeField]
	private Transform _awakeningPosition;

	[SerializeField]
	private float _durationOfawakeningAppear = 0.8f;

	[SerializeField]
	[Header("TwinAppear")]
	private Action _twinAppear;

	[SerializeField]
	private float _startHeight = 5f;

	[SerializeField]
	private float _endDistanceWithPlatformEdge = 2f;

	[SerializeField]
	[Space]
	private float _durationOfTwinAppear = 0.6f;

	[Header("TwinEscape")]
	[SerializeField]
	private float _endHeight = 3f;

	[SerializeField]
	private float _endWidth = 5f;

	[SerializeField]
	[Space]
	private float _durationOfTwinEscape = 0.8f;

	[SerializeField]
	[Header("TwinMeteor")]
	private Action _twinMeteorEscape;

	[SerializeField]
	private Action _twinMeteorPreparing;

	[SerializeField]
	private Action _twinMeteor;

	[SerializeField]
	private Action _twinMeteorEnding;

	[SerializeField]
	[MinMaxSlider(-10f, 0f)]
	private Vector2 _rangeOfPredictTwinMeteorLeft;

	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2 _rangeOfPredictTwinMeteorRight;

	[SerializeField]
	private float _minHeightOfTwinMeteor;

	[SerializeField]
	private float _maxHeightOfTwinMeteor;

	[SerializeField]
	private bool _leftOfTwinMeteor;

	[SerializeField]
	private float _durationOfTwinMeteorEscaping;

	[SerializeField]
	private float _durationOfTwinMeteorPreparing;

	[SerializeField]
	private float _durationOfTwinMeteor;

	[SerializeField]
	[Space]
	private Transform _twinMeteorDestination;

	[SerializeField]
	[Header("GoldenMeteor")]
	private Action _goldenMeteorJump;

	[SerializeField]
	private Action _goldenMeteorReady;

	[SerializeField]
	private Action _goldenMeteorAttack;

	[SerializeField]
	private Action _goldenMeteorLanding;

	[SerializeField]
	private float _heightOfGoldmaneMeteor = 6f;

	[Space]
	[SerializeField]
	private float _durationOfGoldmaneMeteor;

	[SerializeField]
	private float _durationOfGoldmaneMeteorOnHardmode;

	[Header("MeteorInTheAir")]
	[SerializeField]
	[Space]
	private Action _meteorInAirJump;

	[SerializeField]
	private Action _meteorInAirReady;

	[SerializeField]
	private Action _meteorInAirAttack;

	[SerializeField]
	private Action _meteorInAirLanding;

	[SerializeField]
	private Action _meteorInAirStanding;

	[SerializeField]
	private float _durationOfMeteorInAir;

	[SerializeField]
	private float _durationOfMeteorInAirOnHardmode;

	[Header("MeteorInTheGround")]
	[SerializeField]
	private Action _meteorInGroundReady;

	[SerializeField]
	private Action _meteorInGroundAttack;

	[SerializeField]
	private Action _meteorInGroundAttackOnHardmode;

	[SerializeField]
	private Action _meteorInGroundLanding;

	[SerializeField]
	private Action _meteorInGroundStanding;

	[Header("MeteorInTheGround2")]
	[SerializeField]
	private Action _meteorInGround2Ready;

	[SerializeField]
	private Action _meteorInGround2Attack;

	[SerializeField]
	private Action _meteorInGround2Landing;

	[SerializeField]
	private Action _meteorInGround2Standing;

	[Space]
	[SerializeField]
	private float _durationOfMeteorInGround2;

	[SerializeField]
	private float _durationOfMeteorInGround2OnHardmode;

	[Header("RangeAttackHoming")]
	[Attack.Subcomponent(true)]
	[SerializeField]
	[Space]
	private MultiCircularProjectileAttack _rangeAttackHoming;

	[Header("BackStep")]
	[SerializeField]
	private Action _backStep;

	[SerializeField]
	[Space]
	private Transform _backStepDestination;

	[Header("Rush")]
	[SerializeField]
	private Action _rushReady;

	[SerializeField]
	private Action _rushA;

	[SerializeField]
	private Action _rushB;

	[SerializeField]
	private Action _rushC;

	[SerializeField]
	private Action _rushFinish;

	[SerializeField]
	private Action _rushStanding;

	[SerializeField]
	[Subcomponent(typeof(Dash))]
	private Dash _dashOfRush;

	[Space]
	[SerializeField]
	private float _durationOfRush;

	[SerializeField]
	[Header("Dimension Piece")]
	private Action _dimensionPierce;

	[SerializeField]
	private Transform _dimensionPiercePoint;

	[SerializeField]
	[Space]
	private int _dimensionPierceCount;

	[Subcomponent(typeof(Idle))]
	[Header("Idle")]
	[SerializeField]
	private Idle _idle;

	[SerializeField]
	[Subcomponent(typeof(SkipableIdle))]
	private SkipableIdle _skippableIdle;

	[Header("Rising Pierce")]
	[SerializeField]
	private float _preDelayOfRisingPierce = 10f;

	[FormerlySerializedAs("_risingPieceMotion")]
	[SerializeField]
	private Action _risingPierceReady;

	[SerializeField]
	private Action _risingPierceAttackAndEnd;

	[SerializeField]
	private OperationInfos _risingPieceStartAttackOperations;

	[SerializeField]
	private OperationInfos _risingPieceAttackOperations;

	[SerializeField]
	private Collider2D _risingPeieceLeftRange;

	[SerializeField]
	private Collider2D _risingPeieceRightRange;

	[SerializeField]
	private float _risingPeieceTerm;

	[SerializeField]
	private int _risingPeieceCount;

	[SerializeField]
	[Space]
	private float _risingPeieceDistance;

	private float _delayOfRisingPierce = 20f;

	private bool _canUseRisingPierce = true;

	private bool _preDelayOfRisingPierceEnd;

	[SerializeField]
	[Header("Tools")]
	private Collider2D _meleeAttackTrigger;

	[SerializeField]
	private Transform _minHeightTransform;

	private float _platformWidth;

	private CharacterController2D.CollisionState _collisionState;

	private static NonAllocOverlapper _nonAllocOverlapper;

	private const float _maxDistanceOfWall = 4f;

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	private new void Start()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		_collisionState = character.movement.controller.collisionState;
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		_platformWidth = ((Bounds)(ref bounds)).size.x;
		_nonAllocOverlapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _nonAllocOverlapper.contactFilter)).SetLayerMask(Layers.groundMask);
		_risingPieceAttackOperations.Initialize();
		_risingPieceStartAttackOperations.Initialize();
		character.health.onDiedTryCatch += delegate
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			character.status.RemoveAllStatus();
			_body.rotation = Quaternion.identity;
		};
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_durationOfMeteorInGround2 = _durationOfMeteorInGround2OnHardmode;
			_durationOfGoldmaneMeteor = _durationOfGoldmaneMeteorOnHardmode;
			_durationOfMeteorInAir = _durationOfMeteorInAirOnHardmode;
		}
	}

	protected override IEnumerator CProcess()
	{
		yield return CIntro();
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.controller.terrainMask = Layers.terrainMask;
		while (!base.dead)
		{
			yield return null;
			_ = (Object)(object)base.target == (Object)null;
		}
	}

	public IEnumerator CIntro()
	{
		((Component)character).transform.position = Vector2.op_Implicit(new Vector2(_landingPoint.position.x, _landingPoint.position.y + _introFallHeight));
		_introLanding.TryStart();
		while (_introLanding.running && !character.health.dead)
		{
			yield return null;
		}
	}

	public IEnumerator CastAwakening()
	{
		character.status.RemoveAllStatus();
		yield return Chronometer.global.WaitForSeconds(1.5f);
		yield return CastAwakeningAppear();
		_awakening.TryStart();
		while (_awakening.running && !character.health.dead)
		{
			yield return null;
		}
	}

	private IEnumerator CastAwakeningAppear()
	{
		character.movement.config.type = Movement.Config.Type.Flying;
		character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		float num = ((_awakeningPosition.position.x < ((Bounds)(ref bounds)).center.x) ? (((Bounds)(ref bounds)).min.x - 4f) : (((Bounds)(ref bounds)).max.x + 4f));
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(num, ((Bounds)(ref bounds)).max.y + 5f);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(_awakeningPosition.position.x, ((Bounds)(ref bounds)).max.y);
		((Component)character).transform.position = Vector2.op_Implicit(val);
		character.lookingDirection = ((!(_awakeningPosition.position.x < ((Bounds)(ref bounds)).center.x)) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		_twinAppear.TryStart();
		yield return MoveToDestination(Vector2.op_Implicit(val), Vector2.op_Implicit(val2), _twinAppear, _durationOfawakeningAppear, rotate: false, interporate: false);
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.controller.terrainMask = Layers.terrainMask;
	}

	public IEnumerator CastTwinMeteorGround(bool left)
	{
		if (character.health.dead)
		{
			yield break;
		}
		_leftOfTwinMeteor = left;
		yield return CastTwinAppear();
		if (!character.health.dead)
		{
			yield return CastMeteorInGround2();
			if (!character.health.dead)
			{
				yield return EscapeForTwin();
			}
		}
	}

	public IEnumerator CastTwinMeteorChain(bool left, bool ground)
	{
		if (!character.health.dead)
		{
			_leftOfTwinMeteor = left;
			if (ground)
			{
				yield return CastTwinAppear();
				yield return CastMeteorInGround2();
			}
			else
			{
				yield return CastGoldenMeteor();
			}
			yield return EscapeForTwin();
		}
	}

	public IEnumerator CastTwinMeteorPierce(bool left)
	{
		if (!character.health.dead)
		{
			_leftOfTwinMeteor = left;
			MultiCircularProjectileAttack rangeAttackHoming = _rangeAttackHoming;
			Bounds bounds = Map.Instance.bounds;
			rangeAttackHoming.lookTarget = ((Bounds)(ref bounds)).center;
			yield return CastTwinAppear();
			yield return CastRangeAttackHoming(centerTarget: true);
			yield return EscapeForTwin(flipDest: false);
		}
	}

	public IEnumerator CastTwinMeteor(bool left)
	{
		if (base.dead)
		{
			yield break;
		}
		_leftOfTwinMeteor = left;
		character.lookingDirection = ((!_leftOfTwinMeteor) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		Bounds platform = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float num = Random.Range(_minHeightOfTwinMeteor, _maxHeightOfTwinMeteor);
		((Component)character).transform.position = Vector2.op_Implicit(new Vector2(_leftOfTwinMeteor ? (((Bounds)(ref platform)).min.x - 4f) : (((Bounds)(ref platform)).max.x + 4f), ((Bounds)(ref platform)).max.y + num));
		Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
		Vector2 dest = new Vector2(((Component)base.target).transform.position.x, ((Bounds)(ref platform)).max.y);
		_twinMeteorDestination.position = Vector2.op_Implicit(dest);
		_twinMeteorPreparing.TryStart();
		while (_twinMeteorPreparing.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_twinMeteor.TryStart();
		yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _twinMeteor, _durationOfTwinMeteor, rotate: true, interporate: false);
		if (character.lookingDirection == Character.LookingDirection.Right)
		{
			((Component)character).transform.position = Vector2.op_Implicit(new Vector2(Mathf.Max(((Bounds)(ref platform)).min.x, dest.x - 1.5f), ((Bounds)(ref platform)).max.y));
		}
		else
		{
			((Component)character).transform.position = Vector2.op_Implicit(new Vector2(Mathf.Min(((Bounds)(ref platform)).max.x, dest.x + 1.5f), ((Bounds)(ref platform)).max.y));
		}
		_twinMeteorEnding.TryStart();
		while (_twinMeteorEnding.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.controller.terrainMask = Layers.terrainMask;
	}

	public IEnumerator CastPredictTwinMeteor(bool left)
	{
		_leftOfTwinMeteor = left;
		character.lookingDirection = ((!_leftOfTwinMeteor) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		Bounds platform = _collisionState.lastStandingCollider.bounds;
		float num = Random.Range(_minHeightOfTwinMeteor, _maxHeightOfTwinMeteor);
		((Component)character).transform.position = Vector2.op_Implicit(new Vector2(_leftOfTwinMeteor ? (((Bounds)(ref platform)).min.x - 4f) : (((Bounds)(ref platform)).max.x + 4f), ((Bounds)(ref platform)).max.y + num));
		Vector2 val = (MMMaths.RandomBool() ? _rangeOfPredictTwinMeteorLeft : _rangeOfPredictTwinMeteorRight);
		float num2 = Random.Range(val.x, val.y);
		float num3 = Mathf.Clamp(((Component)base.target).transform.position.x + num2, ((Bounds)(ref platform)).min.x, ((Bounds)(ref platform)).max.x);
		Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
		Vector2 dest = new Vector2(num3, ((Bounds)(ref platform)).max.y);
		_twinMeteorDestination.position = Vector2.op_Implicit(dest);
		_twinMeteorPreparing.TryStart();
		while (_twinMeteorPreparing.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_twinMeteor.TryStart();
		yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _twinMeteor, _durationOfTwinMeteor, rotate: true, interporate: false);
		if (character.lookingDirection == Character.LookingDirection.Right)
		{
			((Component)character).transform.position = Vector2.op_Implicit(new Vector2(Mathf.Max(((Bounds)(ref platform)).min.x, dest.x - 2f), ((Bounds)(ref platform)).max.y));
		}
		else
		{
			((Component)character).transform.position = Vector2.op_Implicit(new Vector2(Mathf.Min(((Bounds)(ref platform)).max.x, dest.x + 2f), ((Bounds)(ref platform)).max.y));
		}
		_twinMeteorEnding.TryStart();
		while (_twinMeteorEnding.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.controller.terrainMask = Layers.terrainMask;
	}

	public IEnumerator EscapeForTwinMeteor()
	{
		if (!base.dead)
		{
			_leftOfTwinMeteor = !_leftOfTwinMeteor;
			character.movement.config.type = Movement.Config.Type.Flying;
			character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
			float num = Random.Range(_minHeightOfTwinMeteor, _maxHeightOfTwinMeteor);
			Bounds bounds = _collisionState.lastStandingCollider.bounds;
			Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
			Vector2 dest = new Vector2(_leftOfTwinMeteor ? (((Bounds)(ref bounds)).min.x - _endWidth) : (((Bounds)(ref bounds)).max.x + _endWidth), ((Bounds)(ref bounds)).max.y + num);
			character.ForceToLookAt(_leftOfTwinMeteor ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			yield return null;
			while (character.stunedOrFreezed)
			{
				yield return null;
			}
			_twinMeteorEscape.TryStart();
			yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _twinMeteorEscape, _durationOfTwinMeteorEscaping, rotate: false, interporate: false);
		}
	}

	private IEnumerator EscapeForTwin(bool flipDest = true)
	{
		if (!base.dead)
		{
			if (flipDest)
			{
				_leftOfTwinMeteor = !_leftOfTwinMeteor;
			}
			character.movement.config.type = Movement.Config.Type.Flying;
			character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
			float endHeight = _endHeight;
			Bounds bounds = _collisionState.lastStandingCollider.bounds;
			Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
			Vector2 dest = new Vector2(_leftOfTwinMeteor ? (((Bounds)(ref bounds)).min.x - _endWidth) : (((Bounds)(ref bounds)).max.x + _endWidth), ((Bounds)(ref bounds)).max.y + endHeight);
			character.lookingDirection = (_leftOfTwinMeteor ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			while (character.stunedOrFreezed)
			{
				yield return null;
			}
			_twinMeteorEscape.TryStart();
			yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _twinMeteorEscape, _durationOfTwinEscape);
		}
	}

	public void PrepareTwinMeteorOfBehind()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		_leftOfTwinMeteor = !_leftOfTwinMeteor;
		character.movement.config.type = Movement.Config.Type.Flying;
		character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
		float num = Random.Range(_minHeightOfTwinMeteor, _maxHeightOfTwinMeteor);
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(_leftOfTwinMeteor ? (((Bounds)(ref bounds)).min.x - 4f) : (((Bounds)(ref bounds)).max.x + 4f), ((Bounds)(ref bounds)).max.y + num);
		((Component)character).transform.position = Vector2.op_Implicit(val);
		character.lookingDirection = (_leftOfTwinMeteor ? Character.LookingDirection.Left : Character.LookingDirection.Right);
	}

	public IEnumerator CastGoldenMeteor()
	{
		if (character.health.dead)
		{
			yield break;
		}
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.controller.terrainMask = Layers.terrainMask;
		Bounds platform = _collisionState.lastStandingCollider.bounds;
		float num = ((Component)base.target).transform.position.x;
		if (num + 0.5f >= ((Bounds)(ref platform)).max.x)
		{
			num -= 0.5f;
		}
		else if (num - 0.5f <= ((Bounds)(ref platform)).min.x)
		{
			num += 0.5f;
		}
		((Component)character).transform.position = Vector2.op_Implicit(new Vector2(num, ((Bounds)(ref platform)).max.y + _heightOfGoldmaneMeteor));
		_goldenMeteorJump.TryStart();
		while (_goldenMeteorJump.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_goldenMeteorReady.TryStart();
		while (_goldenMeteorReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_goldenMeteorAttack.TryStart();
		Vector2 val = Vector2.op_Implicit(((Component)character).transform.position);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(((Component)character).transform.position.x, ((Bounds)(ref platform)).max.y);
		yield return MoveToDestination(Vector2.op_Implicit(val), Vector2.op_Implicit(val2), _goldenMeteorAttack, _durationOfGoldmaneMeteor);
		if (!character.health.dead)
		{
			_goldenMeteorLanding.TryStart();
			while (_goldenMeteorLanding.running && !character.health.dead)
			{
				yield return null;
			}
		}
	}

	public IEnumerator CastMeteorInAir()
	{
		if (character.health.dead)
		{
			yield break;
		}
		character.ForceToLookAt(((Component)base.target).transform.position.x);
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInAirJump.TryStart();
		while (_meteorInAirJump.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
		Vector2 dest = new Vector2(((Component)base.target).transform.position.x, ((Bounds)(ref bounds)).max.y);
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInAirReady.TryStart();
		while (_meteorInAirReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		while (character.movement.verticalVelocity > 0f)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		character.ForceToLookAt(dest.x);
		_meteorInAirAttack.TryStart();
		yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _meteorInAirAttack, _durationOfMeteorInAir, rotate: true);
		if (character.health.dead)
		{
			yield break;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInAirLanding.TryStart();
		while (_meteorInAirLanding.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInAirStanding.TryStart();
		while (_meteorInAirStanding.running && !character.health.dead)
		{
			yield return null;
		}
	}

	public IEnumerator CastMeteorInGround()
	{
		if (character.health.dead)
		{
			yield break;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGroundReady.TryStart();
		while (_meteorInGroundReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		if (GameData.HardmodeProgress.hardmode)
		{
			_meteorInGroundAttackOnHardmode.TryStart();
			while (_meteorInGroundAttackOnHardmode.running)
			{
				if (character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
		}
		else
		{
			_meteorInGroundAttack.TryStart();
			while (_meteorInGroundAttack.running)
			{
				if (character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGroundLanding.TryStart();
		while (_meteorInGroundLanding.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGroundStanding.TryStart();
		while (_meteorInGroundStanding.running && !character.health.dead)
		{
			yield return null;
		}
	}

	public IEnumerator CastMeteorInGround2()
	{
		if (character.health.dead)
		{
			yield break;
		}
		Bounds bounds = _collisionState.lastStandingCollider.bounds;
		Vector2 source = Vector2.op_Implicit(((Component)character).transform.position);
		float num = ((source.x > ((Bounds)(ref bounds)).center.x) ? (((Bounds)(ref bounds)).min.x + 2f) : (((Bounds)(ref bounds)).max.x - 2f));
		Vector2 dest = new Vector2(num, ((Component)character).transform.position.y);
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Ready.TryStart();
		while (_meteorInGround2Ready.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Attack.TryStart();
		yield return MoveToDestination(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), _meteorInGround2Attack, _durationOfMeteorInGround2);
		if (character.health.dead)
		{
			yield break;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Landing.TryStart();
		while (_meteorInGround2Landing.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		while (character.stunedOrFreezed)
		{
			yield return null;
		}
		_meteorInGround2Standing.TryStart();
		while (_meteorInGround2Standing.running && !character.health.dead)
		{
			yield return null;
		}
	}

	public IEnumerator CastRush()
	{
		if (character.health.dead)
		{
			yield break;
		}
		character.ForceToLookAt(((Component)base.target).transform.position.x);
		yield return _dashOfRush.CRun(this);
		_rushReady.TryStart();
		while (_rushReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_rushA.TryStart();
		while (_rushA.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		character.ForceToLookAt(((Component)base.target).transform.position.x);
		yield return _dashOfRush.CRun(this);
		_rushReady.TryStart();
		while (_rushReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_rushB.TryStart();
		while (_rushB.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		character.ForceToLookAt(((Component)base.target).transform.position.x);
		yield return _dashOfRush.CRun(this);
		_rushReady.TryStart();
		while (_rushReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_rushC.TryStart();
		while (_rushC.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_rushFinish.TryStart();
			while (_rushFinish.running)
			{
				if (character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
		}
		_rushStanding.TryStart();
		while (_rushStanding.running && !character.health.dead)
		{
			yield return null;
		}
	}

	public IEnumerator CastDimensionPierce()
	{
		if (!character.health.dead)
		{
			_dimensionPierce.TryStart();
			while (_dimensionPierce.running && !character.health.dead)
			{
				yield return null;
			}
		}
	}

	public IEnumerator CastRisingPierce()
	{
		if (character.health.dead || !_canUseRisingPierce)
		{
			yield break;
		}
		((MonoBehaviour)this).StartCoroutine(CCoolDownRisingPierce());
		_risingPierceReady.TryStart();
		while (_risingPierceReady.running)
		{
			if (character.health.dead)
			{
				yield break;
			}
			yield return null;
		}
		_risingPierceAttackAndEnd.TryStart();
		while (_risingPierceAttackAndEnd.running && !character.health.dead)
		{
			yield return null;
		}
	}

	public IEnumerator CastIdle()
	{
		yield return _idle.CRun(this);
	}

	public IEnumerator CastSkippableIdle()
	{
		yield return _skippableIdle.CRun(this);
	}

	private IEnumerator CastPowerWave()
	{
		if (character.health.dead)
		{
			yield break;
		}
		Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float cachedPositionX = ((Component)character).transform.position.x;
		Bounds bounds = _risingPeieceLeftRange.bounds;
		float sizeX = ((Bounds)(ref bounds)).size.x;
		bounds = _risingPeieceLeftRange.bounds;
		float extentsX = ((Bounds)(ref bounds)).extents.x;
		((Component)_risingPieceStartAttackOperations).gameObject.SetActive(true);
		((Component)_risingPeieceLeftRange).transform.position = new Vector3(cachedPositionX, ((Bounds)(ref platformBounds)).max.y);
		Physics2D.SyncTransforms();
		_risingPieceStartAttackOperations.Run(character);
		yield return character.chronometer.animation.WaitForSeconds(_risingPeieceTerm);
		for (int i = 1; i < _risingPeieceCount; i++)
		{
			if (character.health.dead)
			{
				break;
			}
			((Component)_risingPeieceLeftRange).transform.position = new Vector3(cachedPositionX + sizeX * (float)(-i) + (float)(-i) * _risingPeieceDistance - extentsX, ((Bounds)(ref platformBounds)).max.y);
			((Component)_risingPeieceRightRange).transform.position = new Vector3(cachedPositionX + sizeX * (float)i + (float)i * _risingPeieceDistance + extentsX, ((Bounds)(ref platformBounds)).max.y);
			Physics2D.SyncTransforms();
			((Component)_risingPieceAttackOperations).gameObject.SetActive(true);
			_risingPieceAttackOperations.Run(character);
			yield return character.chronometer.animation.WaitForSeconds(_risingPeieceTerm);
		}
	}

	public IEnumerator CastBackstep()
	{
		if (!character.health.dead)
		{
			Bounds bounds = _collisionState.lastStandingCollider.bounds;
			float num = ((((Component)base.target).transform.position.x < ((Component)character).transform.position.x) ? ((Bounds)(ref bounds)).max.x : ((Bounds)(ref bounds)).min.x);
			Character.LookingDirection lookingDirection = ((((Component)base.target).transform.position.x < ((Component)character).transform.position.x) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			if (Mathf.Abs(((Component)character).transform.position.x - num) <= 2f)
			{
				lookingDirection = ((lookingDirection == Character.LookingDirection.Right) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			}
			character.ForceToLookAt(lookingDirection);
			_backStep.TryStart();
			while (_backStep.running && !character.health.dead)
			{
				yield return null;
			}
		}
	}

	public IEnumerator CastRangeAttackHoming(bool centerTarget)
	{
		if (!character.health.dead)
		{
			if (centerTarget)
			{
				Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
				_rangeAttackHoming.lookTarget = ((Bounds)(ref bounds)).center;
			}
			else
			{
				_rangeAttackHoming.lookTarget = ((Component)base.target).transform.position;
			}
			yield return _rangeAttackHoming.CRun(this);
		}
	}

	public bool IsMeleeCombat()
	{
		return (Object)(object)FindClosestPlayerBody(_meleeAttackTrigger) != (Object)null;
	}

	private IEnumerator MoveToDestination(Vector3 source, Vector3 dest, Action action, float duration, bool rotate = false, bool interporate = true)
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
			float num = ((Vector3)(ref val)).magnitude / _platformWidth;
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
			yield return null;
			if (!(character.chronometer.master.deltaTime <= 0f) && !character.stunedOrFreezed)
			{
				Vector2 val3 = Vector2.Lerp(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), elapsed / duration);
				((Component)character).transform.position = Vector2.op_Implicit(val3);
				elapsed += character.chronometer.animation.deltaTime;
				if (character.health.dead)
				{
					character.movement.config.type = Movement.Config.Type.Walking;
					character.movement.controller.terrainMask = Layers.terrainMask;
					_body.rotation = Quaternion.identity;
					yield break;
				}
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
			}
		}
		while (true)
		{
			if (character.chronometer.master.deltaTime <= 0f)
			{
				yield return null;
				continue;
			}
			if (!character.stunedOrFreezed)
			{
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

	public IEnumerator CastDash(float stopDistance = 0f)
	{
		if (!character.health.dead)
		{
			float num = ((Component)character).transform.position.x - ((Component)base.target).transform.position.x;
			character.ForceToLookAt(((Component)base.target).transform.position.x);
			_dash.TryStart();
			float num2 = ((num > 0f) ? stopDistance : (0f - stopDistance));
			Vector2 val = Vector2.op_Implicit(((Component)character).transform.position);
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(((Component)base.target).transform.position.x + num2, ((Component)character).transform.position.y);
			yield return MoveToDestination(Vector2.op_Implicit(val), Vector2.op_Implicit(val2), _dash, _durationOfRush);
			character.CancelAction();
		}
	}

	private IEnumerator CastTwinAppear()
	{
		if (!character.health.dead)
		{
			character.movement.config.type = Movement.Config.Type.Flying;
			character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
			Bounds bounds = _collisionState.lastStandingCollider.bounds;
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(_leftOfTwinMeteor ? (((Bounds)(ref bounds)).min.x - 4f) : (((Bounds)(ref bounds)).max.x + 4f), ((Bounds)(ref bounds)).max.y + _startHeight);
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(_leftOfTwinMeteor ? (((Bounds)(ref bounds)).min.x + _endDistanceWithPlatformEdge) : (((Bounds)(ref bounds)).max.x - _endDistanceWithPlatformEdge), ((Bounds)(ref bounds)).max.y);
			((Component)character).transform.position = Vector2.op_Implicit(val);
			character.lookingDirection = ((!_leftOfTwinMeteor) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			_twinAppear.TryStart();
			yield return MoveToDestination(Vector2.op_Implicit(val), Vector2.op_Implicit(val2), _twinAppear, _durationOfTwinAppear, rotate: false, interporate: false);
			character.movement.config.type = Movement.Config.Type.Walking;
			character.movement.controller.terrainMask = Layers.terrainMask;
		}
	}

	public void Hide()
	{
		((Component)character.@base).gameObject.SetActive(false);
		character.attach.SetActive(false);
	}

	public void Show()
	{
		((Component)character.@base).gameObject.SetActive(true);
		character.attach.SetActive(true);
	}

	public void Dettachinvincibility()
	{
		character.cinematic.Detach(this);
	}

	public void Attachinvincibility()
	{
		character.cinematic.Attach(this);
	}

	public bool CanUseDimensionPierce()
	{
		return _dimensionPierce.canUse;
	}

	public bool CanUseRisingPierce()
	{
		if (_risingPierceAttackAndEnd.canUse && _preDelayOfRisingPierceEnd)
		{
			return _canUseRisingPierce;
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

	public IEnumerator CStartSinglePhasePreDelay()
	{
		_preDelayOfRisingPierceEnd = false;
		yield return character.chronometer.animation.WaitForSeconds(_preDelayOfRisingPierce);
		_preDelayOfRisingPierceEnd = true;
	}

	private IEnumerator CCoolDownRisingPierce()
	{
		_canUseRisingPierce = false;
		yield return character.chronometer.master.WaitForSeconds(_delayOfRisingPierce);
		_canUseRisingPierce = true;
	}
}
