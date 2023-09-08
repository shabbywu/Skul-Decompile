using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class StomArrow : CharacterOperation
{
	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	private Transform _spawnPointContainer;

	[SerializeField]
	private int _emptyCount = 2;

	private int[] _numbers;

	private void Awake()
	{
		_numbers = new int[_spawnPointContainer.childCount];
		for (int i = 0; i < _spawnPointContainer.childCount; i++)
		{
			_numbers[i] = i;
		}
	}

	public override void Run(Character owner)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		_numbers.Shuffle();
		for (int i = 0; i < _spawnPointContainer.childCount - _emptyCount; i++)
		{
			int num = _numbers[i];
			Vector3 position = _spawnPointContainer.GetChild(num).position;
			OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
			((Component)operationInfos).transform.position = position;
			operationInfos.Run(owner);
		}
	}
}
