using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnChargeAction : Trigger
{
	public enum Timing
	{
		Start,
		End
	}

	[SerializeField]
	private Timing _timing;

	[SerializeField]
	private ActionTypeBoolArray _types;

	private Character _character;

	public OnChargeAction()
	{
	}

	public OnChargeAction(ActionTypeBoolArray types)
	{
		_types = types;
	}

	public override void Attach(Character character)
	{
		_character = character;
		if (_timing == Timing.Start)
		{
			Character character2 = _character;
			character2.onStartCharging = (Action<Characters.Actions.Action>)Delegate.Combine(character2.onStartCharging, new Action<Characters.Actions.Action>(OnCharacterCharging));
		}
		else if (_timing == Timing.End)
		{
			Character character3 = _character;
			character3.onCancelCharging = (Action<Characters.Actions.Action>)Delegate.Combine(character3.onCancelCharging, new Action<Characters.Actions.Action>(OnCharacterCharging));
			Character character4 = _character;
			character4.onStopCharging = (Action<Characters.Actions.Action>)Delegate.Combine(character4.onStopCharging, new Action<Characters.Actions.Action>(OnCharacterCharging));
		}
	}

	public override void Detach()
	{
		if (_timing == Timing.Start)
		{
			Character character = _character;
			character.onStartCharging = (Action<Characters.Actions.Action>)Delegate.Remove(character.onStartCharging, new Action<Characters.Actions.Action>(OnCharacterCharging));
		}
		else if (_timing == Timing.End)
		{
			Character character2 = _character;
			character2.onCancelCharging = (Action<Characters.Actions.Action>)Delegate.Remove(character2.onCancelCharging, new Action<Characters.Actions.Action>(OnCharacterCharging));
			Character character3 = _character;
			character3.onStopCharging = (Action<Characters.Actions.Action>)Delegate.Remove(character3.onStopCharging, new Action<Characters.Actions.Action>(OnCharacterCharging));
		}
	}

	private void OnCharacterCharging(Characters.Actions.Action action)
	{
		if (((EnumArray<Characters.Actions.Action.Type, bool>)_types).GetOrDefault(action.type))
		{
			Invoke();
		}
	}
}
