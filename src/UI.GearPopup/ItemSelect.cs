using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using FX;
using InControl;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInput;

namespace UI.GearPopup;

public class ItemSelect : Dialogue
{
	[SerializeField]
	private RectTransform _canvas;

	[Space]
	[SerializeField]
	private GameObject _moreinscriptionFrame;

	[SerializeField]
	private KeywordElement[] _keywordElements;

	[SerializeField]
	[Space]
	private RectTransform _fieldGearContainer;

	[SerializeField]
	private GearPopupForItemSelection _fieldGearPopup;

	[SerializeField]
	[Space]
	private RectTransform _inventoryGearContainer;

	[SerializeField]
	private GearPopupForItemSelection _inventoryGearPopup;

	[Space]
	[SerializeField]
	private ItemSelectNavigation _navigation;

	[SerializeField]
	[Header("Sound")]
	private SoundInfo _openSound;

	[SerializeField]
	private SoundInfo _closeSound;

	[SerializeField]
	private SoundInfo _selectSound;

	private Item _fieldItem;

	private Character _player;

	private ItemInventory _itemInventory;

	private Synergy _synergy;

	private PlayerInput _playerInput;

	public override bool closeWithPauseKey => true;

	private void Awake()
	{
		_navigation.onItemSelected += OnItemSelected;
		Selectable[] componentsInChildren = ((Component)this).GetComponentsInChildren<Selectable>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Component)componentsInChildren[i]).gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = _selectSound;
		}
	}

	private new void Update()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		base.Update();
		UpdateContainerSizeAndPosition(_fieldGearContainer);
		UpdateContainerSizeAndPosition(_inventoryGearContainer);
		FixFocus();
		if (((OneAxisInputControl)KeyMapper.Map.Interaction).WasPressed || ((int)KeyMapper.Map.SimplifiedLastInputType == 2 && ((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed))
		{
			_itemInventory.Drop(_navigation.selectedItemIndex);
			_fieldItem.dropped.InteractWith(_player);
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void FixFocus()
	{
		if (base.focused)
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (!((Object)(object)currentSelectedGameObject != (Object)null) || !((Object)(object)currentSelectedGameObject.GetComponent<ItemSelectElement>() != (Object)null))
			{
				ItemSelectElement component = ((Component)_defaultFocus).GetComponent<ItemSelectElement>();
				EventSystem.current.SetSelectedGameObject(((Component)component).gameObject);
				component.onSelected?.Invoke();
			}
		}
	}

	protected override void OnEnable()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		PersistentSingleton<SoundManager>.Instance.PlaySound(_openSound, Vector3.zero);
		PlayerInput.blocked.Attach((object)this);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0f);
	}

	protected override void OnDisable()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		base.OnDisable();
		PersistentSingleton<SoundManager>.Instance.PlaySound(_closeSound, Vector3.zero);
		PlayerInput.blocked.Detach((object)this);
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
	}

	private void OnItemSelected(int index)
	{
		Item item = _itemInventory.items[index];
		SetInventoryGear(item);
		EnumArray<Inscription.Key, int> delta = new EnumArray<Inscription.Key, int>();
		EnumArray<Inscription.Key, int> val = new EnumArray<Inscription.Key, int>();
		Inscription.Key keyword;
		int num;
		foreach (Item item2 in _itemInventory.items)
		{
			if (!((Object)(object)item2 == (Object)null) && !((Object)(object)item2 == (Object)(object)item))
			{
				keyword = item2.keyword1;
				num = val[keyword];
				val[keyword] = num + 1;
				keyword = item2.keyword2;
				num = val[keyword];
				val[keyword] = num + 1;
			}
		}
		keyword = _fieldItem.keyword1;
		num = val[keyword];
		val[keyword] = num + 1;
		keyword = _fieldItem.keyword2;
		num = val[keyword];
		val[keyword] = num + 1;
		foreach (Item item3 in _itemInventory.items)
		{
			if (!((Object)(object)item3 == (Object)null) && !((Object)(object)item3 == (Object)(object)item))
			{
				item3.EvaluateBonusKeyword(val);
			}
		}
		_fieldItem.EvaluateBonusKeyword(val);
		foreach (Inscription.Key key in Inscription.keys)
		{
			delta[key] = val[key] + _synergy.inscriptions[key].bonusCount - _synergy.inscriptions[key].count;
		}
		KeyValuePair<Inscription.Key, int>[] array = (from pair in val.ToKeyValuePairs()
			where pair.Value > 0 || delta[pair.Key] != 0
			select pair).OrderByDescending(delegate(KeyValuePair<Inscription.Key, int> keywordCount)
		{
			ReadOnlyCollection<int> steps = _synergy.inscriptions[keywordCount.Key].steps;
			if (keywordCount.Value == steps[steps.Count - 1])
			{
				return 2;
			}
			return (keywordCount.Value >= steps[1]) ? 1 : 0;
		}).ThenByDescending((KeyValuePair<Inscription.Key, int> keywordCount) => keywordCount.Value).ToArray();
		KeywordElement[] keywordElements = _keywordElements;
		for (num = 0; num < keywordElements.Length; num++)
		{
			((Component)keywordElements[num]).gameObject.SetActive(false);
		}
		int num2 = 0;
		if (array.Length <= 12)
		{
			_moreinscriptionFrame.SetActive(false);
			num2 = 12;
		}
		else
		{
			_moreinscriptionFrame.SetActive(true);
		}
		int num3 = Math.Min(array.Length, _keywordElements.Length);
		for (int i = 0; i < num3; i++)
		{
			((Component)_keywordElements[i + num2]).gameObject.SetActive(true);
			_keywordElements[i + num2].Set(array[i].Key, delta[array[i].Key]);
		}
	}

	private void UpdateContainerSizeAndPosition(RectTransform container)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = container.sizeDelta / 2f;
		val.x *= ((Transform)container).lossyScale.x;
		val.y *= ((Transform)container).lossyScale.y;
		float num = _canvas.sizeDelta.x * ((Transform)_canvas).localScale.x;
		Vector3 position = ((Transform)container).position;
		position.x = Mathf.Clamp(position.x, val.x, num - val.x);
		((Transform)container).position = position;
	}

	public void Open(Item fieldItem)
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		_itemInventory = _player.playerComponents.inventory.item;
		_synergy = _player.playerComponents.inventory.synergy;
		_playerInput = ((Component)_player).GetComponent<PlayerInput>();
		_fieldItem = fieldItem;
		_fieldGearPopup.Set(fieldItem);
		Open();
	}

	public void SetInventoryGear(Item item)
	{
		_inventoryGearPopup.Set(item);
	}
}
