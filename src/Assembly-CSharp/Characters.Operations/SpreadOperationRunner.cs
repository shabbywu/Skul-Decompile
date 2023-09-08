using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public class SpreadOperationRunner : CharacterOperation
{
	[SerializeField]
	private OperationRunner _operationRunner;

	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private CustomFloat _count;

	[SerializeField]
	private CustomFloat _distance;

	[SerializeField]
	private LayerMask _groundMask;

	private static readonly NonAllocCaster _nonAllocCaster;

	static SpreadOperationRunner()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_nonAllocCaster = new NonAllocCaster(1);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_operationRunner = null;
	}

	public override void Run(Character owner)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		Quaternion rotation = ((Component)_origin).transform.rotation;
		float z = ((Quaternion)(ref rotation)).eulerAngles.z;
		if (z != 0f)
		{
			if (z != 90f)
			{
				if (z != 180f)
				{
					if (z == 270f)
					{
						SpreadLeft(Vector2.op_Implicit(((Component)_origin).transform.position), owner);
					}
				}
				else
				{
					SpreadUp(Vector2.op_Implicit(((Component)_origin).transform.position), owner);
				}
			}
			else
			{
				SpreadRight(Vector2.op_Implicit(((Component)_origin).transform.position), owner);
			}
		}
		else
		{
			SpreadDown(Vector2.op_Implicit(((Component)_origin).transform.position), owner);
		}
	}

	private (bool, RaycastHit2D) TryRayCast(Vector2 origin, Vector2 direction, float distance)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		ReadonlyBoundedList<RaycastHit2D> results = _nonAllocCaster.RayCast(origin, direction, distance).results;
		if (results.Count > 0)
		{
			return (true, results[0]);
		}
		return (false, default(RaycastHit2D));
	}

	private bool TrySpread(Vector2 origin, Vector2 direction, Vector2 groundDirection, int count, float rotation, Character owner)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = origin;
		float x = direction.x;
		if (x != 1f)
		{
			if (x == -1f)
			{
				((Vector2)(ref val))._002Ector(origin.x, origin.y + 0.5f);
			}
		}
		else
		{
			((Vector2)(ref val))._002Ector(origin.x, origin.y + 0.5f);
		}
		x = direction.y;
		if (x != 1f)
		{
			if (x == -1f)
			{
				((Vector2)(ref val))._002Ector(origin.x + 0.5f, origin.y);
			}
		}
		else
		{
			((Vector2)(ref val))._002Ector(origin.x + 0.5f, origin.y);
		}
		float value = _distance.value;
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		if (_nonAllocCaster.RayCast(val, direction, value * (float)count).results.Count > 0)
		{
			return false;
		}
		Vector2 origin2 = origin;
		x = direction.x;
		if (x != 1f)
		{
			if (x == -1f)
			{
				((Vector2)(ref origin2))._002Ector(origin.x - value * (float)count, origin.y + 1f);
			}
		}
		else
		{
			((Vector2)(ref origin2))._002Ector(origin.x + value * (float)count, origin.y + 1f);
		}
		x = direction.y;
		if (x != 1f)
		{
			if (x == -1f)
			{
				((Vector2)(ref origin2))._002Ector(origin.x + 1f, origin.y - value * (float)count);
			}
		}
		else
		{
			((Vector2)(ref origin2))._002Ector(origin.x + 1f, origin.y + value * (float)count);
		}
		var (flag, val2) = TryRayCast(origin2, groundDirection, 2f);
		if (!flag)
		{
			return false;
		}
		OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(((RaycastHit2D)(ref val2)).point), Quaternion.Euler(0f, 0f, rotation));
		((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
		operationInfos.Run(owner);
		return true;
	}

	private void SpreadDown(Vector2 origin, Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float value = _count.value;
		for (int i = 1; (float)i <= value && TrySpread(origin, Vector2.right, Vector2.down, i, 0f, owner); i++)
		{
		}
		for (int j = 1; (float)j <= value && TrySpread(origin, Vector2.left, Vector2.down, j, 0f, owner); j++)
		{
		}
	}

	private void SpreadUp(Vector2 origin, Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float value = _count.value;
		for (int i = 1; (float)i <= value && TrySpread(origin, Vector2.right, Vector2.up, i, 180f, owner); i++)
		{
		}
		for (int j = 1; (float)j <= value && TrySpread(origin, Vector2.left, Vector2.up, j, 180f, owner); j++)
		{
		}
	}

	private void SpreadRight(Vector2 origin, Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float value = _count.value;
		for (int i = 1; (float)i <= value && TrySpread(origin, Vector2.up, Vector2.right, i, 90f, owner); i++)
		{
		}
		for (int j = 1; (float)j <= value && TrySpread(origin, Vector2.down, Vector2.right, j, 90f, owner); j++)
		{
		}
	}

	private void SpreadLeft(Vector2 origin, Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float value = _count.value;
		for (int i = 1; (float)i <= value && TrySpread(origin, Vector2.up, Vector2.left, i, 270f, owner); i++)
		{
		}
		for (int j = 1; (float)j <= value && TrySpread(origin, Vector2.down, Vector2.left, j, 270f, owner); j++)
		{
		}
	}
}
