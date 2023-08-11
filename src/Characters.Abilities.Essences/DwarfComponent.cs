namespace Characters.Abilities.Essences;

public sealed class DwarfComponent : AbilityComponent<Dwarf>, IStackable
{
	public float stack
	{
		get
		{
			return base.baseAbility.attackCount;
		}
		set
		{
			base.baseAbility.attackCount = (int)value;
		}
	}
}
