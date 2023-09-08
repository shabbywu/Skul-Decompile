using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameResources;
using InControl;
using Level;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool;

public class MapList : MonoBehaviour
{
	[SerializeField]
	private MapListElement _mapListElementPrefab;

	[SerializeField]
	private Button _back;

	[SerializeField]
	private TMP_Text _currentChapterFilterText;

	[SerializeField]
	[Header("옵션")]
	private Toggle _enemy;

	[SerializeField]
	private Toggle _fieldNPC;

	[SerializeField]
	private Toggle _darkEnemy;

	[Header("노말 모드")]
	[SerializeField]
	private Button _tutorial;

	[SerializeField]
	private Button _castle;

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
	[Header("하드 모드")]
	private Button _hardmodeCastle;

	[SerializeField]
	private Button _hardChapter1;

	[SerializeField]
	private Button _hardChapter2;

	[SerializeField]
	private Button _hardChapter3;

	[SerializeField]
	private Button _hardChapter4;

	[SerializeField]
	private Button _hardChapter5;

	[SerializeField]
	private Transform _gridContainer;

	private readonly List<MapListElement> _mapListElements = new List<MapListElement>();

	private Chapter.Type _currentChapterType;

	private EnumArray<Chapter.Type, bool> _chpaterLoaded = new EnumArray<Chapter.Type, bool>();

	private void Awake()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		_currentChapterType = Singleton<Service>.Instance.levelManager.currentChapter.type;
		LoadMaps(_currentChapterType);
		((UnityEvent<bool>)(object)_enemy.onValueChanged).AddListener((UnityAction<bool>)delegate(bool value)
		{
			Map.TestingTool.safeZone = !value;
		});
		((UnityEvent<bool>)(object)_fieldNPC.onValueChanged).AddListener((UnityAction<bool>)delegate(bool value)
		{
			Map.TestingTool.fieldNPC = value;
		});
		((UnityEvent<bool>)(object)_darkEnemy.onValueChanged).AddListener((UnityAction<bool>)delegate(bool value)
		{
			Map.TestingTool.darkenemy = value;
		});
		((UnityEvent)_tutorial.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Tutorial);
		});
		((UnityEvent)_castle.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Castle);
		});
		((UnityEvent)_chapter1.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Chapter1);
		});
		((UnityEvent)_chapter2.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Chapter2);
		});
		((UnityEvent)_chapter3.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Chapter3);
		});
		((UnityEvent)_chapter4.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Chapter4);
		});
		((UnityEvent)_chapter5.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.Chapter5);
		});
		((UnityEvent)_hardmodeCastle.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.HardmodeCastle);
		});
		((UnityEvent)_hardChapter1.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.HardmodeChapter1);
		});
		((UnityEvent)_hardChapter2.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.HardmodeChapter2);
		});
		((UnityEvent)_hardChapter3.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.HardmodeChapter3);
		});
		((UnityEvent)_hardChapter4.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.HardmodeChapter4);
		});
		((UnityEvent)_hardChapter5.onClick).AddListener((UnityAction)delegate
		{
			FilterMapList(Chapter.Type.HardmodeChapter5);
		});
	}

	private void LoadMaps(Chapter.Type chapterType)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		if (_chpaterLoaded[chapterType])
		{
			return;
		}
		_chpaterLoaded[chapterType] = true;
		StringBuilder stringBuilder = new StringBuilder();
		int substringIndex = "Assets/Level/".Length;
		int prefabExtensionLength = ".prefab".Length;
		AssetReference val = LevelResource.instance.chapters[(int)chapterType];
		bool flag = false;
		Chapter chapter;
		if (val.IsValid())
		{
			chapter = (Chapter)(object)val.Asset;
		}
		else
		{
			flag = true;
			chapter = val.LoadAssetAsync<Chapter>().WaitForCompletion();
		}
		AssetReference[] stages = chapter.stages;
		foreach (AssetReference val2 in stages)
		{
			bool flag2 = false;
			IStageInfo stageInfo;
			if (val2.IsValid())
			{
				stageInfo = (IStageInfo)(object)val2.Asset;
			}
			else
			{
				flag2 = true;
				stageInfo = val2.LoadAssetAsync<IStageInfo>().WaitForCompletion();
			}
			MapReference[] maps = stageInfo.maps;
			foreach (MapReference map2 in maps)
			{
				AddToMapList(map2);
			}
			if (stageInfo is StageInfo stageInfo2)
			{
				AddToMapList(stageInfo2.entry.reference);
				AddToMapList(stageInfo2.terminal.reference);
			}
			if (flag2)
			{
				Addressables.Release<IStageInfo>(stageInfo);
			}
		}
		if (flag)
		{
			Addressables.Release<Chapter>(chapter);
		}
		void AddToMapList(MapReference map)
		{
			if (!map.IsNullOrEmpty())
			{
				MapListElement mapListElement = Object.Instantiate<MapListElement>(_mapListElementPrefab, _gridContainer);
				stringBuilder.Clear();
				stringBuilder.Append(map.path);
				int num = map.path.IndexOf('/', substringIndex) + 1;
				mapListElement.Set(chapterType, stringBuilder.ToString(num, stringBuilder.Length - num - prefabExtensionLength), map);
				_mapListElements.Add(mapListElement);
			}
		}
	}

	private void Update()
	{
		InputDevice activeDevice = InputManager.ActiveDevice;
		if (((OneAxisInputControl)activeDevice.LeftBumper).WasPressed || ((OneAxisInputControl)activeDevice.LeftTrigger).WasPressed)
		{
			if (_currentChapterType == Chapter.Type.Castle)
			{
				FilterMapList(EnumValues<Chapter.Type>.Values.Last());
			}
			else
			{
				FilterMapList(_currentChapterType - 1);
			}
		}
		else if (((OneAxisInputControl)activeDevice.RightBumper).WasPressed || ((OneAxisInputControl)activeDevice.RightTrigger).WasPressed)
		{
			if (_currentChapterType == EnumValues<Chapter.Type>.Values.Last())
			{
				FilterMapList(EnumValues<Chapter.Type>.Values.First());
			}
			else
			{
				FilterMapList(_currentChapterType + 1);
			}
		}
	}

	private void FilterMapList(Chapter.Type chapter)
	{
		LoadMaps(chapter);
		_currentChapterType = chapter;
		_currentChapterFilterText.text = chapter.ToString();
		foreach (MapListElement mapListElement in _mapListElements)
		{
			((Component)mapListElement).gameObject.SetActive(mapListElement.chapter == chapter);
		}
	}
}
