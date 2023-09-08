using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToLookingSide : Policy
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _lastStandingCollider = true;

	[SerializeField]
	private bool _opposition;

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
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
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
		Character.LookingDirection lookingDirection = _owner.lookingDirection;
		float num = ClampX(_owner, lookingDirection, collider.bounds);
		float num2 = CalculateY(_owner, collider.bounds);
		return new Vector2(num, num2);
	}

	private float ClampX(Character owner, Character.LookingDirection direciton, Bounds platform)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds;
		if (!_opposition)
		{
			if (direciton == Character.LookingDirection.Right)
			{
				float x = ((Bounds)(ref platform)).max.x;
				float num;
				if (!_colliderInterpolate)
				{
					num = 0f;
				}
				else
				{
					bounds = ((Collider2D)owner.collider).bounds;
					num = 0f - ((Bounds)(ref bounds)).extents.x;
				}
				return x + num;
			}
			float x2 = ((Bounds)(ref platform)).min.x;
			float num2;
			if (!_colliderInterpolate)
			{
				num2 = 0f;
			}
			else
			{
				bounds = ((Collider2D)owner.collider).bounds;
				num2 = ((Bounds)(ref bounds)).extents.x;
			}
			return x2 + num2;
		}
		if (direciton == Character.LookingDirection.Right)
		{
			float x3 = ((Bounds)(ref platform)).min.x;
			float num3;
			if (!_colliderInterpolate)
			{
				num3 = 0f;
			}
			else
			{
				bounds = ((Collider2D)owner.collider).bounds;
				num3 = ((Bounds)(ref bounds)).extents.x;
			}
			return x3 + num3;
		}
		float x4 = ((Bounds)(ref platform)).max.x;
		float num4;
		if (!_colliderInterpolate)
		{
			num4 = 0f;
		}
		else
		{
			bounds = ((Collider2D)owner.collider).bounds;
			num4 = 0f - ((Bounds)(ref bounds)).extents.x;
		}
		return x4 + num4;
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
}
