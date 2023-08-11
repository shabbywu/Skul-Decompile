using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using CutScenes;
using GameResources;
using Hardmode.Darktech;
using Level;
using Level.Npc;
using Level.Npc.FieldNpcs;
using Platforms;
using Singletons;
using SkulStories;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Data;

public static class GameData
{
	public class Buff
	{
		private const string key = "Buff";

		private readonly BoolData _attached;

		private readonly FloatData _remainTime;

		private readonly FloatData _stack;

		private static List<Buff> _buffs;

		public bool attached
		{
			get
			{
				return ((Data<bool>)(object)_attached).value;
			}
			set
			{
				((Data<bool>)(object)_attached).value = value;
			}
		}

		public float remainTime
		{
			get
			{
				return ((Data<float>)(object)_remainTime).value;
			}
			set
			{
				((Data<float>)(object)_remainTime).value = value;
			}
		}

		public float stack
		{
			get
			{
				return ((Data<float>)(object)_stack).value;
			}
			set
			{
				((Data<float>)(object)_stack).value = value;
			}
		}

		public static Buff Get(int index)
		{
			return _buffs[index];
		}

		public static Buff Get(SavableAbilityManager.Name name)
		{
			return _buffs[(int)name];
		}

		public static void Initialize(int length)
		{
			_buffs = new List<Buff>(length);
			for (int i = 0; i < length; i++)
			{
				_buffs.Add(new Buff(i));
			}
		}

		public static void ResetAll()
		{
			foreach (Buff buff in _buffs)
			{
				buff.Reset();
				buff.Save();
			}
		}

		private Buff(int index)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			_attached = new BoolData(string.Format("{0}/{1}/{2}", "Buff", index, "attached"), false);
			_remainTime = new FloatData(string.Format("{0}/{1}/{2}", "Buff", index, "remainTime"), false);
			_stack = new FloatData(string.Format("{0}/{1}/{2}", "Buff", index, "stack"), false);
		}

		public void Save()
		{
			((Data)_attached).Save();
			((Data)_remainTime).Save();
			((Data)_stack).Save();
		}

