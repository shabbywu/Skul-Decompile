using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public abstract class SimpleStatBonusKeyword : InscriptionInstance
{
	protected class StatBonus : IAbility, IAbilityInstance
	{
		[NonSerialized]
		public double currentStatBonus;

		[NonSerialized]
		public Stat.Values stat = new Stat.Values(new Stat.Value(0, 0, 0.0));

		private Character _owner;

		Character IAbilityInstance.owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached => true;

		public Sprite icon { get; set; }

		public float iconFillAmount => 0f;

		public bool iconFillInversed => false;

		public bool iconFillFlipped => false;

		public int iconStacks => (int)(currentStatBonus * 100.0);

		public bool expired => false;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		public StatBonus(Character owner)
		{
			_owner = owner;
		}

		public void Initialize()
		{
		}

		public void UpdateTime(float deltaTime)
		{
		}

		public void Refresh()
		{
		}

		void IAbilityInstance.Attach()
		{
			_owner.stat.AttachValues(stat);
		}

		void IAbilityInstance.Detach()
		{
			_owner.stat.DetachValues(stat);
		}

		public void UpdateStat()
		{
			((ReorderableArray<Stat.Value>)stat).values[0].value = currentStatBonus;
			_owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Sprite _icon;

	protected StatBonus _statBonus;

	protected abstract double[] statBonusByStep { get; }

	protected abstract Stat.Category statCategory { get; }

	protected abstract Stat.Kind statKind { get; }

	protected override void Initialize()
	{
		_statBonus = new StatBonus(base.character);
		_statBonus.Initialize();
		_statBonus.icon = _icon;
	}

	protected void UpdateStat()
	{
		_statBonus.currentStatBonus = statBonusByStep[keyword.step];
		if (statCategory.index == Stat.Category.Percent.index)
		{
			_statBonus.currentStatBonus = _statBonus.currentStatBonus * 0.01 + 1.0;
		}
		else if (statCategory.index == Stat.Category.PercentPoint.index)
		{
			_statBonus.currentStatBonus *= 0.01;
		}
		((ReorderableArray<Stat.Value>)_statBonus.stat).values[0].categoryIndex = statCategory.index;
		((ReorderableArray<Stat.Value>)_statBonus.stat).values[0].kindIndex = statKind.index;
		_statBonus.UpdateStat();
	}

	public override void Attach()
	{
		base.character.ability.Add(_statBonus);
	}

	public override void Detach()
	{
		base.character.ability.Remove(_statBonus);
		UpdateStat();
	}
}
