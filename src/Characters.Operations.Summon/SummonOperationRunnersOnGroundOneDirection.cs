using System.Collections;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon;

public class SummonOperationRunnersOnGroundOneDirection : CharacterOperation
{
	private const int _maxTerrainCount = 16;

	private static short spriteLayer = short.MinValue;

	[SerializeField]
	private BoxCollider2D _terrainFindingRange;

	[Tooltip("플랫폼도 포함할 것인지")]
	[SerializeField]
	private bool _includePlatform = true;

	[SerializeField]
	[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
	private float _width;

	[SerializeField]
	private Transform _summonOrigin;

	[SerializeField]
	private bool _onlyOwnersTerrain;

	[SerializeField]
	[Tooltip("Order에 따른 각 요소별 스폰 딜레이")]
	private float _delay;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	[Space]
	private OperationRunner _operationRunner;

	[Space]
	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[SerializeField]
	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	[Space]
	private bool _flipXByLookingDirection;

	[SerializeField]
	[Tooltip("X축 플립")]
	private bool _flipX;

	[SerializeField]
	private bool _copyAttackDamage;

	private AttackDamage _attackDamage;

	private NonAllocOverlapper _overlapper;

	private List<(float2 a, float2 b)> _surfaces = new List<(float2, float2)>(16);

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
		if ((Object)(object)_summonOrigin == (Object)null)
		{
			_summonOrigin = ((Component)this).transform;
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

	private void FindSurfaces(Character owner)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
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
			Bounds bounds2 = _overlapper.results[i].bounds;
			if (!_onlyOwnersTerrain || !((Object)(object)_overlapper.results[i] != (Object)(object)owner.movement.controller.collisionState.lastStandingCollider))
			{
				float2 val = float2.op_Implicit(ExtensionMethods.GetMostLeftTop(bounds2));
				float2 val2 = float2.op_Implicit(ExtensionMethods.GetMostRightTop(bounds2));
				val.x = Mathf.Max(val.x, x);
				val2.x = Mathf.Min(val2.x, x2);
				_surfaces.Add((val, val2));
			}
		}
	}

	public override void Run(Character owner)
	{
		FindSurfaces(owner);
		if (_surfaces.Count != 0)
		{
			SpawnByOrder(owner);
		}
	}

	private void SpawnByOrder(Character owner)
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
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		List<(float2, float)> list = new List<(float2, float)>();
		float2 val = default(float2);
		((float2)(ref val))._002Ector(((Component)_summonOrigin).transform.position.x, ((Component)_summonOrigin).transform.position.y);
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
				if ((owner.lookingDirection != 0 || !(val.x > val2.x)) && (owner.lookingDirection != Character.LookingDirection.Left || !(val.x < val2.x)))
				{
					list.Add((val2, math.distance(val, val2)));
				}
			}
		}
		list.Sort(((float2 position, float distance) a, (float2 position, float distance) b) => a.distance.CompareTo(b.distance));
		if (list.Count != 0)
		{
			((MonoBehaviour)this).StartCoroutine(CSpawnByDelay(owner, list));
		}
	}

	private IEnumerator CSpawnByDelay(Character owner, List<(float2 position, float distance)> spawnPositions)
	{
		float num = spawnPositions[0].distance - _width;
		foreach (var spawnPosition in spawnPositions)
		{
			Spawn(owner, spawnPosition.position);
			yield return Chronometer.global.WaitForSeconds(math.distance(spawnPosition.distance, num) / _width * _delay);
			num = spawnPosition.distance;
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
