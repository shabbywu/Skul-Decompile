using Characters;
using Characters.Minions;
using UnityEngine;

namespace Level;

public class SaplingTarget : MonoBehaviour
{
	[SerializeField]
	private Minion _entMinion;

	[SerializeField]
	private EntSapling _sapling;

	[SerializeField]
	private LayerMask _terrainLayer;

	[SerializeField]
	private float _spawnableTime = 0.1f;

	[SerializeField]
	private CharacterSynchronization _sync;

	[SerializeField]
	private MinionSetting _overrideSetting;

	private float _spawnableCool;

	private bool _spawnable = true;

	public bool spawnable => _spawnable;

	public Minion SummonEntMinion(Character owner, float lifeTime)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)this).transform.position;
		Minion result = owner.playerComponents.minionLeader.Summon(_entMinion, position, _overrideSetting);
		_sapling.Despawn();
		_spawnable = false;
		return result;
	}

	private void OnEnable()
	{
		_spawnableCool = _spawnableTime;
		_spawnable = true;
	}

	private void OnDisable()
	{
		_spawnableCool = _spawnableTime;
		_spawnable = true;
	}

	private void Update()
	{
		if (!(_spawnableCool <= 0f))
		{
			_spawnableCool -= ((ChronometerBase)Chronometer.global).deltaTime;
		}
	}
}
