using Characters;
using Characters.AI;
using UnityEngine;

namespace BT;

public sealed class MoveToLookingDirection : Node
{
	[SerializeField]
	private bool _turnOnEdge = true;

	protected override NodeState UpdateDeltatime(Context context)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		Character character = context.Get<Character>(Key.OwnerCharacter);
		if ((Object)(object)character == (Object)null)
		{
			return NodeState.Fail;
		}
		Vector2 direction = ((character.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
		if (_turnOnEdge)
		{
			character.movement.TurnOnEdge(ref direction);
		}
		if (Precondition.CanMove(character))
		{
			character.movement.MoveHorizontal(direction);
		}
		return NodeState.Success;
	}
}
