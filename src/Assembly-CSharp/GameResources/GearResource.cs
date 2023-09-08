using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace GameResources;

public class GearResource : ScriptableObject
{
	[SerializeField]
	private Sprite[] _gearThumbnails;

	private Dictionary<string, Sprite> _gearThumbnailDictionary;

	[SerializeField]
	private Sprite[] _weaponHudMainIcons;

	private Dictionary<string, Sprite> _weaponHudMainIconDictionary;

	[SerializeField]
	private Sprite[] _weaponHudSubIcons;

	private Dictionary<string, Sprite> _weaponHudSubIconDictionary;

	[SerializeField]
	private Sprite[] _quintessenceHudIcons;

	private Dictionary<string, Sprite> _quintessenceHudIconDictionary;

	[SerializeField]
	private Sprite[] _skillIcons;

	private Dictionary<string, Sprite> _skillIconDictionary;

	[SerializeField]
	private Sprite[] _itemBuffIcons;

	private Dictionary<string, Sprite> _itemBuffIconDictionary;

	[SerializeField]
	private WeaponReference[] _weapons;

	private Dictionary<string, WeaponReference> _weaponDictionaryByName;

	private Dictionary<string, WeaponReference> _weaponDictionaryByGuid;

	[SerializeField]
	private ItemReference[] _items;

	private Dictionary<string, ItemReference> _itemDictionaryByName;

	private Dictionary<string, ItemReference> _itemDictionaryByGuid;

	[SerializeField]
	private EssenceReference[] _essences;

	private Dictionary<string, EssenceReference> _essenceDictionaryByName;

	private Dictionary<string, EssenceReference> _essenceDictionaryByGuid;

	public static GearResource instance { get; private set; }

	public ReadOnlyCollection<WeaponReference> weapons { get; private set; }

	public ReadOnlyCollection<ItemReference> items { get; private set; }

	public ReadOnlyCollection<EssenceReference> essences { get; private set; }

	public Sprite GetGearThumbnail(string name)
	{
		_gearThumbnailDictionary.TryGetValue(name, out var value);
		return value;
	}

	public Sprite GetWeaponHudMainIcon(string name)
	{
		_weaponHudMainIconDictionary.TryGetValue(name, out var value);
		return value;
	}

	public Sprite GetWeaponHudSubIcon(string name)
	{
		_weaponHudSubIconDictionary.TryGetValue(name, out var value);
		return value;
	}

	public Sprite GetQuintessenceHudIcon(string name)
	{
		_quintessenceHudIconDictionary.TryGetValue(name, out var value);
		return value;
	}

	public Sprite GetSkillIcon(string name)
	{
		_skillIconDictionary.TryGetValue(name, out var value);
		return value;
	}

	public Sprite GetItemBuffIcon(string name)
	{
		_itemBuffIconDictionary.TryGetValue(name, out var value);
		return value;
	}

	public bool TryGetWeaponReferenceByName(string name, out WeaponReference reference)
	{
		return _weaponDictionaryByName.TryGetValue(name, out reference);
	}

	public bool TryGetWeaponReferenceByGuid(string guid, out WeaponReference reference)
	{
		return _weaponDictionaryByGuid.TryGetValue(guid, out reference);
	}

	public bool TryGetItemReferenceByName(string name, out ItemReference reference)
	{
		return _itemDictionaryByName.TryGetValue(name, out reference);
	}

	public bool TryGetItemReferenceByGuid(string guid, out ItemReference reference)
	{
		return _itemDictionaryByGuid.TryGetValue(guid, out reference);
	}

	public bool TryGetEssenceReferenceByName(string name, out EssenceReference reference)
	{
		return _essenceDictionaryByName.TryGetValue(name, out reference);
	}

	public bool TryGetEssenceReferenceByGuid(string guid, out EssenceReference reference)
	{
		return _essenceDictionaryByGuid.TryGetValue(guid, out reference);
	}

	public void Initialize()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		((Object)this).hideFlags = (HideFlags)(((Object)this).hideFlags | 0x20);
		_gearThumbnailDictionary = _gearThumbnails.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_weaponHudMainIconDictionary = _weaponHudMainIcons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_weaponHudSubIconDictionary = _weaponHudSubIcons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_quintessenceHudIconDictionary = _quintessenceHudIcons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_skillIconDictionary = _skillIcons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_itemBuffIconDictionary = _itemBuffIcons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		weapons = Array.AsReadOnly(_weapons);
		_weaponDictionaryByName = weapons.ToDictionary<WeaponReference, string>((WeaponReference weapon) => weapon.name, StringComparer.OrdinalIgnoreCase);
		_weaponDictionaryByGuid = weapons.ToDictionary<WeaponReference, string>((WeaponReference weapon) => weapon.guid, StringComparer.OrdinalIgnoreCase);
		items = Array.AsReadOnly(_items);
		_itemDictionaryByName = items.ToDictionary<ItemReference, string>((ItemReference item) => item.name, StringComparer.OrdinalIgnoreCase);
		_itemDictionaryByGuid = items.ToDictionary<ItemReference, string>((ItemReference item) => item.guid, StringComparer.OrdinalIgnoreCase);
		essences = Array.AsReadOnly(_essences);
		_essenceDictionaryByName = essences.ToDictionary<EssenceReference, string>((EssenceReference quintessence) => quintessence.name, StringComparer.OrdinalIgnoreCase);
		_essenceDictionaryByGuid = essences.ToDictionary<EssenceReference, string>((EssenceReference quintessence) => quintessence.guid, StringComparer.OrdinalIgnoreCase);
	}
}
