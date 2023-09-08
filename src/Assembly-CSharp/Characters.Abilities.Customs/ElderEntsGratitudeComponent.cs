namespace Characters.Abilities.Customs;

public class ElderEntsGratitudeComponent : AbilityComponent<ElderEntsGratitude>, IStackable
{
	public float stack
	{
		get
		{
			return GetShieldAmount();
		}
		set
		{
			SetShieldAmount(value);
		}
	}

	public float savedShieldAmount { get; set; }

	private void Awake()
	{
		savedShieldAmount = base.baseAbility.amount;
	}

	public override void Initialize()
	{
		base.Initialize();
		base.baseAbility.component = this;
	}

	private float GetShieldAmount()
	{
		return base.baseAbility.GetShieldAmount();
	}

	private void SetShieldAmount(float amount)
	{
		savedShieldAmount = amount;
		base.baseAbility.SetShieldAmount(amount);
	}
}
