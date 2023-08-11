using Characters.AI;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToTargetOpposition : Policy
{
	[SerializeField]
	private AIController _ai;

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _randomX;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Character character = _ai.character;
		Character target = _ai.target;
		if ((Object)(object)target == (Object)null)
		{
			return Vector2.op_Implicit(((Component)character).transform.position);
		}
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(((Component)target).transform.position);
		}
		Bounds platform = target.movement.controller.collisionState.lastStandingCollider.bounds;
		Vector3 center = ((Bounds)(ref platform)).center;
		float x = CalculateX(target, ref platform, center);
		float num = CalculateY(target, platform);
		x = ClampX(character, x, platform);
		return new Vector2(x, num);
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
