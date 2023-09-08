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
				return _attached.value;
			}
			set
			{
				_attached.value = value;
			}
		}

		public float remainTime
		{
			get
			{
				return _remainTime.value;
			}
			set
			{
				_remainTime.value = value;
			}
		}

		public float stack
		{
			get
			{
				return _stack.value;
			}
			set
			{
				_stack.value = value;
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
			_attached = new BoolData(string.Format("{0}/{1}/{2}", "Buff", index, "attached"));
			_remainTime = new FloatData(string.Format("{0}/{1}/{2}", "Buff", index, "remainTime"));
			_stack = new FloatData(string.Format("{0}/{1}/{2}", "Buff", index, "stack"));
		}

		public void Save()
		{
			_attached.Save();
			_remainTime.Save();
			_stack.Save();
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
				return _balance.value;
			}
			set
			{
				_balance.value = value;
			}
		}

		public int income
		{
			get
			{
				return _income.value;
			}
			set
			{
				_income.value = value;
			}
		}

		public int totalIncome
		{
			get
			{
				return _totalIncome.value;
			}
			set
			{
				_totalIncome.value = value;
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
			currencies = new EnumArray<Type, Currency>(gold, darkQuartz, bone, heartQuartz);
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
			_key = key;
			_balance = new IntData("Currency/" + key + "/balance");
			_income = new IntData("Currency/" + key + "/income");
			_totalIncome = new IntData("Currency/" + key + "/totalIncome");
			_spriteTMPKey = "<sprite name=\"" + spriteCode + "\">";
			this.colorCode = colorCode;
		}

		public void DrawEditor()
		{
		}

		public void Save()
		{
			_balance.Save();
			_income.Save();
			_totalIncome.Save();
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
			double num = multiplier.total * amount;
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
				_played = new BoolData("Tutorial/_played", isRealtime: true);
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
				return _played.value;
			}

			public bool isPlaying()
			{
				return _isPlaying;
			}

			internal void Save()
			{
				_played.Save();
			}

			internal void Reset()
			{
				_played.Reset();
			}

			private void SetData(bool value)
			{
				_played.value = value;
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
				return instance._lastPlayedVersion.value;
			}
			set
			{
				instance._lastPlayedVersion.value = value;
			}
		}

		public static bool playedTutorialDuringEA
		{
			get
			{
				return instance._playedTutorialDuringEA.value;
			}
			set
			{
				instance._playedTutorialDuringEA.value = value;
			}
		}

		public static bool normalEnding
		{
			get
			{
				return instance._normalEnding.value;
			}
			set
			{
				instance._normalEnding.value = value;
			}
		}

		public static int skinIndex
		{
			get
			{
				return instance._skinIndex.value;
			}
			set
			{
				instance._skinIndex.value = value;
			}
		}

		public void Initialize()
		{
			_lastPlayedVersion = new StringData("Generic/_lastPlayedVersion", Application.version, isRealtime: true);
			_tutorial = new Tutorial();
			_normalEnding = new BoolData("Generic/normalEnding");
			_skinIndex = new IntData("Generic/_skinIndex");
			_playedTutorialDuringEA = new BoolData("Generic/tutorialPlayed", isRealtime: true);
		}

		public void DrawEditor()
		{
		}

		public static void ResetAll()
		{
			instance._playedTutorialDuringEA.Reset();
			instance._tutorial.Reset();
			instance._skinIndex.Reset();
			instance._normalEnding.Reset();
			SaveAll();
		}

		public static void SaveAll()
		{
			instance._playedTutorialDuringEA.Save();
			instance._tutorial.Save();
			instance._skinIndex.Save();
			instance._normalEnding.Save();
		}

		public static void SaveSkin()
		{
			instance._skinIndex.Save();
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
				_foldoutLabel = foldoutLabel;
				foreach (T value in EnumValues<T>.Values)
				{
					if (_dictionary.ContainsKey(value))
					{
						Debug.LogError((object)$"The key {value} is duplicated.");
					}
					else
					{
						_dictionary.Add(value, new BoolData(string.Format("{0}/{1}/{2}", "HardmodeProgress", "BoolDataEnumArray", value)));
					}
				}
			}

			public void SaveAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					value.Save();
				}
			}

			public void ResetAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					value.Reset();
				}
			}

			public bool GetData(T key)
			{
				return _dictionary[key].value;
			}

			public void SetData(T key, bool value)
			{
				_dictionary[key].value = value;
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
				refreshCount = new IntData("HardmodeProgress/refreshCount");
				lootCount = new IntData("HardmodeProgress/lootCount");
				lastUniqueDropOrder = new IntData("HardmodeProgress/lastUniqueDropOrder");
				lastLegendaryDropOrder = new IntData("HardmodeProgress/lastLegendaryDropOrder");
				items = new StringDataArray("HardmodeProgress/items", maxRefreshCount);
			}

			public void Save()
			{
				refreshCount.Save();
				lootCount.Save();
				lastUniqueDropOrder.Save();
				lastLegendaryDropOrder.Save();
				items.Save();
			}

			public void Reset()
			{
				refreshCount.Reset();
				lootCount.Reset();
				lastUniqueDropOrder.Reset();
				lastLegendaryDropOrder.Reset();
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
				_inscriptionIndics = new IntData[count];
				for (int i = 0; i < count; i++)
				{
					_inscriptionIndics[i] = new IntData(string.Format("{0}/{1}/{2}", "HardmodeProgress", "InscriptionSynthesisEquipment", i), -1);
				}
			}

			public void Save()
			{
				IntData[] inscriptionIndics = _inscriptionIndics;
				for (int i = 0; i < inscriptionIndics.Length; i++)
				{
					inscriptionIndics[i].Save();
				}
			}

			public void Reset()
			{
				IntData[] inscriptionIndics = _inscriptionIndics;
				for (int i = 0; i < inscriptionIndics.Length; i++)
				{
					inscriptionIndics[i].Reset();
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
				return instance._hardmodeLevel.value;
			}
			set
			{
				instance._hardmodeLevel.value = value;
			}
		}

		public static int clearedLevel
		{
			get
			{
				return instance._clearedLevel.value;
			}
			set
			{
				instance._clearedLevel.value = value;
			}
		}

		public static bool hardmode
		{
			get
			{
				return instance._hardmode.value;
			}
			set
			{
				instance._hardmode.value = value;
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
			instance._hardmodeLevel.Reset();
			instance._clearedLevel.Reset();
			instance._hardmode.Reset();
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
			instance._hardmodeLevel.Save();
			instance._clearedLevel.Save();
			instance._hardmode.Save();
		}

		public void Initialize()
		{
			_luckyMeasuringInstrument = new LuckyMeasuringInstrument();
			_inscriptionSynthesisEquipment = new InscriptionSynthesisEquipment();
			_unlocked = new BoolDataEnumArray<DarktechData.Type>("_unlocked");
			_activated = new BoolDataEnumArray<DarktechData.Type>("_activated");
			_hardmodeLevel = new IntData("HardmodeProgress/hardmodeLevel");
			_clearedLevel = new IntData("HardmodeProgress/clearedLevel", -1);
			_hardmode = new BoolData("HardmodeProgress/hardmode");
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

				private IntData[] _datas = new IntData[4];

				public IntData this[int index] => _datas[index];

				public Bonuses(string key)
				{
					this.key = key;
					for (int i = 0; i < 4; i++)
					{
						_datas[i] = new IntData(string.Format("{0}/{1}/{2}/{3}", "Progress", "WitchMastery", key, i));
					}
				}

				public void Save()
				{
					IntData[] datas = _datas;
					for (int i = 0; i < datas.Length; i++)
					{
						datas[i].Save();
					}
				}

				public void Reset()
				{
					IntData[] datas = _datas;
					for (int i = 0; i < datas.Length; i++)
					{
						datas[i].Reset();
					}
				}

				public int GerFormerRefundAmount()
				{
					int num = 0;
					for (int i = 0; i < _datas.Length; i++)
					{
						num += WitchMasteryFormerPrice.GeRefundAmount(i, _datas[i].value);
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
				_foldoutLabel = foldoutLabel;
				foreach (T value in EnumValues<T>.Values)
				{
					if (_dictionary.ContainsKey(value))
					{
						Debug.LogError((object)$"The key {value} is duplicated.");
					}
					else
					{
						_dictionary.Add(value, new BoolData(string.Format("{0}/{1}", "BoolDataEnumArray", value)));
					}
				}
			}

			public void SaveAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					value.Save();
				}
			}

			public void ResetAll()
			{
				foreach (BoolData value in _dictionary.Values)
				{
					value.Reset();
				}
			}

			public bool GetData(T key)
			{
				return _dictionary[key].value;
			}

			public void SetData(T key, bool value)
			{
				_dictionary[key].value = value;
			}

			public void SetDataAll(bool value)
			{
				foreach (BoolData value2 in _dictionary.Values)
				{
					value2.value = value;
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
				return instance._playTime.value;
			}
			set
			{
				instance._playTime.value = value;
			}
		}

		public static int deaths
		{
			get
			{
				return instance._deaths.value;
			}
			set
			{
				instance._deaths.value = value;
			}
		}

		public static int kills
		{
			get
			{
				return instance._kills.value;
			}
			set
			{
				instance._kills.value = value;
			}
		}

		public static int totalAdventurerKills
		{
			get
			{
				return instance._totalAdventurerKills.value;
			}
			set
			{
				instance._totalAdventurerKills.value = value;
			}
		}

		public static int eliteKills
		{
			get
			{
				return instance._eliteKills.value;
			}
			set
			{
				instance._eliteKills.value = value;
			}
		}

		public static int gainedDarkcite
		{
			get
			{
				return instance._gainedDarkcite.value;
			}
			set
			{
				instance._gainedDarkcite.value = value;
			}
		}

		public static int totalDamage
		{
			get
			{
				return instance._totalDamage.value;
			}
			set
			{
				instance._totalDamage.value = value;
			}
		}

		public static int totalTakingDamage
		{
			get
			{
				return instance._totalTakingDamage.value;
			}
			set
			{
				instance._totalTakingDamage.value = value;
			}
		}

		public static int totalHeal
		{
			get
			{
				return instance._totalHeal.value;
			}
			set
			{
				instance._totalHeal.value = value;
			}
		}

		public static int bestDamage
		{
			get
			{
				return instance._bestDamage.value;
			}
			set
			{
				instance._bestDamage.value = value;
			}
		}

		public static int encounterWeaponCount
		{
			get
			{
				return instance._encounterWeaponCount.value;
			}
			set
			{
				instance._encounterWeaponCount.value = value;
			}
		}

		public static int encounterItemCount
		{
			get
			{
				return instance._encounterItemCount.value;
			}
			set
			{
				instance._encounterItemCount.value = value;
			}
		}

		public static int encounterEssenceCount
		{
			get
			{
				return instance._encounterEssenceCount.value;
			}
			set
			{
				instance._encounterEssenceCount.value = value;
			}
		}

		public static int housingPoint
		{
			get
			{
				return instance._housingPoint.value;
			}
			set
			{
				instance._housingPoint.value = value;
			}
		}

		public static int housingSeen
		{
			get
			{
				return instance._housingSeen.value;
			}
			set
			{
				instance._housingSeen.value = value;
			}
		}

		public static bool reassembleUsed
		{
			get
			{
				return instance._reassembleUsed.value;
			}
			set
			{
				instance._reassembleUsed.value = value;
			}
		}

		public static bool arachneTutorial
		{
			get
			{
				return instance._arachneTutorial.value;
			}
			set
			{
				instance._arachneTutorial.value = value;
			}
		}

		public static bool foxRescued
		{
			get
			{
				return instance._foxRescued.value;
			}
			set
			{
				instance._foxRescued.value = value;
			}
		}

		public static bool ogreRescued
		{
			get
			{
				return instance._ogreRescued.value;
			}
			set
			{
				instance._ogreRescued.value = value;
			}
		}

		public static bool druidRescued
		{
			get
			{
				return instance._druidRescued.value;
			}
			set
			{
				instance._druidRescued.value = value;
			}
		}

		public static bool deathknightRescued
		{
			get
			{
				return instance._deathknightRescued.value;
			}
			set
			{
				instance._deathknightRescued.value = value;
			}
		}

		public static bool GetRescued(Level.Npc.NpcType npcType)
		{
			return instance._rescuedByNpcType[npcType].value;
		}

		public static void SetRescued(Level.Npc.NpcType npcType, bool value)
		{
			instance._rescuedByNpcType[npcType].value = value;
		}

		public static void ResetAll()
		{
			instance._witch.Reset();
			instance._fieldNpcEncountered.ResetAll();
			instance._specialMapEncountered.ResetAll();
			instance._cutscene.ResetAll();
			instance._skulstory.ResetAll();
			instance._playTime.Reset();
			instance._deaths.Reset();
			instance._kills.Reset();
			instance._eliteKills.Reset();
			instance._totalAdventurerKills.Reset();
			instance._gainedDarkcite.Reset();
			instance._totalDamage.Reset();
			instance._totalTakingDamage.Reset();
			instance._totalHeal.Reset();
			instance._bestDamage.Reset();
			instance._encounterWeaponCount.Reset();
			instance._encounterItemCount.Reset();
			instance._encounterEssenceCount.Reset();
			instance._housingPoint.Reset();
			instance._housingSeen.Reset();
			instance._reassembleUsed.Reset();
			instance._arachneTutorial.Reset();
			instance._foxRescued.Reset();
			instance._ogreRescued.Reset();
			instance._druidRescued.Reset();
			instance._deathknightRescued.Reset();
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
			instance._playTime.Save();
			instance._deaths.Save();
			instance._kills.Save();
			instance._eliteKills.Save();
			instance._totalAdventurerKills.Save();
			instance._gainedDarkcite.Save();
			instance._totalDamage.Save();
			instance._totalTakingDamage.Save();
			instance._totalHeal.Save();
			instance._bestDamage.Save();
			instance._encounterWeaponCount.Save();
			instance._encounterItemCount.Save();
			instance._encounterEssenceCount.Save();
			instance._housingPoint.Save();
			instance._housingSeen.Save();
			instance._reassembleUsed.Save();
			instance._arachneTutorial.Save();
			instance._foxRescued.Save();
			instance._ogreRescued.Save();
			instance._druidRescued.Save();
			instance._deathknightRescued.Save();
			witch.Save();
			fieldNpcEncountered.SaveAll();
			specialMapEncountered.SaveAll();
			cutscene.SaveAll();
			skulstory.SaveAll();
		}

		public void Initialize()
		{
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
			_playTime = new IntData("Progress/playTime");
			_deaths = new IntData("Progress/deaths");
			_kills = new IntData("Progress/kills");
			_eliteKills = new IntData("Progress/eliteKills");
			_totalAdventurerKills = new IntData("Progress/totalAdventurerKills");
			_gainedDarkcite = new IntData("Progress/gainedDarkcite");
			_totalDamage = new IntData("Progress/totalDamage");
			_totalTakingDamage = new IntData("Progress/totalTakingDamage");
			_totalHeal = new IntData("Progress/totalHeal");
			_bestDamage = new IntData("Progress/_bestDamage");
			_encounterWeaponCount = new IntData("Progress/encounterWeaponCount");
			_encounterItemCount = new IntData("Progress/encounterItemCount");
			_encounterEssenceCount = new IntData("Progress/encounterEssenceCount");
			_housingPoint = new IntData("Progress/housingPoint");
			_housingSeen = new IntData("Progress/_housingSeen");
			_reassembleUsed = new BoolData("Progress/reassembleUsed");
			_arachneTutorial = new BoolData("Progress/arachneTutorial");
			_foxRescued = new BoolData("Progress/foxRescued");
			_ogreRescued = new BoolData("Progress/ogreRescued");
			_druidRescued = new BoolData("Progress/druidRescued");
			_deathknightRescued = new BoolData("Progress/deathknightRescued");
			_rescuedByNpcType = new EnumArray<Level.Npc.NpcType, BoolData>(null, _foxRescued, _ogreRescued, _druidRescued, _deathknightRescued);
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
				return _hasSave.value;
			}
			set
			{
				_hasSave.value = value;
			}
		}

		public int randomSeed => _randomSeed.value;

		public int health
		{
			get
			{
				return _health.value;
			}
			set
			{
				_health.value = value;
			}
		}

		public int chapterIndex
		{
			get
			{
				return _chapterIndex.value;
			}
			set
			{
				_chapterIndex.value = value;
			}
		}

		public int stageIndex
		{
			get
			{
				return _stageIndex.value;
			}
			set
			{
				_stageIndex.value = value;
			}
		}

		public int pathIndex
		{
			get
			{
				return _pathIndex.value;
			}
			set
			{
				_pathIndex.value = value;
			}
		}

		public int nodeIndex
		{
			get
			{
				return _nodeIndex.value;
			}
			set
			{
				_nodeIndex.value = value;
			}
		}

		public string currentWeapon
		{
			get
			{
				return _currentWeapon.value;
			}
			set
			{
				_currentWeapon.value = value;
			}
		}

		public string nextWeapon
		{
			get
			{
				return _nextWeapon.value;
			}
			set
			{
				_nextWeapon.value = value;
			}
		}

		public string currentWeaponSkill1
		{
			get
			{
				return _currentWeaponSkill1.value;
			}
			set
			{
				_currentWeaponSkill1.value = value;
			}
		}

		public string currentWeaponSkill2
		{
			get
			{
				return _currentWeaponSkill2.value;
			}
			set
			{
				_currentWeaponSkill2.value = value;
			}
		}

		public string nextWeaponSkill1
		{
			get
			{
				return _nextWeaponSkill1.value;
			}
			set
			{
				_nextWeaponSkill1.value = value;
			}
		}

		public string nextWeaponSkill2
		{
			get
			{
				return _nextWeaponSkill2.value;
			}
			set
			{
				_nextWeaponSkill2.value = value;
			}
		}

		public float currentWeaponStack
		{
			get
			{
				return _currentWeaponStack.value;
			}
			set
			{
				_currentWeaponStack.value = value;
			}
		}

		public float nextWeaponStack
		{
			get
			{
				return _nextWeaponStack.value;
			}
			set
			{
				_nextWeaponStack.value = value;
			}
		}

		public string essence
		{
			get
			{
				return _essence.value;
			}
			set
			{
				_essence.value = value;
			}
		}

		public float essenceStack
		{
			get
			{
				return _essenceStack.value;
			}
			set
			{
				_essenceStack.value = value;
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
				return _abilityBuffs.value;
			}
			set
			{
				_abilityBuffs.value = value;
			}
		}

		public void Initialize()
		{
			initilaized = true;
			_hasSave = new BoolData("Save/hasSave");
			_randomSeed = new IntData("Save/randomSeed");
			_health = new IntData("Save/health");
			_chapterIndex = new IntData("Save/chapterIndex");
			_stageIndex = new IntData("Save/stageIndex");
			_pathIndex = new IntData("Save/pathIndex");
			_nodeIndex = new IntData("Save/nodeIndex");
			_currentWeapon = new StringData("Save/currentWeapon");
			_nextWeapon = new StringData("Save/nextWeapon");
			_currentWeaponStack = new FloatData("Save/currentWeaponStack");
			_nextWeaponStack = new FloatData("Save/nextWeaponStack");
			_currentWeaponSkill1 = new StringData("Save/currentWeaponSkill1");
			_currentWeaponSkill2 = new StringData("Save/currentWeaponSkill2");
			_nextWeaponSkill1 = new StringData("Save/nextWeaponSkill1");
			_nextWeaponSkill2 = new StringData("Save/nextWeaponSkill2");
			_essence = new StringData("Save/essence");
			_essenceStack = new FloatData("Save/essenceStack");
			items = new StringDataArray("Save/items", 9);
			itemStacks = new FloatDataArray("Save/itemStacks", 9);
			itemKeywords1 = new IntDataArray("Save/itemKeywords1", 9);
			itemKeywords2 = new IntDataArray("Save/itemKeywords2", 9);
			upgrades = new StringDataArray("Save/upgrades", 4);
			upgradeLevels = new IntDataArray("Save/upgradeLevels", 4);
			upgradeStacks = new FloatDataArray("Save/upgradeStacks", 4);
			_abilityBuffs = new StringData("Save/abilityBuffs");
		}

		public void ResetAll()
		{
			_hasSave.Reset();
			_randomSeed.value = Random.Range(int.MinValue, int.MaxValue);
			_health.Reset();
			_chapterIndex.Reset();
			_stageIndex.Reset();
			_pathIndex.Reset();
			_nodeIndex.Reset();
			_currentWeapon.Reset();
			_nextWeapon.Reset();
			_currentWeaponStack.Reset();
			_nextWeaponStack.Reset();
			_currentWeaponSkill1.Reset();
			_currentWeaponSkill2.Reset();
			_nextWeaponSkill1.Reset();
			_nextWeaponSkill2.Reset();
			_essence.Reset();
			_essenceStack.Reset();
			items.Reset();
			itemStacks.Reset();
			itemKeywords1.Reset();
			itemKeywords2.Reset();
			_abilityBuffs.Reset();
			upgrades.Reset();
			upgradeLevels.Reset();
			upgradeStacks.Reset();
			SaveAll();
		}

		public void SaveAll()
		{
			_hasSave.Save();
			_randomSeed.Save();
			_health.Save();
			_chapterIndex.Save();
			_stageIndex.Save();
			_pathIndex.Save();
			_nodeIndex.Save();
			_currentWeapon.Save();
			_nextWeapon.Save();
			_currentWeaponStack.Save();
			_nextWeaponStack.Save();
			_currentWeaponSkill1.Save();
			_currentWeaponSkill2.Save();
			_nextWeaponSkill1.Save();
			_nextWeaponSkill2.Save();
			_essence.Save();
			_essenceStack.Save();
			items.Save();
			itemStacks.Save();
			itemKeywords1.Save();
			itemKeywords2.Save();
			_abilityBuffs.Save();
			upgrades.Save();
			upgradeLevels.Save();
			upgradeStacks.Save();
		}

		public void ResetRandomSeed()
		{
			_randomSeed.value = Random.Range(-32768, 32767);
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
				return instance._keyBindings.value;
			}
			set
			{
				instance._keyBindings.value = value;
			}
		}

		public static bool arrowDashEnabled
		{
			get
			{
				return instance._arrowDashEnabled.value;
			}
			set
			{
				instance._arrowDashEnabled.value = value;
			}
		}

		public static bool lightEnabled
		{
			get
			{
				return instance._lightEnabled.value;
			}
			set
			{
				instance._lightEnabled.value = value;
			}
		}

		public static float masterVolume
		{
			get
			{
				return instance._masterVolume.value;
			}
			set
			{
				instance._masterVolume.value = value;
			}
		}

		public static bool musicEnabled
		{
			get
			{
				return instance._musicEnabled.value;
			}
			set
			{
				instance._musicEnabled.value = value;
			}
		}

		public static float musicVolume
		{
			get
			{
				return instance._musicVolume.value;
			}
			set
			{
				instance._musicVolume.value = value;
			}
		}

		public static bool sfxEnabled
		{
			get
			{
				return instance._sfxEnabled.value;
			}
			set
			{
				instance._sfxEnabled.value = value;
			}
		}

		public static float sfxVolume
		{
			get
			{
				return instance._sfxVolume.value;
			}
			set
			{
				instance._sfxVolume.value = value;
			}
		}

		public static int language
		{
			get
			{
				return instance._language.value;
			}
			set
			{
				instance._language.value = value;
			}
		}

		public static float cameraShakeIntensity
		{
			get
			{
				return instance._cameraShakeIntensity.value;
			}
			set
			{
				instance._cameraShakeIntensity.value = value;
			}
		}

		public static float vibrationIntensity
		{
			get
			{
				return instance._vibrationIntensity.value;
			}
			set
			{
				instance._vibrationIntensity.value = value;
			}
		}

		public static int particleQuality
		{
			get
			{
				return instance._particleQuality.value;
			}
			set
			{
				instance._particleQuality.value = value;
			}
		}

		public static bool easyMode
		{
			get
			{
				return instance._easyMode.value;
			}
			set
			{
				instance._easyMode.value = value;
			}
		}

		public static bool showTimer
		{
			get
			{
				return instance._showTimer.value;
			}
			set
			{
				instance._showTimer.value = value;
			}
		}

		public static void Save()
		{
			instance._keyBindings.Save();
			instance._arrowDashEnabled.Save();
			instance._lightEnabled.Save();
			instance._masterVolume.Save();
			instance._musicEnabled.Save();
			instance._musicVolume.Save();
			instance._sfxEnabled.Save();
			instance._sfxVolume.Save();
			instance._language.Save();
			instance._cameraShakeIntensity.Save();
			instance._vibrationIntensity.Save();
			instance._particleQuality.Save();
			instance._easyMode.Save();
			instance._showTimer.Save();
			PlayerPrefs.Save();
		}

		public void Initialize()
		{
			_keyBindings = new StringData("Settings/keyBindings", string.Empty, isRealtime: true);
			_arrowDashEnabled = new BoolData("Settings/arrowDashEnabled", defaultValue: false, isRealtime: false);
			bool defaultValue = Application.isConsolePlatform || SystemInfo.systemMemorySize >= 4000 || SystemInfo.graphicsMemorySize >= 1000;
			_lightEnabled = new BoolData("Settings/lightEnabled", defaultValue, isRealtime: false);
			Light2D.lightEnabled = _lightEnabled.value;
			_masterVolume = new FloatData("Settings/masterVolume", 0.8f);
			_musicEnabled = new BoolData("Settings/musicEnabled", defaultValue: true, isRealtime: false);
			_musicVolume = new FloatData("Settings/musicVolume", 0.6f);
			_sfxEnabled = new BoolData("Settings/sfxEnabled", defaultValue: true, isRealtime: false);
			_sfxVolume = new FloatData("Settings/sfxVolume", 0.8f);
			_language = new IntData("Settings/language", -1);
			Localization.ValidateLanguage();
			_cameraShakeIntensity = new FloatData("Settings/cameraShakeIntensity", 0.5f);
			_vibrationIntensity = new FloatData("Settings/vibrationIntensity", 0.5f);
			_particleQuality = new IntData("Settings/particleQuality", 3);
			_easyMode = new BoolData("Settings/easyMode", defaultValue: false, isRealtime: false);
			_showTimer = new BoolData("Settings/showTimer", defaultValue: false, isRealtime: false);
		}

		public void ResetKeyBindings()
		{
			_keyBindings.Reset();
		}

		public void ResetLanguage()
		{
			_language.Reset();
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
			return _version.value;
		}
		set
		{
			_version.value = value;
		}
	}

	public static void Initialize()
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
		_version = new IntData("version", isRealtime: true);
		int value = _version.value;
		_version.value = 9;
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
