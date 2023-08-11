using System;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Controllers;
using Data;
using GameResources;
using InControl;
using Level;
using Platforms;
using Scenes;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI.TestingTool;

public class Panel : Dialogue
{
	public class GearOptionData : OptionData
	{
		public readonly GearReference gearInfo;

		public GearOptionData(GearReference gearInfo, Sprite image)
			: base(gearInfo.name, image)
		{
			this.gearInfo = gearInfo;
		}
	}

	public enum Type
	{
		Main,
		MapList,
		GearList,
		Log,
		DataControl,
		BonusStat
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static UnityAction _003C_003E9__67_9;

		public static UnityAction _003C_003E9__67_10;

		public static UnityAction _003C_003E9__67_11;

		public static UnityAction _003C_003E9__67_12;

		public static UnityAction _003C_003E9__67_13;

		public static UnityAction<bool> _003C_003E9__67_19;

		internal void _003CAwake_003Eb__67_9()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = !Scene<GameBase>.instance.uiManager.headupDisplay.visible;
		}

		internal void _003CAwake_003Eb__67_10()
		{
			GameData.Currency.gold.Earn(10000);
		}

		internal void _003CAwake_003Eb__67_11()
		{
			GameData.Currency.darkQuartz.Earn(1000);
		}

		internal void _003CAwake_003Eb__67_12()
		{
			GameData.Currency.bone.Earn(100);
		}

		internal void _003CAwake_003Eb__67_13()
		{
			GameData.Currency.heartQuartz.Earn(100);
		}

