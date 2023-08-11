using System;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OldSpellBookCover : Ability
{
	public sealed class Instance : AbilityInstance<OldSpellBookCover>
	{
		private const int initialStack = 1;

		private IDictionary<Character, int> _history;

		public override Sprite icon
		{
			get
			{
				if (ability.stack <= 0)
				{
					return null;
				}
				return base.icon;
			}
		}

		public override int iconStacks => ability.stack;

		public Instance(Character owner, OldSpellBookCover ability)
			: base(owner, ability)
		{
			_history = new Dictionary<Character, int>();
		}

		protected override void OnAttach()
		{
			AttachTrigger();
		}

		protected override void OnDetach()
		{
			DetachTrigger();
		}

		private void AttachTrigger()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		private void DetachTrigger()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[gaveDamage.motionType] && !((Object)(object)target.character == (Object)null) && ((EnumArray<Character.Type, bool>)ability._targetTypeFilter)[target.character.type] && target != null && !((Object)(object)target.character == (Object)null))
			{
				Invoke(target.character);
			}
		}

		private void Invoke(Character character)
		{
			if (!_history.ContainsKey(character))
			{
				_history.Add(character, 1);
				IncreaseStack(character);
				return;
			}
			int num = _history[character];
			if (num < ability._maxStackPerTarget)
			{
				_history[character] = num + 1;
				IncreaseStack(character);
			}
		}

		private void IncreaseStack(Character target)
		{
			ability._lastTargetPositionInfo.Attach(target, ability._lastTargetPoint);
			ability._onIncreaseStack.Run(owner);
			ability.stack++;
			if (ability.stack >= ability._stackForUpgrade)
			{
				UpgradeItem();
			}
		}

		private void UpgradeItem()
		{
			ability._onUpgrade.Run(owner);
		}
	}

	[SerializeField]
	private PositionInfo _lastTargetPositionInfo;

	[SerializeField]
	private Transform _lastTargetPoint;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private CharacterTypeBoolArray _targetTypeFilter;

	[SerializeField]
	private int _maxStackPerTarget;

	[SerializeField]
	private int _stackForUpgrade;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _onIncreaseStack;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onUpgrade;

	public int stack { get; set; }

	public override void Initialize()
	{
		base.Initialize();
		_onIncreaseStack.Initialize();
		_onUpgrade.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
