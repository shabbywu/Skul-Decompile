namespace Characters.Abilities.Weapons;

public class PrisonerLancePassiveComponent : AbilityComponent<PrisonerLancePassive>
{
	public void StartDetect()
	{
		base.baseAbility.StartDetect();
	}

	public void StopDetect()
	{
		base.baseAbility.StopDetect();
	}
}
