namespace Characters.Abilities.Customs;

public class UnknownSeedComponent : AbilityComponent<UnknownSeed>, IStackable
{
	public float healed { get; set; }

	public float healedBefore { get; set; }

	public float stack
	{
		get
		{
			return healed;
		}
		set
		{
			healed = value;
			base.baseAbility.UpdateStat();
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		base.baseAbility.component = this;
	}
}
