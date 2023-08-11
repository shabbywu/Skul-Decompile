using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs;

public class SpawnEffectProportionToScale : Operation
{
	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private bool _attachToSpawnPosition;

	[SerializeField]
	private EffectInfo _info;

	private void Awake()
	{
		if ((Object)(object)_spawnPosition == (Object)null)
		{
			_spawnPosition = ((Component)this).transform;
		}
	}

	public override void Run(IProjectile projectile)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		EffectInfo info = _info;
		Vector3 position = _spawnPosition.position;
		Quaternion rotation = _spawnPosition.rotation;
		EffectPoolInstance effectPoolInstance = info.Spawn(position, ((Quaternion)(ref rotation)).eulerAngles.z);
		if (_attachToSpawnPosition)
		{
			((Component)effectPoolInstance).transform.parent = _spawnPosition;
		}
		((Component)effectPoolInstance).transform.localScale = ((Component)this).transform.lossyScale;
	}
}
