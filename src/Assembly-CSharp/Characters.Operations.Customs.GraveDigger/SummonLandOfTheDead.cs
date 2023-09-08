using Characters.Abilities.Customs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs.GraveDigger;

public sealed class SummonLandOfTheDead : CharacterOperation
{
	[SerializeField]
	private GraveDiggerPassiveComponent _passive;

	[SerializeField]
	private float _groundFindingDistance;

	[SerializeField]
	private float _range;

	[SerializeField]
	private float _width;

	[SerializeField]
	private OperationRunner _operationRunner;

	[SerializeField]
	private OperationRunner _corpseSpawner;

	[SerializeField]
	private bool _copyAttackDamage;

	private AttackDamage _attackDamage;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner)
	{
		if (TryFindGround(owner, out var collider))
		{
			SummonLand(owner, collider);
		}
	}

	private bool TryFindGround(Character character, out Collider2D collider)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (character.movement.isGrounded)
		{
			collider = character.movement.controller.collisionState.lastStandingCollider;
			return true;
		}
		if (character.movement.TryGetClosestBelowCollider(out collider, Layers.groundMask))
		{
			return true;
		}
		return false;
	}

	private void SummonLand(Character owner, Collider2D collider)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = collider.bounds;
		Vector2 mostLeftTop = bounds.GetMostLeftTop();
		Vector2 mostRightTop = bounds.GetMostRightTop();
		Vector3 position = ((Component)owner).transform.position;
		mostLeftTop.x = math.max(mostLeftTop.x, position.x - _range);
		mostRightTop.x = math.min(mostRightTop.x, position.x + _range);
		float num = (mostRightTop.x - mostLeftTop.x) / _width;
		float num2 = num - (float)(int)num;
		Vector2 val = mostLeftTop;
		val.x = mostLeftTop.x + num2 * _width / 2f;
		short num3 = 0;
		while ((float)num3 < num)
		{
			Vector2 position2 = val;
			position2.x += _width * (float)num3;
			Summon(owner, position2, num3);
			num3++;
		}
		SummonCorpseSpawner(owner, Vector2.op_Implicit(position), mostLeftTop, mostRightTop);
	}

	private void Summon(Character owner, Vector2 position, short sortingOrder)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		OperationRunner operationRunner = _operationRunner.Spawn();
		OperationInfos operationInfos = operationRunner.operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.identity);
		if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
		{
			operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
			operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
		}
		SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
		if ((Object)(object)component != (Object)null)
		{
			component.sortingOrder = sortingOrder++;
		}
		operationInfos.Run(owner);
	}

	private void SummonCorpseSpawner(Character owner, Vector2 position, Vector2 left, Vector2 right)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		OperationInfos operationInfos = _corpseSpawner.Spawn().operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.identity);
		((Component)operationInfos).GetComponentInChildren<SpawnCorpseForLandOfTheDead>().Set(_passive, left, right);
		operationInfos.Run(owner);
	}
}
