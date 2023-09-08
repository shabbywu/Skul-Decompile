namespace Characters.Abilities.Customs;

public class ForbiddenSwordComponent : AbilityComponent<ForbiddenSword>, IStackable
{
	public int currentKillCount { get; set; }

	public float stack
	{
		get
		{
			return currentKillCount;
		}
		set
		{
			currentKillCount = (int)value;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		base.baseAbility.component = this;
	}
}
