using Characters;
using UnityEngine;

namespace FX;

public abstract class VisualEffect : MonoBehaviour
{
	public static void PostProcess(PoolObject poolObject, Character target, float scale, float angle, Vector3 offset, EffectInfo.AttachInfo attachInfo, bool relativeScaleToTargetSize, bool overwrite = false)
	{
	}

	public static void PostProcess(PoolObject poolObject, Character target, float scale, float angle, Vector3 offset, bool attachToTarget, bool relativeScaleToTargetSize, bool overwrite = false)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		if (relativeScaleToTargetSize)
		{
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			num = Mathf.Min(size.x, size.y);
		}
		if (attachToTarget)
		{
			((Component)poolObject).transform.parent = ((Component)target).transform;
		}
		PostProcess(poolObject, scale * num, angle, offset, overwrite);
	}

	public static void PostProcess(PoolObject poolObject, float scale, float angle, Vector3 offset, bool overwrite = false)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		if (overwrite)
		{
			((Component)poolObject).transform.localScale = Vector3.one * scale;
			((Component)poolObject).transform.eulerAngles = new Vector3(0f, 0f, angle);
		}
		else
		{
			Transform transform = ((Component)poolObject).transform;
			transform.localScale *= scale;
			Transform transform2 = ((Component)poolObject).transform;
			transform2.eulerAngles += new Vector3(0f, 0f, angle);
		}
		Transform transform3 = ((Component)poolObject).transform;
		transform3.localPosition += offset;
	}
}
