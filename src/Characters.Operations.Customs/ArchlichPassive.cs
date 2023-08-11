using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs;

public class ArchlichPassive : CharacterOperation
{
	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	private Collider2D _searchRange;

	private const int _spawnCount = 5;

	private const int _radius = 2;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper;

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(5);
		((Behaviour)_searchRange).enabled = false;
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_searchRange).enabled = true;
		_overlapper.OverlapCollider(_searchRange);
		((Behaviour)_searchRange).enabled = false;
		List<Target> components = GetComponentExtension.GetComponents<Collider2D, Target>((IEnumerable<Collider2D>)_overlapper.results, true);
		if (components.Count == 0)
		{
			return;
		}
		float num = Random.value * (float)Math.PI * 2f;
		foreach (Target item in components)
		{
			Bounds bounds = item.collider.bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			_ = (((Bounds)(ref bounds)).size.x + ((Bounds)(ref bounds)).size.y) / 2f;
			int num2 = 5 - components.Count + 1;
			for (int i = 0; i < num2; i++)
			{
				num += (1f + Random.value) / (float)(num2 * 2) * (float)Math.PI * 2f;
				Vector3 val = center;
				val.x += Mathf.Cos(num) * 2f;
				val.y += Mathf.Sin(num) * 2f;
				OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
				((Component)operationInfos).transform.SetPositionAndRotation(val, Quaternion.Euler(0f, 0f, num * 57.29578f + 180f));
				operationInfos.Initialize();
				operationInfos.Run(owner);
			}
		}
	}
}
