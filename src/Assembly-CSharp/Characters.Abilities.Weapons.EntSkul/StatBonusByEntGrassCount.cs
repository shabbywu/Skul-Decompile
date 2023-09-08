using System;
using Characters.Usables;
using UnityEngine;

namespace Characters.Abilities.Weapons.EntSkul;

[Serializable]
public class StatBonusByEntGrassCount : Ability
{
	public class Instance : AbilityInstance<StatBonusByEntGrassCount>
	{
		private Stat.Values _stat;

		private int _iconStacks;

		public override Sprite icon
		{
			get
			{
				if (_iconStacks <= 0)
				{
					return null;
				}
				return ability.defaultIcon;
			}
		}

		public override int iconStacks => _iconStacks;

		public Instance(Character owner, StatBonusByEntGrassCount ability)
			: base(owner, ability)
		{
			_stat = ability._statPerStack.Clone();
		}

		protected override void OnAttach()
		{
			owner.stat.AttachValues(_stat);
			ability._liquidMaster.onChanged += UpdateStat;
			UpdateStat();
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
			ability._liquidMaster.onChanged -= UpdateStat;
		}

		public void UpdateStat()
		{
			double num = (double)ability._liquidMaster.GetStack() * ability._stackMultiplier;
			_iconStacks = (int)(num * ability._iconStacksPerStack);
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._statPerStack.values[i].GetStackedValue(num);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private LiquidMaster _liquidMaster;

	[SerializeField]
	private int _maxStack;

	[Tooltip("잔디 갯수에 이 값을 곱한 숫자가 스택이 됨")]
	[SerializeField]
	private double _stackMultiplier = 1.0;

	[SerializeField]
	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	private double _iconStacksPerStack = 1.0;

	[SerializeField]
	private Stat.Values _statPerStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
