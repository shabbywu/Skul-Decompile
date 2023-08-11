using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}TurnOnEdge.png")]
[TaskDescription("뒤를 바라본다.")]
public sealed class TurnAround : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	private Character _ownerValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override TaskStatus OnUpdate()
	{
		_ownerValue.ForceToLookAt((_ownerValue.lookingDirection != Character.LookingDirection.Left) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		return (TaskStatus)2;
	}
}
