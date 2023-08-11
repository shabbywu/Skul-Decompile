using GameResources;
using Singletons;
using UnityEngine;

namespace FX;

public class VignetteSpawner : Singleton<VignetteSpawner>
{
	private static class Assets
	{
		internal static readonly PoolObject effect = CommonResource.instance.vignetteEffect;
	}

	public void Spawn(Color startColor, Color endColor, Curve curve)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		PoolObject obj = Assets.effect.Spawn(true);
		((Component)obj).transform.SetParent(((Component)this).transform, false);
		((Component)obj).GetComponent<Vignette>().Initialize(startColor, endColor, curve);
	}
}