		public void Reset()
		{
			attached = false;
			remainTime = 0f;
			stack = 0f;
		}
	}

	public class Currency : IEditorDrawer
	{
		public enum Type
		{
			Gold,
			DarkQuartz,
			Bone,
			HeartQuartz
		}

		public delegate void OnEarnDelegate(int amount);

		public delegate void OnConsumeDelegate(int amount);

		public static string hasMoneyColorCode = "FFDE37";

		public static string noMoneyColorCode = "EE1111";

		public static string returnColorCode;

		public static Currency gold;

		public static Currency darkQuartz;

		public static Currency bone;

		public static Currency heartQuartz;

		public static Currency dimentionQuartz;

		public static EnumArray<Type, Currency> currencies;

		public readonly SumDouble multiplier = new SumDouble(1.0);

		private readonly string _key;

		private readonly string _spriteTMPKey;

		private readonly IntData _balance;

		private readonly IntData _income;

		private readonly IntData _totalIncome;

		public readonly string colorCode;

		private double _remainder;

		public int balance
		{
			get
			{
				return ((Data<int>)(object)_balance).value;
			}
			set
			{
				((Data<int>)(object)_balance).value = value;
			}
		}

		public int income
		{
			get
			{
				return ((Data<int>)(object)_income).value;
			}
			set
			{
				((Data<int>)(object)_income).value = value;
			}
		}

		public int totalIncome
		{
			get
			{
				return ((Data<int>)(object)_totalIncome).value;
			}
			set
			{
				((Data<int>)(object)_totalIncome).value = value;
			}
		}

		public string spriteTMPKey => _spriteTMPKey;

		public event OnEarnDelegate onEarn;

		public event OnConsumeDelegate onConsume;

		public static void Initialize()
		{
			gold = new Currency("gold", "FFDE37", "Others/Gold_Icon");
			darkQuartz = new Currency("darkQuartz", "b57dff", "Others/DarkQuartz_Icon");
			bone = new Currency("bone", "EEEEEE", "Others/BoneChip_Icon");
			heartQuartz = new Currency("heartQuartz", "AC00E4", "Others/DarkHeart_Icon");
			currencies = new EnumArray<Type, Currency>(new Currency[4] { gold, darkQuartz, bone, heartQuartz });
		}

		public static void ResetAll()
		{
			gold.ResetNonpermaAll();
			darkQuartz.ResetNonpermaAll();
			bone.ResetNonpermaAll();
			heartQuartz.ResetNonpermaAll();
			SaveAll();
		}

		public static void SaveAll()
		{
			gold.Save();
			darkQuartz.Save();
			bone.Save();
			heartQuartz.Save();
		}

		private Currency(string key, string colorCode, string spriteCode)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			_key = key;
			_balance = new IntData("Currency/" + key + "/balance", false);
			_income = new IntData("Currency/" + key + "/income", false);
			_totalIncome = new IntData("Currency/" + key + "/totalIncome", false);
			_spriteTMPKey = "<sprite name=\"" + spriteCode + "\">";
			this.colorCode = colorCode;
		}

		public void DrawEditor()
		{
		}

		public void Save()
		{
			((Data)_balance).Save();
			((Data)_income).Save();
			((Data)_totalIncome).Save();
		}

		public void ResetNonpermaAll()
		{
			balance = 0;
			income = 0;
		}

		public void Reset()
		{
			balance = 0;
			income = 0;
			totalIncome = 0;
		}

		public void Earn(double amount)
		{
			double num = ((Sum<double>)(object)multiplier).total * amount;
			int num2 = (int)num;
			_remainder += num - (double)num2;
			if (_remainder >= 1.0)
			{
				int num3 = (int)_remainder;
				_remainder -= num3;
				num2 += num3;
			}
			balance += num2;
			income += num2;
			totalIncome += num2;
			this.onEarn?.Invoke(num2);
		}

		public void Earn(int amount)
		{
			Earn((double)amount);
		}

		public bool Has(int amount)
		{
			return balance >= amount;
		}

		public bool Consume(int amount)
		{
			if (!Has(amount))
			{
				return false;
			}
			balance -= amount;
			this.onConsume?.Invoke(amount);
			return true;
		}
	}

	public interface IEditorDrawer
	{
		void DrawEditor();
	}

	public static class Gear
	{
		private const string key = "gear";

		private const string unlockedKey = "gear/unlocked";

		private const string foundedKey = "gear/founded";

		public static bool IsUnlocked(string typeName, string name)
		{
			return PersistentSingleton<PlatformManager>.Instance.platform.data.GetBool("gear/unlocked/" + typeName + "/" + name, false);
		}

		public static void SetUnlocked(string typeName, string name, bool value)
		{
			PersistentSingleton<PlatformManager>.Instance.platform.data.SetBool("gear/unlocked/" + typeName + "/" + name, value);
		}

		public static bool IsFounded(string typeName, string name)
		{
			return PersistentSingleton<PlatformManager>.Instance.platform.data.GetBool("gear/founded/" + typeName + "/" + name, false);
		}

		public static void SetFounded(string typeName, string name, bool value)
		{
			PersistentSingleton<PlatformManager>.Instance.platform.data.SetBool("gear/founded/" + typeName + "/" + name, value);
		}

		public static void ResetAll()
		{
			PersistentSingleton<PlatformManager>.Instance.platform.data.DeleteKey((Predicate<string>)((string key) => key.StartsWith(key)));
		}
	}

	public class Generic : IEditorDrawer
	{
		public enum Skin
		{
			Skul,
			HeroSkul
		}

		public class Tutorial : IEditorDrawer
		{
			private BoolData _played;

			private bool _isPlaying;

			public Tutorial()
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Expected O, but got Unknown
				_played = new BoolData("Tutorial/_played", true);
			}

			public void Start()
			{
				_isPlaying = true;
			}

			public void Stop()
			{
				_isPlaying = false;
			}

			public void End()
			{
				_isPlaying = false;
				SetData(value: true);
			}

			public bool isPlayed()
			{
				return ((Data<bool>)(object)_played).value;
			}

			public bool isPlaying()
			{
				return _isPlaying;
			}

			internal void Save()
			{
				((Data)_played).Save();
			}

			internal void Reset()
			{
				((Data)_played).Reset();
			}

			private void SetData(bool value)
			{
				((Data<bool>)(object)_played).value = value;
				Save();
			}

			public void DrawEditor()
			{
			}
		}

		private Tutorial _tutorial;

		private StringData _lastPlayedVersion;

		private BoolData _playedTutorialDuringEA;

		private IntData _skinIndex;

		private BoolData _normalEnding;

		public static readonly Generic instance = new Generic();

		private const string _playedTutorialDuringEA_DataPath = "Generic/tutorialPlayed";

		public static Tutorial tutorial => instance._tutorial;

		public static string lastPlayedVersion
		{
			get
			{
				return ((Data<string>)(object)instance._lastPlayedVersion).value;
			}
			set
			{
				((Data<string>)(object)instance._lastPlayedVersion).value = value;
			}
		}

		public static bool playedTutorialDuringEA
		{
			get
			{
				return ((Data<bool>)(object)instance._playedTutorialDuringEA).value;
			}
			set
			{
				((Data<bool>)(object)instance._playedTutorialDuringEA).value = value;
			}
		}

		public static bool normalEnding
		{
			get
			{
				return ((Data<bool>)(object)instance._normalEnding).value;
			}
			set
			{
				((Data<bool>)(object)instance._normalEnding).value = value;
			}
		}

		public static int skinIndex
		{
			get
			{
				return ((Data<int>)(object)instance._skinIndex).value;
			}
			set
			{
				((Data<int>)(object)instance._skinIndex).value = value;
			}
		}

		public void Initialize()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			_lastPlayedVersion = new StringData("Generic/_lastPlayedVersion", Application.version, true);
			_tutorial = new Tutorial();
			_normalEnding = new BoolData("Generic/normalEnding", false);
			_skinIndex = new IntData("Generic/_skinIndex", false);
			_playedTutorialDuringEA = new BoolData("Generic/tutorialPlayed", true);
		}

		public void DrawEditor()
		{
		}

		public static void ResetAll()
		{
			((Data)instance._playedTutorialDuringEA).Reset();
			instance._tutorial.Reset();
			((Data)instance._skinIndex).Reset();
			((Data)instance._normalEnding).Reset();
			SaveAll();
		}

		public static void SaveAll()
		{
			((Data)instance._playedTutorialDuringEA).Save();
			instance._tutorial.Save();
			((Data)instance._skinIndex).Save();
			((Data)instance._normalEnding).Save();
		}

		public static void SaveSkin()
		{
			((Data)instance._skinIndex).Save();
		}
	}

	public class HardmodeProgress : IEditorDrawer
	{
		public class BoolDataEnumArray<T> : IEditorDrawer, IEnumerable<KeyValuePair<T, BoolData>>, IEnumerable where T : Enum
		{
			private readonly Dictionary<T, BoolData> _dictionary = new Dictionary<T, BoolData>();

			private readonly string _foldoutLabel;

			private bool _foldout;

			public BoolDataEnumArray(string foldoutLabel)
			{
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Expected O, but got Unknown
				_foldoutLabel = foldoutLabel;
				foreach (T value in EnumValues<T>.Values)
				{
					if (_dictionary.ContainsKey(value))
					{
						Debug.LogError((object)$"The key {value} is duplicated.");
					}
					else
					{
						_dictionary.Add(value, new BoolData(string.Format("{0}/{1}/{2}", "HardmodeProgress", "BoolDataEnumArray", value), false));
					}
				}
			}

			public void SaveAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					((Data)value).Save();
				}
			}

			public void ResetAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					((Data)value).Reset();
				}
			}

			public bool GetData(T key)
			{
				return ((Data<bool>)(object)_dictionary[key]).value;
			}

			public void SetData(T key, bool value)
			{
				((Data<bool>)(object)_dictionary[key]).value = value;
			}

			public void DrawEditor()
			{
			}

			public IEnumerator<KeyValuePair<T, BoolData>> GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}
		}

		public class LuckyMeasuringInstrument
		{
			public readonly int maxRefreshCount = 20;

			public IntData refreshCount;

			public IntData lootCount;

			public IntData lastUniqueDropOrder;

			public IntData lastLegendaryDropOrder;

			public StringDataArray items;

			private bool _foldout = true;

			public LuckyMeasuringInstrument()
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Expected O, but got Unknown
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Expected O, but got Unknown
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Expected O, but got Unknown
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Expected O, but got Unknown
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Expected O, but got Unknown
				refreshCount = new IntData("HardmodeProgress/refreshCount", false);
				lootCount = new IntData("HardmodeProgress/lootCount", false);
				lastUniqueDropOrder = new IntData("HardmodeProgress/lastUniqueDropOrder", false);
				lastLegendaryDropOrder = new IntData("HardmodeProgress/lastLegendaryDropOrder", false);
				items = new StringDataArray("HardmodeProgress/items", maxRefreshCount, false);
			}

			public void Save()
			{
				((Data)refreshCount).Save();
				((Data)lootCount).Save();
				((Data)lastUniqueDropOrder).Save();
				((Data)lastLegendaryDropOrder).Save();
				items.Save();
			}

			public void Reset()
			{
				((Data)refreshCount).Reset();
				((Data)lootCount).Reset();
				((Data)lastUniqueDropOrder).Reset();
				((Data)lastLegendaryDropOrder).Reset();
				items.Reset();
			}
		}

		public class InscriptionSynthesisEquipment
		{
			public static int count = 2;

			private IntData[] _inscriptionIndics;

			public IntData this[int index]
			{
				get
				{
					return _inscriptionIndics[index];
				}
				set
				{
					_inscriptionIndics[index] = value;
				}
			}

			public InscriptionSynthesisEquipment()
			{
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				_inscriptionIndics = (IntData[])(object)new IntData[count];
				for (int i = 0; i < count; i++)
				{
					_inscriptionIndics[i] = new IntData(string.Format("{0}/{1}/{2}", "HardmodeProgress", "InscriptionSynthesisEquipment", i), -1, false);
				}
			}

			public void Save()
			{
				IntData[] inscriptionIndics = _inscriptionIndics;
				for (int i = 0; i < inscriptionIndics.Length; i++)
				{
					((Data)inscriptionIndics[i]).Save();
				}
			}

			public void Reset()
			{
				IntData[] inscriptionIndics = _inscriptionIndics;
				for (int i = 0; i < inscriptionIndics.Length; i++)
				{
					((Data)inscriptionIndics[i]).Reset();
				}
			}
		}

		public static readonly HardmodeProgress instance = new HardmodeProgress();

		private IntData _hardmodeLevel;

		private IntData _clearedLevel;

		private BoolData _hardmode;

		private LuckyMeasuringInstrument _luckyMeasuringInstrument;

		private InscriptionSynthesisEquipment _inscriptionSynthesisEquipment;

		private BoolDataEnumArray<DarktechData.Type> _unlocked;

		private BoolDataEnumArray<DarktechData.Type> _activated;

		public static readonly int maxLevel = 10;

		public static LuckyMeasuringInstrument luckyMeasuringInstrument => instance._luckyMeasuringInstrument;

		public static InscriptionSynthesisEquipment inscriptionSynthesisEquipment => instance._inscriptionSynthesisEquipment;

		public static BoolDataEnumArray<DarktechData.Type> unlocked => instance._unlocked;

		public static BoolDataEnumArray<DarktechData.Type> activated => instance._activated;

		public static int hardmodeLevel
		{
			get
			{
				return ((Data<int>)(object)instance._hardmodeLevel).value;
			}
			set
			{
				((Data<int>)(object)instance._hardmodeLevel).value = value;
			}
		}

		public static int clearedLevel
		{
			get
			{
				return ((Data<int>)(object)instance._clearedLevel).value;
			}
			set
			{
				((Data<int>)(object)instance._clearedLevel).value = value;
			}
		}

		public static bool hardmode
		{
			get
			{
				return ((Data<bool>)(object)instance._hardmode).value;
			}
			set
			{
				((Data<bool>)(object)instance._hardmode).value = value;
				if (value)
				{
					Settings.easyMode = false;
				}
			}
		}

		public static void ResetAll()
		{
			instance._luckyMeasuringInstrument.Reset();
			instance._inscriptionSynthesisEquipment.Reset();
			unlocked.ResetAll();
			activated.ResetAll();
			((Data)instance._hardmodeLevel).Reset();
			((Data)instance._clearedLevel).Reset();
			((Data)instance._hardmode).Reset();
			SaveAll();
		}

		public static void ResetNonpermaAll()
		{
			instance._luckyMeasuringInstrument.Reset();
			instance._inscriptionSynthesisEquipment.Reset();
			SaveAll();
		}

		public static void SaveAll()
		{
			instance._luckyMeasuringInstrument.Save();
			instance._inscriptionSynthesisEquipment.Save();
			unlocked.SaveAll();
			activated.SaveAll();
			((Data)instance._hardmodeLevel).Save();
			((Data)instance._clearedLevel).Save();
			((Data)instance._hardmode).Save();
		}

		public void Initialize()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			_luckyMeasuringInstrument = new LuckyMeasuringInstrument();
			_inscriptionSynthesisEquipment = new InscriptionSynthesisEquipment();
			_unlocked = new BoolDataEnumArray<DarktechData.Type>("_unlocked");
			_activated = new BoolDataEnumArray<DarktechData.Type>("_activated");
			_hardmodeLevel = new IntData("HardmodeProgress/hardmodeLevel", false);
			_clearedLevel = new IntData("HardmodeProgress/clearedLevel", -1, false);
			_hardmode = new BoolData("HardmodeProgress/hardmode", false);
		}

		public void DrawEditor()
		{
		}
	}

	public class Progress : IEditorDrawer
	{
		public class WitchMastery : IEditorDrawer
		{
			public class Bonuses : IEditorDrawer
			{
				public readonly string key;

				private bool _foldout = true;

				private IntData[] _datas = (IntData[])(object)new IntData[4];

				public IntData this[int index] => _datas[index];

				public Bonuses(string key)
				{
					//IL_0059: Unknown result type (might be due to invalid IL or missing references)
					//IL_005f: Expected O, but got Unknown
					this.key = key;
					for (int i = 0; i < 4; i++)
					{
						_datas[i] = new IntData(string.Format("{0}/{1}/{2}/{3}", "Progress", "WitchMastery", key, i), false);
					}
				}

				public void Save()
				{
					IntData[] datas = _datas;
					for (int i = 0; i < datas.Length; i++)
					{
						((Data)datas[i]).Save();
					}
				}

				public void Reset()
				{
					IntData[] datas = _datas;
					for (int i = 0; i < datas.Length; i++)
					{
						((Data)datas[i]).Reset();
					}
				}

				public int GerFormerRefundAmount()
				{
					int num = 0;
					for (int i = 0; i < _datas.Length; i++)
					{
						num += WitchMasteryFormerPrice.GeRefundAmount(i, ((Data<int>)(object)_datas[i]).value);
					}
					return num;
				}

				public void DrawEditor()
				{
				}
			}

			private const int count = 4;

			public readonly Bonuses skull = new Bonuses("skull");

			public readonly Bonuses body = new Bonuses("body");

			public readonly Bonuses soul = new Bonuses("soul");

			private bool _foldout;

			public void Save()
			{
				skull.Save();
				body.Save();
				soul.Save();
			}

			public void Reset()
			{
				skull.Reset();
				body.Reset();
				soul.Reset();
			}

			public void RefundFormer()
			{
				int num = 0;
				num += skull.GerFormerRefundAmount();
				num += body.GerFormerRefundAmount();
				num += soul.GerFormerRefundAmount();
				if (num != 0)
				{
					Debug.Log((object)$"Old witch mastery is refunded. Refunded dark quartz amount : {num}");
					Currency.darkQuartz.balance += num;
					Reset();
				}
			}

			public void DrawEditor()
			{
			}
		}

		public class BoolDataEnumArray<T> : IEditorDrawer, IEnumerable<KeyValuePair<T, BoolData>>, IEnumerable where T : Enum
		{
			private readonly Dictionary<T, BoolData> _dictionary = new Dictionary<T, BoolData>();

			private readonly string _foldoutLabel;

			private bool _foldout;

			public BoolDataEnumArray(string foldoutLabel)
			{
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Expected O, but got Unknown
				_foldoutLabel = foldoutLabel;
				foreach (T value in EnumValues<T>.Values)
				{
					if (_dictionary.ContainsKey(value))
					{
						Debug.LogError((object)$"The key {value} is duplicated.");
					}
					else
					{
						_dictionary.Add(value, new BoolData(string.Format("{0}/{1}", "BoolDataEnumArray", value), false));
					}
				}
			}

			public void SaveAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					((Data)value).Save();
				}
			}

			public void ResetAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					((Data)value).Reset();
				}
			}

			public bool GetData(T key)
			{
				return ((Data<bool>)(object)_dictionary[key]).value;
			}

			public void SetData(T key, bool value)
			{
				((Data<bool>)(object)_dictionary[key]).value = value;
			}

			public void SetDataAll(bool value)
			{
				foreach (BoolData value2 in _dictionary.Values)
				{
					((Data<bool>)(object)value2).value = value;
				}
			}

			public void DrawEditor()
			{
			}

			public IEnumerator<KeyValuePair<T, BoolData>> GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}
		}

		public static readonly Progress instance = new Progress();

		private BoolDataEnumArray<Level.Npc.FieldNpcs.NpcType> _fieldNpcEncountered;

		private BoolDataEnumArray<SpecialMap.Type> _specialMapEncountered;

		private BoolDataEnumArray<CutScenes.Key> _cutscene;

		private BoolDataEnumArray<SkulStories.Key> _skulstory;

		private WitchMastery _witch;

		private IntData _playTime;

		private IntData _deaths;

		private IntData _kills;

		private IntData _eliteKills;

		private IntData _totalAdventurerKills;

		private IntData _gainedDarkcite;

		private IntData _housingPoint;

		private IntData _housingSeen;

		private BoolData _reassembleUsed;

		private IntData _totalDamage;

		private IntData _totalTakingDamage;

		private IntData _totalHeal;

		private IntData _bestDamage;

		private IntData _encounterWeaponCount;

		private IntData _encounterItemCount;

		private IntData _encounterEssenceCount;

		private BoolData _arachneTutorial;

		private BoolData _foxRescued;

		private BoolData _ogreRescued;

		private BoolData _druidRescued;

		private BoolData _deathknightRescued;

		private EnumArray<Level.Npc.NpcType, BoolData> _rescuedByNpcType;

		public static WitchMastery witch => instance._witch;

		public static BoolDataEnumArray<Level.Npc.FieldNpcs.NpcType> fieldNpcEncountered => instance._fieldNpcEncountered;

		public static BoolDataEnumArray<SpecialMap.Type> specialMapEncountered => instance._specialMapEncountered;

		public static BoolDataEnumArray<CutScenes.Key> cutscene => instance._cutscene;

		public static BoolDataEnumArray<SkulStories.Key> skulstory => instance._skulstory;

		public static int playTime
		{
			get
			{
				return ((Data<int>)(object)instance._playTime).value;
			}
			set
			{
				((Data<int>)(object)instance._playTime).value = value;
			}
		}

		public static int deaths
		{
			get
			{
				return ((Data<int>)(object)instance._deaths).value;
			}
			set
			{
				((Data<int>)(object)instance._deaths).value = value;
			}
		}

		public static int kills
		{
			get
			{
				return ((Data<int>)(object)instance._kills).value;
			}
			set
			{
				((Data<int>)(object)instance._kills).value = value;
			}
		}

		public static int totalAdventurerKills
		{
			get
			{
				return ((Data<int>)(object)instance._totalAdventurerKills).value;
			}
			set
			{
				((Data<int>)(object)instance._totalAdventurerKills).value = value;
			}
		}

		public static int eliteKills
		{
			get
			{
				return ((Data<int>)(object)instance._eliteKills).value;
			}
			set
			{
				((Data<int>)(object)instance._eliteKills).value = value;
			}
		}

		public static int gainedDarkcite
		{
			get
			{
				return ((Data<int>)(object)instance._gainedDarkcite).value;
			}
			set
			{
				((Data<int>)(object)instance._gainedDarkcite).value = value;
			}
		}

		public static int totalDamage
		{
			get
			{
				return ((Data<int>)(object)instance._totalDamage).value;
			}
			set
			{
				((Data<int>)(object)instance._totalDamage).value = value;
			}
		}

		public static int totalTakingDamage
		{
			get
			{
				return ((Data<int>)(object)instance._totalTakingDamage).value;
			}
			set
			{
				((Data<int>)(object)instance._totalTakingDamage).value = value;
			}
		}

		public static int totalHeal
		{
			get
			{
				return ((Data<int>)(object)instance._totalHeal).value;
			}
			set
			{
				((Data<int>)(object)instance._totalHeal).value = value;
			}
		}

		public static int bestDamage
		{
			get
			{
				return ((Data<int>)(object)instance._bestDamage).value;
			}
			set
			{
				((Data<int>)(object)instance._bestDamage).value = value;
			}
		}

		public static int encounterWeaponCount
		{
			get
			{
				return ((Data<int>)(object)instance._encounterWeaponCount).value;
			}
			set
			{
				((Data<int>)(object)instance._encounterWeaponCount).value = value;
			}
		}

		public static int encounterItemCount
		{
			get
			{
				return ((Data<int>)(object)instance._encounterItemCount).value;
			}
			set
			{
				((Data<int>)(object)instance._encounterItemCount).value = value;
			}
		}

		public static int encounterEssenceCount
		{
			get
			{
				return ((Data<int>)(object)instance._encounterEssenceCount).value;
			}
			set
			{
				((Data<int>)(object)instance._encounterEssenceCount).value = value;
			}
		}

		public static int housingPoint
		{
			get
			{
				return ((Data<int>)(object)instance._housingPoint).value;
			}
			set
			{
				((Data<int>)(object)instance._housingPoint).value = value;
			}
		}

		public static int housingSeen
		{
			get
			{
				return ((Data<int>)(object)instance._housingSeen).value;
			}
			set
			{
				((Data<int>)(object)instance._housingSeen).value = value;
			}
		}

		public static bool reassembleUsed
		{
			get
			{
				return ((Data<bool>)(object)instance._reassembleUsed).value;
			}
			set
			{
				((Data<bool>)(object)instance._reassembleUsed).value = value;
			}
		}

		public static bool arachneTutorial
		{
			get
			{
				return ((Data<bool>)(object)instance._arachneTutorial).value;
			}
			set
			{
				((Data<bool>)(object)instance._arachneTutorial).value = value;
			}
		}

		public static bool foxRescued
		{
			get
			{
				return ((Data<bool>)(object)instance._foxRescued).value;
			}
			set
			{
				((Data<bool>)(object)instance._foxRescued).value = value;
			}
		}

		public static bool ogreRescued
		{
			get
			{
				return ((Data<bool>)(object)instance._ogreRescued).value;
			}
			set
			{
				((Data<bool>)(object)instance._ogreRescued).value = value;
			}
		}

		public static bool druidRescued
		{
			get
			{
				return ((Data<bool>)(object)instance._druidRescued).value;
			}
			set
			{
				((Data<bool>)(object)instance._druidRescued).value = value;
			}
		}

		public static bool deathknightRescued
		{
			get
			{
				return ((Data<bool>)(object)instance._deathknightRescued).value;
			}
			set
			{
				((Data<bool>)(object)instance._deathknightRescued).value = value;
			}
		}

		public static bool GetRescued(Level.Npc.NpcType npcType)
		{
			return ((Data<bool>)(object)instance._rescuedByNpcType[npcType]).value;
		}

		public static void SetRescued(Level.Npc.NpcType npcType, bool value)
		{
			((Data<bool>)(object)instance._rescuedByNpcType[npcType]).value = value;
		}

		public static void ResetAll()
		{
			instance._witch.Reset();
			instance._fieldNpcEncountered.ResetAll();
			instance._specialMapEncountered.ResetAll();
			instance._cutscene.ResetAll();
			instance._skulstory.ResetAll();
			((Data)instance._playTime).Reset();
			((Data)instance._deaths).Reset();
			((Data)instance._kills).Reset();
			((Data)instance._eliteKills).Reset();
			((Data)instance._totalAdventurerKills).Reset();
			((Data)instance._gainedDarkcite).Reset();
			((Data)instance._totalDamage).Reset();
			((Data)instance._totalTakingDamage).Reset();
			((Data)instance._totalHeal).Reset();
			((Data)instance._bestDamage).Reset();
			((Data)instance._encounterWeaponCount).Reset();
			((Data)instance._encounterItemCount).Reset();
			((Data)instance._encounterEssenceCount).Reset();
			((Data)instance._housingPoint).Reset();
			((Data)instance._housingSeen).Reset();
			((Data)instance._reassembleUsed).Reset();
			((Data)instance._arachneTutorial).Reset();
			((Data)instance._foxRescued).Reset();
			((Data)instance._ogreRescued).Reset();
			((Data)instance._druidRescued).Reset();
			((Data)instance._deathknightRescued).Reset();
			SaveAll();
		}

		public static void ResetNonpermaAll()
		{
			playTime = 0;
			kills = 0;
			eliteKills = 0;
			gainedDarkcite = 0;
			totalDamage = 0;
			totalTakingDamage = 0;
			totalHeal = 0;
			bestDamage = 0;
			encounterWeaponCount = 0;
			encounterItemCount = 0;
			encounterEssenceCount = 0;
			reassembleUsed = false;
			fieldNpcEncountered.ResetAll();
			specialMapEncountered.ResetAll();
			SaveAll();
		}

		public static void SaveAll()
		{
			((Data)instance._playTime).Save();
			((Data)instance._deaths).Save();
			((Data)instance._kills).Save();
			((Data)instance._eliteKills).Save();
			((Data)instance._totalAdventurerKills).Save();
			((Data)instance._gainedDarkcite).Save();
			((Data)instance._totalDamage).Save();
			((Data)instance._totalTakingDamage).Save();
			((Data)instance._totalHeal).Save();
			((Data)instance._bestDamage).Save();
			((Data)instance._encounterWeaponCount).Save();
			((Data)instance._encounterItemCount).Save();
			((Data)instance._encounterEssenceCount).Save();
			((Data)instance._housingPoint).Save();
			((Data)instance._housingSeen).Save();
			((Data)instance._reassembleUsed).Save();
			((Data)instance._arachneTutorial).Save();
			((Data)instance._foxRescued).Save();
			((Data)instance._ogreRescued).Save();
			((Data)instance._druidRescued).Save();
			((Data)instance._deathknightRescued).Save();
			witch.Save();
			fieldNpcEncountered.SaveAll();
			specialMapEncountered.SaveAll();
			cutscene.SaveAll();
			skulstory.SaveAll();
		}

		public void Initialize()
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Expected O, but got Unknown
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Expected O, but got Unknown
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Expected O, but got Unknown
			_witch = new WitchMastery();
			_fieldNpcEncountered = new BoolDataEnumArray<Level.Npc.FieldNpcs.NpcType>("fieldNpcEncountered");
			_specialMapEncountered = new BoolDataEnumArray<SpecialMap.Type>("specialMapEncountered");
			_cutscene = new BoolDataEnumArray<CutScenes.Key>("CutScenes");
			if (_cutscene.GetData(CutScenes.Key.ending))
			{
				Generic.normalEnding = true;
				Generic.SaveAll();
			}
			_skulstory = new BoolDataEnumArray<SkulStories.Key>("SkulStories");
			_playTime = new IntData("Progress/playTime", false);
			_deaths = new IntData("Progress/deaths", false);
			_kills = new IntData("Progress/kills", false);
			_eliteKills = new IntData("Progress/eliteKills", false);
			_totalAdventurerKills = new IntData("Progress/totalAdventurerKills", false);
			_gainedDarkcite = new IntData("Progress/gainedDarkcite", false);
			_totalDamage = new IntData("Progress/totalDamage", false);
			_totalTakingDamage = new IntData("Progress/totalTakingDamage", false);
			_totalHeal = new IntData("Progress/totalHeal", false);
			_bestDamage = new IntData("Progress/_bestDamage", false);
			_encounterWeaponCount = new IntData("Progress/encounterWeaponCount", false);
			_encounterItemCount = new IntData("Progress/encounterItemCount", false);
			_encounterEssenceCount = new IntData("Progress/encounterEssenceCount", false);
			_housingPoint = new IntData("Progress/housingPoint", false);
			_housingSeen = new IntData("Progress/_housingSeen", false);
			_reassembleUsed = new BoolData("Progress/reassembleUsed", false);
			_arachneTutorial = new BoolData("Progress/arachneTutorial", false);
			_foxRescued = new BoolData("Progress/foxRescued", false);
			_ogreRescued = new BoolData("Progress/ogreRescued", false);
			_druidRescued = new BoolData("Progress/druidRescued", false);
			_deathknightRescued = new BoolData("Progress/deathknightRescued", false);
			_rescuedByNpcType = new EnumArray<Level.Npc.NpcType, BoolData>((BoolData[])(object)new BoolData[5]
			{
				default(BoolData),
				_foxRescued,
				_ogreRescued,
				_druidRescued,
				_deathknightRescued
			});
		}

		public void DrawEditor()
		{
		}
	}

	public class Record
	{
		private const string _key = "Record";

		private const string bestTimeKey = "Record/bestTime";

		public static int GetBestTime(string key)
		{
			return PersistentSingleton<PlatformManager>.Instance.platform.data.GetInt("Record/bestTime/" + key, int.MaxValue);
		}

		public static void SetBestTime(string key, int value)
		{
			PersistentSingleton<PlatformManager>.Instance.platform.data.SetInt("Record/bestTime/" + key, value);
		}

		public static bool UpdateBestTime(string key)
		{
			int bestTime = GetBestTime(key);
			int playTime = Progress.playTime;
			if (bestTime < playTime)
			{
				return false;
			}
			SetBestTime(key, playTime);
			return true;
		}

		public static void ResetAll()
		{
			PersistentSingleton<PlatformManager>.Instance.platform.data.DeleteKey((Predicate<string>)((string key) => key.StartsWith(key)));
		}
	}

	public class Save : IEditorDrawer
	{
		private const string _key = "Save";

		public static readonly Save instance = new Save();

		private BoolData _hasSave;

		private IntData _randomSeed;

		private IntData _health;

		private IntData _chapterIndex;

		private IntData _stageIndex;

		private IntData _pathIndex;

		private IntData _nodeIndex;

		private StringData _currentWeapon;

		private StringData _nextWeapon;

		private StringData _currentWeaponSkill1;

		private StringData _currentWeaponSkill2;

		private StringData _nextWeaponSkill1;

		private StringData _nextWeaponSkill2;

		private FloatData _currentWeaponStack;

		private FloatData _nextWeaponStack;

		private StringData _essence;

		private FloatData _essenceStack;

		public StringData _abilityBuffs;

		public bool initilaized { get; private set; }

		public bool hasSave
		{
			get
			{
				return ((Data<bool>)(object)_hasSave).value;
			}
			set
			{
				((Data<bool>)(object)_hasSave).value = value;
			}
		}

		public int randomSeed => ((Data<int>)(object)_randomSeed).value;

		public int health
		{
			get
			{
				return ((Data<int>)(object)_health).value;
			}
			set
			{
				((Data<int>)(object)_health).value = value;
			}
		}

		public int chapterIndex
		{
			get
			{
				return ((Data<int>)(object)_chapterIndex).value;
			}
			set
			{
				((Data<int>)(object)_chapterIndex).value = value;
			}
		}

		public int stageIndex
		{
			get
			{
				return ((Data<int>)(object)_stageIndex).value;
			}
			set
			{
				((Data<int>)(object)_stageIndex).value = value;
			}
		}

		public int pathIndex
		{
			get
			{
				return ((Data<int>)(object)_pathIndex).value;
			}
			set
			{
				((Data<int>)(object)_pathIndex).value = value;
			}
		}

		public int nodeIndex
		{
			get
			{
				return ((Data<int>)(object)_nodeIndex).value;
			}
			set
			{
				((Data<int>)(object)_nodeIndex).value = value;
			}
		}

		public string currentWeapon
		{
			get
			{
				return ((Data<string>)(object)_currentWeapon).value;
			}
			set
			{
				((Data<string>)(object)_currentWeapon).value = value;
			}
		}

		public string nextWeapon
		{
			get
			{
				return ((Data<string>)(object)_nextWeapon).value;
			}
			set
			{
				((Data<string>)(object)_nextWeapon).value = value;
			}
		}

		public string currentWeaponSkill1
		{
			get
			{
				return ((Data<string>)(object)_currentWeaponSkill1).value;
			}
			set
			{
				((Data<string>)(object)_currentWeaponSkill1).value = value;
			}
		}

		public string currentWeaponSkill2
		{
			get
			{
				return ((Data<string>)(object)_currentWeaponSkill2).value;
			}
			set
			{
				((Data<string>)(object)_currentWeaponSkill2).value = value;
			}
		}

		public string nextWeaponSkill1
		{
			get
			{
				return ((Data<string>)(object)_nextWeaponSkill1).value;
			}
			set
			{
				((Data<string>)(object)_nextWeaponSkill1).value = value;
			}
		}

		public string nextWeaponSkill2
		{
			get
			{
				return ((Data<string>)(object)_nextWeaponSkill2).value;
			}
			set
			{
				((Data<string>)(object)_nextWeaponSkill2).value = value;
			}
		}

		public float currentWeaponStack
		{
			get
			{
				return ((Data<float>)(object)_currentWeaponStack).value;
			}
			set
			{
				((Data<float>)(object)_currentWeaponStack).value = value;
			}
		}

		public float nextWeaponStack
		{
			get
			{
				return ((Data<float>)(object)_nextWeaponStack).value;
			}
			set
			{
				((Data<float>)(object)_nextWeaponStack).value = value;
			}
		}

		public string essence
		{
			get
			{
				return ((Data<string>)(object)_essence).value;
			}
			set
			{
				((Data<string>)(object)_essence).value = value;
			}
		}

		public float essenceStack
		{
			get
			{
				return ((Data<float>)(object)_essenceStack).value;
			}
			set
			{
				((Data<float>)(object)_essenceStack).value = value;
			}
		}

		public StringDataArray items { get; private set; }

		public FloatDataArray itemStacks { get; private set; }

		public IntDataArray itemKeywords1 { get; private set; }

		public IntDataArray itemKeywords2 { get; private set; }

		public StringDataArray upgrades { get; private set; }

		public IntDataArray upgradeLevels { get; private set; }

		public FloatDataArray upgradeStacks { get; private set; }

		public string abilityBuffs
		{
			get
			{
				return ((Data<string>)(object)_abilityBuffs).value;
			}
			set
			{
				((Data<string>)(object)_abilityBuffs).value = value;
			}
		}

		public void Initialize()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Expected O, but got Unknown
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Expected O, but got Unknown
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Expected O, but got Unknown
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Expected O, but got Unknown
			initilaized = true;
			_hasSave = new BoolData("Save/hasSave", false);
			_randomSeed = new IntData("Save/randomSeed", false);
			_health = new IntData("Save/health", false);
			_chapterIndex = new IntData("Save/chapterIndex", false);
			_stageIndex = new IntData("Save/stageIndex", false);
			_pathIndex = new IntData("Save/pathIndex", false);
			_nodeIndex = new IntData("Save/nodeIndex", false);
			_currentWeapon = new StringData("Save/currentWeapon", false);
			_nextWeapon = new StringData("Save/nextWeapon", false);
			_currentWeaponStack = new FloatData("Save/currentWeaponStack", false);
			_nextWeaponStack = new FloatData("Save/nextWeaponStack", false);
			_currentWeaponSkill1 = new StringData("Save/currentWeaponSkill1", false);
			_currentWeaponSkill2 = new StringData("Save/currentWeaponSkill2", false);
			_nextWeaponSkill1 = new StringData("Save/nextWeaponSkill1", false);
			_nextWeaponSkill2 = new StringData("Save/nextWeaponSkill2", false);
			_essence = new StringData("Save/essence", false);
			_essenceStack = new FloatData("Save/essenceStack", false);
			items = new StringDataArray("Save/items", 9, false);
			itemStacks = new FloatDataArray("Save/itemStacks", 9, false);
			itemKeywords1 = new IntDataArray("Save/itemKeywords1", 9, false);
			itemKeywords2 = new IntDataArray("Save/itemKeywords2", 9, false);
			upgrades = new StringDataArray("Save/upgrades", 4, false);
			upgradeLevels = new IntDataArray("Save/upgradeLevels", 4, false);
			upgradeStacks = new FloatDataArray("Save/upgradeStacks", 4, false);
			_abilityBuffs = new StringData("Save/abilityBuffs", false);
		}

		public void ResetAll()
		{
			((Data)_hasSave).Reset();
			((Data<int>)(object)_randomSeed).value = Random.Range(int.MinValue, int.MaxValue);
			((Data)_health).Reset();
			((Data)_chapterIndex).Reset();
			((Data)_stageIndex).Reset();
			((Data)_pathIndex).Reset();
			((Data)_nodeIndex).Reset();
			((Data)_currentWeapon).Reset();
			((Data)_nextWeapon).Reset();
			((Data)_currentWeaponStack).Reset();
			((Data)_nextWeaponStack).Reset();
			((Data)_currentWeaponSkill1).Reset();
			((Data)_currentWeaponSkill2).Reset();
			((Data)_nextWeaponSkill1).Reset();
			((Data)_nextWeaponSkill2).Reset();
			((Data)_essence).Reset();
			((Data)_essenceStack).Reset();
			items.Reset();
			itemStacks.Reset();
			itemKeywords1.Reset();
			itemKeywords2.Reset();
			((Data)_abilityBuffs).Reset();
			upgrades.Reset();
			upgradeLevels.Reset();
			upgradeStacks.Reset();
			SaveAll();
		}

		public void SaveAll()
		{
			((Data)_hasSave).Save();
			((Data)_randomSeed).Save();
			((Data)_health).Save();
			((Data)_chapterIndex).Save();
			((Data)_stageIndex).Save();
			((Data)_pathIndex).Save();
			((Data)_nodeIndex).Save();
			((Data)_currentWeapon).Save();
			((Data)_nextWeapon).Save();
			((Data)_currentWeaponStack).Save();
			((Data)_nextWeaponStack).Save();
			((Data)_currentWeaponSkill1).Save();
			((Data)_currentWeaponSkill2).Save();
			((Data)_nextWeaponSkill1).Save();
			((Data)_nextWeaponSkill2).Save();
			((Data)_essence).Save();
			((Data)_essenceStack).Save();
			items.Save();
			itemStacks.Save();
			itemKeywords1.Save();
			itemKeywords2.Save();
			((Data)_abilityBuffs).Save();
			upgrades.Save();
			upgradeLevels.Save();
			upgradeStacks.Save();
		}

		public void ResetRandomSeed()
		{
			((Data<int>)(object)_randomSeed).value = Random.Range(-32768, 32767);
		}

		public void DrawEditor()
		{
		}
	}

	public class Settings : IEditorDrawer
	{
		public static readonly Settings instance = new Settings();

		private StringData _keyBindings;

		private BoolData _arrowDashEnabled;

		private BoolData _lightEnabled;

		private FloatData _masterVolume;

		private BoolData _musicEnabled;

		private FloatData _musicVolume;

		private BoolData _sfxEnabled;

		private FloatData _sfxVolume;

		private IntData _language;

		private FloatData _cameraShakeIntensity;

		private FloatData _vibrationIntensity;

		private IntData _particleQuality;

		private BoolData _easyMode;

		private BoolData _showTimer;

		public static string keyBindings
		{
			get
			{
				return ((Data<string>)(object)instance._keyBindings).value;
			}
			set
			{
				((Data<string>)(object)instance._keyBindings).value = value;
			}
		}

		public static bool arrowDashEnabled
		{
			get
			{
				return ((Data<bool>)(object)instance._arrowDashEnabled).value;
			}
			set
			{
				((Data<bool>)(object)instance._arrowDashEnabled).value = value;
			}
		}

		public static bool lightEnabled
		{
			get
			{
				return ((Data<bool>)(object)instance._lightEnabled).value;
			}
			set
			{
				((Data<bool>)(object)instance._lightEnabled).value = value;
			}
		}

		public static float masterVolume
		{
			get
			{
				return ((Data<float>)(object)instance._masterVolume).value;
			}
			set
			{
				((Data<float>)(object)instance._masterVolume).value = value;
			}
		}

		public static bool musicEnabled
		{
			get
			{
				return ((Data<bool>)(object)instance._musicEnabled).value;
			}
			set
			{
				((Data<bool>)(object)instance._musicEnabled).value = value;
			}
		}

		public static float musicVolume
		{
			get
			{
				return ((Data<float>)(object)instance._musicVolume).value;
			}
			set
			{
				((Data<float>)(object)instance._musicVolume).value = value;
			}
		}

		public static bool sfxEnabled
		{
			get
			{
				return ((Data<bool>)(object)instance._sfxEnabled).value;
			}
			set
			{
				((Data<bool>)(object)instance._sfxEnabled).value = value;
			}
		}

		public static float sfxVolume
		{
			get
			{
				return ((Data<float>)(object)instance._sfxVolume).value;
			}
			set
			{
				((Data<float>)(object)instance._sfxVolume).value = value;
			}
		}

		public static int language
		{
			get
			{
				return ((Data<int>)(object)instance._language).value;
			}
			set
			{
				((Data<int>)(object)instance._language).value = value;
			}
		}

		public static float cameraShakeIntensity
		{
			get
			{
				return ((Data<float>)(object)instance._cameraShakeIntensity).value;
			}
			set
			{
				((Data<float>)(object)instance._cameraShakeIntensity).value = value;
			}
		}

		public static float vibrationIntensity
		{
			get
			{
				return ((Data<float>)(object)instance._vibrationIntensity).value;
			}
			set
			{
				((Data<float>)(object)instance._vibrationIntensity).value = value;
			}
		}

		public static int particleQuality
		{
			get
			{
				return ((Data<int>)(object)instance._particleQuality).value;
			}
			set
			{
				((Data<int>)(object)instance._particleQuality).value = value;
			}
		}

		public static bool easyMode
		{
			get
			{
				return ((Data<bool>)(object)instance._easyMode).value;
			}
			set
			{
				((Data<bool>)(object)instance._easyMode).value = value;
			}
		}

		public static bool showTimer
		{
			get
			{
				return ((Data<bool>)(object)instance._showTimer).value;
			}
			set
			{
				((Data<bool>)(object)instance._showTimer).value = value;
			}
		}

		public static void Save()
		{
			((Data)instance._keyBindings).Save();
			((Data)instance._arrowDashEnabled).Save();
			((Data)instance._lightEnabled).Save();
			((Data)instance._masterVolume).Save();
			((Data)instance._musicEnabled).Save();
			((Data)instance._musicVolume).Save();
			((Data)instance._sfxEnabled).Save();
			((Data)instance._sfxVolume).Save();
			((Data)instance._language).Save();
			((Data)instance._cameraShakeIntensity).Save();
			((Data)instance._vibrationIntensity).Save();
			((Data)instance._particleQuality).Save();
			((Data)instance._easyMode).Save();
			((Data)instance._showTimer).Save();
			PlayerPrefs.Save();
		}

		public void Initialize()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			_keyBindings = new StringData("Settings/keyBindings", string.Empty, true);
			_arrowDashEnabled = new BoolData("Settings/arrowDashEnabled", false, false);
			bool flag = Application.isConsolePlatform || SystemInfo.systemMemorySize >= 4000 || SystemInfo.graphicsMemorySize >= 1000;
			_lightEnabled = new BoolData("Settings/lightEnabled", flag, false);
			Light2D.lightEnabled = ((Data<bool>)(object)_lightEnabled).value;
			_masterVolume = new FloatData("Settings/masterVolume", 0.8f, false);
			_musicEnabled = new BoolData("Settings/musicEnabled", true, false);
			_musicVolume = new FloatData("Settings/musicVolume", 0.6f, false);
			_sfxEnabled = new BoolData("Settings/sfxEnabled", true, false);
			_sfxVolume = new FloatData("Settings/sfxVolume", 0.8f, false);
			_language = new IntData("Settings/language", -1, false);
			Localization.ValidateLanguage();
			_cameraShakeIntensity = new FloatData("Settings/cameraShakeIntensity", 0.5f, false);
			_vibrationIntensity = new FloatData("Settings/vibrationIntensity", 0.5f, false);
			_particleQuality = new IntData("Settings/particleQuality", 3, false);
			_easyMode = new BoolData("Settings/easyMode", false, false);
			_showTimer = new BoolData("Settings/showTimer", false, false);
		}

		public void ResetKeyBindings()
		{
			((Data)_keyBindings).Reset();
		}

		public void ResetLanguage()
		{
			((Data)_language).Reset();
			Localization.ValidateLanguage();
		}

		public void DrawEditor()
		{
		}
	}

	private const int _currentVersion = 9;

	private static IntData _version;

	public static int version
	{
		get
		{
			return ((Data<int>)(object)_version).value;
		}
		set
		{
			((Data<int>)(object)_version).value = value;
		}
	}

	public static void Initialize()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
		_version = new IntData("version", true);
		int value = ((Data<int>)(object)_version).value;
		((Data<int>)(object)_version).value = 9;
		Generic.instance.Initialize();
		Currency.Initialize();
		Progress.instance.Initialize();
		Settings.instance.Initialize();
		Save.instance.Initialize();
		Buff.Initialize(Enum.GetValues(typeof(SavableAbilityManager.Name)).Length);
		HardmodeProgress.instance.Initialize();
		if (value > 0 && value <= 2)
		{
			Progress.witch.RefundFormer();
		}
		if (value > 0 && value <= 3)
		{
			Settings.particleQuality++;
		}
		if (value > 0 && value <= 4)
		{
			if (Settings.language == 4)
			{
				Settings.language = 5;
			}
			else if (Settings.language == 5)
			{
				Settings.language = 4;
			}
		}
		if (value == 5 && (int)Application.systemLanguage == 15)
		{
			Settings.language = 5;
		}
		if (value < 9)
		{
			Save.instance.ResetAll();
		}
	}
}
