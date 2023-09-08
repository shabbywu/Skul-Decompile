using Characters;
using Characters.Abilities;

namespace Level.Curse;

public class Sanctuary : InteractiveObject, ISanctuary
{
	public override void InteractWith(Character character)
	{
		RemoveCurse(character);
	}

	public void RemoveCurse(Character character)
	{
		character.playerComponents.savableAbilityManager.Remove(SavableAbilityManager.Name.Curse);
	}
}
