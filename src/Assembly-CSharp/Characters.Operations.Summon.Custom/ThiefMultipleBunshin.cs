using System;
using System.Collections.Generic;
using Characters.Operations.Summon.SummonInRange.Policy;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon.Custom;

public class ThiefMultipleBunshin : CharacterOperation
{
	[Serializable]
	public struct OperationRunnerInfo
	{
		[SerializeField]
		internal OperationRunner operationRunner;

		[SerializeField]
		internal int count;
	}

	private static short spriteLayer = short.MinValue;

	[SerializeField]
	private OperationRunnerInfo[] _operationRunnerInfos;

	[SerializeField]
	private BoxCollider2D _summonRange;

	[SerializeReference]
	[SubclassSelector]
	private ISummonPosition _positionPolicy;

	[SerializeField]
	[Space]
	private bool _attachToOwner;

	[SerializeField]
	private bool _copyAttackDamage;

	private AttackDamage _attackDamage;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		List<OperationRunner> list = new List<OperationRunner>();
		Vector2 originPosition = Vector2.op_Implicit(((Component)_summonRange).transform.position);
		List<int> list2 = new List<int>();
		for (int i = 0; i < _operationRunnerInfos.Length; i++)
		{
			for (int j = 0; j < _operationRunnerInfos[i].count; j++)
			{
				list2.Add(i);
			}
		}
		list2.Shuffle();
		for (int k = 0; k < list2.Count; k++)
		{
			Vector2 position = _positionPolicy.GetPosition(originPosition, _summonRange.size.x, list2.Count, k);
			int num = list2[k];
			OperationRunner operationRunner = _operationRunnerInfos[num].operationRunner.Spawn();
			list.Add(operationRunner);
			((Component)operationRunner).transform.localScale = new Vector3(1f, 1f, 1f);
			((Component)operationRunner).transform.SetPositionAndRotation(Vector2.op_Implicit(position), Quaternion.identity);
		}
		foreach (OperationRunner item in list)
		{
			if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
			{
				item.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
				item.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
			}
			SortingGroup component = ((Component)item).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			if (_attachToOwner)
			{
				((Component)item).transform.parent = ((Component)this).transform;
			}
		}
		foreach (OperationRunner item2 in list)
		{
			item2.operationInfos.Run(owner);
		}
	}
}
