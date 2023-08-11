namespace Characters.Abilities.Essences;

public class KirizComponent : AbilityComponent<Kiriz>
{
	public void SetAttacker(Character attacker)
	{
		_ability.SetAttacker(attacker);
	}
}
