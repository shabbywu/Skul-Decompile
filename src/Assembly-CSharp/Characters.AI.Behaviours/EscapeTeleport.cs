using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class EscapeTeleport : Behaviour
{
	[SerializeField]
	[UnityEditor.Subcomponent(typeof(Teleport))]
	private Teleport _teleport;

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
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character target = controller.target;
		Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
		Vector3 val = ((Component)target).transform.position - ((Bounds)(ref _teleportBounds)).size / 2f;
		Vector3 val2 = ((Component)target).transform.position + ((Bounds)(ref _teleportBounds)).size / 2f;
		float num = Random.Range(Mathf.Max(((Bounds)(ref bounds)).min.x, val.x), Mathf.Min(((Bounds)(ref bounds)).max.x, val2.x));
		_destinationTransform.position = new Vector3(num, ((Bounds)(ref bounds)).max.y);
		yield return _teleport.CRun(controller);
	}

	public bool CanUse()
	{
		return _actionForCooldown.cooldown.canUse;
	}
}
