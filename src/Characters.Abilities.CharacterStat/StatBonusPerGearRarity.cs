using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusPerGearRarity : Ability
{
	public class Instance : AbilityInstance<StatBonusPerGearRarity>
	{
		private int _stack;

		private Stat.Values _stat;

		public override int iconStacks => _stack;

		public Instance(Character owner, StatBonusPerGearRarity ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerGearTag.Clone();
			owner.stat.AttachValues(_stat);
			((MonoBehaviour)owner).StartCoroutine(CWaitForLoadAndUpdateStack());
			owner.playerComponents.inventory.onUpdated += UpdateStatBonus;
		}

		protected override void OnDetach()
		{
			owner.playerComponents.inventory.onUpdated -= UpdateStatBonus;
			owner.stat.DetachValues(_stat);
		}

		private void UpdateStatBonus()
		{
			UpdateStack();
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerGearTag).values[i].GetStackedValue(_stack);
			}
			owner.stat.SetNeedUpdate();
		}

		private IEnumerator CWaitForLoadAndUpdateStack()
		{
			yield return null;
			ability._component.loaded = true;
			UpdateStatBonus();
		}

		private void UpdateStack()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			if (ability._component.loaded)
			{
				int itemCountByRarity = owner.playerComponents.inventory.item.GetItemCountByRarity(ability._rarity);
				int countByRarity = owner.playerComponents.inventory.weapon.GetCountByRarity(ability._rarity);
				int countByRarity2 = owner.playerComponents.inventory.quintessence.GetCountByRarity(ability._rarity);
				_stack = Mathf.Min(ability._maxStack, itemCountByRarity + countByRarity + countByRarity2);
				if (ability._component.stack < (float)ability._operationRunCount && _stack >= ability._maxStack)
				{
					((MonoBehaviour)owner).StartCoroutine(ability._onMaxStack.CRun(owner));
					ability._component.stack += 1f;
				}
			}
		}
	}

	[SerializeField]
	private StatBonusPerGearRarityComponent _component;

	[SerializeField]
	private Rarity _rarity;

	[SerializeField]
	private Stat.Values _statPerGearTag;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private int _operationRunCount;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onMaxStack;

	public int count { get; set; }

	public StatBonusPerGearRarity()
	{
	}

	public StatBonusPerGearRarity(Stat.Values stat)
	{
		_statPerGearTag = stat;
	}

	public override void Initialize()
	{
		base.Initialize();
		_onMaxStack.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
