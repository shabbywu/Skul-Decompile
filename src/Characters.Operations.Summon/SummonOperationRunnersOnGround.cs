using System.Collections;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon;

public class SummonOperationRunnersOnGround : CharacterOperation
{
	public enum Order
	{
		AtOnce,
		InsideToOutside,
		OutsideToInside
	}

	private const int _maxTerrainCount = 16;

	private static short spriteLayer = short.MinValue;

	[SerializeField]
	private BoxCollider2D _terrainFindingRange;

	[SerializeField]
	[Tooltip("플랫폼도 포함할 것인지")]
	private bool _includePlatform = true;

	[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
	[SerializeField]
	private float _width;

	[SerializeField]
	private Order _order;

	[SerializeField]
	private Transform _orderOrigin;

	[Tooltip("Order에 따른 각 요소별 스폰 딜레이")]
	[SerializeField]
	private float _delay;

	[Tooltip("오퍼레이션 프리팹")]
	[SerializeField]
	[Space]
	private OperationRunner _operationRunner;

	[Space]
	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[Space]
	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	[SerializeField]
	private bool _flipXByLookingDirection;

	[Tooltip("X축 플립")]
	[SerializeField]
	private bool _flipX;

	[SerializeField]
	private bool _copyAttackDamage;

	[SerializeField]
	private bool _onlySpawnToLastStandingCollider;

	private AttackDamage _attackDamage;

	private NonAllocOverlapper _overlapper;

	private List<(float2 a, float2 b)> _surfaces = new List<(float2, float2)>(16);

	private Character _owner;

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
		if ((Object)(object)_orderOrigin == (Object)null)
		{
			_orderOrigin = ((Component)this).transform;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_operationRunner = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	private void FindSurfaces()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_terrainFindingRange).enabled = true;
		_overlapper.OverlapCollider((Collider2D)(object)_terrainFindingRange);
		Bounds bounds = ((Collider2D)_terrainFindingRange).bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = ((Collider2D)_terrainFindingRange).bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		((Behaviour)_terrainFindingRange).enabled = false;
		_surfaces.Clear();
		if (_overlapper.results.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _overlapper.results.Count; i++)
		{
			Collider2D val = _overlapper.results[i];
			if (!_onlySpawnToLastStandingCollider || !((Object)(object)val != (Object)(object)_owner.movement.controller.collisionState.lastStandingCollider))
			{
				Bounds bounds2 = val.bounds;
				float2 val2 = float2.op_Implicit(bounds2.GetMostLeftTop());
				float2 val3 = float2.op_Implicit(bounds2.GetMostRightTop());
				val2.x = Mathf.Max(val2.x, x);
				val3.x = Mathf.Min(val3.x, x2);
				_surfaces.Add((val2, val3));
			}
		}
	}

	public override void Run(Character owner)
	{
		_owner = owner;
		FindSurfaces();
		if (_surfaces.Count != 0)
		{
			if (_order == Order.AtOnce)
			{
				SpawnAtOnce(owner);
			}
			else if (_order == Order.InsideToOutside || _order == Order.OutsideToInside)
			{
				SpawnByWorldOrder(owner);
			}
		}
	}

	private void SpawnAtOnce(Character owner)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _surfaces.Count; i++)
		{
			(float2, float2) tuple = _surfaces[i];
			float num = (tuple.Item2.x - tuple.Item1.x) / _width;
			float num2 = num - (float)(int)num;
			float2 item = tuple.Item1;
			item.x = tuple.Item1.x + num2 * _width / 2f;
			for (int j = 0; (float)j < num; j++)
			{
				float2 position = item + float2.op_Implicit(_noise.EvaluateAsVector2());
				position.x += _width * (float)j;
				Spawn(owner, position);
			}
		}
	}

	private void SpawnByWorldOrder(Character owner)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		List<(float2, float)> list = new List<(float2, float)>();
		float2 val = default(float2);
		((float2)(ref val))._002Ector(((Component)_orderOrigin).transform.position.x, ((Component)_orderOrigin).transform.position.y);
		for (int i = 0; i < _surfaces.Count; i++)
		{
			(float2, float2) tuple = _surfaces[i];
			float num = (tuple.Item2.x - tuple.Item1.x) / _width;
			float num2 = num - (float)(int)num;
			float2 item = tuple.Item1;
			item.x = tuple.Item1.x + num2 * _width / 2f;
			for (int j = 0; (float)j < num; j++)
			{
				float2 val2 = item + float2.op_Implicit(_noise.EvaluateAsVector2());
				val2.x += _width * (float)j;
				list.Add((val2, math.distance(val, val2)));
			}
		}
		if (_order == Order.InsideToOutside)
		{
			list.Sort(((float2 position, float distance) a, (float2 position, float distance) b) => a.distance.CompareTo(b.distance));
		}
		else if (_order == Order.OutsideToInside)
		{
			list.Sort(((float2 position, float distance) a, (float2 position, float distance) b) => b.distance.CompareTo(a.distance));
		}
		((MonoBehaviour)this).StartCoroutine(CSpawnByDelay(owner, list));
	}

	private IEnumerator CSpawnByDelay(Character owner, List<(float2 position, float distance)> spawnPositions)
	{
		float item = spawnPositions[0].distance;
		foreach (var spawnPosition in spawnPositions)
		{
			Spawn(owner, spawnPosition.position);
			yield return Chronometer.global.WaitForSeconds(math.distance(spawnPosition.distance, item) / _width * _delay);
			item = spawnPosition.distance;
		}
	}

	private void Spawn(Character owner, float2 position)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(0f, 0f, _angle.value);
		int num;
		if (_flipXByLookingDirection)
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
		if (_flipX)
		{
			val.z = (180f - val.z) % 360f;
		}
		OperationRunner operationRunner = _operationRunner.Spawn();
		OperationInfos operationInfos = operationRunner.operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.Euler(val));
		if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
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
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		else
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f) * _scale.value;
		}
		if (_flipX)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		operationInfos.Run(owner);
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}
}
