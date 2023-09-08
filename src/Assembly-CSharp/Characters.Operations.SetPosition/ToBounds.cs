using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBounds : Policy
{
	[SerializeField]
	private LayerMask _groundMask = Layers.groundMask;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private bool _onPlatform;

	private static NonAllocCaster _belowCaster;

	static ToBounds()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_belowCaster = new NonAllocCaster(1);
	}

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (!_onPlatform)
		{
			return MMMaths.RandomPointWithinBounds(_collider.bounds);
		}
		return GetProjectionPointToPlatform(_groundMask);
	}

	private Vector2 GetProjectionPointToPlatform(LayerMask layerMask, float distance = 100f)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.RandomPointWithinBounds(_collider.bounds);
		((ContactFilter2D)(ref _belowCaster.contactFilter)).SetLayerMask(layerMask);
		_belowCaster.RayCast(val, Vector2.down, distance);
		ReadonlyBoundedList<RaycastHit2D> results = _belowCaster.results;
		if (results.Count < 0)
		{
			return val;
		}
		int index = 0;
		RaycastHit2D val2 = results[0];
		float num = ((RaycastHit2D)(ref val2)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val2 = results[i];
			float distance2 = ((RaycastHit2D)(ref val2)).distance;
			if (distance2 < num)
			{
				num = distance2;
				index = i;
			}
		}
		val2 = results[index];
		return ((RaycastHit2D)(ref val2)).point;
	}
}
