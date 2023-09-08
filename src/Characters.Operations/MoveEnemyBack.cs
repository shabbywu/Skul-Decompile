using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations;

public class MoveEnemyBack : CharacterOperation
{
	[SerializeField]
	private bool _optimizeCollider = true;

	[SerializeField]
	private Collider2D _collider2D;

	[SerializeField]
	private float _distance = 1f;

	private static readonly NonAllocOverlapper _enemyOverlapper;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onSuccess;

	static MoveEnemyBack()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Awake()
	{
		_onSuccess.Initialize();
	}

	public override void Run(Character owner)
	{
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		Character character = FindClosestPlayerBody(owner, _collider2D);
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
				((MonoBehaviour)this).StartCoroutine(_onSuccess.CRun(owner));
			}
			else if (owner.movement.controller.Teleport(Vector2.op_Implicit(((Component)character).transform.position), direction, _distance))
			{
				owner.movement.verticalVelocity = 0f;
				((MonoBehaviour)this).StartCoroutine(_onSuccess.CRun(owner));
			}
		}
		else
		{
			destination = Vector2.op_Implicit(((Component)character).transform.position);
			if (owner.movement.controller.Teleport(destination, _distance))
			{
				owner.movement.verticalVelocity = 0f;
				((MonoBehaviour)this).StartCoroutine(_onSuccess.CRun(owner));
			}
		}
	}

	private void OnDisable()
	{
		_onSuccess?.StopAll();
	}

	private Character FindClosestPlayerBody(Character character, Collider2D collider)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
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
			if (!((Object)(object)components[i].character == (Object)null) && (components[i].character.type == Character.Type.TrashMob || components[i].character.type == Character.Type.Adventurer || components[i].character.type == Character.Type.Boss) && components[i].character.movement.isGrounded)
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
