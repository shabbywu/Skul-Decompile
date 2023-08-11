using System;
using System.Collections.Generic;
using Characters.Gear.Items;
using Characters.Operations;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class SunAndMoon : InscriptionInstance
{
	private class Composition
	{
		public string first;

		public string second;

		public AssetReference result;

		public Action onChanged;
	}

	[Header("2세트 효과")]
	[Space]
	[SerializeField]
	private AssetReference _swordOfSun;

	[SerializeField]
	private AssetReference _ringOfMoon;

	[SerializeField]
	private AssetReference _shardOfDarkness;

	[Space]
	[SerializeField]
	private AssetReference _brightDawn;

	[SerializeField]
	private AssetReference _superSolorItem;

	[SerializeField]
	private AssetReference _superLunarItem;

	[Space]
	[SerializeField]
	private AssetReference _solarEclipse;

	[SerializeField]
	private AssetReference _lunarEclipse;

	[SerializeField]
	private AssetReference _unknownDarkness;

	[Subcomponent(typeof(OperationInfos))]
	[Space]
	[SerializeField]
	private OperationInfos _onChanged;

	[Header("아이템 별 오퍼레이션")]
	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onChangedToBrightDawn;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onChangedToSuperSolor;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onChangedToSuperLunar;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onChangedToSolarEclipse;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onChangedToLunarEclipse;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onChangedToUnknownDarkness;

	private Composition[] _compositions;

	private List<(string, int)> _targetItemInfo;

	private bool _upgrading;

	private void Awake()
	{
	}

	protected override void Initialize()
	{
		_targetItemInfo = new List<(string, int)>(2);
		if (GearResource.instance.TryGetItemReferenceByGuid(_swordOfSun.AssetGUID, out var reference) && GearResource.instance.TryGetItemReferenceByGuid(_ringOfMoon.AssetGUID, out var reference2) && GearResource.instance.TryGetItemReferenceByGuid(_shardOfDarkness.AssetGUID, out var reference3))
		{
			_compositions = new Composition[9]
			{
				new Composition
				{
					first = reference.name,
					second = reference.name,
					result = _superSolorItem,
					onChanged = OnUpgradeToSuperSolar
				},
				new Composition
				{
					first = reference2.name,
					second = reference2.name,
					result = _superLunarItem,
					onChanged = OnUpgradeToSuperLunar
				},
				new Composition
				{
					first = reference3.name,
					second = reference3.name,
					result = _unknownDarkness,
					onChanged = OnUpgradeToUnknownDarkness
				},
				new Composition
				{
					first = reference.name,
					second = reference2.name,
					result = _brightDawn,
					onChanged = OnUpgradeToBrightDawn
				},
				new Composition
				{
					first = reference2.name,
					second = reference.name,
					result = _brightDawn,
					onChanged = OnUpgradeToBrightDawn
				},
				new Composition
				{
					first = reference.name,
					second = reference3.name,
					result = _solarEclipse,
					onChanged = OnUpgradeToSolarEclipse
				},
				new Composition
				{
					first = reference3.name,
					second = reference.name,
					result = _solarEclipse,
					onChanged = OnUpgradeToSolarEclipse
				},
				new Composition
				{
					first = reference2.name,
					second = reference3.name,
					result = _lunarEclipse,
					onChanged = OnUpgradeToLunarEclipse
				},
				new Composition
				{
					first = reference3.name,
					second = reference2.name,
					result = _lunarEclipse,
					onChanged = OnUpgradeToLunarEclipse
				}
			};
			if ((Object)(object)_onChanged != (Object)null)
			{
				_onChanged.Initialize();
			}
			if ((Object)(object)_onChangedToSuperSolor != (Object)null)
			{
				_onChangedToSuperSolor.Initialize();
			}
			if ((Object)(object)_onChangedToSuperLunar != (Object)null)
			{
				_onChangedToSuperLunar.Initialize();
			}
			if ((Object)(object)_onChangedToSolarEclipse != (Object)null)
			{
				_onChangedToSolarEclipse.Initialize();
			}
			if ((Object)(object)_onChangedToLunarEclipse != (Object)null)
			{
				_onChangedToLunarEclipse.Initialize();
			}
			if ((Object)(object)_onChangedToUnknownDarkness != (Object)null)
			{
				_onChangedToUnknownDarkness.Initialize();
			}
		}
	}

	public override void Attach()
	{
	}

	private void HandleOnChanged()
	{
		if (_upgrading)
		{
			return;
		}
		_upgrading = true;
		_targetItemInfo.Clear();
		ItemInventory item = base.character.playerComponents.inventory.item;
		for (int i = 0; i < item.items.Count; i++)
		{
			Item item2 = item.items[i];
			if (!((Object)(object)item2 == (Object)null) && (item2.keyword1 == Inscription.Key.SunAndMoon || item2.keyword2 == Inscription.Key.SunAndMoon))
			{
				_targetItemInfo.Add((((Object)item2).name, i));
			}
		}
		if (_targetItemInfo.Count < 2)
		{
			return;
		}
		for (int j = 0; j < _compositions.Length; j++)
		{
			Composition composition = _compositions[j];
			string item3 = _targetItemInfo[0].Item1;
			if (composition.first.Equals(item3))
			{
				item3 = _targetItemInfo[1].Item1;
				if (composition.second.Equals(item3))
				{
					int item4 = _targetItemInfo[0].Item2;
					int item5 = _targetItemInfo[1].Item2;
					MergeItem(item4, item5, composition.result);
					composition.onChanged?.Invoke();
					break;
				}
			}
		}
	}

	private void MergeItem(int index1, int index2, AssetReference changeTo)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		ItemInventory item = base.character.playerComponents.inventory.item;
		Item item2 = item.items[index1];
		item.Remove(index2);
		if (GearResource.instance.TryGetItemReferenceByGuid(changeTo.AssetGUID, out var reference))
		{
			ItemRequest itemRequest = reference.LoadAsync();
			itemRequest.WaitForCompletion();
			Item item3 = Singleton<Service>.Instance.levelManager.DropItem(itemRequest, Vector3.zero);
			item2.ChangeOnInventory(item3);
			item.Trim();
			((Component)_onChanged).gameObject.SetActive(true);
			_onChanged.Run(base.character);
			_upgrading = false;
		}
	}

	private void OnUpgradeToBrightDawn()
	{
		((Component)_onChangedToBrightDawn).gameObject.SetActive(true);
		_onChangedToBrightDawn.Run(base.character);
	}

	private void OnUpgradeToSuperSolar()
	{
		((Component)_onChangedToSuperSolor).gameObject.SetActive(true);
		_onChangedToSuperSolor.Run(base.character);
	}

	private void OnUpgradeToSuperLunar()
	{
		((Component)_onChangedToSuperLunar).gameObject.SetActive(true);
		_onChangedToSuperLunar.Run(base.character);
	}

	private void OnUpgradeToSolarEclipse()
	{
		((Component)_onChangedToSolarEclipse).gameObject.SetActive(true);
		_onChangedToSolarEclipse.Run(base.character);
	}

	private void OnUpgradeToLunarEclipse()
	{
		((Component)_onChangedToLunarEclipse).gameObject.SetActive(true);
		_onChangedToLunarEclipse.Run(base.character);
	}

	private void OnUpgradeToUnknownDarkness()
	{
		((Component)_onChangedToUnknownDarkness).gameObject.SetActive(true);
		_onChangedToUnknownDarkness.Run(base.character);
	}

	public override void Detach()
	{
		base.character.playerComponents.inventory.item.onChanged -= HandleOnChanged;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		if (keyword.isMaxStep)
		{
			HandleOnChanged();
		}
	}
}
