using Characters.Projectiles;
using UnityEngine;

namespace Characters.Usables;

public sealed class EffectZoneSizeByProjectileCount : MonoBehaviour
{
	[SerializeField]
	private EffectZone _effectZone;

	[SerializeField]
	private Projectile[] _projectilesToCount;

	[SerializeField]
	private Vector2[] _sizeRangeByProjectileCount;

	private void OnEnable()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Projectile[] projectilesToCount = _projectilesToCount;
		foreach (Projectile projectile in projectilesToCount)
		{
			num += projectile.reusable.spawnedCount;
		}
		int num2 = Mathf.Clamp(num, 0, _sizeRangeByProjectileCount.Length - 1);
		_effectZone.sizeRange = _sizeRangeByProjectileCount[num2];
	}
}
