using System.Collections.Generic;
using System.Linq;
using PhysicsUtils;
using UnityEngine;

namespace Characters;

public static class TargetFinder
{
	private static NonAllocOverlapper _overalpper = new NonAllocOverlapper(32);

	private static NonAllocCaster _caster = new NonAllocCaster(32);

	public static bool BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direciton, float distance, LayerMask layerMask, ref RaycastHit2D hit)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(layerMask);
		return TryToGetClosestHit(_caster.BoxCast(origin, size, angle, direciton, distance).results, ref hit);
	}

	public static bool RayCast(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask, ref RaycastHit2D hit)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(layerMask);
		return TryToGetClosestHit(_caster.RayCast(origin, direction, distance).results, ref hit);
	}

	public static void FindTargetInRange(Vector2 origin, float radius, LayerMask layerMask, List<Target> targets)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		targets.Clear();
		((ContactFilter2D)(ref _overalpper.contactFilter)).SetLayerMask(layerMask);
		foreach (Collider2D result in _overalpper.OverlapCircle(origin, radius).results)
		{
			Target component = ((Component)result).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null))
			{
				targets.Add(component);
			}
		}
	}

	public static Character GetRandomTarget(Collider2D findRange, LayerMask layerMask)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overalpper.contactFilter)).SetLayerMask(layerMask);
		ReadonlyBoundedList<Collider2D> results = _overalpper.OverlapCollider(findRange).results;
		if (results.Count <= 0)
		{
			return null;
		}
		if (((IEnumerable<Collider2D>)results).Where((Collider2D collider) => (Object)(object)((Component)collider).GetComponent<Target>() != (Object)null).Count() <= 0)
		{
			return null;
		}
		Collider2D[] array = ((IEnumerable<Collider2D>)results).ToArray();
		ExtensionMethods.Shuffle<Collider2D>((IList<Collider2D>)array);
		Collider2D[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Target component = ((Component)array2[i]).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null))
			{
				return component.character;
			}
		}
		return null;
	}

	public static void FindCharacterInRange(Vector2 origin, float radius, LayerMask layerMask, List<Character> targets)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		targets.Clear();
		((ContactFilter2D)(ref _overalpper.contactFilter)).SetLayerMask(layerMask);
		foreach (Collider2D result in _overalpper.OverlapCircle(origin, radius).results)
		{
			Target component = ((Component)result).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null))
			{
				targets.Add(component.character);
			}
		}
	}

	public static Character FindClosestTarget(Collider2D range, Collider2D ownerCollider, LayerMask layerMask)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overalpper.contactFilter)).SetLayerMask(layerMask);
		return FindClosestTarget(_overalpper, range, ownerCollider);
	}

	public static Character FindClosestTarget(NonAllocOverlapper overlapper, Collider2D range, Collider2D ownerCollider)
	{
		return GetClosestTarget(overlapper.OverlapCollider(range).GetComponents<Target>(true), ownerCollider);
	}

	public static Character FindClosestTarget(Vector2 point, float range, LayerMask layerMask)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overalpper.contactFilter)).SetLayerMask(layerMask);
		return FindClosestTarget(_overalpper, point, range);
	}

	public static Character FindClosestTarget(NonAllocOverlapper overlapper, Vector2 point, float range)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return GetClosestTarget(overlapper.OverlapCircle(point, range).GetComponents<Target>(true), point);
	}

	public static Character FindClosestTarget(NonAllocOverlapper overlapper, Vector2 point, Vector2 size, float angle = 0f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return GetClosestTarget(overlapper.OverlapBox(point, size, angle).GetComponents<Target>(true), point);
	}

	private static Character GetClosestTarget(List<Target> results, Vector2 center)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (results.Count == 0)
		{
			return null;
		}
		if (results.Count == 1)
		{
			return results[0].character;
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < results.Count; i++)
		{
			Collider2D collider = results[i].collider;
			if ((Object)(object)results[i].character != (Object)null)
			{
				collider = (Collider2D)(object)results[i].character.collider;
			}
			float num2 = Vector2.Distance(center, Vector2.op_Implicit(((Component)collider).transform.position));
			if (num > num2)
			{
				index = i;
				num = num2;
			}
		}
		return results[index].character;
	}

	private static Character GetClosestTarget(List<Target> results, Collider2D ownerCollider)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (results.Count == 0)
		{
			return null;
		}
		if (results.Count == 1)
		{
			return results[0].character;
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < results.Count; i++)
		{
			ColliderDistance2D val = Physics2D.Distance(results[i].collider, ownerCollider);
			float distance = ((ColliderDistance2D)(ref val)).distance;
			if (num > distance)
			{
				index = i;
				num = distance;
			}
		}
		return results[index].character;
	}

	private static bool TryToGetClosestHit(ReadonlyBoundedList<RaycastHit2D> results, ref RaycastHit2D hit)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (results.Count <= 0)
		{
			return false;
		}
		int num = 0;
		RaycastHit2D val = results[0];
		float num2 = ((RaycastHit2D)(ref val)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val = results[i];
			float distance = ((RaycastHit2D)(ref val)).distance;
			if (distance < num2)
			{
				num2 = distance;
				num = i;
			}
		}
		hit = results[num];
		return true;
	}
}
