using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToOppositionPlatform : Policy
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private bool _onPlatform = true;

	[SerializeField]
	private bool _lastStandingCollider = true;

	[SerializeField]
	private bool _randomX;

	[SerializeField]
	private bool _colliderInterpolate;

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
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_owner == (Object)null)
		{
			return Vector2.op_Implicit(((Component)_owner).transform.position);
		}
		Collider2D collider = _owner.movement.controller.collisionState.lastStandingCollider;
		if (!_lastStandingCollider)
		{
			_owner.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
		}
		if ((Object)(object)collider == (Object)null)
		{
			return Vector2.op_Implicit(((Component)_owner).transform.position);
		}
		Bounds platform = collider.bounds;
		Vector3 center = ((Bounds)(ref platform)).center;
		float num = CalculateX(_owner, ref platform, center);
		float num2 = CalculateY(_owner, platform);
		if (_colliderInterpolate)
		{
			num = ClampX(_owner, num, platform);
		}
		return new Vector2(num, num2);
	}

	private float ClampX(Character owner, float x, Bounds platform)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		float num = x;
		float x2 = ((Bounds)(ref platform)).min.x;
		Bounds bounds = ((Collider2D)owner.collider).bounds;
		if (num <= x2 + ((Bounds)(ref bounds)).extents.x)
		{
			float x3 = ((Bounds)(ref platform)).min.x;
			bounds = ((Collider2D)owner.collider).bounds;
			x = x3 + ((Bounds)(ref bounds)).extents.x;
		}
		else
		{
			float num2 = x;
			float x4 = ((Bounds)(ref platform)).max.x;
			bounds = ((Collider2D)owner.collider).bounds;
			if (num2 >= x4 - ((Bounds)(ref bounds)).extents.x)
			{
				float x5 = ((Bounds)(ref platform)).max.x;
				bounds = ((Collider2D)owner.collider).bounds;
				x = x5 - ((Bounds)(ref bounds)).extents.x;
			}
		}
		return x;
	}

	private float CalculateY(Character target, Bounds platform)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (!_onPlatform)
		{
			return ((Component)target).transform.position.y;
		}
		return ((Bounds)(ref platform)).max.y;
	}

	private float CalculateX(Character target, ref Bounds platform, Vector3 center)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)target).transform.position.x > center.x)
		{
			return _randomX ? Random.Range(((Bounds)(ref platform)).min.x, ((Bounds)(ref platform)).center.x) : ((Bounds)(ref platform)).min.x;
		}
		return _randomX ? Random.Range(((Bounds)(ref platform)).center.x, ((Bounds)(ref platform)).max.x) : ((Bounds)(ref platform)).max.x;
	}
}
