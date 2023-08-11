using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class Bite : Behaviour
{
	[SerializeField]
	private Collider2D _trigger;

	[SerializeField]
	[Header("Ready")]
	private OperationInfos _readyOperations;

	[SerializeField]
	[Header("Attack")]
	private OperationInfos _attackOperations;

	[Header("Hit")]
	[SerializeField]
	private OperationInfos _terrainHitOperations;

	[Header("End")]
	[SerializeField]
	private OperationInfos _endOperations;

	private void Awake()
	{
		_readyOperations.Initialize();
		_attackOperations.Initialize();
		_endOperations.Initialize();
		_terrainHitOperations.Initialize();
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public void Hit(Character character)
	{
		((Component)_terrainHitOperations).gameObject.SetActive(true);
		_terrainHitOperations.Run(character);
	}

	public void End(Character character)
	{
		((Component)_endOperations).gameObject.SetActive(true);
		_endOperations.Run(character);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		((Component)_attackOperations).gameObject.SetActive(true);
		_attackOperations.Run(controller.character);
		base.result = Result.Done;
		yield break;
	}

	public bool CanUse(AIController controller)
	{
		return (Object)(object)controller.FindClosestPlayerBody(_trigger) != (Object)null;
	}
}
