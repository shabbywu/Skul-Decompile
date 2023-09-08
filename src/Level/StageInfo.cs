using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameResources;
using Hardmode;
using Level.Npc;
using Singletons;
using UnityEngine;

namespace Level;

[CreateAssetMenu]
public class StageInfo : IStageInfo
{
	[Serializable]
	public class ExtraMapInfo : SerializablePathNode
	{
		[Serializable]
		public new class Reorderable : ReorderableArray<ExtraMapInfo>
		{
		}

		[Range(0f, 100f)]
		public float possibility = 100f;

		[MinMaxSlider(0f, 100f)]
		public Vector2Int positionRange;
	}

	[SerializeField]
	protected SerializablePathNode _entry;

	[SerializeField]
	protected SerializablePathNode _terminal;

	[SerializeField]
	protected Gate.Type _lastGate;

	[SerializeField]
	protected ExtraMapInfo _castleNpc;

	[SerializeField]
	protected NpcType _npcType;

	[SerializeField]
	private ParallaxBackground _background;

	[Tooltip("일반 전투 맵 개수")]
	[SerializeField]
	protected Vector2Int _normalMaps;

	[Tooltip("헤드 보상 맵 개수")]
	[SerializeField]
	protected Vector2Int _headRewards;

	[Tooltip("아이템 보상 맵 개수")]
	[SerializeField]
	protected Vector2Int _itemRewards;

	[Header("Special Maps")]
	[Tooltip("해당 스테이지에 스페셜 맵이 n개 나올 비중. 예를 들어 [0, 30, 70]이면 30% 확률로 1개 등장, 70% 확률로 2개 등장")]
	[SerializeField]
	private float[] _specialMapWeights;

	[SerializeField]
	[Header("Extra Maps")]
	protected ExtraMapInfo.Reorderable _extraMaps;

	public (PathNode type1, PathNode type2)[] _path;

	private ILookup<Map.Type, MapReference> _maps;

	protected EnumArray<Map.Type, List<MapReference>> _remainMaps = new EnumArray<Map.Type, List<MapReference>>();

	public SerializablePathNode entry => _entry;

	public SerializablePathNode terminal => _terminal;

	public override (PathNode node1, PathNode node2) currentMapPath => GetPathAt(pathIndex);

	public override (PathNode node1, PathNode node2) nextMapPath => GetPathAt(pathIndex + 1);

	public override ParallaxBackground background => _background;

	public override void Initialize()
	{
		_maps = maps.ToLookup((MapReference m) => m.type);
	}

	private (PathNode node1, PathNode node2) GetPathAt(int pathIndex)
	{
		if (pathIndex >= _path.Length)
		{
			return (PathNode.none, PathNode.none);
		}
		return _path[pathIndex];
	}

