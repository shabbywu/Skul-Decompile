using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Summon;

public class MinionGroupOperations : CharacterOperation
{
	[SerializeField]
	private Minion _keyPrefab;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	public override void Run(Character owner)
	{
		owner.playerComponents.minionLeader?.Commands(_keyPrefab, CommandTo);
	}

	private void CommandTo(Minion minion)
	{
		((MonoBehaviour)minion).StartCoroutine(_operations.CRun(minion.character));
	}
}
