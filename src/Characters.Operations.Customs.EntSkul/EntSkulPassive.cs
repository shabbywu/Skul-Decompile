using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul;

public class EntSkulPassive : CharacterOperation
{
	[SerializeField]
	private Collider2D _searchRange;

	[Tooltip("가시가 항상 바닥에 나와야해서, 적 기준으로 바로 아래쪽 땅을 찾는데 그 때 땅을 찾기 위한 최대 거리를 의미함")]
	[SerializeField]
	private float _groundFinderDirection = 5f;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper;

	private RayCaster _groundFinder;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(1);
		_groundFinder = new RayCaster
		{
			direction = Vector2.down,
			distance = _groundFinderDirection
		};
		((ContactFilter2D)(ref ((Caster)_groundFinder).contactFilter)).SetLayerMask(Layers.groundMask);
		((Behaviour)_searchRange).enabled = false;
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_searchRange).enabled = true;
		_overlapper.OverlapCollider(_searchRange);
		List<Target> components = ((IEnumerable<Collider2D>)_overlapper.results).GetComponents<Collider2D, Target>(clearList: true);
		if (components.Count == 0)
		{
			((Behaviour)_searchRange).enabled = false;
			return;
		}
		((Behaviour)_searchRange).enabled = false;
		Target target = components.Random();
		((Caster)_groundFinder).origin = Vector2.op_Implicit(((Component)target).transform.position);
		RaycastHit2D val = ((Caster)_groundFinder).SingleCast();
		if (RaycastHit2D.op_Implicit(val))
		{
			SpawnOperationRunner(owner, Vector2.op_Implicit(((RaycastHit2D)(ref val)).point));
		}
	}

	private void SpawnOperationRunner(Character owner, Vector3 position)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(position, Quaternion.identity);
		operationInfos.Run(owner);
	}
}
