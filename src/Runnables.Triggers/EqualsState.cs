using Runnables.States;
using UnityEngine;

namespace Runnables.Triggers;

public class EqualsState : Trigger
{
	[SerializeField]
	private StateMachine _stateMachine;

	[SerializeField]
	private State _state;

	protected override bool Check()
	{
		return (Object)(object)_stateMachine.currentState == (Object)(object)_state;
	}
}
