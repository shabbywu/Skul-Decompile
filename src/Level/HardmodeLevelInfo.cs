using System;
using Characters;
using Data;
using UnityEngine;

namespace Level;

[CreateAssetMenu]
public sealed class HardmodeLevelInfo : ScriptableObject
{
	[Serializable]
	internal class DarkEnemyInfoByLevel
	{
		[SerializeField]
		internal EnemyStatInfo _statInfo;

		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		internal Vector2Int _totalRange;

		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		internal Vector2Int _perMapRange;
	}

	[Serializable]
	public struct EnemyStatInfo
	{
		[SerializeField]
		[Range(0f, 10f)]
		private float _healthMultiplier;

		[Range(0f, 10f)]
		[SerializeField]
		private float _attackMultiplier;

		[Range(0f, 100f)]
		[SerializeField]
		private float _castingBreakDamageMultiplier;

		public float healthMultiplier => _healthMultiplier;

		public float attackMultiplier => _attackMultiplier;

		public float castingBreakDamageMultiplier => _castingBreakDamageMultiplier;
	}

	[Serializable]
	internal class InfoByLevel
	{
		[SerializeField]
		[Header("일반 적")]
		internal EnemyStatInfo _trashMobInfo;

		[SerializeField]
		[Header("검은 적")]
		internal DarkEnemyInfoByLevel _darkEnemyInfo;

		[Header("모험가")]
		[SerializeField]
		internal EnemyStatInfo _adventurerInfo;

		[SerializeField]
		[Header("보스")]
		internal EnemyStatInfo _bossInfo;

		[SerializeField]
		[Header("금화 획득량")]
		internal Vector2Int _goldAmountMultiplier;
	}

	private static HardmodeLevelInfo _instance;

	[SerializeField]
	private InfoByLevel[] _infoByLevel;

	public static HardmodeLevelInfo instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<HardmodeLevelInfo>("HardmodeSetting/HardmodeLevelInfo");
			}
			return _instance;
		}
	}

	public int GetDarkEnemyTotalCount(Random random)
	{
		DarkEnemyInfoByLevel darkEnemyInfo = _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._darkEnemyInfo;
		return random.Next(((Vector2Int)(ref darkEnemyInfo._totalRange)).x, ((Vector2Int)(ref darkEnemyInfo._totalRange)).y + 1);
	}

	public int GetDarkEnemyCountPerMap(Random random)
	{
		DarkEnemyInfoByLevel darkEnemyInfo = _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._darkEnemyInfo;
		return random.Next(((Vector2Int)(ref darkEnemyInfo._perMapRange)).x, ((Vector2Int)(ref darkEnemyInfo._perMapRange)).y + 1);
	}

	public EnemyStatInfo GetEnemyStatInfoByType(Character.Type type)
	{
		switch (type)
		{
		case Character.Type.TrashMob:
		case Character.Type.Summoned:
			return _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._trashMobInfo;
		case Character.Type.Named:
			return _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._darkEnemyInfo._statInfo;
		case Character.Type.Adventurer:
			return _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._adventurerInfo;
		case Character.Type.Boss:
			return _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._bossInfo;
		default:
			return _infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._trashMobInfo;
		}
	}
}
