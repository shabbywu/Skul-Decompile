using System.Collections.Generic;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Utils;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Projectiles.Operations;

public class SummonOperationRunnersOnGround : Operation
{
	private const int _maxTerrainCount = 16;

	private static short spriteLayer = short.MinValue;

	[SerializeField]
	private BoxCollider2D _terrainFindingRange;

	[Tooltip("플랫폼도 포함할 것인지")]
	[SerializeField]
	private bool _includePlatform = true;

	[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
	[SerializeField]
	private float _width;

	[SerializeField]
	private Transform _orderOrigin;

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

	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	[Space]
	[SerializeField]
	private bool _flipXByLookingDirection;

	[Tooltip("X축 플립")]
	[SerializeField]
	private bool _flipX;

	[Header("Sweepattack만 가능")]
	[SerializeField]
	private bool _attackGroup;

	private NonAllocOverlapper _overlapper;

	private List<(float2 a, float2 b)> _surfaces = new List<(float2, float2)>(16);

	private HitHistoryManager _hitHistoryManager;

	private void Awake()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(16);
		if (_attackGroup)
		{
			_hitHistoryManager = new HitHistoryManager(512);
		}
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

	private void OnDestroy()
	{
		_operationRunner = null;
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

	public override void Run(IProjectile projectile)
	{
		FindSurfaces();
		if (_surfaces.Count != 0)
		{
			SpawnAtOnce(projectile.owner);
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

	private void Spawn(Character owner, float2 position)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
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
		if (_attackGroup)
		{
			_hitHistoryManager.Clear();
			SweepAttack[] components = ((Component)operationInfos).GetComponents<SweepAttack>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].collisionDetector.hits = _hitHistoryManager;
			}
		}
		operationInfos.Run(owner);
	}
}
