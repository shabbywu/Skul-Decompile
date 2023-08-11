using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul;

public class EntSkulThornyVine : CharacterOperation
{
	[SerializeField]
	private int _count = 15;

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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(_count);
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
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_searchRange).enabled = true;
		_overlapper.OverlapCollider(_searchRange);
		List<Target> components = GetComponentExtension.GetComponents<Collider2D, Target>((IEnumerable<Collider2D>)_overlapper.results, true);
		if (components.Count == 0)
		{
			((Behaviour)_searchRange).enabled = false;
			return;
		}
		((Behaviour)_searchRange).enabled = false;
		foreach (Target item in components)
		{
			if (!((Object)(object)item.character == (Object)null))
			{
				((Caster)_groundFinder).origin = Vector2.op_Implicit(((Component)item).transform.position);
				RaycastHit2D val = ((Caster)_groundFinder).SingleCast();
				if (!RaycastHit2D.op_Implicit(val))
				{
					break;
				}
				SpawnOperationRunner(owner, Vector2.op_Implicit(((RaycastHit2D)(ref val)).point));
			}
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
