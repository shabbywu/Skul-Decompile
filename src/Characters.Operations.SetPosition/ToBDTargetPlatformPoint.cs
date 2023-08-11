using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBDTargetPlatformPoint : Policy
{
	private enum Point
	{
		Left,
		Center,
		Right,
		Random
	}

	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _targetName = "Target";

	[SerializeField]
	private LayerMask _platformLayer = Layers.footholdMask;

	[SerializeField]
	private bool _lastStandingCollider = true;

	[SerializeField]
	private bool _colliderInterpolation = true;

	[SerializeField]
	private Point _point;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetName)).Value;
		if ((Object)(object)value == (Object)null)
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = value.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				value.movement.TryGetClosestBelowCollider(out collider, _platformLayer);
			}
		}
		else
		{
			value.movement.TryGetClosestBelowCollider(out collider, _platformLayer);
		}
		Bounds bounds = collider.bounds;
		Vector2 result = default(Vector2);
		switch (_point)
		{
		case Point.Left:
			((Vector2)(ref result))._002Ector(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(value, ((Bounds)(ref bounds)).min.x, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		case Point.Center:
			((Vector2)(ref result))._002Ector(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(value, ((Bounds)(ref bounds)).center.x, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		case Point.Right:
			((Vector2)(ref result))._002Ector(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(value, ((Bounds)(ref bounds)).max.x, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		case Point.Random:
		{
			float num = Random.Range(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.x);
			((Vector2)(ref result))._002Ector(num, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(value, num, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		}
		default:
			return Vector2.op_Implicit(((Component)value).transform.position);
		}
	}

	private float ClampX(Character owner, float x, Bounds platform)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (x <= ((Bounds)(ref platform)).min.x + owner.collider.size.x)
		{
			x = ((Bounds)(ref platform)).min.x + owner.collider.size.x;
		}
		else if (x >= ((Bounds)(ref platform)).max.x - owner.collider.size.x)
		{
			x = ((Bounds)(ref platform)).max.x - owner.collider.size.x;
		}
		return x;
	}
}
