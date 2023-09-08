using Characters.Monsters;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon;

public sealed class SummonDarkOrb : CharacterOperation
{
	[SerializeField]
	[Range(0f, 100f)]
	private int _healthPercent;

	[SerializeField]
	private bool _containSummonWave;

	[Information("비어 있어도 문제 없음,", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private MonsterContainer _container;

	[SerializeField]
	private Monster _monsterPrefab;

	[Information("비워둘 경우 플레이어 위치에 1마리 소환, 그 외에는 지정된 위치마다 소환됨", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private Transform[] _spawnPositions;

	public override void Run(Character owner)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (_spawnPositions.Length == 0)
		{
			Monster monster = _monsterPrefab.Summon(((Component)owner).transform.position);
			SetHealth(monster.character, owner);
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
			SetHealth(monster2.character, owner);
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

	private void SetHealth(Character target, Character owner)
	{
		double maximumHealth = owner.health.maximumHealth;
		target.health.SetMaximumHealth(maximumHealth * (double)_healthPercent * 0.009999999776482582);
		target.health.SetCurrentHealth(maximumHealth * (double)_healthPercent * 0.009999999776482582);
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
