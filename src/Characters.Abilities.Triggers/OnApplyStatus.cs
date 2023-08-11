using System;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnApplyStatus : Trigger
{
	private enum Timing
	{
		Apply,
		Refresh,
		Release
	}

	[SerializeField]
	private PositionInfo _positionInfo = new PositionInfo();

	[SerializeField]
	private Transform _moveToTargetPoint;

	[SerializeField]
	private Timing _timing;

	[SerializeField]
	private CharacterStatus.Kind _kind;

	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		CharacterStatus.Timing timing = CharacterStatus.Timing.Apply;
		switch (_timing)
		{
		case Timing.Apply:
			timing = CharacterStatus.Timing.Apply;
			break;
		case Timing.Refresh:
			timing = CharacterStatus.Timing.Refresh;
			break;
		case Timing.Release:
			timing = CharacterStatus.Timing.Release;
			break;
		}
		_character.status.Register(_kind, timing, Invoke);
	}

	public override void Detach()
	{
		CharacterStatus.Timing timing = CharacterStatus.Timing.Apply;
		switch (_timing)
		{
		case Timing.Apply:
			timing = CharacterStatus.Timing.Apply;
			break;
		case Timing.Refresh:
			timing = CharacterStatus.Timing.Refresh;
			break;
		case Timing.Release:
			timing = CharacterStatus.Timing.Release;
			break;
		}
		_character.status.Unregister(_kind, timing, Invoke);
	}

	public void Invoke(Character giver, Character target)
	{
		_positionInfo.Attach(target, _moveToTargetPoint);
		Invoke();
	}
}
