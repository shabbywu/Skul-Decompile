using Characters.Abilities;

public interface ISavableAbility
{
	float remainTime { get; set; }

	float stack { get; set; }

	IAbility ability { get; }
}
