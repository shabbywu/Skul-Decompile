namespace Characters.Abilities.Upgrades;

public sealed class TimeBombGiverComponent : AbilityComponent<TimeBombGiver>
{
	public void Attack(Character target)
	{
		base.baseAbility.Attack(target);
	}
}
