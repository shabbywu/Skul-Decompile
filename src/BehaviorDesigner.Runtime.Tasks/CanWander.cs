using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Wander를 할 수 있는가? 현재 땅 위에있지 않거나, 바닥 타일의 크기가 3보다 작을 경우 False")]
public class CanWander : Conditional
{
	[SerializeField]
	[Tooltip("행동 주체")]
	private SharedCharacter _character;

	public override TaskStatus OnUpdate()
	{
		if (Precondition.CanMove(((SharedVariable<Character>)_character).Value))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
