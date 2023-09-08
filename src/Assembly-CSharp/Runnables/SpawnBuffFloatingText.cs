using Characters;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public class SpawnBuffFloatingText : Runnable
{
	[SerializeField]
	private string _floatingTextkey;

	[SerializeField]
	private Transform _floatingPoint;

	[SerializeField]
	private bool _toPlayerPosition;

	public override void Run()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.op_Implicit(((Component)this).transform.position);
		if ((Object)(object)_floatingPoint != (Object)null)
		{
			val = Vector2.op_Implicit(_floatingPoint.position);
		}
		if (_toPlayerPosition)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Bounds bounds = ((Collider2D)player.collider).bounds;
			float x = ((Bounds)(ref bounds)).center.x;
			bounds = ((Collider2D)player.collider).bounds;
			((Vector2)(ref val))._002Ector(x, ((Bounds)(ref bounds)).max.y + 0.5f);
		}
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(Localization.GetLocalizedString(_floatingTextkey), Vector2.op_Implicit(val));
	}
}
