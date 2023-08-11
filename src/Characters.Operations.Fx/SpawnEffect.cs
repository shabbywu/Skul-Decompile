using FX;
using UnityEngine;

namespace Characters.Operations.Fx;

public class SpawnEffect : CharacterOperation
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_info.Dispose();
	}

	public override void Run(Character owner)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		EffectPoolInstance effectPoolInstance;
		if (_extraAngleBySpawnPositionRotation)
		{
			EffectInfo info = _info;
			Vector3 position = _spawnPosition.position;
			Quaternion rotation = _spawnPosition.rotation;
			effectPoolInstance = info.Spawn(position, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
		}
		else
		{
			effectPoolInstance = _info.Spawn(_spawnPosition.position, owner);
		}
		if (_attachToSpawnPosition)
		{
			((Component)effectPoolInstance).transform.parent = _spawnPosition;
		}
		if (_scaleBySpawnPositionScale)
		{
			Vector3 lossyScale = _spawnPosition.lossyScale;
			lossyScale.x = Mathf.Abs(lossyScale.x);
			lossyScale.y = Mathf.Abs(lossyScale.y);
			((Component)effectPoolInstance).transform.localScale = Vector3.Scale(((Component)effectPoolInstance).transform.localScale, lossyScale);
		}
	}

	public override void Stop()
	{
		_info.DespawnChildren();
	}
}
