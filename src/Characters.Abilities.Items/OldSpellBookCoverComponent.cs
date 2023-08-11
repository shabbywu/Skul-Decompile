namespace Characters.Abilities.Items;

public sealed class OldSpellBookCoverComponent : AbilityComponent<OldSpellBookCover>, IStackable
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
