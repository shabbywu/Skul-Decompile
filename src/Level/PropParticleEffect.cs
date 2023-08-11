using UnityEngine;

namespace Level;

public class PropParticleEffect : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Prop _prop;

	[SerializeField]
	private PoolObject _effect;

	[SerializeField]
	private bool _relativeScaleToTargetSize = true;

	[SerializeField]
	private ParticleEffectInfo _particleInfo;

	public void Spawn(Vector2 spawnPoint, Vector2 force)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		if (_relativeScaleToTargetSize)
		{
			Bounds bounds = _prop.collider.bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			num = Mathf.Min(size.x, size.y);
		}
		if ((Object)(object)_effect != (Object)null)
		{
			((Component)_effect.Spawn(Vector2.op_Implicit(spawnPoint), true)).transform.localScale = Vector3.one * num;
		}
		_particleInfo?.Emit(Vector2.op_Implicit(((Component)_prop).transform.position), _prop.collider.bounds, force);
	}
}
