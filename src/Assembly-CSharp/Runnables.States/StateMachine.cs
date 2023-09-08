using UnityEngine;

namespace Runnables.States;

public class StateMachine : MonoBehaviour
{
	[SerializeField]
	[Header("초기값 설정")]
	private State _state;

	public State currentState => _state;

	public void TransitTo(State state)
	{
		_state = state;
	}
}
