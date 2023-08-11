using System;
using Characters.Player;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnUseEssence : Trigger
{
	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		QuintessenceInventory quintessence = character.playerComponents.inventory.quintessence;
		quintessence.onChanged += ObserveUsing;
		if (!((Object)(object)quintessence.items[0] == (Object)null))
		{
			quintessence.items[0].onUse -= base.Invoke;
			quintessence.items[0].onUse += base.Invoke;
		}
	}

	private void ObserveUsing()
	{
		QuintessenceInventory quintessence = _character.playerComponents.inventory.quintessence;
		if (!((Object)(object)quintessence.items[0] == (Object)null))
		{
			quintessence.items[0].onUse -= base.Invoke;
			quintessence.items[0].onUse += base.Invoke;
		}
	}

	public override void Detach()
	{
		_character.playerComponents.inventory.quintessence.onChanged -= ObserveUsing;
	}
}
