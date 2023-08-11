using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations;

public class AttachCurseOfLight : TargetedCharacterOperation
{
	public override void Run(Character owner, Character target)
	{
		if (!((Object)(object)target == (Object)null) && target.liveAndActive && target.playerComponents != null)
		{
			target.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
		}
	}

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
