using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public static class PlaftormSelector
{
	private static NonAllocCaster caster;

	static PlaftormSelector()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		caster = new NonAllocCaster(1);
	}

	private static Collider2D GetPlatform()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref caster.contactFilter)).SetLayerMask(Layers.groundMask);
		NonAllocCaster obj = caster;
		Vector2 val = Vector2.op_Implicit(((Component)Singleton<Service>.Instance.levelManager.player).transform.position);
		Bounds bounds = ((Collider2D)Singleton<Service>.Instance.levelManager.player.collider).bounds;
		NonAllocCaster val2 = obj.BoxCast(val, Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f, Vector2.down, 100f);
		if (val2.results.Count == 0)
		{
			return null;
		}
		RaycastHit2D val3 = val2.results[0];
		return ((RaycastHit2D)(ref val3)).collider;
	}
}
