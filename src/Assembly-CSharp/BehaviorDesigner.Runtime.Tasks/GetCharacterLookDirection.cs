using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class GetCharacterLookDirection : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedVector2 _lookingDirection;

	public override TaskStatus OnUpdate()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_owner).Value;
		if (!((Object)(object)value == (Object)null))
		{
			((SharedVariable)_lookingDirection).SetValue((object)((value.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left));
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
