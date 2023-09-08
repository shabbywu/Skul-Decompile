using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.CharacterStat;
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

public sealed class Masterpiece : InscriptionInstance
{
	[Serializable]
	private class EnhancementMap
	{
		[SerializeField]
		private AssetReference _from;

		[SerializeField]
		private AssetReference _to;

		public AssetReference from => _from;

		public AssetReference to => _to;
	}

	[Header("2세트 효과")]
	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	[Subcomponent(typeof(StatBonusComponent))]
	private StatBonusComponent _statBonus;

	[Header("4세트 효과")]
	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onEnhanced;

	[SerializeField]
	private EnhancementMap[] _enhancementMaps;

	private bool _canUse;

	private bool _upgrading;

	private Queue<(int, int)> _itemIndics;

	private float _remainCooldownTime;

	protected override void Initialize()
	{
		_statBonus.Initialize();
		_canUse = true;
		_itemIndics = new Queue<(int, int)>(9);
	}

	public override void Attach()
	{
		base.character.playerComponents.inventory.item.onChanged += EnhanceItems;
	}

	public override void Detach()
	{
		base.character.playerComponents.inventory.item.onChanged -= EnhanceItems;
		base.character.onGiveDamage.Remove(OnGiveDamageDelegate);
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		if (_upgrading)
		{
			return;
		}
		if (keyword.step >= 1)
		{
			if (!base.character.onGiveDamage.Contains(OnGiveDamageDelegate))
			{
				base.character.onGiveDamage.Add(int.MinValue, OnGiveDamageDelegate);
			}
		}
		else
		{
			base.character.onGiveDamage.Remove(OnGiveDamageDelegate);
		}
	}

	private bool OnGiveDamageDelegate(ITarget target, ref Damage damage)
	{
		if (!_canUse || damage.attribute != 0)
		{
			return false;
		}
		base.character.ability.Add(_statBonus.ability);
		_remainCooldownTime = _cooldownTime;
		return false;
	}

	private void Update()
	{
		if (keyword.step >= 1)
		{
			_remainCooldownTime -= base.character.chronometer.master.deltaTime;
			if (_remainCooldownTime < 0f)
			{
				_canUse = true;
			}
			else
			{
				_canUse = false;
			}
		}
	}

	private void EnhanceItems()
	{
		if (!_upgrading && keyword.step >= keyword.steps.Count - 1 && base.character.playerComponents.inventory.item.items.Any(delegate(Item item)
		{
			if ((Object)(object)item == (Object)null)
			{
				return false;
			}
			return item.keyword1 == keyword.key || item.keyword2 == keyword.key;
		}))
		{
			_upgrading = true;
			UpdateItemIndex();
			ChangeItems();
			_upgrading = false;
		}
	}

	private void UpdateItemIndex()
	{
		ItemInventory item = base.character.playerComponents.inventory.item;
		for (int i = 0; i < item.items.Count; i++)
		{
			Item item2 = item.items[i];
			if ((Object)(object)item2 == (Object)null || (item2.keyword1 != keyword.key && item2.keyword2 != keyword.key))
			{
				continue;
			}
			for (int j = 0; j < _enhancementMaps.Length; j++)
			{
				if (GearResource.instance.TryGetItemReferenceByGuid(_enhancementMaps[j].from.AssetGUID, out var reference) && ((Object)item2).name.Equals(reference.name, StringComparison.OrdinalIgnoreCase))
				{
					_itemIndics.Enqueue((i, j));
					break;
				}
			}
		}
	}

	private void ChangeItems()
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		ItemInventory item = base.character.playerComponents.inventory.item;
		if (_itemIndics.Count > 0)
		{
			((MonoBehaviour)this).StartCoroutine(_onEnhanced.CRun(base.character));
		}
		while (_itemIndics.Count > 0)
		{
			(int, int) tuple = _itemIndics.Dequeue();
			int item2 = tuple.Item1;
			int item3 = tuple.Item2;
			Item item4 = item.items[item2];
			if (GearResource.instance.TryGetItemReferenceByGuid(_enhancementMaps[item3].to.AssetGUID, out var reference))
			{
				ItemRequest itemRequest = reference.LoadAsync();
				itemRequest.WaitForCompletion();
				Item item5 = Singleton<Service>.Instance.levelManager.DropItem(itemRequest, Vector3.zero);
				item5.keyword1 = item4.keyword1;
				item5.keyword2 = item4.keyword2;
				item4.ChangeOnInventory(item5);
			}
		}
	}
}
