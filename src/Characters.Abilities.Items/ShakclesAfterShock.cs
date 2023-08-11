using System;
using Characters.Actions;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class ShakclesAfterShock : Ability
{
	public class Instance : AbilityInstance<ShakclesAfterShock>
	{
		public Instance(Character owner, ShakclesAfterShock ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			foreach (Characters.Actions.Action action in owner.actions)
			{
				_ = action;
			}
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
		}

		protected override void OnDetach()
		{
			throw new NotImplementedException();
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
