namespace Characters.Abilities.Items;

public class BloodEatingSwordComponent : AbilityComponent<BloodEatingSword>, IStackable
{
	public int currentStack { get; set; }

	public float stack
	{
		get
		{
			return currentStack;
		}
		set
		{
			currentStack = (int)value;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		base.baseAbility.component = this;
	}
}
