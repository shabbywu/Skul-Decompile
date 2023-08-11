using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("타겟이 Grounded인지 판별합니다.")]
public class IsTargetGrounded : Conditional
{
	[SerializeField]
	private SharedCharacter _target;

	public override TaskStatus OnUpdate()
	{
		Character value = ((SharedVariable<Character>)_target).Value;
		if (!((Object)(object)value == (Object)null))
		{
			if (value.movement.isGrounded)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)1;
	}
}