		internal void _003CAwake_003Eb__67_19(bool isOn)
		{
			GameData.HardmodeProgress.hardmode = isOn;
		}
	}

	[SerializeField]
	private TMP_Text _mapName;

	[SerializeField]
	private TMP_Text _version;

	[Space]
	[SerializeField]
	private GameObject _main;

	[SerializeField]
	private GameObject _mapList;

	[SerializeField]
	private GameObject _gearList;

	[SerializeField]
	private Log _log;

	[SerializeField]
	private GameObject _dataControl;

	[SerializeField]
	private GameObject _bonusStatPanel;

	[SerializeField]
	private Button _rerollSkill;

	private EnumArray<Type, GameObject> _panels;

	[SerializeField]
	[Space]
	private Button _openMapList;

	[SerializeField]
	private Button _chapter1;

	[SerializeField]
	private Button _chapter2;

	[SerializeField]
	private Button _chapter3;

	[SerializeField]
	private Button _chapter4;

	[SerializeField]
	private Button _chapter5;

	[SerializeField]
	private Button _nextStage;

	[SerializeField]
	private Button _nextMap;

	[Space]
	[SerializeField]
	private Button _openGearList;

	[SerializeField]
	private Button _hideUI;

	[SerializeField]
	[Space]
	private Button _getGold;

	[SerializeField]
	private Button _getDarkquartz;

	[SerializeField]
	private Button _getBone;

	[SerializeField]
	private Button _getHeartQuartz;

	[SerializeField]
	private Button _awake;

	[Space]
	[SerializeField]
	private Button _right3;

	[SerializeField]
	private Button _damageBuff;

	[SerializeField]
	private Button _noCooldown;

	[SerializeField]
	private Button _hp10k;

	[SerializeField]
	private Button _shield10;

	[SerializeField]
	private Button _testMap;

	[SerializeField]
	private Map _testMapPrefab;

	[Header("하드모드")]
	[SerializeField]
	[Space]
	private Toggle _hardmodeToggle;

	[SerializeField]
	private Slider _hardmodeLevelSlider;

	[SerializeField]
	private TMP_Text _hardmodeLevel;

	[SerializeField]
	private Slider _hardmodeClearedLevelSlider;

	[SerializeField]
	private TMP_Text _hardmodeClearedLevel;

	[Header("타임")]
	[SerializeField]
	private Slider _timeScaleSlider;

	[SerializeField]
	private TMP_Text _timeScaleValue;

	[SerializeField]
	private Button _timeScaleReset;

	[Space]
	[SerializeField]
	private TMP_Text _localNow;

	[SerializeField]
	private TMP_Text _utcNow;

	[SerializeField]
	[Header("밸런싱 테스트 도구")]
	private Toggle _infiniteRevive;

	[SerializeField]
	private TMP_Text _reviveCountText;

	[SerializeField]
	private Toggle _verification;

	[SerializeField]
	private DetailModeHealth _detailModeHealth;

	[SerializeField]
	private GameObject _resistanceValue;

	private int _reviveCount;

	private bool _damageBuffAttached;

	private bool _noCooldownAttached;

	private bool _hp10kAttached;

	private Stat.Values _damageBuffStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, 100.0));

	private Stat.Values _cooldownBuffStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.CooldownSpeed, 100.0));

	private Stat.Values _hp10kStat = new Stat.Values(new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 9900.0));

	public bool canUse => PersistentSingleton<PlatformManager>.Instance.cheatEnabled;

	public override bool closeWithPauseKey => true;

	public void Open(Type type)
	{
		foreach (GameObject panel in _panels)
		{
			panel.SetActive(false);
		}
		_panels[type].SetActive(true);
	}

	public void OpenMain()
	{
		Open(Type.Main);
	}

	public void OpenMapList()
	{
		Open(Type.MapList);
	}

	public void OpenGearList()
	{
		Open(Type.GearList);
	}

	public void OpenLog()
	{
		Open(Type.Log);
	}

	public void OpenDataControl()
	{
		Open(Type.DataControl);
	}

	public void OpenBonusStat()
	{
		Open(Type.BonusStat);
	}

	private void Awake()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected O, but got Unknown
		_panels = new EnumArray<Type, GameObject>((GameObject[])(object)new GameObject[6]
		{
			_main,
			_mapList,
			_gearList,
			((Component)_log).gameObject,
			_dataControl,
			_bonusStatPanel
		});
		_log.StartLog();
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		_ = CommonResource.instance;
		_version.text = "version : " + Application.version;
		((UnityEvent)_awake.onClick).AddListener((UnityAction)delegate
		{
			levelManager.player.playerComponents.inventory.weapon.UpgradeCurrentWeapon();
		});
		((UnityEvent)_testMap.onClick).AddListener((UnityAction)delegate
		{
			levelManager.EnterOutTrack(_testMapPrefab);
		});
		((UnityEvent)_chapter1.onClick).AddListener((UnityAction)delegate
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				levelManager.Load(Chapter.Type.HardmodeChapter1);
			}
			else
			{
				levelManager.Load(Chapter.Type.Chapter1);
			}
		});
		((UnityEvent)_chapter2.onClick).AddListener((UnityAction)delegate
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				levelManager.Load(Chapter.Type.HardmodeChapter2);
			}
			else
			{
				levelManager.Load(Chapter.Type.Chapter2);
			}
		});
		((UnityEvent)_chapter3.onClick).AddListener((UnityAction)delegate
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				levelManager.Load(Chapter.Type.HardmodeChapter3);
			}
			else
			{
				levelManager.Load(Chapter.Type.Chapter3);
			}
		});
		((UnityEvent)_chapter4.onClick).AddListener((UnityAction)delegate
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				levelManager.Load(Chapter.Type.HardmodeChapter4);
			}
			else
			{
				levelManager.Load(Chapter.Type.Chapter4);
			}
		});
		((UnityEvent)_chapter5.onClick).AddListener((UnityAction)delegate
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				levelManager.Load(Chapter.Type.HardmodeChapter5);
			}
			else
			{
				levelManager.Load(Chapter.Type.Chapter5);
			}
		});
		((UnityEvent)_nextStage.onClick).AddListener((UnityAction)delegate
		{
			levelManager.LoadNextStage();
		});
		((UnityEvent)_nextMap.onClick).AddListener((UnityAction)delegate
		{
			levelManager.LoadNextMap();
		});
		ButtonClickedEvent onClick = _hideUI.onClick;
		object obj = _003C_003Ec._003C_003E9__67_9;
		if (obj == null)
		{
			UnityAction val = delegate
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.visible = !Scene<GameBase>.instance.uiManager.headupDisplay.visible;
			};
			_003C_003Ec._003C_003E9__67_9 = val;
			obj = (object)val;
		}
		((UnityEvent)onClick).AddListener((UnityAction)obj);
		ButtonClickedEvent onClick2 = _getGold.onClick;
		object obj2 = _003C_003Ec._003C_003E9__67_10;
		if (obj2 == null)
		{
			UnityAction val2 = delegate
			{
				GameData.Currency.gold.Earn(10000);
			};
			_003C_003Ec._003C_003E9__67_10 = val2;
			obj2 = (object)val2;
		}
		((UnityEvent)onClick2).AddListener((UnityAction)obj2);
		ButtonClickedEvent onClick3 = _getDarkquartz.onClick;
		object obj3 = _003C_003Ec._003C_003E9__67_11;
		if (obj3 == null)
		{
			UnityAction val3 = delegate
			{
				GameData.Currency.darkQuartz.Earn(1000);
			};
			_003C_003Ec._003C_003E9__67_11 = val3;
			obj3 = (object)val3;
		}
		((UnityEvent)onClick3).AddListener((UnityAction)obj3);
		ButtonClickedEvent onClick4 = _getBone.onClick;
		object obj4 = _003C_003Ec._003C_003E9__67_12;
		if (obj4 == null)
		{
			UnityAction val4 = delegate
			{
				GameData.Currency.bone.Earn(100);
			};
			_003C_003Ec._003C_003E9__67_12 = val4;
			obj4 = (object)val4;
		}
		((UnityEvent)onClick4).AddListener((UnityAction)obj4);
		ButtonClickedEvent onClick5 = _getHeartQuartz.onClick;
		object obj5 = _003C_003Ec._003C_003E9__67_13;
		if (obj5 == null)
		{
			UnityAction val5 = delegate
			{
				GameData.Currency.heartQuartz.Earn(100);
			};
			_003C_003Ec._003C_003E9__67_13 = val5;
			obj5 = (object)val5;
		}
		((UnityEvent)onClick5).AddListener((UnityAction)obj5);
		((UnityEvent)_right3.onClick).AddListener((UnityAction)delegate
		{
			for (int i = 0; i < 3; i++)
			{
				((UnityEvent)_damageBuff.onClick).Invoke();
				((UnityEvent)_noCooldown.onClick).Invoke();
				((UnityEvent)_hp10k.onClick).Invoke();
			}
		});
		((UnityEvent)_damageBuff.onClick).AddListener((UnityAction)delegate
		{
			_damageBuffAttached = !_damageBuffAttached;
			if (_damageBuffAttached)
			{
				levelManager.player.stat.AttachValues(_damageBuffStat);
			}
			else
			{
				levelManager.player.stat.DetachValues(_damageBuffStat);
			}
		});
		((UnityEvent)_noCooldown.onClick).AddListener((UnityAction)delegate
		{
			_noCooldownAttached = !_noCooldownAttached;
			if (_noCooldownAttached)
			{
				levelManager.player.stat.AttachValues(_cooldownBuffStat);
			}
			else
			{
				levelManager.player.stat.DetachValues(_cooldownBuffStat);
			}
		});
		((UnityEvent)_hp10k.onClick).AddListener((UnityAction)delegate
		{
			_hp10kAttached = !_hp10kAttached;
			if (_hp10kAttached)
			{
				levelManager.player.stat.AttachValues(_hp10kStat);
				levelManager.player.health.ResetToMaximumHealth();
			}
			else
			{
				levelManager.player.stat.DetachValues(_hp10kStat);
			}
		});
		((UnityEvent)_shield10.onClick).AddListener((UnityAction)delegate
		{
			levelManager.player.health.shield.Add(this, 10f);
		});
		_hardmodeToggle.isOn = GameData.HardmodeProgress.hardmode;
		((UnityEvent<bool>)(object)_hardmodeToggle.onValueChanged).AddListener((UnityAction<bool>)delegate(bool isOn)
		{
			GameData.HardmodeProgress.hardmode = isOn;
		});
		((UnityEvent<float>)(object)_hardmodeLevelSlider.onValueChanged).AddListener((UnityAction<float>)delegate(float value)
		{
			int hardmodeLevel = Mathf.Min(new int[3]
			{
				(int)Mathf.Ceil(value),
				GameData.HardmodeProgress.clearedLevel + 1,
				GameData.HardmodeProgress.maxLevel
			});
			_hardmodeLevel.text = hardmodeLevel.ToString();
			GameData.HardmodeProgress.hardmodeLevel = hardmodeLevel;
		});
		((UnityEvent<float>)(object)_hardmodeClearedLevelSlider.onValueChanged).AddListener((UnityAction<float>)delegate(float value)
		{
			int clearedLevel = Mathf.Min((int)Mathf.Ceil(value), GameData.HardmodeProgress.maxLevel);
			_hardmodeClearedLevel.text = clearedLevel.ToString();
			GameData.HardmodeProgress.clearedLevel = clearedLevel;
		});
		((UnityEvent)_rerollSkill.onClick).AddListener((UnityAction)delegate
		{
			levelManager.player.playerComponents.inventory.weapon.current.RerollSkills();
		});
		((UnityEvent<float>)(object)_timeScaleSlider.onValueChanged).AddListener((UnityAction<float>)delegate(float value)
		{
			((ChronometerBase)Chronometer.global).DetachTimeScale((object)_timeScaleSlider);
			((ChronometerBase)Chronometer.global).AttachTimeScale((object)_timeScaleSlider, value);
			_timeScaleValue.text = $"{value:0.00}";
		});
		((UnityEvent)_timeScaleReset.onClick).AddListener((UnityAction)delegate
		{
			_timeScaleSlider.value = 1f;
		});
		_timeScaleValue.text = ((ChronometerBase)Chronometer.global).timeScale.ToString();
		((UnityEvent<bool>)(object)_infiniteRevive.onValueChanged).AddListener((UnityAction<bool>)delegate(bool isOn)
		{
			if (isOn)
			{
				levelManager.player.health.onDie += Revive;
				levelManager.player.health.onDie -= InitReviveCount;
				_reviveCountText.text = $"부활횟수 ({_reviveCount})";
			}
			else
			{
				levelManager.player.health.onDie -= Revive;
				levelManager.player.health.onDie += InitReviveCount;
				_reviveCountText.text = "무한부활";
			}
		});
		((UnityEvent<bool>)(object)_verification.onValueChanged).AddListener((UnityAction<bool>)delegate(bool isOn)
		{
			_resistanceValue.gameObject.SetActive(isOn);
			if (isOn)
			{
				levelManager.onMapChangedAndFadedIn += AttachHealthNumber;
			}
			else
			{
				levelManager.onMapChangedAndFadedIn -= AttachHealthNumber;
			}
		});
	}

	private void AttachHealthNumber(Map old, Map @new)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		foreach (Character allEnemy in Map.Instance.waveContainer.GetAllEnemies())
		{
			DetailModeHealth detailModeHealth = Object.Instantiate<DetailModeHealth>(_detailModeHealth, allEnemy.attach.transform);
			Transform transform = ((Component)detailModeHealth).transform;
			Bounds bounds = ((Collider2D)allEnemy.collider).bounds;
			float x = ((Bounds)(ref bounds)).center.x;
			bounds = ((Collider2D)allEnemy.collider).bounds;
			transform.position = Vector2.op_Implicit(new Vector2(x, ((Bounds)(ref bounds)).max.y + 2f));
			detailModeHealth.Initialize(allEnemy);
		}
	}

	private void Revive()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		player.health.Revive(player.health.maximumHealth * 0.6000000238418579);
		_reviveCount++;
		_reviveCountText.text = $"부활횟수 ({_reviveCount})";
	}

	private void InitReviveCount()
	{
		_reviveCount = 0;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		PlayerInput.blocked.Attach((object)this);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0f);
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		_mapName.text = string.Format("Map : {0}/{1}/{2}\nSeed : {3}", levelManager.currentChapter.type, ((Object)levelManager.currentChapter.currentStage).name, ((Object)Map.Instance).name.Replace(" (Clone)", ""), GameData.Save.instance.randomSeed);
		_localNow.text = $"Local now : {DateTime.Now}";
		_utcNow.text = $"Utc now : {DateTime.UtcNow}";
		_hardmodeLevelSlider.value = GameData.HardmodeProgress.hardmodeLevel;
		_hardmodeLevel.text = GameData.HardmodeProgress.hardmodeLevel.ToString();
		_hardmodeClearedLevelSlider.value = GameData.HardmodeProgress.clearedLevel;
		_hardmodeClearedLevel.text = GameData.HardmodeProgress.clearedLevel.ToString();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		PlayerInput.blocked.Detach((object)this);
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
	}

	protected override void Update()
	{
		base.Update();
		if (!_panels[Type.Main].activeSelf && ((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed)
		{
			OpenMain();
		}
	}
}
