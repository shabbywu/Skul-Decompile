using Characters;
using Characters.AI;
using UnityEngine;

namespace BT;

public sealed class MoveTowards : Node
{
	[SerializeField]
	[Range(0f, 1f)]
	private float _rightChance;

	[SerializeField]
	private bool _turnOnEdge = true;

	protected override NodeState UpdateDeltatime(Context context)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		Character character = context.Get<Character>(Key.OwnerCharacter);
		Vector2 direction = ((!MMMaths.Chance(_rightChance)) ? Vector2.left : Vector2.right);
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
