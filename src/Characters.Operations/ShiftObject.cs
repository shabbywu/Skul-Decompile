using Characters.AI;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public class ShiftObject : CharacterOperation
{
	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private float _offsetY;

	[SerializeField]
	private float _offsetX;

	[SerializeField]
	private bool _lastStandingPlatform = true;

	[SerializeField]
	private bool _fromPlatform;

	[SerializeField]
	private bool _underTheCeiling;

	private static NonAllocCaster caster;

	static ShiftObject()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		caster = new NonAllocCaster(1);
	}

	public override void Run(Character owner)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		Character target = _controller.target;
		if (!((Object)(object)target == (Object)null))
		{
			Collider2D platform = GetPlatform();
			Bounds val = (((Object)(object)platform != (Object)null) ? platform.bounds : target.movement.controller.collisionState.lastStandingCollider.bounds);
			float num = ((Component)target).transform.position.x + _offsetX;
			float num2;
			if (_underTheCeiling)
			{
				num2 = (_fromPlatform ? GetClosestCeiling(_offsetY, Vector2.op_Implicit(new Vector2(((Component)target).transform.position.x, ((Bounds)(ref val)).max.y))) : GetClosestCeiling(_offsetY, ((Component)target).transform.position));
			}
			else
			{
				num2 = (_fromPlatform ? ((Bounds)(ref val)).max.y : ((Component)target).transform.position.y);
				num2 += _offsetY;
			}
			_object.position = Vector2.op_Implicit(new Vector2(num, num2));
		}
	}

	private float GetClosestCeiling(float distance, Vector3 from)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(from), Vector2.up, distance, LayerMask.op_Implicit(Layers.groundMask));
		if (RaycastHit2D.op_Implicit(val))
		{
			return ((RaycastHit2D)(ref val)).point.y;
		}
		return from.y + distance;
	}

	private Collider2D GetPlatform()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		if (_lastStandingPlatform)
		{
			return null;
		}
		((ContactFilter2D)(ref caster.contactFilter)).SetLayerMask(Layers.groundMask);
		NonAllocCaster obj = caster;
		Vector2 val = Vector2.op_Implicit(((Component)_controller.target).transform.position);
		Bounds bounds = ((Collider2D)_controller.target.collider).bounds;
		NonAllocCaster val2 = obj.BoxCast(val, Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f, Vector2.down, 100f);
		if (val2.results.Count == 0)
		{
			return null;
		}
		RaycastHit2D val3 = val2.results[0];
		return ((RaycastHit2D)(ref val3)).collider;
	}
}
