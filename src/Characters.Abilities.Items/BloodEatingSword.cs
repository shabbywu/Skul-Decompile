using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public class BloodEatingSword : Ability
{
	public class Instance : AbilityInstance<BloodEatingSword>
	{
		private bool changed;

		private Stat.Values _stat;

		public override int iconStacks => ability.component.currentStack;

		public override float iconFillAmount => 0f;

		public Instance(Character owner, BloodEatingSword ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
			owner.status.Register(CharacterStatus.Kind.Wound, CharacterStatus.Timing.Release, OnApplyBleed);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
			owner.status.Unregister(CharacterStatus.Kind.Wound, CharacterStatus.Timing.Release, OnApplyBleed);
		}

		private void OnApplyBleed(Character attacker, Character target)
		{
			if (!((Object)(object)target == (Object)null) && ability._characterTypeFilter[target.type])
			{
				ability.component.currentStack++;
				UpdateStack();
				if (!((float)ability.component.currentStack < ability._maxStack) && !changed)
				{
					changed = true;
					ability._operations.Run(owner);
				}
			}
		}

		public void UpdateStack()
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._statPerStack.values[i].GetStackedValue(ability.component.currentStack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private float _maxStack = 666f;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _operations;

	public BloodEatingSwordComponent component { get; set; }

	public override void Initialize()
	{
		base.Initialize();
		_operations.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
