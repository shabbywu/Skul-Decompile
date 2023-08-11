namespace Characters.Abilities.Customs;

public sealed class MagicWandComponent : AbilityComponent<MagicWand>, IStackable
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
