namespace Characters.Abilities.Weapons.Minotaurus;

public sealed class MinotaurusPassiveComponent : AbilityComponent<MinotaurusPassive>
{
	public void StartRecordingAttacks()
	{
		_ability.StartRecordingAttacks();
	}

	public void StopRecodingAttacks()
	{
		_ability.StopRecodingAttacks();
	}
}
