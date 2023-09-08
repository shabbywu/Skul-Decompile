using Data;
using GameResources;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class DropSkulHead : Operation
{
	private class Assets
	{
		internal static readonly PoolObject skulHead = CommonResource.instance.droppedSkulHead;

		internal static readonly PoolObject heroSkulHead = CommonResource.instance.droppedHeroSkulHead;
	}

	public override void Run(IProjectile projectile)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		PoolObject val = Assets.skulHead;
		if (GameData.HardmodeProgress.hardmode && GameData.Generic.skinIndex == 1)
		{
			val = Assets.heroSkulHead;
		}
		val.Spawn(((Component)this).transform.position, true);
	}
}
