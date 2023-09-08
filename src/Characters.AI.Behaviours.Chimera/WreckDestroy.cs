using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using PhysicsUtils;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class WreckDestroy : Behaviour
{
	[Header("Ready")]
	[SerializeField]
	private OperationInfos _readyOperations;

	[SerializeField]
	[Header("Attack")]
	private OperationInfos _attackOperations;

	[Header("End")]
	[SerializeField]
	private OperationInfos _endOperations;

	[Header("Hit")]
	[SerializeField]
	private OperationInfos _hitOperations;

	[SerializeField]
	private Collider2D _wreckFindRange;

	private static readonly NonAllocOverlapper _wreckOverlapper;

	static WreckDestroy()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_wreckOverlapper = new NonAllocOverlapper(100);
		((ContactFilter2D)(ref _wreckOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Awake()
	{
		_readyOperations.Initialize();
		_attackOperations.Initialize();
		_endOperations.Initialize();
		_hitOperations.Initialize();
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public void Attack(Character character)
	{
		((Component)_attackOperations).gameObject.SetActive(true);
		_attackOperations.Run(character);
	}

	public void End(Character character)
	{
		character.status.unstoppable.Detach(character);
		((Component)_endOperations).gameObject.SetActive(true);
		_endOperations.Run(character);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		DestroyWreck(controller.character);
		((Component)_hitOperations).gameObject.SetActive(true);
		_hitOperations.Run(controller.character);
		base.result = Result.Done;
		yield break;
	}

	private List<ChimeraWreck> GetChimeraWrecks()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _wreckOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
		return _wreckOverlapper.OverlapCollider(_wreckFindRange).GetComponents<ChimeraWreck>(true);
	}

	public void DestroyWreck(Character character)
	{
		List<ChimeraWreck> chimeraWrecks = GetChimeraWrecks();
		for (int i = 0; i < chimeraWrecks.Count; i++)
		{
			chimeraWrecks[i].DestroyProp(character);
		}
	}
}
