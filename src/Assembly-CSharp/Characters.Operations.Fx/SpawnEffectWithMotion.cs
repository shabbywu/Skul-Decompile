using FX;
using UnityEngine;

namespace Characters.Operations.Fx;

public class SpawnEffectWithMotion : CharacterOperation
{
	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private EffectInfo _info;

	private Character _owner;

	private EffectPoolInstance _spawned;

	private void Awake()
	{
		if ((Object)(object)_spawnPosition == (Object)null)
		{
			_spawnPosition = ((Component)this).transform;
		}
		_info.loop = true;
	}

	public override void Run(Character owner)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_owner = owner;
		_ = _info.duration;
		_info.duration = 0f;
		_spawned = _info.Spawn(_spawnPosition.position, owner);
		owner.health.onTookDamage += Health_onTookDamage;
	}

	private void Health_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		_spawned.Stop();
		_owner.health.onTookDamage -= Health_onTookDamage;
	}
}
