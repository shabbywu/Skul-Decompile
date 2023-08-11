using Characters.Actions;
using UnityEngine;

namespace Characters.Operations;

public sealed class ModifyRemainCooldownTime : CharacterOperation
{
	[SerializeField]
	private ActionTypeBoolArray _type;

	[SerializeField]
	private CustomFloat _amount;

	public override void Run(Character owner)
	{
		foreach (Action action in owner.actions)
		{
			if ((action.cooldown.time != null || action.type == Action.Type.Swap) && ((EnumArray<Action.Type, bool>)_type)[action.type])
			{
				if (action.type == Action.Type.Swap)
				{
					owner.playerComponents.inventory.weapon.SetSwapCooldown(_amount.value);
				}
				else if (action.cooldown.time.stacks > 1 && action.cooldown.time.stacks != action.cooldown.time.maxStack)
				{
					action.cooldown.time.remainTime = _amount.value;
				}
				else if (!action.cooldown.time.canUse)
				{
					action.cooldown.time.remainTime = _amount.value;
				}
			}
		}
	}
}
