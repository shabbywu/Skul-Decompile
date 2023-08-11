namespace Characters.Abilities.Statuses;

public abstract class CharacterStatusAbility
{
	public CharacterStatus.OnTimeDelegate onAttached;

	public CharacterStatus.OnTimeDelegate onRefreshed;

	public CharacterStatus.OnTimeDelegate onDetached;

	public StatusEffect.EffectHandler effectHandler { get; set; }

	public Character attacker { get; set; }

	public float durationMultiplier { get; set; } = 1f;


	public abstract event CharacterStatus.OnTimeDelegate onAttachEvents;

	public abstract event CharacterStatus.OnTimeDelegate onRefreshEvents;

	public abstract event CharacterStatus.OnTimeDelegate onDetachEvents;
}
