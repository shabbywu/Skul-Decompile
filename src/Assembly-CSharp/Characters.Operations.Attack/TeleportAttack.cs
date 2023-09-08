using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.Attack;

public sealed class TeleportAttack : CharacterOperation
{
	private static readonly NonAllocOverlapper _enemyOverlapper;

	[SerializeField]
	[Header("타겟 지정")]
	private CharacterTypeBoolArray _targetFilter;

	[SerializeField]
	private bool _optimizeCollider = true;

	[SerializeField]
	private Collider2D _collider2D;

	[SerializeField]
	private float _distance = 1f;

	[Header("공격")]
	[SerializeField]
	private CustomFloat _additionalDamageAmount;

	[SerializeField]
	private bool _adaptiveForce;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private PositionInfo _targetPointInfo;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _onHitTargetOperations;

	static TeleportAttack()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Awake()
	{
		_onHitTargetOperations.Initialize();
	}

	public override void Run(Character owner)
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		Character character = FindClosestTarget(owner, _collider2D);
		if ((Object)(object)character == (Object)null)
		{
			return;
		}
		Vector2 destination = default(Vector2);
		if (character.movement.config.type == Characters.Movements.Movement.Config.Type.Walking)
		{
			Vector2 direction;
			if (character.lookingDirection == Character.LookingDirection.Right)
			{
				((Vector2)(ref destination))._002Ector(((Component)character).transform.position.x - _distance, ((Component)character).transform.position.y);
				direction = Vector2.left;
			}
			else
			{
				((Vector2)(ref destination))._002Ector(((Component)character).transform.position.x + _distance, ((Component)character).transform.position.y);
				direction = Vector2.right;
			}
			if (owner.movement.controller.Teleport(destination, direction, _distance))
			{
				owner.ForceToLookAt(((Component)character).transform.position.x);
				owner.movement.verticalVelocity = 0f;
				Attack(owner, character);
			}
			else if (owner.movement.controller.Teleport(Vector2.op_Implicit(((Component)character).transform.position), direction, _distance))
			{
				owner.movement.verticalVelocity = 0f;
				Attack(owner, character);
			}
		}
		else
		{
			destination = Vector2.op_Implicit(((Component)character).transform.position);
			if (owner.movement.controller.Teleport(destination, _distance))
			{
				owner.movement.verticalVelocity = 0f;
				Attack(owner, character);
			}
		}
	}

	private void Attack(Character attacker, Character target)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		if (!target.health.dead && ((Component)((Component)target).transform).gameObject.activeSelf)
		{
			if ((Object)(object)_targetPoint != (Object)null)
			{
				Bounds bounds = ((Collider2D)target.collider).bounds;
				Vector3 center = ((Bounds)(ref bounds)).center;
				bounds = ((Collider2D)target.collider).bounds;
				Vector3 size = ((Bounds)(ref bounds)).size;
				size.x *= _targetPointInfo.pivotValue.x;
				size.y *= _targetPointInfo.pivotValue.y;
				Vector3 position = center + size;
				_targetPoint.position = position;
			}
			if (_adaptiveForce)
			{
				_additionalHit.ChangeAdaptiveDamageAttribute(attacker);
			}
			Damage damage = attacker.stat.GetDamage(_additionalDamageAmount.value, MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds), _additionalHit);
			((MonoBehaviour)attacker).StartCoroutine(_onHitTargetOperations.CRun(attacker, target));
			attacker.Attack(target, ref damage);
		}
	}

	private Character FindClosestTarget(Character character, Collider2D collider)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)collider).enabled = true;
		List<Target> components = _enemyOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			if (_optimizeCollider)
			{
				((Behaviour)collider).enabled = false;
			}
			return null;
		}
		if (components.Count == 1)
		{
			if (_optimizeCollider)
			{
				((Behaviour)collider).enabled = false;
			}
			return components[0].character;
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < components.Count; i++)
		{
			if (!((Object)(object)components[i].character == (Object)null) && _targetFilter[components[i].character.type] && components[i].character.movement.isGrounded)
			{
				ColliderDistance2D val = Physics2D.Distance((Collider2D)(object)components[i].character.collider, (Collider2D)(object)character.collider);
				float distance = ((ColliderDistance2D)(ref val)).distance;
				if (num > distance)
				{
					index = i;
					num = distance;
				}
			}
		}
		if (_optimizeCollider)
		{
			((Behaviour)collider).enabled = false;
		}
		return components[index].character;
	}
}
