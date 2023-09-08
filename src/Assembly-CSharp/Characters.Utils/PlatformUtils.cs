using PhysicsUtils;
using UnityEngine;

namespace Characters.Utils;

public static class PlatformUtils
{
	public static Collider2D GetClosestPlatform(Vector2 origin, Vector2 direction, NonAllocCaster caster, LayerMask layerMask, float distance = 100f)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref caster.contactFilter)).SetLayerMask(layerMask);
		caster.RayCast(origin, direction, distance);
		ReadonlyBoundedList<RaycastHit2D> results = caster.results;
		if (results.Count < 0)
		{
			return null;
		}
		int index = 0;
		RaycastHit2D val = results[0];
		float num = ((RaycastHit2D)(ref val)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val = results[i];
			float distance2 = ((RaycastHit2D)(ref val)).distance;
			if (distance2 < num)
			{
				num = distance2;
				index = i;
			}
		}
		val = results[index];
		return ((RaycastHit2D)(ref val)).collider;
	}

	public static Vector2 GetProjectionPointToPlatform(Vector2 origin, Vector2 direction, NonAllocCaster caster, LayerMask layerMask, float distance = 100f)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref caster.contactFilter)).SetLayerMask(layerMask);
		caster.RayCast(origin, direction, distance);
		ReadonlyBoundedList<RaycastHit2D> results = caster.results;
		if (results.Count <= 0)
		{
			return origin;
		}
		int index = 0;
		RaycastHit2D val = results[0];
		float num = ((RaycastHit2D)(ref val)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val = results[i];
			float distance2 = ((RaycastHit2D)(ref val)).distance;
			if (distance2 < num)
			{
				num = distance2;
				index = i;
			}
		}
		val = results[index];
		return ((RaycastHit2D)(ref val)).point;
	}

	public static bool Teleport(Transform traget, Bounds bounds, Vector2 destination, float maxRetryDistance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.Vector3ToVector2(traget.position) - destination;
		return Teleport(traget, bounds, destination, ((Vector2)(ref val)).normalized, maxRetryDistance);
	}

	public static bool Teleport(Transform traget, Bounds bounds, Vector2 destination, Vector2 direction, float maxRetryDistance)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; (float)i <= maxRetryDistance; i++)
		{
			if (Teleport(traget, bounds, destination + direction * (float)i))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Teleport(Transform traget, Bounds bounds, Vector2 destination)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		((Bounds)(ref bounds)).center = Vector2.op_Implicit(new Vector2(destination.x, destination.y + (((Bounds)(ref bounds)).center.y - ((Bounds)(ref bounds)).min.y)));
		((ContactFilter2D)(ref NonAllocOverlapper.shared.contactFilter)).SetLayerMask(Layers.terrainMask);
		if (NonAllocOverlapper.shared.OverlapBox(Vector2.op_Implicit(((Bounds)(ref bounds)).center), Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f).results.Count == 0)
		{
			traget.position = Vector2.op_Implicit(destination);
			return true;
		}
		return false;
	}
}
