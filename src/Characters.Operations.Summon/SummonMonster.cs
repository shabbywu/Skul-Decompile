using Characters.Monsters;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonMonster : CharacterOperation
{
	[SerializeField]
	private bool _containSummonWave;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private MonsterContainer _container;

	[SerializeField]
	private Monster _monsterPrefab;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private Transform[] _spawnPositions;

	public override void Run(Character owner)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (_spawnPositions.Length == 0)
		{
			Monster monster = _monsterPrefab.Summon(((Component)owner).transform.position);
			if ((Object)(object)_container != (Object)null)
			{
				AddContainer(monster);
			}
			if (_containSummonWave)
			{
				Map.Instance.waveContainer.summonWave.Attach(monster.character);
			}
			return;
		}
		Transform[] spawnPositions = _spawnPositions;
		foreach (Transform val in spawnPositions)
		{
			Monster monster2 = _monsterPrefab.Summon(val.position);
			if ((Object)(object)_container != (Object)null)
			{
				AddContainer(monster2);
			}
			if (_containSummonWave)
			{
				Map.Instance.waveContainer.summonWave.Attach(monster2.character);
			}
		}
	}

	private void AddContainer(Monster summoned)
	{
		_container.Add(summoned);
		summoned.character.health.onDied += OnDied;
		void OnDied()
		{
			_container.Remove(summoned);
			summoned.character.health.onDied -= OnDied;
		}
	}
}
