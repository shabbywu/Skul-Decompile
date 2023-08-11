using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class BreakPlayerShield : Operation
{
	public override void Run()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		if (!((Object)(object)player == (Object)null))
		{
			if (player.health.shield.amount > 0.0)
			{
				Bounds bounds = ((Collider2D)player.collider).bounds;
				float x = ((Bounds)(ref bounds)).center.x;
				bounds = ((Collider2D)player.collider).bounds;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(x, ((Bounds)(ref bounds)).max.y);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnBreakShield(Vector2.op_Implicit(val));
			}
			while (player.health.shield.amount > 0.0)
			{
				player.health.shield.Consume(player.health.shield.amount + 1.0);
			}
		}
	}
}
