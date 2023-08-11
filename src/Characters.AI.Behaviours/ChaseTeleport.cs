using System.Collections;
using Characters.Actions;
using Level;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class ChaseTeleport : Behaviour
{
	private enum Type
	{
		RandomDestination,
		BackOfTarget
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	[Teleport.Subcomponent(true)]
	private Behaviour _teleport;

	[SerializeField]
	private Transform _destinationTransform;

	[SerializeField]
	private Collider2D _teleportBoundsCollider;

	[SerializeField]
	private Action _actionForCooldown;

	private Bounds _teleportBounds;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_teleportBounds = _teleportBoundsCollider.bounds;
		_destinationTransform.parent = ((Component)Map.Instance).transform;
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		_destinationTransform.position = SetDestination(controller);
		yield return _teleport.CRun(controller);
		base.result = Result.Done;
	}

	private Vector3 SetDestination(AIController controller)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Character target = controller.target;
		switch (_type)
		{
		case Type.BackOfTarget:
		{
			Collider2D lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)lastStandingCollider != (Object)null)
			{
				float x = ((Component)controller.target).transform.position.x;
				Bounds bounds2 = lastStandingCollider.bounds;
				return Vector2.op_Implicit(new Vector2(x, ((Bounds)(ref bounds2)).max.y));
			}
			return ((Component)target).transform.position;
		}
		case Type.RandomDestination:
		{
			if ((Object)(object)target == (Object)null)
			{
				return ((Component)controller).transform.position;
			}
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector3 val = ((Component)target).transform.position - ((Bounds)(ref _teleportBounds)).size / 2f;
			Vector3 val2 = ((Component)target).transform.position + ((Bounds)(ref _teleportBounds)).size / 2f;
			return new Vector3(Random.Range(Mathf.Max(((Bounds)(ref bounds)).min.x, val.x), Mathf.Min(((Bounds)(ref bounds)).max.x, val2.x)), ((Bounds)(ref bounds)).max.y);
		}
		default:
			return _destinationTransform.position;
		}
	}

	public bool CanUse()
	{
		return _actionForCooldown.cooldown.canUse;
	}
}
