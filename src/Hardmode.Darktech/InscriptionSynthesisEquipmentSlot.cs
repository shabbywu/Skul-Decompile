using System;
using System.Collections.Generic;
using Characters;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Operations;
using Characters.Operations.Fx;
using Characters.Player;
using Data;
using GameResources;
using InControl;
using Level;
using Platforms;
using Services;
using Singletons;
using TMPro;
using UnityEditor;
using UnityEngine;
using UserInput;

namespace Hardmode.Darktech;

public sealed class InscriptionSynthesisEquipmentSlot : InteractiveObject
{
	private static readonly int _idleHash = Animator.StringToHash("Activate");

	private static readonly int _upHash = Animator.StringToHash("Up");

	private static readonly int _downHash = Animator.StringToHash("Down");

	private static readonly int _fixHash = Animator.StringToHash("Fix");

	private static readonly int _fixLoopHash = Animator.StringToHash("Fix_Loop");

	[SerializeField]
	private int _amount;

	[SerializeField]
	private SpriteRenderer _display;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private GameObject _selectGuide;

	[SerializeField]
	private TMP_Text _selectGuideText;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onSelect;

	[Subcomponent(typeof(PlaySound))]
	[SerializeField]
	private PlaySound _moveSound;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _selectSound;

	private int _currentIndex = -1;

	private Inscription.Key? _selected;

	private bool _selecting;

	private List<Inscription.Key> _keys;

	private Comparison<Inscription.Key> _keyComparison;

	private int _slotIndex;

	private InscriptionSynthesisEquipment _machine;

	private int _price;

	private GameData.Currency _selectCurrency = GameData.Currency.gold;

	private CharacterInteraction _interactionCache;

	private Inventory _inventoryCache;

	private const int nullIndex = -1;

	public Inscription.Key? selected => _selected;

	public string changeInteraction => Localization.GetLocalizedString("label/interaction/changeInscription");

	public string fixInscription => Localization.GetLocalizedString("label/interaction/fixInscription");

	protected override void Awake()
	{
		base.Awake();
		_keys = new List<Inscription.Key>(Inscription.keys.Count);
		foreach (Inscription.Key key in Inscription.keys)
		{
			if (key != 0)
			{
				_keys.Add(key);
			}
		}
		_keyComparison = delegate(Inscription.Key key1, Inscription.Key key2)
		{
			Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
			if (synergy.inscriptions[key1].isMaxStep)
			{
				if (synergy.inscriptions[key2].isMaxStep)
				{
					return 0;
				}
				return -2;
			}
			if (synergy.inscriptions[key2].isMaxStep)
			{
				return 2;
			}
			if (synergy.inscriptions[key1].step > synergy.inscriptions[key2].step)
			{
				return -1;
			}
			if (synergy.inscriptions[key1].step < synergy.inscriptions[key2].step)
			{
				return 1;
			}
			if (synergy.inscriptions[key1].count > synergy.inscriptions[key2].count)
			{
				return -1;
			}
			return (synergy.inscriptions[key1].count < synergy.inscriptions[key2].count) ? 1 : 0;
		};
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_price = Singleton<DarktechManager>.Instance.setting.GetInscriptionBonusCostByStage(currentChapter.type, currentChapter.stageIndex);
	}

	private void Start()
	{
		Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.onChanged += UpdateDisplayToSelected;
	}

