namespace Characters.Abilities.Weapons.Ghoul;

public class GhoulPassive2Component : AbilityComponent<GhoulPassive2>
{
	public void Recover()
	{
		_ability.Recover();
	}

	public void AddStack()
	{
		_ability.AddStack();
	}
}
