using UnityEngine;

namespace Characters.Operations.Summon;

public class DespawnMinions : CharacterOperation
{
	[SerializeField]
	private Minion _keyPrefab;

	public override void Run(Character owner)
	{
		owner.playerComponents.minionLeader?.DespawnAll(_keyPrefab);
	}

	private void Despawn(Minion minion)
	{
		minion.Despawn();
	}
}
