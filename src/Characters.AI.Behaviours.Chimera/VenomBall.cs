using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class VenomBall : Behaviour
{
	[SerializeField]
	[Header("Ready")]
	private OperationInfos _readyOperations;

	[Header("Fire")]
	[SerializeField]
	private OperationInfos _fireOperation;

	private void Awake()
	{
		_fireOperation.Initialize();
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		((Component)_fireOperation).gameObject.SetActive(true);
		_fireOperation.Run(controller.character);
		base.result = Result.Done;
		yield break;
	}
}
