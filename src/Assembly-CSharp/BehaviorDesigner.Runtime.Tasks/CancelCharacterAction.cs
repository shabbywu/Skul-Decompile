using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("실행중인 액션을 취소합니다. character.CancelAction()")]
public sealed class CancelCharacterAction : Action
{
	[SerializeField]
	private SharedCharacter _character;

	public override TaskStatus OnUpdate()
	{
		if (_character != null)
		{
			((SharedVariable<Character>)_character).Value.CancelAction();
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
