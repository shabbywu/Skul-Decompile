namespace Characters.Abilities.CharacterStat;

public class StackableStatBonusComponent : AbilityComponent<StackableStatBonus>, IStackable
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

	public bool isMax => base.baseAbility.stack >= base.baseAbility.maxStack;
}
