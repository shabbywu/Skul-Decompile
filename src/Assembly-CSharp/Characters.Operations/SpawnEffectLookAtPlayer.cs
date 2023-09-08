using System.Collections.Generic;
using Characters.AI;
using FX;
using UnityEngine;

namespace Characters.Operations;

public class SpawnEffectLookAtPlayer : CharacterOperation
{
	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private bool _attachToSpawnPosition;

	[SerializeField]
	private AIController _aIController;

	[SerializeField]
	private EffectInfo _info;

	private readonly List<ReusableChronoSpriteEffect> _effects = new List<ReusableChronoSpriteEffect>();

	private void Awake()
	{
		if ((Object)(object)_spawnPosition == (Object)null)
		{
			_spawnPosition = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		float x = _spawnPosition.position.x;
		float x2 = ((Component)owner).transform.position.x;
		float x3 = ((Component)_aIController.target).transform.position.x;
		_info.flipXByOwnerDirection = ((Mathf.Abs(x2 - x) < Mathf.Abs(x2 - x3)) ? true : false);
		EffectInfo info = _info;
		Vector3 position = _spawnPosition.position;
		Quaternion rotation = _spawnPosition.rotation;
		EffectPoolInstance effectPoolInstance = info.Spawn(position, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
		if (_attachToSpawnPosition)
		{
			((Component)effectPoolInstance).transform.parent = _spawnPosition;
		}
	}

	public override void Stop()
	{
		_info.DespawnChildren();
	}
}
