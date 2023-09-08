using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBDTargetOpposition : Policy
{
	[SerializeField]
	private BehaviorTree _tree;

	[SerializeField]
	private string _ownerValueName = "Owner";

	[SerializeField]
	private string _targetValueName = "Target";

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
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		SharedCharacter sharedCharacter = ((Behavior)_tree).GetVariable(_ownerValueName) as SharedCharacter;
		if (!(((Behavior)_tree).GetVariable(_targetValueName) is SharedCharacter sharedCharacter2))
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		Character value = ((SharedVariable<Character>)sharedCharacter2).Value;
		Character value2 = ((SharedVariable<Character>)sharedCharacter).Value;
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(((Component)value).transform.position);
		}
		Bounds platform = value.movement.controller.collisionState.lastStandingCollider.bounds;
		Vector3 center = ((Bounds)(ref platform)).center;
		float x = CalculateX(value, ref platform, center);
		float num = CalculateY(value, platform);
		x = ClampX(value2, x, platform);
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
