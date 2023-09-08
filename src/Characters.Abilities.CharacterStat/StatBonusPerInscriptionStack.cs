using System;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusPerInscriptionStack : Ability
{
	public class Instance : AbilityInstance<StatBonusPerInscriptionStack>
	{
		private int _stack;

		private Stat.Values _stat;

		public override int iconStacks => _stack;

		public Instance(Character owner, StatBonusPerInscriptionStack ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts += UpdateStack;
			owner.stat.AttachValues(_stat);
			UpdateStack();
		}

		protected override void OnDetach()
		{
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts -= UpdateStack;
			owner.stat.DetachValues(_stat);
		}

		public void UpdateStack()
		{
			Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
			_stack = 0;
			foreach (Inscription inscription in synergy.inscriptions)
			{
				int count = inscription.count;
				switch (ability._comparer)
				{
				case Comparer.EqualTo:
					if (count == ability._inscriptionLevelForStackCounting)
					{
						_stack++;
					}
					break;
				case Comparer.NotEqualTo:
					if (count != ability._inscriptionLevelForStackCounting)
					{
						_stack++;
					}
					break;
				case Comparer.GreaterThan:
					if (count > ability._inscriptionLevelForStackCounting)
					{
						_stack++;
					}
					break;
				case Comparer.GreaterThanOrEqualTo:
					if (count >= ability._inscriptionLevelForStackCounting)
					{
						_stack++;
					}
					break;
				case Comparer.LessThan:
					if (count < ability._inscriptionLevelForStackCounting)
					{
						_stack++;
					}
					break;
				case Comparer.LessThanOrEqualTo:
					if (count <= ability._inscriptionLevelForStackCounting)
					{
						_stack++;
					}
					break;
				}
			}
			_stack = Mathf.Min(_stack, ability._maxStack);
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._statPerStack.values[i].GetStackedValue(_stack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	private enum Comparer
	{
		LessThan,
		LessThanOrEqualTo,
		EqualTo,
		NotEqualTo,
		GreaterThanOrEqualTo,
		GreaterThan
	}

	[SerializeField]
	private Comparer _comparer;

	[Header("비교 대상 숫자")]
	[SerializeField]
	private int _inscriptionLevelForStackCounting = 1;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private Stat.Values _statPerStack;

	public override void Initialize()
	{
		base.Initialize();
		if (_maxStack == 0)
		{
			_maxStack = int.MaxValue;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
