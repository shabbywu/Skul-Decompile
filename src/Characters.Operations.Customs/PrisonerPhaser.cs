using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs;

public class PrisonerPhaser : CharacterOperation
{
	[SerializeField]
	private Collider2D _collider2D;

	[SerializeField]
	private float _distance = 1f;

	private static readonly NonAllocOverlapper _enemyOverlapper;

	static PrisonerPhaser()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	public override void Run(Character owner)
	{
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		Target target = FindClosestPlayerBody(owner, _collider2D);
		if ((Object)(object)target == (Object)null)
		{
			return;
		}
		Vector2 destination = default(Vector2);
		if ((Object)(object)target.character != (Object)null && target.character.movement.config.type == Characters.Movements.Movement.Config.Type.Walking)
		{
			Vector2 direction;
			if (target.character.lookingDirection == Character.LookingDirection.Right)
			{
				((Vector2)(ref destination))._002Ector(((Component)target).transform.position.x - _distance, ((Component)target).transform.position.y);
				direction = Vector2.left;
			}
			else
			{
				((Vector2)(ref destination))._002Ector(((Component)target).transform.position.x + _distance, ((Component)target).transform.position.y);
				direction = Vector2.right;
			}
			if (owner.movement.controller.Teleport(destination, direction, _distance))
			{
				owner.ForceToLookAt(target.character.lookingDirection);
				owner.movement.verticalVelocity = 0f;
			}
			else if (owner.movement.controller.Teleport(Vector2.op_Implicit(((Component)target).transform.position), direction, _distance))
			{
				owner.movement.verticalVelocity = 0f;
			}
		}
		else
		{
			Vector2 direction;
			if (MMMaths.RandomBool())
			{
				((Vector2)(ref destination))._002Ector(((Component)target).transform.position.x - _distance, ((Component)target).transform.position.y);
				direction = Vector2.left;
			}
			else
			{
				((Vector2)(ref destination))._002Ector(((Component)target).transform.position.x + _distance, ((Component)target).transform.position.y);
				direction = Vector2.right;
			}
			if (owner.movement.controller.Teleport(destination, direction, _distance))
			{
				owner.movement.verticalVelocity = 0f;
			}
		}
	}

	private Target FindClosestPlayerBody(Character character, Collider2D collider)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		List<Target> components = _enemyOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			return null;
		}
		if (components.Count == 1)
		{
			return components[0];
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < components.Count; i++)
		{
			if (!((Object)(object)components[i].character == (Object)null) && components[i].character.movement.isGrounded)
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
		return components[index];
	}
}
