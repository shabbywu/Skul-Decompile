using Characters;
using UnityEngine;

namespace FX.EffectProperties;

public class SimpleScale : EffectProperty
{
	[SerializeField]
	private Vector3 _scale;

	[SerializeField]
	private float _angle;

	public override void Apply(PoolObject spawned, Character owner, Target target)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((Component)spawned).transform.localScale = _scale;
		((Component)spawned).transform.localEulerAngles = new Vector3(0f, 0f, _angle);
	}
}
