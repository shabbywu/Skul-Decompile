using UnityEngine;

namespace Characters.Operations;

public sealed class ModifyEssenceRemainCooldownTime : CharacterOperation
{
	[SerializeField]
	private CustomFloat _amount;

	public override void Run(Character owner)
	{
		if (owner.playerComponents != null)
		{
			owner.playerComponents.inventory.quintessence.items[0].cooldown.time.remainTime = _amount.value;
		}
	}
}
