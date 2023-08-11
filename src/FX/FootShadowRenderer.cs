using System.Collections.ObjectModel;
using GameResources;
using PhysicsUtils;
using UnityEngine;

namespace FX;

public class FootShadowRenderer
{
	private class Assets
	{
		internal static readonly SpriteRenderer footShadow;

		static Assets()
		{
			footShadow = CommonResource.instance.footShadow;
		}
	}

	private const float _maxDistance = 5f;

	private LineSequenceNonAllocCaster _lineSequenceCaster;

	private Vector2 _size;

	public SpriteRenderer spriteRenderer { get; private set; }

	public FootShadowRenderer(int accuracy, Transform transform)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0038: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		_lineSequenceCaster = new LineSequenceNonAllocCaster(1, accuracy * 2 + 1)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.down,
				distance = 5f
			}
		};
		((ContactFilter2D)(ref _lineSequenceCaster.caster.contactFilter)).SetLayerMask(Layers.groundMask);
		spriteRenderer = Object.Instantiate<SpriteRenderer>(Assets.footShadow, transform);
		_size = spriteRenderer.size;
	}

	public void SetBounds(Bounds bounds)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		_lineSequenceCaster.start = Vector2.op_Implicit(((Bounds)(ref bounds)).min);
		_lineSequenceCaster.end.x = ((Bounds)(ref bounds)).max.x;
		_lineSequenceCaster.end.y = ((Bounds)(ref bounds)).min.y;
		_size.x = ((Bounds)(ref bounds)).size.x;
		spriteRenderer.size = _size;
	}

	public void Update()
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		_lineSequenceCaster.Cast();
		ReadOnlyCollection<NonAllocCaster> nonAllocCasters = _lineSequenceCaster.nonAllocCasters;
		int num = -1;
		for (int i = 0; i < nonAllocCasters.Count; i++)
		{
			if (nonAllocCasters[i].results.Count == 0)
			{
				continue;
			}
			if (num != -1)
			{
				RaycastHit2D val = nonAllocCasters[num].results[0];
				float distance = ((RaycastHit2D)(ref val)).distance;
				val = nonAllocCasters[i].results[0];
				if (!(distance > ((RaycastHit2D)(ref val)).distance))
				{
					continue;
				}
			}
			num = i;
		}
		if (num == -1)
		{
			((Component)spriteRenderer).gameObject.SetActive(false);
			return;
		}
		RaycastHit2D val2 = nonAllocCasters[num].results[0];
		float distance2 = ((RaycastHit2D)(ref val2)).distance;
		Vector3 position = ((Component)spriteRenderer).transform.position;
		position.y = ((RaycastHit2D)(ref val2)).point.y;
		float num2 = (5f - distance2) / 5f;
		((Component)spriteRenderer).gameObject.SetActive(true);
		((Component)spriteRenderer).transform.position = position;
		((Component)spriteRenderer).transform.localScale = Vector3.one * num2;
	}

	public void DrawDebugLine()
	{
		_lineSequenceCaster.DrawDebugLine();
	}
}
