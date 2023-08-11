using GameResources;
using Singletons;
using UnityEngine;

namespace FX;

public class ScreenFlashSpawner : Singleton<ScreenFlashSpawner>
{
	private static class Assets
	{
		internal static readonly PoolObject effect = CommonResource.instance.screenFlashEffect;
	}

	public ScreenFlash Spawn(ScreenFlash.Info info)
	{
		PoolObject obj = Assets.effect.Spawn(true);
		((Component)obj).transform.parent = ((Component)this).transform;
		ScreenFlash component = ((Component)obj).GetComponent<ScreenFlash>();
		component.Play(info);
		return component;
	}
}
