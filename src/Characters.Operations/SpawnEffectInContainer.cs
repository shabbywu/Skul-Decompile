using FX;
using UnityEngine;

namespace Characters.Operations;

public sealed class SpawnEffectInContainer : CharacterOperation
{
	[SerializeField]
	private Transform _container;

	[SerializeField]
	private bool _attachToSpawnPosition;

	[SerializeField]
	private bool _scaleBySpawnPositionScale;

	[SerializeField]
	private EffectInfo _info;

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in _container)
		{
			Transform val = item;
			EffectInfo info = _info;
			Vector3 position = val.position;
			Quaternion rotation = val.rotation;
			EffectPoolInstance effectPoolInstance = info.Spawn(position, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
			if (_attachToSpawnPosition)
			{
				((Component)effectPoolInstance).transform.parent = val;
			}
			if (_scaleBySpawnPositionScale)
			{
				Vector3 lossyScale = val.lossyScale;
				lossyScale.x = Mathf.Abs(lossyScale.x);
				lossyScale.y = Mathf.Abs(lossyScale.y);
				((Component)effectPoolInstance).transform.localScale = Vector3.Scale(((Component)effectPoolInstance).transform.localScale, lossyScale);
			}
		}
	}

	public override void Stop()
	{
		_info.DespawnChildren();
	}
}
