using FX;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class ScreenFlash : Operation
{
	[SerializeField]
	private FX.ScreenFlash.Info _info;

	public override void Run(IProjectile projectile)
	{
		Singleton<ScreenFlashSpawner>.Instance.Spawn(_info);
	}
}
