using System.Collections.Generic;
using Characters.Gear.Upgrades;
using FX;
using Singletons;
using UnityEngine;

namespace UI.Upgrades;

public sealed class UpgradableContainer : MonoBehaviour
{
	[SerializeField]
	private UpgradeElement _elementPrefab;

	[SerializeField]
	private EnumArray<UpgradeObject.Type, Transform> _elementParents;

	private Panel _panel;

	private List<UpgradeElement> _upgradeElements;

	[SerializeField]
	private SoundInfo _moveSoundInfo;

	[SerializeField]
	private SoundInfo _closeSoundInfo;

	[SerializeField]
	private SoundInfo _buySoundInfo;

	[SerializeField]
	private SoundInfo _upgradeSoundInfo;

	[SerializeField]
	private SoundInfo _clearSoundInfo;

	[SerializeField]
	private SoundInfo _failSoundInfo;

	[SerializeField]
	private SoundInfo _findSoundInfo;

	public SoundInfo moveSoundInfo => _moveSoundInfo;

	public SoundInfo closeSoundInfo => _closeSoundInfo;

	public SoundInfo buySoundInfo => _buySoundInfo;

	public SoundInfo upgradeSoundInfo => _upgradeSoundInfo;

	public SoundInfo clearSoundInfo => _clearSoundInfo;

	public SoundInfo failSoundInfo => _failSoundInfo;

	public SoundInfo findSoundInfo => _findSoundInfo;

	public void Initialize(Panel panel)
	{
		_panel = panel;
		_upgradeElements = new List<UpgradeElement>();
	}

	public void UpdateElements()
	{
		DestroyElements();
		CreateElements();
		if (_upgradeElements.Count > 0)
		{
			_panel.Focus(GetDefaultFocusTarget().selectable);
		}
	}

	public void CreateElements()
	{
		List<UpgradeResource.Reference> riskObjects = Singleton<UpgradeShop>.Instance.GetRiskObjects();
		for (int i = 0; i < 2; i++)
		{
			UpgradeResource.Reference reference = riskObjects[i];
			UpgradeElement upgradeElement = Object.Instantiate<UpgradeElement>(_elementPrefab, _elementParents[reference.type]);
			upgradeElement.Initialize(reference, _panel);
			_upgradeElements.Add(upgradeElement);
		}
		foreach (UpgradeResource.Reference upgradable in Singleton<UpgradeShop>.Instance.GetUpgradables())
		{
			if (upgradable.type != UpgradeObject.Type.Cursed)
			{
				UpgradeElement upgradeElement2 = Object.Instantiate<UpgradeElement>(_elementPrefab, _elementParents[UpgradeObject.Type.Normal]);
				upgradeElement2.Initialize(upgradable, _panel);
				_upgradeElements.Add(upgradeElement2);
			}
		}
	}

	public void DestroyElements()
	{
		if (_upgradeElements.Count != 0)
		{
			for (int num = _upgradeElements.Count - 1; num >= 0; num--)
			{
				Object.Destroy((Object)(object)((Component)_upgradeElements[num]).gameObject);
			}
			_upgradeElements.Clear();
		}
	}

	public UpgradeElement GetDefaultFocusTarget()
	{
		foreach (UpgradeElement upgradeElement in _upgradeElements)
		{
			if (!((Object)(object)upgradeElement == (Object)null) && upgradeElement.reference != null && upgradeElement.reference.type != UpgradeObject.Type.Cursed && upgradeElement.selectable.interactable)
			{
				return upgradeElement;
			}
		}
		return null;
	}
}