	protected virtual void GeneratePath()
	{
		Random random = new Random(GameData.Save.instance.randomSeed);
		Debug.Log((object)$"[Generate Path] seed {GameData.Save.instance.randomSeed}");
		int num = random.Next(((Vector2Int)(ref _normalMaps)).x, ((Vector2Int)(ref _normalMaps)).y + 1);
		int num2 = random.Next(((Vector2Int)(ref _headRewards)).x, ((Vector2Int)(ref _headRewards)).y + 1);
		int num3 = random.Next(((Vector2Int)(ref _itemRewards)).x, ((Vector2Int)(ref _itemRewards)).y + 1);
		if (num2 > num)
		{
			throw new ArgumentOutOfRangeException("headRewards", "headRewards must be less than normalMaps");
		}
		if (num3 > num)
		{
			throw new ArgumentOutOfRangeException("itemRewards", "itemRewards must be less than normalMaps");
		}
		(PathNode, PathNode)[] array = new(PathNode, PathNode)[num];
		for (int i = 0; i < array.Length; i++)
		{
			List<MapReference> list = _remainMaps[Map.Type.Normal];
			int index = list.RandomIndex(random);
			MapReference reference = list[index];
			list.RemoveAt(index);
			array[i] = (new PathNode(reference, MapReward.Type.Gold, Gate.Type.Normal), new PathNode(reference, MapReward.Type.Gold, Gate.Type.Normal));
		}
		if (num2 > 0)
		{
			int[] array2 = MMMaths.MultipleRandomWithoutDuplactes(random, num2, 0, num);
			foreach (int num4 in array2)
			{
				array[num4].Item1.reward = MapReward.Type.Head;
				array[num4].Item1.gate = Gate.Type.Grave;
			}
		}
		if (num3 > 0)
		{
			int[] array2 = MMMaths.MultipleRandomWithoutDuplactes(random, num3, 0, num);
			foreach (int num5 in array2)
			{
				array[num5].Item2.reward = MapReward.Type.Item;
				array[num5].Item2.gate = Gate.Type.Chest;
			}
		}
		int[] array3 = null;
		if (_remainMaps[Map.Type.Special] != null)
		{
			List<MapReference> list2 = _remainMaps[Map.Type.Special].Where((MapReference m) => !SpecialMap.GetEncoutered(m.specialMapType)).ToList();
			int count = Math.Min(GetSpecialMapCount(random), list2.Count);
			array3 = MMMaths.MultipleRandomWithoutDuplactes(random, count, 0, num);
			for (int k = 0; k < array3.Length; k++)
			{
				int index2 = list2.RandomIndex(random);
				MapReference reference2 = list2[index2];
				list2.RemoveAt(index2);
				int num6 = array3[k];
				array[num6].Item1.reference = reference2;
				array[num6].Item2.reference = reference2;
			}
		}
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			int num7 = Mathf.Min(num, DarkEnemySelector.instance.SetTargetCountInStage());
			if (num7 > 0 && num > 0)
			{
				int[] array4 = null;
				int[] array2;
				if (_remainMaps[Map.Type.Special] == null)
				{
					array4 = MMMaths.MultipleRandomWithoutDuplactes(random, num7, 0, num);
				}
				else
				{
					bool flag = true;
					int num8 = 0;
					int num9 = 100;
					while (flag && num8 < num9)
					{
						array4 = MMMaths.MultipleRandomWithoutDuplactes(random, num7, 0, num);
						flag = false;
						num8++;
						array2 = array4;
						foreach (int num10 in array2)
						{
							int[] array5 = array3;
							foreach (int num11 in array5)
							{
								if (num10 == num11)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
				}
				array2 = array4;
				foreach (int num12 in array2)
				{
					array[num12].Item1.reference.darkEnemy = true;
					array[num12].Item2.reference.darkEnemy = true;
				}
			}
		}
		int minValue = Mathf.RoundToInt((float)(num * ((Vector2Int)(ref _castleNpc.positionRange)).x) * 0.01f);
		int maxValue = Mathf.RoundToInt((float)(num * ((Vector2Int)(ref _castleNpc.positionRange)).y) * 0.01f);
		int num13 = random.Next(minValue, maxValue) + 1;
		if (!_castleNpc.reference.IsNullOrEmpty() && !GameData.Progress.GetRescued(_npcType))
		{
			array[num13].Item1 = _castleNpc;
			array[num13].Item2 = _castleNpc;
		}
		for (int n = 0; n < array.Length; n++)
		{
			if (random.Next(2) == 0)
			{
				PathNode item = array[n].Item1;
				array[n].Item1 = array[n].Item2;
				array[n].Item2 = item;
			}
		}
		List<(PathNode, PathNode)> list3 = new List<(PathNode, PathNode)>(num + 2);
		list3.AddRange(array);
		if (!_entry.reference.IsNullOrEmpty())
		{
			list3.Insert(0, (_entry, PathNode.none));
		}
		if (!_terminal.reference.IsNullOrEmpty())
		{
			list3.Add((_terminal, PathNode.none));
		}
		List<ExtraMapInfo> list4 = new List<ExtraMapInfo>();
		ExtraMapInfo[] values = _extraMaps.values;
		foreach (ExtraMapInfo extraMapInfo in values)
		{
			if (MMMaths.Chance(random, extraMapInfo.possibility / 100f))
			{
				list4.Add(extraMapInfo);
			}
		}
		foreach (ExtraMapInfo item2 in list4)
		{
			int minValue2 = Mathf.RoundToInt((float)(num * ((Vector2Int)(ref item2.positionRange)).x) * 0.01f);
			int maxValue2 = Mathf.RoundToInt((float)(num * ((Vector2Int)(ref item2.positionRange)).y) * 0.01f);
			int index3 = random.Next(minValue2, maxValue2) + 1;
			list3.Insert(index3, (item2, item2));
		}
		PathNode pathNode = new PathNode(null, MapReward.Type.None, _lastGate);
		list3.Add((pathNode, pathNode));
		_path = list3.ToArray();
	}

	protected int GetSpecialMapCount(Random random)
	{
		double num = random.NextDouble() * (double)_specialMapWeights.Sum();
		for (int i = 0; i < _specialMapWeights.Length; i++)
		{
			num -= (double)_specialMapWeights[i];
			if (num <= 0.0)
			{
				return i;
			}
		}
		return 0;
	}

	public override void Reset()
	{
		foreach (IGrouping<Map.Type, MapReference> map in _maps)
		{
			_remainMaps[map.Key] = map.ToList();
		}
		GeneratePath();
	}

	public override PathNode Next(NodeIndex nodeIndex)
	{
		base.nodeIndex = nodeIndex;
		pathIndex++;
		if (pathIndex >= _path.Length)
		{
			return null;
		}
		var (result, result2) = _path[pathIndex];
		if (nodeIndex == NodeIndex.Node2)
		{
			return result2;
		}
		return result;
	}

	public override void UpdateReferences()
	{
	}
}
