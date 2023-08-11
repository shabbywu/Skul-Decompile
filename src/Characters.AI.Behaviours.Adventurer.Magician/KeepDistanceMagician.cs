using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Adventurer.Magician;

public class KeepDistanceMagician : Behaviour
{
	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[SerializeField]
	private MoveToDestinationWithFly _moveToDestinationWithFly;

	[MinMaxSlider(0f, 30f)]
	[SerializeField]
	private Vector2Int _distance;

	[SerializeField]
	private float _minDistanceWithSide;

	[SerializeField]
	private Action _backMotion;

	[SerializeField]
	private Action _frontMotion;

	private Action _motion;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		Character target = controller.target;
		int num = Random.Range(((Vector2Int)(ref _distance)).x, ((Vector2Int)(ref _distance)).y);
		float moveDirection = GetMoveDirection(((Component)character).transform.position);
		SetMotion(Vector2.op_Implicit(((Component)character).transform.position), moveDirection, Vector2.op_Implicit(((Component)target).transform.position), character);
		SetDestination(controller, Vector2.op_Implicit(((Component)character).transform.position), moveDirection, num);
		_motion.TryStart();
		character.ForceToLookAt(((Component)character).transform.position.x + moveDirection);
		yield return _moveToDestinationWithFly.CRun(controller);
		if (!controller.stuned && !controller.dead)
		{
			character.CancelAction();
		}
		base.result = Result.Done;
	}

	private float GetMoveDirection(Vector3 origin)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = (MMMaths.RandomBool() ? Vector2.right : Vector2.left);
		if (RaycastHit2D.op_Implicit(Physics2D.Raycast(Vector2.op_Implicit(origin), val, _minDistanceWithSide, LayerMask.op_Implicit(Layers.groundMask))))
		{
			val *= -1f;
		}
		return val.x;
	}

	private void SetMotion(Vector2 origin, float direction, Vector2 target, Character character)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		if (target.x - origin.x > 0f && direction > 0f)
		{
			_motion = _frontMotion;
		}
		else if (target.x - origin.x > 0f && direction < 0f)
		{
			_motion = _backMotion;
		}
		else if (target.x - origin.x < 0f && direction > 0f)
		{
			_motion = _backMotion;
		}
		else if (target.x - origin.x < 0f && direction < 0f)
		{
			_motion = _frontMotion;
		}
		else
		{
			_motion = _frontMotion;
		}
	}

	private void SetDestination(AIController controller, Vector2 origin, float direction, float distance)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		Character character = controller.character;
		RaycastHit2D point;
		Bounds bounds;
		if ((Object)(object)character == (Object)null)
		{
			controller.destination = new Vector2(origin.x, origin.y);
		}
		else if (character.movement.TryBelowRayCast(character.movement.controller.terrainMask, out point, 20f))
		{
			Collider2D collider = ((RaycastHit2D)(ref point)).collider;
			if (direction > 0f)
			{
				float num = origin.x + direction * distance;
				bounds = collider.bounds;
				if (num > ((Bounds)(ref bounds)).max.x - 1f)
				{
					bounds = collider.bounds;
					controller.destination = new Vector2(((Bounds)(ref bounds)).max.x - 1f, origin.y);
				}
				else
				{
					controller.destination = new Vector2(origin.x + direction * distance, origin.y);
				}
			}
			else
			{
				float num2 = origin.x + direction * distance;
				bounds = collider.bounds;
				if (num2 < ((Bounds)(ref bounds)).min.x + 1f)
				{
					bounds = collider.bounds;
					controller.destination = new Vector2(((Bounds)(ref bounds)).min.x + 1f, origin.y);
				}
				else
				{
					controller.destination = new Vector2(origin.x + direction * distance, origin.y);
				}
			}
		}
		else
		{
			RaycastHit2D val = Physics2D.Raycast(origin, new Vector2(direction, 0f), distance, LayerMask.op_Implicit(Layers.terrainMask));
			float num3 = ((distance > 0f) ? (-0.5f) : 0.5f);
			controller.destination = new Vector2(origin.x + direction * distance + num3, origin.y);
			if (RaycastHit2D.op_Implicit(val))
			{
				bounds = controller.target.movement.controller.collisionState.lastStandingCollider.bounds;
				float x = ((Bounds)(ref bounds)).center.x;
				int num4 = ((!(((RaycastHit2D)(ref val)).point.x > x)) ? 1 : (-1));
				controller.destination = new Vector2(((RaycastHit2D)(ref val)).point.x + (float)num4, origin.y);
			}
		}
	}
}
