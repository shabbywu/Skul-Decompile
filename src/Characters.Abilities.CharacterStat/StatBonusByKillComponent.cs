namespace Characters.Abilities.CharacterStat;

public class StatBonusByKillComponent : AbilityComponent<StatBonusByKill>, IStackable
{
	public float stack
	{
		get
		{
			return base.baseAbility.stack;
		}
		set
		{
			base.baseAbility.stack = (int)value;
		}
	}
}
