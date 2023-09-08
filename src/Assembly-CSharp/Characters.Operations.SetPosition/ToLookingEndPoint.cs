using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToLookingEndPoint : Policy
{
	private enum Point
	{
		Left,
		Center,
		Right
	}

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private Point _point;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = owner;
		}
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _owner.movement.controller.collisionState.lastStandingCollider.bounds;
		_ = _owner.lookingDirection;
		_ = 1;
		return (Vector2)(_point switch
		{
			Point.Left => new Vector2(ClampX(_owner, ((Bounds)(ref bounds)).min.x, bounds), ((Bounds)(ref bounds)).max.y), 
			Point.Center => new Vector2(ClampX(_owner, ((Bounds)(ref bounds)).center.x, bounds), ((Bounds)(ref bounds)).max.y), 
			Point.Right => new Vector2(ClampX(_owner, ((Bounds)(ref bounds)).max.x, bounds), ((Bounds)(ref bounds)).max.y), 
			_ => Vector2.op_Implicit(((Component)_owner).transform.position), 
		});
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
