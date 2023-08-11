using System;
using System.Linq;
using Characters;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using FX;
using InControl;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UserInput;

namespace UI.Inventory;

public class Panel : Dialogue
{
	[Header("하드모드")]
	[SerializeField]
	private Sprite _normalmodeScrollSprite;

	[SerializeField]
	private Sprite _hardmodeScrollSprite;

	[SerializeField]
	private Image _scroll;

	[SerializeField]
	private GameObject _harmodeObjects;

	[Space]
	[SerializeField]
	private GearOption _gearOption;

	[SerializeField]
	private Image _focus;

	[Space]
	[SerializeField]
	private KeywordDisplay _keywordDisplay;

	[Space]
	[FormerlySerializedAs("_itemDiscardButton")]
	[SerializeField]
	private PressingButton _gearDiscardButton;

	[Space]
	[SerializeField]
	private GearElement[] _weapons;

	[SerializeField]
	private GearElement[] _quintessences;

	[SerializeField]
	private GearElement[] _items;

	[SerializeField]
	private GearElement[] _upgrades;

	[SerializeField]
	[Space]
	private SoundInfo _openSound;

	[SerializeField]
	private SoundInfo _closeSound;

	[SerializeField]
	private SoundInfo _selectSound;

	private Action _swapSkill;

	private Action _discardGear;

	public override bool closeWithPauseKey => true;

