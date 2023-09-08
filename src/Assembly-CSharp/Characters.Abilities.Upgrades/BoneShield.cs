using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class BoneShield : Ability
{
	public sealed class Instance : AbilityInstance<BoneShield>
	{
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

		public Instance(Character owner, BoneShield ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(100, HandleOnTakeDamage);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}

		private void HandleOnKilled(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && character.type == Character.Type.Named)
			{
				ability.stack = Mathf.Min(ability._maxStack, ability.stack + ability._stackPerKilledDarkEnemy);
			}
		}

		private bool HandleOnTakeDamage(ref Damage damage)
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			if (owner.invulnerable.value || owner.evasion.value)
			{
				return false;
			}
			if (damage.attackType.Equals(Damage.AttackType.None))
			{
				return false;
			}
			if (damage.@null)
			{
				return false;
			}
			if (ability.stack <= 0)
			{
				return false;
			}
			owner.ability.Add(ability._evasion);
			ability.stack--;
			damage.@null = true;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachAudioClipInfo, ((Component)owner).transform.position);
			return false;
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove(HandleOnTakeDamage);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}
	}

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private int _stackPerKilledDarkEnemy;

	[SerializeField]
	private StackableComponent _stackableComponent;

	[SerializeField]
	private GetEvasion _evasion;

	[SerializeField]
	private SoundInfo _attachAudioClipInfo;

	public int stack
	{
		get
		{
			return (int)_stackableComponent.stack;
		}
		set
		{
			_stackableComponent.stack = value;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		_evasion.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
