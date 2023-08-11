using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem;

public class Rush : Behaviour, IPattern
{
	[SerializeField]
	private Action _ready;

	[SerializeField]
	private Action _action;

	[SerializeField]
	[Subcomponent(typeof(MoveToDestination))]
	private MoveToDestination _moveToDestination;

	public bool CanUse(AIController controller)
	{
		if (!_action.canUse)
		{
			return false;
		}
		Character target = controller.target;
		Character character = controller.character;
		Collider2D lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
		Collider2D lastStandingCollider2 = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)(object)lastStandingCollider2)
		{
			return true;
		}
		return false;
	}

	public bool CanUse()
	{
		return _action.canUse;
	}

	public override IEnumerator CRun(AIController controller)
	{
		Character target = controller.target;
		Character character = controller.character;
		Collider2D ownerPlatform = character.movement.controller.collisionState.lastStandingCollider;
		character.ForceToLookAt(((Component)target).transform.position.x);
		_ready.TryStart();
		while (_ready.running)
		{
			yield return null;
		}
		_action.TryStart();
		if (((Component)target).transform.position.x > ((Component)character).transform.position.x)
		{
			SetWalkDestinationToMax(controller, ownerPlatform.bounds);
		}
		else
		{
			SetWalkDestinationToMin(controller, ownerPlatform.bounds);
		}
		yield return _moveToDestination.CRun(controller);
		character.CancelAction();
	}

	private void SetWalkDestinationToMin(AIController controller, Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		float x = ((Bounds)(ref bounds)).min.x;
		Bounds bounds2 = ((Collider2D)controller.character.collider).bounds;
		float num = x + ((Bounds)(ref bounds2)).size.x;
		float y = ((Bounds)(ref bounds)).max.y;
		controller.destination = new Vector2(num, y);
	}

	private void SetWalkDestinationToMax(AIController controller, Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		float x = ((Bounds)(ref bounds)).max.x;
		Bounds bounds2 = ((Collider2D)controller.character.collider).bounds;
		float num = x - ((Bounds)(ref bounds2)).size.x;
		float y = ((Bounds)(ref bounds)).max.y;
		controller.destination = new Vector2(num, y);
	}
}
