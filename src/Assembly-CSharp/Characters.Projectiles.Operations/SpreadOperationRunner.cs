using Characters.Operations;
using PhysicsUtils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Projectiles.Operations;

public class SpreadOperationRunner : HitOperation
{
	private static short spriteLayer;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	internal OperationRunner _operationRunner;

	[SerializeField]
	private LayerMask _groundMask;

	[SerializeField]
	private float _distance;

	private static readonly NonAllocCaster _nonAllocCaster;

	static SpreadOperationRunner()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		spriteLayer = short.MinValue;
		_nonAllocCaster = new NonAllocCaster(1);
	}

	private void OnDestroy()
	{
		_operationRunner = null;
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		Character owner = projectile.owner;
		var (flag, val) = TryRayCast(Vector2.op_Implicit(projectile.transform.position), Vector2.left);
		if (flag)
		{
			SpreadLeft(((RaycastHit2D)(ref val)).point, owner);
		}
		var (flag2, val2) = TryRayCast(Vector2.op_Implicit(projectile.transform.position), Vector2.right);
		if (flag2)
		{
			SpreadRight(((RaycastHit2D)(ref val2)).point, owner);
		}
		var (flag3, val3) = TryRayCast(Vector2.op_Implicit(projectile.transform.position), Vector2.up);
		if (flag3)
		{
			SpreadUp(((RaycastHit2D)(ref val3)).point, owner);
		}
		var (flag4, val4) = TryRayCast(Vector2.op_Implicit(projectile.transform.position), Vector2.down);
		if (flag4)
		{
			SpreadDown(((RaycastHit2D)(ref val4)).point, owner);
		}
	}

	private (bool, RaycastHit2D) TryRayCast(Vector2 origin, Vector2 direction)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		ReadonlyBoundedList<RaycastHit2D> results = _nonAllocCaster.RayCast(origin, direction, _distance).results;
		if (results.Count > 0)
		{
			return (true, results[0]);
		}
		return (false, default(RaycastHit2D));
	}

	private void SpreadUp(Vector2 origin, Character owner)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		ReadonlyBoundedList<RaycastHit2D> results = _nonAllocCaster.RayCast(origin, Vector2.up, _distance).results;
		if (results.Count != 0)
		{
			RaycastHit2D val = results[0];
			OperationRunner operationRunner = _operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			if (((Component)((RaycastHit2D)(ref val)).collider).gameObject.layer == 17)
			{
				((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(((RaycastHit2D)(ref val)).point), Quaternion.Euler(0f, 0f, 0f));
			}
			else
			{
				((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(((RaycastHit2D)(ref val)).point), Quaternion.Euler(0f, 0f, 180f));
			}
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			operationInfos.Run(owner);
		}
	}

	private void SpreadDown(Vector2 origin, Character owner)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		ReadonlyBoundedList<RaycastHit2D> results = _nonAllocCaster.RayCast(origin, Vector2.down, _distance).results;
		if (results.Count != 0)
		{
			RaycastHit2D val = results[0];
			OperationRunner operationRunner = _operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(((RaycastHit2D)(ref val)).point), Quaternion.Euler(0f, 0f, 0f));
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			operationInfos.Run(owner);
		}
	}

	private void SpreadLeft(Vector2 origin, Character owner)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		ReadonlyBoundedList<RaycastHit2D> results = _nonAllocCaster.RayCast(origin, Vector2.left, _distance).results;
		if (results.Count != 0)
		{
			RaycastHit2D val = results[0];
			OperationRunner operationRunner = _operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(((RaycastHit2D)(ref val)).point), Quaternion.Euler(0f, 0f, 270f));
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			operationInfos.Run(owner);
		}
	}

	private void SpreadRight(Vector2 origin, Character owner)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_groundMask);
		ReadonlyBoundedList<RaycastHit2D> results = _nonAllocCaster.RayCast(origin, Vector2.right, _distance).results;
		if (results.Count != 0)
		{
			RaycastHit2D val = results[0];
			OperationRunner operationRunner = _operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(((RaycastHit2D)(ref val)).point), Quaternion.Euler(0f, 0f, 90f));
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			operationInfos.Run(owner);
		}
	}
}
