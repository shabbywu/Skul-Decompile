using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("{SkinColor}TurnOnEdge.png")]
public sealed class LookTarget : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedVector2 _moveDirection;

	public override TaskStatus OnUpdate()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_owner).Value;
		Character value2 = ((SharedVariable<Character>)_target).Value;
		if (!((Object)(object)value2 == (Object)null))
		{
			if (((Component)value2).transform.position.x > ((Component)value).transform.position.x)
			{
				((SharedVariable)_moveDirection).SetValue((object)Vector2.right);
			}
			else
			{
				((SharedVariable)_moveDirection).SetValue((object)Vector2.left);
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
