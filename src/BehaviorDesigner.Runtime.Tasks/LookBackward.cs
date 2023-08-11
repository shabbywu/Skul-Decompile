using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Treu일 경우 캐릭터가 움직일때도 계속 뒤를 봅니다.원래대로 돌리려면 backward를 false로 바꿔줘야합니다.")]
[TaskIcon("{SkinColor}TurnOnEdge.png")]
public sealed class LookBackward : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private bool _backward;

	public override TaskStatus OnUpdate()
	{
		Character value = ((SharedVariable<Character>)_owner).Value;
		if (!((Object)(object)value == (Object)null))
		{
			value.movement.moveBackward = _backward;
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
