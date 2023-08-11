using System.Collections;
using System.Collections.Generic;
using Characters.Operations.Summon.SummonInRange.Policy;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon.Custom;

public class SupportingArcherArrowRain : CharacterOperation
{
	private static short spriteLayer = short.MinValue;

	[SerializeField]
	[Space]
	internal OperationRunner _operationRunner;

	[SerializeField]
	private Transform _summonPosition;

	[SerializeField]
	private BoxCollider2D _summonRange;

	[SerializeField]
	private int _summonCount;

	[SerializeField]
	private CustomFloat _delay;

	[SerializeReference]
	[SubclassSelector]
	private ISummonPosition _positionPolicy;

	[Space]
	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[Space]
	[SerializeField]
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(0f, 0f, _angle.value);
		List<OperationRunner> list = new List<OperationRunner>();
		Vector2 originPosition = Vector2.op_Implicit(_summonPosition.position);
		for (int i = 0; i < _summonCount; i++)
		{
			Vector2 position = _positionPolicy.GetPosition(originPosition, _summonRange.size.x, _summonCount, i);
			OperationRunner operationRunner = _operationRunner.Spawn();
			list.Add(operationRunner);
			((Component)operationRunner).transform.SetPositionAndRotation(Vector2.op_Implicit(position), Quaternion.Euler(val));
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
		((MonoBehaviour)this).StartCoroutine(CRunAfterDelay(owner, list));
	}

	private IEnumerator CRunAfterDelay(Character owner, List<OperationRunner> operationRunners)
	{
		foreach (OperationRunner operationRunner in operationRunners)
		{
			operationRunner.operationInfos.Run(owner);
			yield return (object)new WaitForSeconds(_delay.value);
		}
	}
}
