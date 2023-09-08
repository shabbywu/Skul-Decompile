using System;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnConsumeShield : Trigger
{
	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		_character.health.onConsumeShield += HandleOnConsumeShield;
	}

	public override void Detach()
	{
		_character.health.onConsumeShield -= HandleOnConsumeShield;
	}

	private void HandleOnConsumeShield()
	{
		Invoke();
	}
}
