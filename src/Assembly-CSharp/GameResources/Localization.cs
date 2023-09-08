using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using UnityEngine;

namespace GameResources;

public static class Localization
{
	public enum Language
	{
		Korean,
		English,
		Japanese,
		Chinese_Simplifed,
		Chinese_Traditional,
		German,
		Spanish,
		Portguese,
		Russian,
		Polish,
		French
	}

	public sealed class Key : IEquatable<Key>
	{
		public static readonly Key languageCode = new Key("language/code");

		public static readonly Key languageSystem = new Key("language/system");

		public static readonly Key languageNative = new Key("language/native");

		public static readonly Key languageNumber = new Key("language/number");

		public static readonly Key languageDisplayOrder = new Key("language/displayOrder");

		public static readonly Key titleFont = new Key("font/title");

		public static readonly Key bodyFont = new Key("font/body");

		public static readonly Key colorClose = new Key("cc");

		public static readonly Key colorOpenGold = new Key("cogold");

		public static readonly Key colorOpenDarkQuartz = new Key("codark");

		public static readonly Key colorOpenBone = new Key("cobone");

		public readonly string key;

		public readonly int hashcode;

		private Key(string key)
		{
			this.key = key;
			hashcode = StringComparer.OrdinalIgnoreCase.GetHashCode(key);
		}

		public override int GetHashCode()
		{
			return hashcode;
		}

		public bool Equals(Key other)
		{
			int num = hashcode;
			return num.Equals(other.hashcode);
		}
	}

	private static string[] systemLanguages = new string[11]
	{
		"Korean", "English", "Japanese", "ChineseSimplified", "ChineseTraditional", "German", "Spanish", "Portuguese", "Russian", "Polish",
		"French"
	};

	public const string language = "language";

	public const string label = "label";

	public const string name = "name";

	public const string desc = "desc";

	public const string flavor = "flavor";

	public const string active = "active";

	public const string skill = "skill";

	public static readonly EnumArray<Language, int> languangeNumberToIndex = new EnumArray<Language, int>();

	public static readonly EnumArray<Language, int> displayOrderToIndex = new EnumArray<Language, int>();

	public static ReadOnlyCollection<string> nativeNames { get; private set; }

	private static int _current
	{
		get
		{
			return GameData.Settings.language;
		}
		set
		{
			GameData.Settings.language = value;
		}
	}

	public static event Action OnChange;

	public static void Initialize()
	{
		LocalizationStringResource instance = LocalizationStringResource.instance;
		nativeNames = Array.AsReadOnly(instance.GetStrings(Key.languageNative.hashcode));
		string[] strings = instance.GetStrings(Key.languageNumber.hashcode);
		for (int i = 0; i < strings.Length; i++)
		{
			if (int.TryParse(strings[i], out var result))
			{
				languangeNumberToIndex.Array[result] = i;
			}
		}
	}

	public static void ValidateLanguage()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		SystemLanguage systemLanguage = Application.systemLanguage;
		string text = ((object)(SystemLanguage)(ref systemLanguage)).ToString();
		if (_current >= 0 && _current < systemLanguages.Length)
		{
			return;
		}
		_current = Convert.ToInt32(Language.English);
		for (int i = 0; i < systemLanguages.Length; i++)
		{
			if (systemLanguages[i].Equals(text, StringComparison.OrdinalIgnoreCase))
			{
				Debug.Log((object)("System language is automatically detected : " + text));
				_current = i;
				break;
			}
		}
	}

	public static bool TryGetLocalizedString(Key key, out string @string)
	{
		return TryGetLocalizedStringAt(key.hashcode, _current, out @string);
	}

	public static bool TryGetLocalizedString(string key, out string @string)
	{
		return TryGetLocalizedStringAt(StringComparer.OrdinalIgnoreCase.GetHashCode(key), _current, out @string);
	}

	public static string GetLocalizedString(Key key)
	{
		return GetLocalizedStringAt(key.hashcode, _current);
	}

	public static bool TryGetLocalizedStringArray(string key, out string[] strings)
	{
		List<string> list = new List<string>();
		if (TryGetLocalizedString(key, out var @string))
		{
			strings = new string[1] { @string };
			return true;
		}
		int i;
		for (i = 0; TryGetLocalizedString($"{key}/{i}", out @string); i++)
		{
			list.Add(@string);
		}
		if (i == 0)
		{
			strings = null;
			return false;
		}
		strings = list.ToArray();
		return true;
	}

	public static string[] GetLocalizedStringArray(string key)
	{
		List<string> list = new List<string>();
		if (TryGetLocalizedString(key, out var @string))
		{
			return new string[1] { @string };
		}
		for (int i = 0; TryGetLocalizedString($"{key}/{i}", out @string); i++)
		{
			list.Add(@string);
		}
		return list.ToArray();
	}

	public static string[][] GetLocalizedStringArrays(string key)
	{
		List<string[]> list = new List<string[]>();
		string[] strings;
		for (int i = 0; TryGetLocalizedStringArray($"{key}/{i}", out strings); i++)
		{
			list.Add(strings);
		}
		return list.ToArray();
	}

	public static string[] GetLocalizedStrings(params Key[] keys)
	{
		string[] array = new string[keys.Length];
		for (int i = 0; i < keys.Length; i++)
		{
			array[i] = GetLocalizedStringAt(keys[i].hashcode, _current);
		}
		return array;
	}

	public static string[] GetLocalizedStrings(params string[] keys)
	{
		string[] array = new string[keys.Length];
		for (int i = 0; i < keys.Length; i++)
		{
			array[i] = GetLocalizedStringAt(StringComparer.OrdinalIgnoreCase.GetHashCode(keys[i]), _current);
		}
		return array;
	}

	public static string GetLocalizedString(string key)
	{
		return GetLocalizedStringAt(StringComparer.OrdinalIgnoreCase.GetHashCode(key), _current);
	}

	private static string GetLocalizedStringAt(int key, int index)
	{
		TryGetLocalizedStringAt(key, index, out var @string);
		return @string;
	}

	private static bool TryGetLocalizedStringAt(int key, int number, out string @string)
	{
		if ((Object)(object)LocalizationStringResource.instance == (Object)null)
		{
			@string = string.Empty;
			return false;
		}
		if (LocalizationStringResource.instance.TryGetStrings(key, out var result))
		{
			int num = languangeNumberToIndex.Array[number];
			@string = result[num];
			if (string.IsNullOrWhiteSpace(@string))
			{
				@string = result[0];
			}
			return true;
		}
		@string = string.Empty;
		return false;
	}

	public static void Change(int number)
	{
		_current = languangeNumberToIndex.Array[number];
		Localization.OnChange?.Invoke();
	}

	public static void Change(Language language)
	{
		Change(Convert.ToInt32(language));
	}
}
