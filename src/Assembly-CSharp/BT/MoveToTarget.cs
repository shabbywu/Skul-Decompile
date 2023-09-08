using Characters;
using UnityEngine;

namespace BT;

public class MoveToTarget : Node
{
	[SerializeField]
	private bool _turnOnEdge;

	[SerializeField]
	private bool _stopOnCloseToTarget;

	[SerializeField]
	private Collider2D _stopTrigger;

	[SerializeField]
	private float _stopDistance;

	protected override NodeState UpdateDeltatime(Context context)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		Character character = context.Get<Character>(Key.Target);
		Character character2 = context.Get<Character>(Key.OwnerCharacter);
		if ((Object)(object)character == (Object)null)
		{
			return NodeState.Fail;
		}
		if (character.health.dead)
		{
			return NodeState.Fail;
		}
		if (_stopOnCloseToTarget)
		{
			if (Mathf.Abs(((Component)character2).transform.position.x - ((Component)character).transform.position.x) <= _stopDistance)
			{
				return NodeState.Success;
			}
			if ((Object)(object)TargetFinder.GetRandomTarget(_stopTrigger, LayerMask.op_Implicit(1024)) != (Object)null)
			{
				return NodeState.Success;
			}
		}
		Vector2 direction = ((!(((Component)character).transform.position.x > ((Component)character2).transform.position.x)) ? Vector2.left : Vector2.right);
		if (_turnOnEdge)
		{
			character2.movement.TurnOnEdge(ref direction);
		}
		if ((Object)(object)character2.movement.controller.collisionState.lastStandingCollider != (Object)null)
		{
			character2.movement.MoveHorizontal(direction);
		}
		if (_stopOnCloseToTarget)
		{
			return NodeState.Running;
		}
		return NodeState.Success;
	}
}
