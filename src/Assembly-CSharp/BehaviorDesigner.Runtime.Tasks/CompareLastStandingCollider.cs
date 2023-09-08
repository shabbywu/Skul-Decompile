using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Owner와 Target의 lastStandingCollider를 비교합니다.lastStandingCollider이 둘중 하나라도 Null일 경우는 Failure를 반환합니다.")]
public class CompareLastStandingCollider : Conditional
{
	public enum Operation
	{
		Equal,
		NotEqual
	}

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCharacter _target;

	public Operation operation;

	public override TaskStatus OnUpdate()
	{
		if (_target == null || _owner == null)
		{
			return (TaskStatus)1;
		}
		Character value = ((SharedVariable<Character>)_owner).Value;
		Character value2 = ((SharedVariable<Character>)_target).Value;
		Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
		Collider2D lastStandingCollider2 = value2.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider2 == (Object)null || (Object)(object)lastStandingCollider == (Object)null)
		{
			return (TaskStatus)1;
		}
		switch (operation)
		{
		case Operation.Equal:
			if ((Object)(object)lastStandingCollider == (Object)(object)lastStandingCollider2)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.NotEqual:
			if ((Object)(object)lastStandingCollider != (Object)(object)lastStandingCollider2)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		default:
			return (TaskStatus)1;
		}
	}
}
