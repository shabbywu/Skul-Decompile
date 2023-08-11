using System;
using System.Collections.Generic;
using Characters.Minions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Player;

public class MinionLeader : IDisposable
{
	public delegate void MinionCommands(Minion minion);

	public readonly Character player;

	private readonly Dictionary<Minion, MinionGroup> _minionGroups = new Dictionary<Minion, MinionGroup>();

	public MinionLeader(Character player)
	{
		this.player = player;
		Singleton<Service>.Instance.levelManager.onMapLoaded += ResetMinionPositions;
	}

	public void Dispose()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= ResetMinionPositions;
		}
	}

	public IEnumerable<Minion> GetMinionEnumerable()
	{
		foreach (KeyValuePair<Minion, MinionGroup> minionGroup in _minionGroups)
		{
			foreach (Minion item in minionGroup.Value)
			{
				yield return item;
			}
		}
	}

	public IEnumerable<Minion> GetMinionEnumerable(Minion targetMinion)
	{
		if (!_minionGroups.TryGetValue(targetMinion, out var value))
		{
			yield break;
		}
		foreach (Minion item in value)
		{
			yield return item;
		}
	}

	public Minion Summon(Minion minionPrefab, Vector3 position, MinionSetting seting)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (!_minionGroups.TryGetValue(minionPrefab, out var value))
		{
			value = new MinionGroup();
			_minionGroups.Add(minionPrefab, value);
		}
		if (value.Count >= minionPrefab.maxCount)
		{
			value.DespawnOldest();
		}
		return minionPrefab.Summon(this, position, value, seting);
	}

	public void Commands(Minion target, MinionCommands commands)
	{
		if (!_minionGroups.ContainsKey(target))
		{
			return;
		}
		_minionGroups.TryGetValue(target, out var value);
		foreach (Minion item in value)
		{
			commands?.Invoke(item);
		}
	}

	public void DespawnAll(Minion target)
	{
		if (_minionGroups.ContainsKey(target))
		{
			_minionGroups.TryGetValue(target, out var value);
			value.DespawnAll();
		}
	}

	private void ResetMinionPositions()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		foreach (KeyValuePair<Minion, MinionGroup> minionGroup in _minionGroups)
		{
			foreach (Minion item in minionGroup.Value)
			{
				((Component)item).transform.position = ((Component)player).transform.position;
				if ((Object)(object)item.character.movement != (Object)null)
				{
					item.character.movement.controller.ResetBounds();
				}
			}
		}
	}
}
