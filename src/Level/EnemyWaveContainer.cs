using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level;

public class EnemyWaveContainer : MonoBehaviour, IEnumerable<Character>, IEnumerable
{
	public enum State
	{
		Empty,
		Remain
	}

	public State state { get; private set; }

	public Wave[] waves { get; private set; }

	public EnemyWave[] enemyWaves { get; private set; }

	public SummonWave summonWave { get; private set; }

	public SummonWave summonEnemyWave { get; private set; }

	public event Action<State> onStateChanged;

	public void Initialize()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		waves = ((Component)this).GetComponentsInChildren<Wave>(true);
		enemyWaves = ((Component)this).GetComponentsInChildren<EnemyWave>(true);
		GameObject val = new GameObject("SummonWave");
		val.transform.SetParent(((Component)this).transform);
		summonWave = val.AddComponent<SummonWave>();
		GameObject val2 = new GameObject("SummonEnemyWave");
		val2.transform.SetParent(((Component)this).transform);
		summonEnemyWave = val2.AddComponent<SummonWave>();
		Wave[] array = waves;
		foreach (Wave obj in array)
		{
			obj.Initialize();
			obj.onClear += CheckWaveState;
			obj.onSpawn += CheckWaveState;
		}
		summonEnemyWave.onClear += CheckWaveState;
		state = GetState();
		this.onStateChanged?.Invoke(state);
	}

	public void HideAll()
	{
		EnemyWave[] array = enemyWaves;
		foreach (EnemyWave enemyWave in array)
		{
			foreach (Character character in enemyWave.characters)
			{
				((Component)character).gameObject.SetActive(false);
			}
			((Component)enemyWave).gameObject.SetActive(false);
		}
	}

	public List<Character> GetAllEnemies()
	{
		List<Character> list = new List<Character>();
		EnemyWave[] array = enemyWaves;
		foreach (EnemyWave enemyWave in array)
		{
			list.AddRange(enemyWave.characters);
		}
		if ((Object)(object)summonWave != (Object)null)
		{
			list.AddRange(summonWave.characters);
		}
		if ((Object)(object)summonEnemyWave != (Object)null)
		{
			list.AddRange(summonEnemyWave.characters);
		}
		return list;
	}

	public List<Character> GetAllSpawnedEnemies()
	{
		List<Character> list = new List<Character>();
		EnemyWave[] array = enemyWaves;
		foreach (EnemyWave enemyWave in array)
		{
			if (enemyWave.state == Wave.State.Spawned)
			{
				list.AddRange(enemyWave.characters);
			}
		}
		if ((Object)(object)summonWave != (Object)null)
		{
			list.AddRange(summonWave.characters);
		}
		if ((Object)(object)summonEnemyWave != (Object)null)
		{
			list.AddRange(summonEnemyWave.characters);
		}
		return list;
	}

	public int GetAllSpawnedEnemiesCount()
	{
		int num = 0;
		EnemyWave[] array = enemyWaves;
		foreach (EnemyWave enemyWave in array)
		{
			if (enemyWave.state == Wave.State.Spawned)
			{
				num += enemyWave.characters.Count;
			}
		}
		if ((Object)(object)summonWave != (Object)null)
		{
			num += summonWave.characters.Count;
		}
		if ((Object)(object)summonEnemyWave != (Object)null)
		{
			num += summonEnemyWave.characters.Count;
		}
		return num;
	}

	public IEnumerator<Character> GetEnumerator()
	{
		EnemyWave[] array = enemyWaves;
		foreach (EnemyWave enemyWave in array)
		{
			foreach (Character character in enemyWave.characters)
			{
				yield return character;
			}
		}
		if (!((Object)(object)summonWave != (Object)null))
		{
			yield break;
		}
		foreach (Character character2 in summonWave.characters)
		{
			yield return character2;
		}
	}

	public Character GetRandomEnemy()
	{
		int num = 0;
		EnemyWave[] array = enemyWaves;
		foreach (EnemyWave enemyWave in array)
		{
			num += enemyWave.characters.Count;
		}
		int num2 = Random.Range(0, num);
		array = enemyWaves;
		foreach (EnemyWave enemyWave2 in array)
		{
			if (num2 < enemyWave2.characters.Count)
			{
				return enemyWave2.characters[num2];
			}
			num2 -= enemyWave2.characters.Count;
		}
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Stop()
	{
		Wave[] array = waves;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
	}

	public void Attach(Character character)
	{
		((Component)character).transform.parent = ((Component)summonWave).transform;
		summonWave.Attach(character);
	}

	public void AttachToSummonEnemyWave(Character character)
	{
		((Component)character).transform.parent = ((Component)summonEnemyWave).transform;
		summonEnemyWave.Attach(character);
	}

	private State GetState()
	{
		State result = State.Empty;
		Wave[] array = waves;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].state == Wave.State.Spawned)
			{
				result = State.Remain;
				break;
			}
		}
		if (summonEnemyWave.state == Wave.State.Spawned)
		{
			result = State.Remain;
		}
		return result;
	}

	private void CheckWaveState()
	{
		State state = GetState();
		if (this.state != state)
		{
			this.state = state;
			this.onStateChanged?.Invoke(this.state);
		}
	}
}