	private void OnDestroy()
	{
		if (!((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
		{
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.onChanged -= UpdateDisplayToSelected;
		}
	}

	private void Update()
	{
		if (!base.activated)
		{
			return;
		}
		UpdateSelectingState();
		if (_selecting)
		{
			if (((OneAxisInputControl)KeyMapper.Map.Up).WasPressed)
			{
				_animator.Play(_upHash);
				_moveSound.Run(Singleton<Service>.Instance.levelManager.player);
				MoveFocus(1);
			}
			else if (((OneAxisInputControl)KeyMapper.Map.Down).WasPressed)
			{
				_animator.Play(_downHash);
				_moveSound.Run(Singleton<Service>.Instance.levelManager.player);
				MoveFocus(-1);
			}
		}
	}

	private void UpdateSelectingState()
	{
		if (_selecting && !_interactionCache.IsInteracting(this))
		{
			if (_selected.HasValue)
			{
				_animator.Play(_fixLoopHash);
			}
			_selecting = false;
			UpdateCurrentIndex();
			UpdateDisplay();
		}
		if (!_selecting && _interactionCache.IsInteracting(this))
		{
			_selecting = true;
			UpdateCurrentIndex();
			UpdateDisplayToSelected();
		}
	}

	public void Initialize(InscriptionSynthesisEquipment machine, int slotIndex)
	{
		_machine = machine;
		_slotIndex = slotIndex;
		_onSelect.Initialize();
		Character player = Singleton<Service>.Instance.levelManager.player;
		_interactionCache = ((Component)player).GetComponent<CharacterInteraction>();
		_inventoryCache = player.playerComponents.inventory;
		if (((Data)GameData.HardmodeProgress.inscriptionSynthesisEquipment[_slotIndex]).isDefaultValue)
		{
			UpdateDisplayToSelected();
			return;
		}
		LoadSelectedData();
		UpdateDisplayToSelected();
	}

	public override void InteractWith(Character character)
	{
		if (!IsSelectable())
		{
			_animator.Play(_idleHash);
			return;
		}
		_selectCurrency.Consume(_price);
		SelectCurrentKey();
		_animator.Play(_fixHash);
		for (int i = 0; i < GameData.HardmodeProgress.InscriptionSynthesisEquipment.count && ((Data<int>)(object)GameData.HardmodeProgress.inscriptionSynthesisEquipment[i]).value != -1; i++)
		{
			if (i == GameData.HardmodeProgress.InscriptionSynthesisEquipment.count - 1)
			{
				ExtensionMethods.Set((Type)71);
			}
		}
	}

	public void Select(Inscription.Key key)
	{
		if (_selected.HasValue)
		{
			_inventoryCache.synergy.inscriptions[_selected.Value].bonusCount -= _amount;
		}
		_selected = key;
		_inventoryCache.synergy.inscriptions[_selected.Value].bonusCount += _amount;
		_inventoryCache.UpdateSynergy();
		SaveSelectedData();
		UpdateDisplayToSelected();
	}

	private void UpdateSelectGuide()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (_currentIndex == -1)
		{
			_selectGuide.SetActive(false);
			return;
		}
		Inscription.Key key = _keys[_currentIndex];
		if (_selected.HasValue && key == _selected.Value)
		{
			_selectGuide.SetActive(false);
			return;
		}
		string text = ColorUtility.ToHtmlStringRGB(_selectCurrency.Has(_price) ? Color.yellow : Color.red);
		_selectGuideText.text = $"{Inscription.GetName(key)} {fixInscription}(<color=#{text}>{_price}</color>)";
		_selectGuide.SetActive(true);
	}

	private void UpdateDisplay()
	{
		UpdateSelectGuide();
		if (_currentIndex == -1)
		{
			_display.sprite = null;
			return;
		}
		Inscription.Key key = _keys[_currentIndex];
		Inscription inscription = _inventoryCache.synergy.inscriptions[key];
		int num = 0;
		_ = inscription.step;
		_display.sprite = inscription.icon;
	}

	public override void OpenPopupBy(Character character)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		base.OpenPopupBy(character);
		_selecting = true;
		string arg = ColorUtility.ToHtmlStringRGB(_selectCurrency.Has(_price) ? Color.yellow : Color.red);
		_selectGuideText.text = $"{fixInscription}(<color=#{arg}>{_price}</color>)";
	}

	private bool IsSelectable()
	{
		if (_currentIndex == -1)
		{
			return false;
		}
		Inscription.Key key = _keys[_currentIndex];
		if (_inventoryCache.synergy.inscriptions[key].count < 1)
		{
			return false;
		}
		if (!_selectCurrency.Has(_price))
		{
			return false;
		}
		if (_selected.HasValue && _selected.Value == key)
		{
			return false;
		}
		return true;
	}

	private void SelectCurrentKey()
	{
		((MonoBehaviour)this).StartCoroutine(_onSelect.CRun(Singleton<Service>.Instance.levelManager.player));
		_selectSound.Run(Singleton<Service>.Instance.levelManager.player);
		Select(_keys[_currentIndex]);
	}

	private void MoveFocus(int moveAmount)
	{
		int num = 0;
		int currentIndex = _currentIndex;
		Inscription.Key key;
		do
		{
			int num2 = _currentIndex + moveAmount;
			if (num2 < 0)
			{
				num2 += _keys.Count;
			}
			_currentIndex = num2 % _keys.Count;
			key = _keys[_currentIndex];
			num++;
			if (num >= _keys.Count)
			{
				_currentIndex = currentIndex;
				break;
			}
		}
		while (_inventoryCache.synergy.inscriptions[key].count < 1 || !_machine.IsSelectable(this, key) || (_selected.HasValue && _selected.Value == key));
		if (num < _keys.Count)
		{
			UpdateDisplay();
		}
	}

	private void UpdateDisplayToSelected()
	{
		UpdateSelectGuide();
		if (!_selected.HasValue)
		{
			_display.sprite = null;
			return;
		}
		Inscription.Key value = _selected.Value;
		Inscription inscription = _inventoryCache.synergy.inscriptions[value];
		_display.sprite = inscription.icon;
	}

	private void UpdateCurrentIndex()
	{
		_keys.Sort(_keyComparison);
		_currentIndex = -1;
		if (!_selected.HasValue)
		{
			return;
		}
		for (int i = 0; i < _keys.Count; i++)
		{
			if (_keys[i] == _selected.Value)
			{
				_currentIndex = i;
			}
		}
	}

	private void SaveSelectedData()
	{
		((Data<int>)(object)GameData.HardmodeProgress.inscriptionSynthesisEquipment[_slotIndex]).value = (int)_selected.Value;
	}

	private void LoadSelectedData()
	{
		int num = 0;
		foreach (Inscription.Key key in Inscription.keys)
		{
			if (num == ((Data<int>)(object)GameData.HardmodeProgress.inscriptionSynthesisEquipment[_slotIndex]).value)
			{
				_selected = key;
				_animator.Play(_fixLoopHash);
				break;
			}
			num++;
		}
	}
}