	private void Awake()
	{
		EventSystem.current.sendNavigationEvents = true;
		Selectable[] componentsInChildren = ((Component)this).GetComponentsInChildren<Selectable>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Component)componentsInChildren[i]).gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = _selectSound;
		}
		_gearDiscardButton.onPressed += delegate
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, ((Component)this).transform.position);
			_discardGear?.Invoke();
		};
	}

	protected override void OnEnable()
	{
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		Navigation navigation;
		if (GameData.HardmodeProgress.hardmode)
		{
			_scroll.sprite = _hardmodeScrollSprite;
			_harmodeObjects.gameObject.SetActive(true);
			GearElement obj = _items[6];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_upgrades[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[3];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_items[8];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_items[7];
			((Selectable)obj).navigation = navigation;
			GearElement obj2 = _items[7];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_upgrades[1];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[4];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_items[6];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_items[8];
			((Selectable)obj2).navigation = navigation;
			GearElement obj3 = _items[8];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_upgrades[2];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[5];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_items[7];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_items[6];
			((Selectable)obj3).navigation = navigation;
			GearElement obj4 = _weapons[0];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_quintessences[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_upgrades[0];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_weapons[1];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_weapons[1];
			((Selectable)obj4).navigation = navigation;
			GearElement obj5 = _weapons[1];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_quintessences[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_upgrades[1];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_weapons[0];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_weapons[0];
			((Selectable)obj5).navigation = navigation;
		}
		else
		{
			_scroll.sprite = _normalmodeScrollSprite;
			_harmodeObjects.gameObject.SetActive(false);
			GearElement obj6 = _items[6];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_weapons[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[3];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_items[8];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_items[7];
			((Selectable)obj6).navigation = navigation;
			GearElement obj7 = _items[7];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_weapons[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[4];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_items[6];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_items[8];
			((Selectable)obj7).navigation = navigation;
			GearElement obj8 = _items[8];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_weapons[1];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[5];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_items[7];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_items[6];
			((Selectable)obj8).navigation = navigation;
			GearElement obj9 = _weapons[0];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_quintessences[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[6];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_weapons[1];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_weapons[1];
			((Selectable)obj9).navigation = navigation;
			GearElement obj10 = _weapons[1];
			navigation = default(Navigation);
			((Navigation)(ref navigation)).mode = (Mode)4;
			((Navigation)(ref navigation)).selectOnDown = (Selectable)(object)_quintessences[0];
			((Navigation)(ref navigation)).selectOnUp = (Selectable)(object)_items[7];
			((Navigation)(ref navigation)).selectOnLeft = (Selectable)(object)_weapons[0];
			((Navigation)(ref navigation)).selectOnRight = (Selectable)(object)_weapons[0];
			((Selectable)obj10).navigation = navigation;
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_openSound, Vector3.zero);
		PlayerInput.blocked.Attach((object)this);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0f);
		_ = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
		UpdateGearInfo();
	}

	private void ClearOption()
	{
		_gearOption.Clear();
		_discardGear = null;
	}

	private void UpdateGearInfo()
	{
		_keywordDisplay.UpdateElements();
		Character player = Singleton<Service>.Instance.levelManager.player;
		WeaponInventory weapon2 = player.playerComponents.inventory.weapon;
		QuintessenceInventory quintessenceInventory = player.playerComponents.inventory.quintessence;
		UpgradeInventory upgrade2 = player.playerComponents.inventory.upgrade;
		ItemInventory itemInventory = player.playerComponents.inventory.item;
		for (int i = 0; i < _weapons.Length; i++)
		{
			Weapon weapon;
			if (i == 0)
			{
				weapon = weapon2.polymorphOrCurrent;
			}
			else
			{
				weapon = weapon2.next;
			}
			if ((Object)(object)weapon != (Object)null)
			{
				Sprite icon = weapon.icon;
				_weapons[i].SetIcon(icon);
				_weapons[i].onSelected = delegate
				{
					((Component)_gearOption).gameObject.SetActive(true);
					_gearOption.Set(weapon);
					_discardGear = null;
					_swapSkill = delegate
					{
						weapon.SwapSkillOrder();
						_gearOption.Set(weapon);
					};
				};
			}
			else
			{
				_weapons[i].Deactivate();
				_weapons[i].onSelected = ClearOption;
			}
		}
		for (int j = 0; j < _quintessences.Length; j++)
		{
			Quintessence quintessence = quintessenceInventory.items[j];
			if ((Object)(object)quintessence != (Object)null)
			{
				Sprite icon2 = quintessence.icon;
				_quintessences[j].SetIcon(icon2);
				int cachedIndex2 = j;
				_quintessences[j].onSelected = delegate
				{
					((Component)_gearOption).gameObject.SetActive(true);
					_gearOption.Set(quintessence);
					_discardGear = delegate
					{
						if (quintessenceInventory.Discard(cachedIndex2))
						{
							_quintessences[cachedIndex2].Deactivate();
							UpdateGearInfo();
							_quintessences[cachedIndex2].onSelected();
						}
					};
					_swapSkill = null;
				};
			}
			else
			{
				_quintessences[j].Deactivate();
				_quintessences[j].onSelected = ClearOption;
			}
		}
		string[] itemKeys = (from item in itemInventory.items
			where (Object)(object)item != (Object)null
			select ((Object)item).name).ToArray();
		for (int k = 0; k < _items.Length; k++)
		{
			Item item2 = itemInventory.items[k];
			GearElement gearElement = _items[k];
			if ((Object)(object)item2 != (Object)null)
			{
				Sprite icon3 = item2.icon;
				gearElement.SetIcon(icon3);
				if (item2.setItemKeys.Any((string setKey) => itemKeys.Contains<string>(setKey, StringComparer.OrdinalIgnoreCase)))
				{
					if ((Object)(object)item2.setItemImage != (Object)null)
					{
						gearElement.SetSetImage(item2.setItemImage);
					}
					if ((Object)(object)item2.setItemAnimator != (Object)null)
					{
						gearElement.SetSetAnimator(item2.setItemAnimator);
					}
				}
				else
				{
					gearElement.DisableSetEffect();
				}
				int cachedIndex = k;
				gearElement.onSelected = delegate
				{
					((Component)_gearOption).gameObject.SetActive(true);
					_gearOption.Set(item2);
					_discardGear = delegate
					{
						if (itemInventory.Discard(cachedIndex))
						{
							itemInventory.Trim();
							_items[cachedIndex].Deactivate();
							UpdateGearInfo();
							_items[cachedIndex].onSelected();
						}
					};
					_swapSkill = null;
				};
			}
			else
			{
				gearElement.Deactivate();
				gearElement.onSelected = ClearOption;
			}
		}
		for (int l = 0; l < _upgrades.Length; l++)
		{
			UpgradeObject upgrade = upgrade2.upgrades[l];
			if ((Object)(object)upgrade != (Object)null)
			{
				Sprite icon4 = upgrade.icon;
				_upgrades[l].SetIcon(icon4);
				_upgrades[l].onSelected = delegate
				{
					((Component)_gearOption).gameObject.SetActive(true);
					_gearOption.Set(upgrade);
					_discardGear = null;
					_swapSkill = null;
				};
			}
			else
			{
				_upgrades[l].Deactivate();
				_upgrades[l].onSelected = ClearOption;
			}
		}
	}

	protected override void OnDisable()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		base.OnDisable();
		PersistentSingleton<SoundManager>.Instance.PlaySound(_closeSound, Vector3.zero);
		PlayerInput.blocked.Detach((object)this);
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
	}

	protected override void Update()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		base.Update();
		if (!base.focused)
		{
			return;
		}
		if (((OneAxisInputControl)KeyMapper.Map.Inventory).WasPressed)
		{
			Close();
			return;
		}
		if (((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed)
		{
			Close();
			return;
		}
		if (((OneAxisInputControl)KeyMapper.Map.UiInteraction1).WasPressed && _discardGear == null)
		{
			_swapSkill?.Invoke();
		}
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (!((Object)(object)currentSelectedGameObject == (Object)null))
		{
			((Transform)((Graphic)_focus).rectTransform).position = currentSelectedGameObject.transform.position;
		}
	}
}
