using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnAction : Trigger
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

	[SerializeField]
	private int _cycle;

	private int _count;

	private Character _character;

	public OnAction()
	{
	}

	public OnAction(Timing timing, ActionTypeBoolArray types)
	{
		_timing = timing;
		_types = types;
	}

	public override void Attach(Character character)
	{
		_character = character;
		_count = 0;
		if (_timing == Timing.Start)
		{
			_character.onStartAction += OnCharacterAction;
		}
		else if (_timing == Timing.End)
		{
			_character.onCancelAction += OnCharacterAction;
			_character.onEndAction += OnCharacterAction;
		}
	}

	public override void Detach()
	{
		if (_timing == Timing.Start)
		{
			_character.onStartAction -= OnCharacterAction;
		}
		else if (_timing == Timing.End)
		{
			_character.onCancelAction -= OnCharacterAction;
			_character.onEndAction -= OnCharacterAction;
		}
	}

	private void OnCharacterAction(Characters.Actions.Action action)
	{
		if (_types.GetOrDefault(action.type) && (action.type != Characters.Actions.Action.Type.Skill || !action.cooldown.usedByStreak))
		{
			_count++;
			if (_count >= _cycle)
			{
				_count = 0;
				Invoke();
			}
		}
	}
}
