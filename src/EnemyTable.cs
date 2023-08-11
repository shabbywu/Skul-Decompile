using System.Collections.Generic;
using System.IO;
using Characters;
using GameResources;
using Level;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTable", menuName = "ScriptableObjects/EnemyTable", order = 1)]
public class EnemyTable : ScriptableObject
{
	[SerializeField]
	[MinMaxSlider(1f, 10f)]
	private Vector2 _groupB;

	private const string assets = "Assets";

	private const string enemy = "Enemies";

	private const string hardmode = "Hardmode";

	private const string common = "Common";

	private const string normalEnemyPath = "Assets/Enemies";

	private const string hardmodeEnemyPath = "Assets/Enemies/Hardmode";

	private IDictionary<Key, Character> _common;

	private IDictionary<Key, Character> _chapterCommon;

	private IDictionary<Key, Character> _enemies;

	private static EnemyTable _instance;

	public static EnemyTable instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<EnemyTable>("HardmodeSetting/EnemyTable");
				_instance.Initialize();
			}
			return _instance;
		}
	}

	private void Initialize()
	{
		_common = new Dictionary<Key, Character>();
		_chapterCommon = new Dictionary<Key, Character>();
		_enemies = new Dictionary<Key, Character>();
		LoadCommon();
	}

	public void Dispose()
	{
		_common.Clear();
		_chapterCommon.Clear();
		_enemies.Clear();
	}

	public Character Get(Key key)
	{
		if (_common.TryGetValue(key, out var value))
		{
			return value;
		}
		if (_chapterCommon.TryGetValue(key, out value))
		{
			return value;
		}
		_enemies.TryGetValue(key, out value);
		return value;
	}

	private void LoadCommon()
	{
		Character[] array = CommonResource.LoadAll<Character>("Assets/Enemies/Hardmode/Common", "*.prefab", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < array.Length; i++)
		{
			_common.Add(array[i].key, array[i]);
		}
	}

	private void LoadChapterCommon(Chapter.Type type)
	{
		_chapterCommon.Clear();
		Chapter.Type type2 = type;
		switch (type)
		{
		case Chapter.Type.HardmodeChapter1:
			type2 = Chapter.Type.Chapter1;
			break;
		case Chapter.Type.HardmodeChapter2:
			type2 = Chapter.Type.Chapter2;
			break;
		case Chapter.Type.HardmodeChapter3:
			type2 = Chapter.Type.Chapter3;
			break;
		case Chapter.Type.HardmodeChapter4:
			type2 = Chapter.Type.Chapter4;
			break;
		case Chapter.Type.HardmodeChapter5:
			type2 = Chapter.Type.Chapter5;
			break;
		}
		Character[] array = CommonResource.LoadAll<Character>("Assets/Enemies/" + type2, "*.prefab", SearchOption.TopDirectoryOnly);
		Debug.Log((object)("Assets/Enemies/" + type2));
		for (int i = 0; i < array.Length; i++)
		{
			if (!_chapterCommon.ContainsKey(array[i].key))
			{
				_chapterCommon.Add(array[i].key, array[i]);
			}
		}
	}
}
