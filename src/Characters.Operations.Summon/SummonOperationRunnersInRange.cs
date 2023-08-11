using System;
using System.Collections;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon;

public sealed class SummonOperationRunnersInRange : CharacterOperation
{
	[Serializable]
	private class SummonOption
	{
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		[Space]
		internal OperationRunner operationRunner;

		[SerializeField]
		[Space]
		internal CustomFloat scale = new CustomFloat(1f);

		[SerializeField]
		internal CustomAngle angle;

		[SerializeField]
		internal PositionNoise noise;

		[Space]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[SerializeField]
		internal bool flipXByLookingDirection;

		[Tooltip("X축 플립")]
		[SerializeField]
		internal bool flipX;

		[SerializeField]
		internal bool copyAttackDamage;

		internal void Dispose()
		{
			operationRunner = null;
		}
	}

	[SerializeField]
	private BoxCollider2D _terrainFindingRange;

	[SerializeField]
	[Tooltip("플랫폼도 포함할 것인지")]
	private bool _includePlatform = true;

	[SerializeField]
	private int _count = 1;

	[SerializeField]
	private float _delay;

	[SerializeField]
	private SummonOption _summonOption;

	private const int _maxTerrainCount = 16;

	private static short spriteLayer = short.MinValue;

	private NonAllocOverlapper _overlapper;

	private List<(float2 a, float2 b)> _surfaces = new List<(float2, float2)>(16);

	private AttackDamage _attackDamage;

	private int[] _weights = new int[16];

	private void Awake()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(16);
		int num = 262144;
		if (_includePlatform)
		{
			num |= 0x20000;
		}
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(num));
		((Behaviour)_terrainFindingRange).enabled = false;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_summonOption.Dispose();
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner)
	{
		FindSurfaces();
		if (_surfaces.Count > 0)
		{
			((MonoBehaviour)this).StartCoroutine(CSummonAll(owner));
		}
	}

	private void FindSurfaces()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_terrainFindingRange).enabled = true;
		_overlapper.OverlapCollider((Collider2D)(object)_terrainFindingRange);
		Bounds bounds = ((Collider2D)_terrainFindingRange).bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = ((Collider2D)_terrainFindingRange).bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		((Behaviour)_terrainFindingRange).enabled = false;
		_surfaces.Clear();
		if (_overlapper.results.Count != 0)
		{
			for (int i = 0; i < _overlapper.results.Count; i++)
			{
				Bounds bounds2 = _overlapper.results[i].bounds;
				float2 val = float2.op_Implicit(ExtensionMethods.GetMostLeftTop(bounds2));
				float2 val2 = float2.op_Implicit(ExtensionMethods.GetMostRightTop(bounds2));
				val.x = Mathf.Max(val.x, x);
				val2.x = Mathf.Min(val2.x, x2);
				_surfaces.Add((val, val2));
			}
		}
	}

	private IEnumerator CSummonAll(Character owner)
	{
		int remain = _count;
		ExtensionMethods.PseudoShuffle<(float2, float2)>((IList<(float2, float2)>)_surfaces);
		float num = 0f;
		foreach (var surface2 in _surfaces)
		{
			num += Mathf.Abs(surface2.b.x - surface2.a.x);
		}
		for (int l = 0; l < _surfaces.Count; l++)
		{
			float num2 = Mathf.Abs(_surfaces[l].b.x - _surfaces[l].a.x);
			_weights[l] = Mathf.RoundToInt(num2 / num * (float)_count);
		}
		for (int j = 0; j < _surfaces.Count - 1; j++)
		{
			if (remain <= 0)
			{
				break;
			}
			(float2 a, float2 b) surface = _surfaces[j];
			for (int k = 0; k < _weights[j]; k++)
			{
				Summon(owner, surface);
				yield return Chronometer.global.WaitForSeconds(_delay);
			}
			remain -= _weights[j];
		}
		(float2 a, float2 b) lastSurface = _surfaces[_surfaces.Count - 1];
		for (int j = 0; j < remain; j++)
		{
			Summon(owner, lastSurface);
			yield return Chronometer.global.WaitForSeconds(_delay);
		}
	}

	private void Summon(Character owner, (float2 left, float2 right) spawnRange)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(spawnRange.left.x, spawnRange.right.x);
		float num2 = (spawnRange.left.y + spawnRange.right.y) / 2f;
		Summon(owner, float2.op_Implicit(new Vector2(num, num2)));
	}

	private void Summon(Character owner, float2 position)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(0f, 0f, _summonOption.angle.value);
		int num;
		if (_summonOption.flipXByLookingDirection)
		{
			num = ((owner.lookingDirection == Character.LookingDirection.Left) ? 1 : 0);
			if (num != 0)
			{
				val.z = (180f - val.z) % 360f;
			}
		}
		else
		{
			num = 0;
		}
		if (_summonOption.flipX)
		{
			val.z = (180f - val.z) % 360f;
		}
		OperationRunner operationRunner = _summonOption.operationRunner.Spawn();
		OperationInfos operationInfos = operationRunner.operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.Euler(val));
		if (_summonOption.copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
		{
			operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
			operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
		}
		SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
		if ((Object)(object)component != (Object)null)
		{
			component.sortingOrder = spriteLayer++;
		}
		if (num != 0)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _summonOption.scale.value;
		}
		else
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f) * _summonOption.scale.value;
		}
		if (_summonOption.flipX)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _summonOption.scale.value;
		}
		operationInfos.Run(owner);
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}
}
