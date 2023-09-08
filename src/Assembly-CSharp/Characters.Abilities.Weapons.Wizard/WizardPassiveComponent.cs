namespace Characters.Abilities.Weapons.Wizard;

public sealed class WizardPassiveComponent : AbilityComponent<WizardPassive>
{
	public bool transcendence
	{
		get
		{
			return base.baseAbility.transcendence;
		}
		set
		{
			base.baseAbility.transcendence = value;
		}
	}

	public float manaChargingSpeedMultiplier
	{
		get
		{
			return base.baseAbility.manaChargingSpeedMultiplier;
		}
		set
		{
			base.baseAbility.manaChargingSpeedMultiplier = value;
		}
	}

	public bool IsMaxGauge()
	{
		return base.baseAbility.IsMaxGauge();
	}

	public bool TryReduceMana(float value)
	{
		return base.baseAbility.TryReduceMana(value);
	}
}
