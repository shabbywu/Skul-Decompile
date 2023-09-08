using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class SpawnEffect : Operation
{
	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private bool _extraAngleBySpawnPositionRotation = true;

	[SerializeField]
	private bool _attachToSpawnPosition;

	[SerializeField]
	private bool _scaleBySpawnPositionScale;

	[SerializeField]
	private EffectInfo _info;

	private void Awake()
	{
		if ((Object)(object)_spawnPosition == (Object)null)
		{
			_spawnPosition = ((Component)this).transform;
		}
		_info.LoadReference();
	}

	private void OnDestroy()
	{
		_info.Dispose();
	}

	public override void Run(IProjectile projectile)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		Quaternion rotation;
		EffectPoolInstance effectPoolInstance;
		if (_extraAngleBySpawnPositionRotation)
		{
			EffectInfo info = _info;
			Vector3 position = _spawnPosition.position;
			rotation = _spawnPosition.rotation;
			effectPoolInstance = info.Spawn(position, ((Quaternion)(ref rotation)).eulerAngles.z);
		}
		else
		{
			effectPoolInstance = _info.Spawn(_spawnPosition.position);
		}
		if (_attachToSpawnPosition)
		{
			((Component)effectPoolInstance).transform.parent = _spawnPosition;
		}
		if (_scaleBySpawnPositionScale)
		{
			Vector3 lossyScale = _spawnPosition.lossyScale;
			lossyScale.x = Mathf.Abs(lossyScale.x);
			rotation = _spawnPosition.rotation;
			if (Mathf.Abs(((Quaternion)(ref rotation)).eulerAngles.y) == 180f)
			{
				lossyScale.x *= -1f;
			}
			lossyScale.y = Mathf.Abs(lossyScale.y);
			((Component)effectPoolInstance).transform.localScale = Vector3.Scale(((Component)effectPoolInstance).transform.localScale, lossyScale);
		}
	}
}
