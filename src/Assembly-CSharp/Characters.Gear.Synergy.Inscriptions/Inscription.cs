using System;
using System.Collections.ObjectModel;
using Characters.Player;
using GameResources;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Inscription
{
	public enum Key
	{
		None,
		Antique,
		Arms,
		Artifact,
		Bone,
		Brave,
		FairyTale,
		Duel,
		Fortress,
		Arson,
		Execution,
		Strike,
		Manatech,
		Soar,
		Relic,
		Heirloom,
		Mutation,
		Chase,
		ManaCycle,
		Misfortune,
		AbsoluteZero,
		Spoils,
		Brawl,
		SunAndMoon,
		Rapidity,
		Revenge,
		Poisoning,
		ExcessiveBleeding,
		Wisdom,
		Masterpiece,
		HiddenBlade,
		Heritage,
		Treasure,
		Dizziness,
		Omen,
		Sin
	}

	public int count;

	public int bonusCount;

	private ItemInventory _itemInventory;

	private InscriptionInstance _instance;

	private AsyncOperationHandle<GameObject> _handle;

	public static ReadOnlyCollection<Key> keys => EnumValues<Key>.Values;

	public Key key { get; private set; }

	public InscriptionSettings settings { get; private set; }

	public Character character { get; private set; }

	public int step { get; private set; }

	public ReadOnlyCollection<int> steps { get; private set; }

	public string name => GetName(key);

	public Sprite icon
	{
		get
		{
			if (isMaxStep)
			{
				return fullActiveIcon;
			}
			if (active)
			{
				return activeIcon;
			}
			return deactiveIcon;
		}
	}

	public Sprite activeIcon { get; private set; }

	public Sprite deactiveIcon { get; private set; }

	public Sprite fullActiveIcon { get; private set; }

	public bool active { get; private set; }

	public bool omen { get; private set; }

	public bool isMaxStep { get; private set; }

	public int maxStep { get; private set; }

	public static Sprite GetActiveIcon(Key key)
	{
		return CommonResource.instance.keywordIconDictionary[key.ToString()];
	}

	public static Sprite GetDeactiveIcon(Key key)
	{
		return CommonResource.instance.keywordDeactiveIconDictionary[key.ToString()];
	}

	public static Sprite GetFullActiveIcon(Key key)
	{
		return CommonResource.instance.keywordFullactiveIconDictionary[key.ToString()];
	}

	public static string GetName(Key key)
	{
		return Localization.GetLocalizedString($"synergy/key/{key}/name");
	}

	public void Initialize(Key key, InscriptionSettings settings, Character character)
	{
		this.key = key;
		this.settings = settings;
		this.character = character;
		_itemInventory = character.playerComponents.inventory.item;
		steps = Array.AsReadOnly(settings.steps);
		if (key != 0)
		{
			activeIcon = GetActiveIcon(key);
			deactiveIcon = GetDeactiveIcon(key);
			fullActiveIcon = GetFullActiveIcon(key);
		}
		maxStep = ((steps.Count != 0) ? steps[steps.Count - 1] : 0);
	}

	public void Clear()
	{
		if (!((Object)(object)_instance == (Object)null))
		{
			Object.Destroy((Object)(object)((Component)_instance).gameObject);
			_instance = null;
			UnloadReference();
		}
	}

	public void Update()
	{
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		int num = steps.Count;
		if (num == 0)
		{
			return;
		}
		int num2 = 0;
		for (int i = 0; i < num && count >= steps[i]; i++)
		{
			num2 = i;
		}
		bool flag = omen;
		if (settings.omenItem != null)
		{
			omen = _itemInventory.Has(settings.omenItem.AssetGUID);
		}
		if (num2 == step && flag == omen)
		{
			return;
		}
		bool flag2 = active;
		active = num2 > 0;
		step = num2;
		isMaxStep = step == num - 1;
		if (!active)
		{
			if (flag2)
			{
				if ((Object)(object)_instance == (Object)null)
				{
					Debug.LogError((object)$"WasActive is true, but there is no inscription instance of {key}.");
				}
				else
				{
					_instance.Detach();
				}
				Clear();
			}
		}
		else if (flag2)
		{
			_instance.UpdateBonus(flag2, flag);
		}
		else if (!((Object)(object)_instance != (Object)null))
		{
			LoadReference();
			GameObject val = Object.Instantiate<GameObject>(_handle.WaitForCompletion(), ((Component)character).transform);
			val.transform.position = ((Component)character).transform.position;
			_instance = val.GetComponent<InscriptionInstance>();
			_instance.keyword = this;
			_instance.Initialize(character);
			_instance.Attach();
			_instance.UpdateBonus(flag2, flag);
		}
	}

	private void LoadReference()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (!_handle.IsValid())
		{
			_handle = settings.reference.LoadAssetAsync<GameObject>();
		}
	}

	private void UnloadReference()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (_handle.IsValid())
		{
			Addressables.Release<GameObject>(_handle);
		}
	}

	public Sprite GetIconForStep(int step)
	{
		if (step >= maxStep)
		{
			return fullActiveIcon;
		}
		if (step >= settings.steps[1])
		{
			return activeIcon;
		}
		return deactiveIcon;
	}

	public string GetDescription()
	{
		return GetDescription(step);
	}

	public string GetDescription(int step)
	{
		return Localization.GetLocalizedString($"synergy/key/{key}/desc/{step}");
	}
}
