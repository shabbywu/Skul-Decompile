using System;
using Characters.Player;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class GoludaluSummonBook : Ability
{
	public class Instance : AbilityInstance<GoludaluSummonBook>
	{
		public Instance(Character owner, GoludaluSummonBook ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			QuintessenceInventory quintessence = owner.playerComponents.inventory.quintessence;
			if (quintessence.items.Count > 0 && quintessence.items[0].cooldown.canUse)
			{
				quintessence.items[0].Use();
			}
		}

		protected override void OnDetach()
		{
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
