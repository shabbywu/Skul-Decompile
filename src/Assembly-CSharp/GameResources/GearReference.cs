using Characters.Gear;
using Data;
using Scenes;
using UnityEngine;

namespace GameResources;

public abstract class GearReference
{
	public string name;

	public string guid;

	public string path;

	public bool obtainable;

	public bool needUnlock;

	public Sprite unlockIcon;

	public Rarity rarity;

	public Sprite icon;

	public Sprite thumbnail;

	public string displayNameKey;

	public Gear.Tag gearTag;

	public abstract Gear.Type type { get; }

	public bool unlocked
	{
		get
		{
			if (!needUnlock)
			{
				return true;
			}
			return GameData.Gear.IsUnlocked(type.ToString(), name);
		}
	}

	public void Unlock()
	{
		if (!GameData.Gear.IsUnlocked(type.ToString(), name))
		{
			GameData.Gear.SetUnlocked(type.ToString(), name, value: true);
			Scene<GameBase>.instance.uiManager.unlockNotice.Show(unlockIcon, Localization.GetLocalizedString(displayNameKey));
		}
	}

	public GearRequest LoadAsync()
	{
		return new GearRequest(path);
	}
}
