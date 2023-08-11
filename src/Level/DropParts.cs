using System.Collections;
using UnityEngine;

namespace Level;

public class DropParts : MonoBehaviour
{
	[SerializeField]
	[FrameTime]
	private float _delay;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private ParticleEffectInfo _particleEffectInfo;

	private void OnDestroy()
	{
		_particleEffectInfo = null;
	}

	public IEnumerator CSpawn(Vector2 position, Bounds bounds, Vector2 force)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		yield return Chronometer.global.WaitForSeconds(_delay);
		_particleEffectInfo.Emit(position, bounds, force);
	}

	public IEnumerator CSpawn()
	{
		yield return Chronometer.global.WaitForSeconds(_delay);
		_particleEffectInfo.Emit(Vector2.op_Implicit(((Component)this).transform.position), _range.bounds, Vector2.zero);
	}
}
