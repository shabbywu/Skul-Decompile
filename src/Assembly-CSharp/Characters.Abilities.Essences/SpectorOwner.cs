using System;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Essences;

[Serializable]
public sealed class SpectorOwner : Ability
{
	public sealed class Instance : AbilityInstance<SpectorOwner>
	{
		private QuintessenceInventory _inventory;

		public Instance(Character owner, SpectorOwner ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.quintessence;
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && gaveDamage.key.Equals(ability._attackKey) && target.character.health.dead)
			{
				_inventory.items[0].cooldown.time.remainTime = 0f;
			}
		}
	}

	[SerializeField]
	private string _attackKey;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
