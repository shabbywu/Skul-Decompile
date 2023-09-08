using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}TurnOnEdge.png")]
[TaskDescription("캐릭터를 바라본다.")]
public sealed class LookCharacter : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCharacter _target;

	public override TaskStatus OnUpdate()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_owner).Value;
		Character value2 = ((SharedVariable<Character>)_target).Value;
		if (!((Object)(object)value2 == (Object)null))
		{
			value.ForceToLookAt(((Component)value2).transform.position.x);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
