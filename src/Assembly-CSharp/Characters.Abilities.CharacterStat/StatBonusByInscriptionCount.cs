using System;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusByInscriptionCount : Ability
{
	public class Instance : AbilityInstance<StatBonusByInscriptionCount>
	{
		private int _stack;

		private Stat.Values _stat;

		public override int iconStacks => (int)((float)_stack * ability._iconStacksPerStack);

		public Instance(Character owner, StatBonusByInscriptionCount ability)
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
			if (!Service.quitting && !((Object)(object)Singleton<Service>.Instance.levelManager == (Object)null) && !((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null) && Singleton<Service>.Instance.levelManager.player.playerComponents.inventory != null)
			{
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts -= UpdateStack;
				owner.stat.DetachValues(_stat);
			}
		}

		public override void Refresh()
		{
			if (ability._refreshRemainTime)
			{
				base.Refresh();
			}
		}

		public void UpdateStack()
		{
			Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
			int num = 0;
			Inscription.Key[] keys = ability._keys;
			foreach (Inscription.Key key in keys)
			{
				num += synergy.inscriptions[key].count;
			}
			_stack = num;
			if (_stack < ability._maxStack)
			{
				for (int j = 0; j < _stat.values.Length; j++)
				{
					_stat.values[j].value = ability._statPerStack.values[j].GetStackedValue(_stack);
				}
				owner.stat.SetNeedUpdate();
			}
		}
	}

	[SerializeField]
	private Inscription.Key[] _keys;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
	private bool _refreshRemainTime = true;

	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	[SerializeField]
	private float _iconStacksPerStack = 1f;

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
