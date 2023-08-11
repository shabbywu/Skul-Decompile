using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameResources;

[PreferBinarySerialization]
public class LocalizationStringResource : ScriptableObject
{
	[Serializable]
	public struct StringsByLanguage
	{
		public string[] strings;
	}

	[SerializeField]
	private int _columnCount;

	[SerializeField]
	private int _rowCount;

	[SerializeField]
	[HideInInspector]
	private int[] _keyHashes;

	[SerializeField]
	[HideInInspector]
	private StringsByLanguage[] _stringsByLanguage;

	private Dictionary<int, int> _keyHashToIndex;

	private string[] _emptyStrings;

	public static LocalizationStringResource instance { get; private set; }

	public int columnCount => _columnCount;

	public int rowCount => _rowCount;

	public StringsByLanguage[] stringsByLanguage => _stringsByLanguage;

	public void Initialize()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		((Object)this).hideFlags = (HideFlags)(((Object)this).hideFlags | 0x20);
		_emptyStrings = new string[_columnCount];
		CreateHashToIndexDictionary();
		Localization.Initialize();
	}

	public void CreateHashToIndexDictionary()
	{
		_keyHashToIndex = new Dictionary<int, int>(_rowCount);
		for (int i = 0; i < _rowCount; i++)
		{
			_keyHashToIndex.Add(_keyHashes[i], i);
		}
	}

	public bool TryGetStrings(int keyHash, out string[] result)
	{
		if (!_keyHashToIndex.TryGetValue(keyHash, out var value))
		{
			Debug.LogWarning((object)string.Format("[{0}] There is no hash : {1}.", "LocalizationStringResource", keyHash));
			result = _emptyStrings;
			return false;
		}
		result = stringsByLanguage[value].strings;
		return true;
	}

	public string[] GetStrings(int keyHash)
	{
		TryGetStrings(keyHash, out var result);
		return result;
	}
}
