using System;
using Characters.Actions;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnDashEvade : Trigger
{
	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		_character.onEvade += OnEvade;
	}

	private void OnEvade(ref Damage damage)
	{
		foreach (Characters.Actions.Action action in _character.actions)
		{
			if (action.type == Characters.Actions.Action.Type.Dash && action.running)
			{
				Invoke();
				break;
			}
		}
	}

	public override void Detach()
	{
		_character.onEvade -= OnEvade;
	}
}
