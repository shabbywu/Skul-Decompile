using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class VenomCannon : Behaviour
{
	[Header("Ready")]
	[SerializeField]
	private OperationInfos _readyOperations;

	[SerializeField]
	[Header("Fire")]
	private OperationInfos[] _fireOperations;

	private const int _maxOrder = 4;

	private int _order;

	private void Awake()
	{
		for (int i = 0; i < _fireOperations.Length; i++)
		{
			_fireOperations[i].Initialize();
		}
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		((Component)_fireOperations[_order]).gameObject.SetActive(true);
		_fireOperations[_order].Run(controller.character);
		_order++;
		_order %= 4;
		if (_order == 3)
		{
			((Component)_fireOperations[_order]).gameObject.SetActive(true);
			_fireOperations[_order].Run(controller.character);
			_order++;
			_order %= 4;
		}
		base.result = Result.Done;
		yield break;
	}
}
