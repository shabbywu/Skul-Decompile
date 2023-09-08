using Characters.Monsters;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public sealed class ToOcculistPotPoint : Policy
{
	[SerializeField]
	private MonsterContainer _occulistPotContainer;

	[SerializeField]
	private Character _base;

	[SerializeField]
	private float _sizeOfPot = 1f;

	[SerializeField]
	private CustomFloat _distanceFromBaseHead;

	[SerializeField]
	private CustomFloat _minDistanceFromAnother;

	[SerializeField]
	private CustomFloat _stride;

	private static readonly NonAllocCaster _leftNonAllocCaster;

	private static readonly NonAllocCaster _rightNonAllocCaster;

	static ToOcculistPotPoint()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		_leftNonAllocCaster = new NonAllocCaster(1);
		((ContactFilter2D)(ref _leftNonAllocCaster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1 << LayerMask.op_Implicit(Layers.terrainMask)));
		_rightNonAllocCaster = new NonAllocCaster(1);
		((ContactFilter2D)(ref _rightNonAllocCaster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1 << LayerMask.op_Implicit(Layers.terrainMask)));
	}

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		MoveAmountFromBaseHead(out var position);
		MoveAmountFromAnother(ref position);
		return position;
	}

	private void MoveAmountFromBaseHead(out Vector2 position)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)_base.collider).bounds;
		float x = ((Bounds)(ref bounds)).center.x;
		bounds = ((Collider2D)_base.collider).bounds;
		position = new Vector2(x, ((Bounds)(ref bounds)).max.y + _distanceFromBaseHead.value);
	}

	private void MoveAmountFromAnother(ref Vector2 position)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		float value = _minDistanceFromAnother.value;
		float value2 = _stride.value;
		if (CanSetPosition(position, value))
		{
			return;
		}
		int i = 1;
		int num = 10;
		Vector2 val = position;
		for (; i < num; i++)
		{
			val = position + (float)i * value2 * Vector2.right;
			if (CanSetPosition(val, value))
			{
				break;
			}
			val = position + (float)i * value2 * Vector2.left;
			if (CanSetPosition(val, value))
			{
				break;
			}
		}
		if (i < num)
		{
			position = val;
		}
	}

	private bool CanSetPosition(Vector2 position, float minDistance)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		if (!Map.Instance.IsInMap(Vector2.op_Implicit(position)))
		{
			return false;
		}
		ReadonlyBoundedList<RaycastHit2D> results = _leftNonAllocCaster.RayCast(position, Vector2.left, _sizeOfPot).results;
		ReadonlyBoundedList<RaycastHit2D> results2 = _rightNonAllocCaster.RayCast(position, Vector2.right, _sizeOfPot).results;
		if (results.Count > 0 && results2.Count > 0)
		{
			return false;
		}
		float num = minDistance * minDistance;
		foreach (Monster monster in _occulistPotContainer.monsters)
		{
			if (Vector2.SqrMagnitude(position - Vector2.op_Implicit(((Component)monster).transform.position)) < num)
			{
				return false;
			}
		}
		return true;
	}
}
