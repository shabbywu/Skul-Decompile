namespace Characters.Abilities.Customs;

public class EmptyPotionComponent : AbilityComponent<EmptyPotion>, IStackable
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
