using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Movements;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class BraveAI : AIController
{
	[Header("Behaviours")]
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[SerializeField]
	private MoveToDestinationWithFly _moveToTargetHead;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _attackReady;

	[SerializeField]
	[Subcomponent(typeof(MoveToDestinationWithFly))]
	private MoveToDestinationWithFly _moveToTargetGround;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _attackOperations;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _landingOperations;

	[SerializeField]
	[Header("Tools")]
	[Space]
	private Collider2D _attackTrigger;

	[SerializeField]
	private float _attackHeight;

	private const float _widthCheckRange = 0.2f;

	private void Awake()
	{
		character.status.unstoppable.Attach((object)this);
		base.behaviours = new List<Behaviour> { _checkWithinSight, _moveToTargetHead, _moveToTargetGround };
		_attackOperations.Initialize();
		_landingOperations.Initialize();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		character.movement.onGrounded += delegate
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			character.movement.config.type = Movement.Config.Type.Walking;
			character.movement.controller.oneWayPlatformMask = LayerMask.op_Implicit(131072);
			((MonoBehaviour)this).StartCoroutine(_landingOperations.CRun(character));
		};
		while (!base.dead)
		{
			character.movement.config.type = Movement.Config.Type.Walking;
			character.movement.config.gravity = -300f;
			character.movement.controller.terrainMask = Layers.terrainMask;
			yield return null;
			if (!((Object)(object)base.target == (Object)null))
			{
				if (base.stuned)
				{
					Debug.Log((object)(((Object)this).name + " is stuned"));
				}
				else if (CheckAttackable())
				{
					yield return CAttack();
					yield return _idle.CRun(this);
				}
			}
		}
	}

	private bool CheckAttackable()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)FindClosestPlayerBody(_attackTrigger) == (Object)null)
		{
			return false;
		}
		if ((Object)(object)base.target.movement.controller.collisionState.lastStandingCollider == (Object)null)
		{
			return false;
		}
		if (!base.target.movement.controller.isGrounded)
		{
			return false;
		}
		Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
		Bounds bounds2 = character.movement.controller.collisionState.lastStandingCollider.bounds;
		if (((Bounds)(ref bounds)).max.y != ((Bounds)(ref bounds2)).max.y)
		{
			return false;
		}
		Bounds bounds3 = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
		float x = ((Component)base.target).transform.position.x;
		float num = ((Bounds)(ref bounds3)).max.y + _attackHeight;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(x, num);
		Bounds bounds4 = ((Collider2D)character.collider).bounds;
		((Bounds)(ref bounds4)).size = Vector2.op_Implicit(new Vector2(0.2f, ((Bounds)(ref bounds4)).size.y));
		((Bounds)(ref bounds4)).center = Vector2.op_Implicit(new Vector2(val.x, val.y + (((Bounds)(ref bounds4)).center.y - ((Bounds)(ref bounds4)).min.y)));
		return !TerrainColliding(bounds4);
	}

	private bool TerrainColliding(Bounds range)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref NonAllocOverlapper.shared.contactFilter)).SetLayerMask(Layers.terrainMask);
		return NonAllocOverlapper.shared.OverlapBox(Vector2.op_Implicit(((Bounds)(ref range)).center), Vector2.op_Implicit(((Bounds)(ref range)).size), 0f).results.Count != 0;
	}

	private IEnumerator CAttack()
	{
		Bounds platform = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
		float x = ((Component)base.target).transform.position.x;
		float num = ((Bounds)(ref platform)).max.y + _attackHeight;
		base.destination = new Vector2(x, num);
		character.movement.config.type = Movement.Config.Type.Flying;
		character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
		character.movement.controller.oneWayPlatformMask = LayerMask.op_Implicit(0);
		yield return _moveToTargetHead.CRun(this);
		yield return _attackReady.CRun(this);
		num = ((Bounds)(ref platform)).max.y;
		base.destination = new Vector2(x, num);
		((MonoBehaviour)this).StartCoroutine(_attackOperations.CRun(character));
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.controller.terrainMask = Layers.terrainMask;
		character.movement.controller.oneWayPlatformMask = LayerMask.op_Implicit(131072);
	}
}
