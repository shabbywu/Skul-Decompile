using System.Collections;
using Characters.AI.Behaviours;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class TrolleyMaid : AIController
{
	[Tooltip("Turn Action의 이동량과 연관있음, 땅 끝에서 떨어진다면 값을 키울 것")]
	[SerializeField]
	private float _distanceFromEdgeToTurn = 2f;

	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[SerializeField]
	private Behaviour _rushReady;

	[SerializeField]
	private Behaviour _rush;

	[SerializeField]
	private Behaviour _turn;

	[SerializeField]
	private Action _action;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onRush;

	[SerializeField]
	private CharacterAnimation _charactrerAnimation;

	[SerializeField]
	private AnimationClip _rushWalk;

	private Bounds? platformBounds;

	private void Awake()
	{
		_onRush.Initialize();
	}

	private void OnDestroy()
	{
		_rushWalk = null;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _wander.CRun(this);
		while (!platformBounds.HasValue)
		{
			if (Object.op_Implicit((Object)(object)character.movement.controller.collisionState.lastStandingCollider))
			{
				platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
				break;
			}
			yield return null;
		}
		yield return CRunFirstRush();
		while (!base.dead)
		{
			if (base.stuned)
			{
				yield return null;
				continue;
			}
			platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			yield return CRush();
			yield return _turn.CRun(this);
		}
	}

	private IEnumerator CRunFirstRush()
	{
		SetDestination();
		_charactrerAnimation.SetWalk(_rushWalk);
		yield return _rushReady.CRun(this);
		((MonoBehaviour)this).StartCoroutine(_onRush.CRun(character));
		if (!base.stuned)
		{
			yield return _rush.CRun(this);
			_onRush.StopAll();
			if (!base.stuned)
			{
				yield return _turn.CRun(this);
			}
		}
		void SetDestination()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = ((Component)base.target).transform.position;
			Bounds bounds = ((Collider2D)character.collider).bounds;
			float num = ((Bounds)(ref bounds)).max.x - ((Bounds)(ref bounds)).center.x;
			float num2 = ((Bounds)(ref bounds)).center.x - ((Bounds)(ref bounds)).min.x;
			Bounds value;
			if (((Component)character).transform.position.x < position.x)
			{
				value = platformBounds.Value;
				float num3 = ((Bounds)(ref value)).max.x - num - _distanceFromEdgeToTurn;
				value = platformBounds.Value;
				base.destination = new Vector2(num3, ((Bounds)(ref value)).max.y);
			}
			else
			{
				value = platformBounds.Value;
				float num4 = ((Bounds)(ref value)).min.x - num2 + _distanceFromEdgeToTurn;
				value = platformBounds.Value;
				base.destination = new Vector2(num4, ((Bounds)(ref value)).max.y);
			}
		}
	}

	private IEnumerator CRush()
	{
		SetDestination();
		((MonoBehaviour)this).StartCoroutine(_onRush.CRun(character));
		yield return _rush.CRun(this);
		_onRush.StopAll();
		void SetDestination()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			Bounds bounds = ((Collider2D)character.collider).bounds;
			float num = ((Bounds)(ref bounds)).max.x - ((Bounds)(ref bounds)).center.x;
			float num2 = ((Bounds)(ref bounds)).center.x - ((Bounds)(ref bounds)).min.x;
			float x = ((Component)character).transform.position.x;
			Bounds value = platformBounds.Value;
			if (x < ((Bounds)(ref value)).center.x)
			{
				value = platformBounds.Value;
				float num3 = ((Bounds)(ref value)).max.x - num - _distanceFromEdgeToTurn;
				value = platformBounds.Value;
				base.destination = new Vector2(num3, ((Bounds)(ref value)).max.y);
			}
			else
			{
				value = platformBounds.Value;
				float num4 = ((Bounds)(ref value)).min.x + num2 + _distanceFromEdgeToTurn;
				value = platformBounds.Value;
				base.destination = new Vector2(num4, ((Bounds)(ref value)).max.y);
			}
		}
	}
}
