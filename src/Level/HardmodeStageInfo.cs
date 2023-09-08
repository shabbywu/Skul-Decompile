using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameResources;
using UnityEngine;

namespace Level;

[CreateAssetMenu]
public sealed class HardmodeStageInfo : StageInfo
{
	[Header("HardmodeData")]
	[SerializeField]
	private int a;

	protected override void GeneratePath()
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
		int num4 = DarkEnemySelector.instance.SetTargetCountInStage();
		if (num4 > 0 && num > 0)
		{
			Debug.Log((object)$"DarkEnemies : {num4}");
			Debug.Log((object)$"Normal Maps : {num}");
			int[] array2 = MMMaths.MultipleRandomWithoutDuplactes(random, num4, 0, num);
			foreach (int num5 in array2)
			{
				array[num5].Item1.reference.darkEnemy = true;
				array[num5].Item2.reference.darkEnemy = true;
			}
		}
		if (num2 > 0)
		{
			int[] array2 = MMMaths.MultipleRandomWithoutDuplactes(random, num2, 0, num);
			foreach (int num6 in array2)
			{
				array[num6].Item1.reward = MapReward.Type.Head;
				array[num6].Item1.gate = Gate.Type.Grave;
			}
		}
		if (num3 > 0)
		{
			int[] array2 = MMMaths.MultipleRandomWithoutDuplactes(random, num3, 0, num);
			foreach (int num7 in array2)
			{
				array[num7].Item2.reward = MapReward.Type.Item;
				array[num7].Item2.gate = Gate.Type.Chest;
			}
		}
		if (_remainMaps[Map.Type.Special] != null)
		{
			List<MapReference> list2 = _remainMaps[Map.Type.Special].Where((MapReference m) => !SpecialMap.GetEncoutered(m.specialMapType)).ToList();
			int count = Math.Min(GetSpecialMapCount(random), list2.Count);
			int[] array3 = MMMaths.MultipleRandomWithoutDuplactes(random, count, 0, num);
			for (int k = 0; k < array3.Length; k++)
			{
				int index2 = list2.RandomIndex(random);
				MapReference reference2 = list2[index2];
				list2.RemoveAt(index2);
				int num8 = array3[k];
				array[num8].Item1.reference = reference2;
				array[num8].Item2.reference = reference2;
			}
		}
		int minValue = Mathf.RoundToInt((float)(num * ((Vector2Int)(ref _castleNpc.positionRange)).x) * 0.01f);
		int maxValue = Mathf.RoundToInt((float)(num * ((Vector2Int)(ref _castleNpc.positionRange)).y) * 0.01f);
		int num9 = random.Next(minValue, maxValue) + 1;
		if (!_castleNpc.reference.IsNullOrEmpty() && !GameData.Progress.GetRescued(_npcType))
		{
			array[num9].Item1 = _castleNpc;
			array[num9].Item2 = _castleNpc;
		}
		for (int l = 0; l < array.Length; l++)
		{
			if (random.Next(2) == 0)
			{
				PathNode item = array[l].Item1;
				array[l].Item1 = array[l].Item2;
				array[l].Item2 = item;
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
}
