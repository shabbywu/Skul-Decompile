using System.Collections.Generic;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Essences;

public sealed class Naias : CharacterOperation
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

	[Tooltip("Order에 따른 각 요소별 스폰 딜레이")]
	[SerializeField]
	private float _delay;

	[SerializeField]
	private Transform[] _naiasProps;

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
		if ((Object)(object)_orderOrigin == (Object)null)
		{
			_orderOrigin = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		FindSurfaces();
		if (_surfaces.Count != 0)
		{
			SpawnByWorldOrder(owner);
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _surfaces.Count; i++)
		{
			(float2, float2) tuple = _surfaces[i];
			float num = (tuple.Item2.x - tuple.Item1.x) / _width;
			float num2 = num - (float)(int)num;
			float2 item = tuple.Item1;
			item.x = tuple.Item1.x + num2 * _width / 2f;
			for (int j = 0; (float)j < num; j++)
			{
				float2 position = item;
				position.x += _width * (float)j;
				Spawn(position, j);
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
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
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
				float2 val2 = item;
				val2.x += _width * (float)j;
				list.Add((val2, math.distance(val, val2)));
			}
		}
		list.Sort(((float2 position, float distance) a, (float2 position, float distance) b) => a.distance.CompareTo(b.distance));
		for (int k = 0; k < list.Count; k++)
		{
			Spawn(list[k].Item1, k);
		}
	}

	private void Spawn(float2 position, int index)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (index < _naiasProps.Length)
		{
			SortingGroup component = ((Component)_naiasProps[index]).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			((Component)_naiasProps[index]).transform.position = new Vector3(position.x, position.y);
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
}
