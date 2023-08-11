namespace Characters.Abilities.CharacterStat;

public sealed class StackableStatDebuffOnStatusComponent : AbilityComponent<StackableStatDebuffOnStatus>, IStackable
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
