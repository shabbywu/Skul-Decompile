using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class MultiDecoy : CharacterOperation
{
	[SerializeField]
	private OperationRunner _decoy;

	[SerializeField]
	private Transform _spawnPointContainer;

	public override void Run(Character owner)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		int childCount = _spawnPointContainer.childCount;
		Shuffle();
		for (int i = 0; i < childCount - 1; i++)
		{
			OperationInfos operationInfos = _decoy.Spawn().operationInfos;
			((Component)operationInfos).transform.position = _spawnPointContainer.GetChild(i).position;
			operationInfos.Run(owner);
		}
		((Component)owner).transform.position = _spawnPointContainer.GetChild(childCount - 1).position;
	}

	private void Shuffle()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in _spawnPointContainer)
		{
			item.SetSiblingIndex(Random.Range(0, _spawnPointContainer.childCount - 1));
		}
	}
}
