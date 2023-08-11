using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OmenSunAndMoon : Ability
{
	public sealed class Instance : AbilityInstance<OmenSunAndMoon>
	{
		private bool _detached;

		public override Sprite icon => base.icon;

		public override int iconStacks
		{
			get
			{
				if (!_detached)
				{
					return 0;
				}
				return ability._killCount - ability.stack + 1;
			}
		}

		public override float iconFillAmount => _detached ? 1 : 0;

		public Instance(Character owner, OmenSunAndMoon ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if (ability.stack == 0 || ability.stack > ability._killCount)
			{
				owner.ability.Add(ability._abilityComponent.ability);
				return;
			}
			_detached = true;
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
			owner.ability.Add(ability._abilityOnDied.ability);
		}

		protected override void OnDetach()
		{
			owner.ability.Remove(ability._abilityOnDied.ability);
			owner.ability.Remove(ability._abilityComponent.ability);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}

		public override void UpdateTime(float deltaTime)
		{
			if (!owner.ability.Contains(ability._abilityComponent.ability) && !_detached)
			{
				_detached = true;
				ability.stack = 1;
				Character character = owner;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
				owner.ability.Add(ability._abilityOnDied.ability);
			}
		}

		private void HandleOnKilled(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ((EnumArray<Character.Type, bool>)ability._killTargetFilter)[character.type])
			{
				ability.stack++;
				if (ability.stack > ability._killCount)
				{
					_detached = false;
					Character character2 = owner;
					character2.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character2.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
					owner.ability.Add(ability._abilityComponent.ability);
					ability._onRechargeRevive.Run(owner);
				}
			}
		}

		internal void LoadStack()
		{
			if (ability.stack != 0 && ability.stack <= ability._killCount)
			{
				_detached = true;
				owner.ability.Remove(ability._abilityComponent.ability);
				Character character = owner;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
				owner.ability.Add(ability._abilityOnDied.ability);
			}
		}
	}

	[SerializeField]
	private int _killCount;

	[SerializeField]
	private CharacterTypeBoolArray _killTargetFilter;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityOnDied;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onRechargeRevive;

	private Instance _instance;

	public int stack { get; set; }

	public override void Initialize()
	{
		base.Initialize();
		_abilityComponent.Initialize();
		_abilityOnDied.Initialize();
		_onRechargeRevive.Initialize();
	}

	public void LoadStack()
	{
		if (_instance != null)
		{
			_instance.LoadStack();
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_instance = new Instance(owner, this);
		return _instance;
	}
}
