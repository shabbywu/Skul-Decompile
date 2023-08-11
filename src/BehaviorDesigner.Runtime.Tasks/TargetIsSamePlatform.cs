using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("행동 주체와 타겟이 같은 플랫폼인가?")]
public class TargetIsSamePlatform : Conditional
{
	[Tooltip("행동 주체")]
	[SerializeField]
	private SharedCharacter _owner;

	[Tooltip("타겟")]
	[SerializeField]
	private SharedCharacter _target;

	private Character _ownerValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override TaskStatus OnUpdate()
	{
		Character value = ((SharedVariable<Character>)_target).Value;
		if (!((Object)(object)value == (Object)null))
		{
			Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
			if (!((Object)(object)lastStandingCollider == (Object)null))
			{
				Collider2D lastStandingCollider2 = _ownerValue.movement.controller.collisionState.lastStandingCollider;
				if (!((Object)(object)lastStandingCollider2 == (Object)null))
				{
					if (!((Object)(object)lastStandingCollider != (Object)(object)lastStandingCollider2))
					{
						return (TaskStatus)2;
					}
					return (TaskStatus)1;
				}
				return (TaskStatus)1;
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)1;
	}
}
