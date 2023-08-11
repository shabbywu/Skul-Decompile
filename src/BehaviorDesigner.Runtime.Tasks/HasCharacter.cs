using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("{SkinColor}TurnOnEdge.png")]
public sealed class HasCharacter : Action
{
	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private bool _onSamePlatform;

	public override TaskStatus OnUpdate()
	{
		Character value = ((SharedVariable<Character>)_target).Value;
		if (!((Object)(object)value == (Object)null))
		{
			if (_onSamePlatform)
			{
				Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
				if ((Object)(object)lastStandingCollider == (Object)null)
				{
					return (TaskStatus)1;
				}
				if ((Object)(object)((SharedVariable<Character>)_owner).Value.movement.controller.collisionState.lastStandingCollider != (Object)(object)lastStandingCollider)
				{
					return (TaskStatus)1;
				}
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
