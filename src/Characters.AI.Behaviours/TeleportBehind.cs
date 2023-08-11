using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class TeleportBehind : Behaviour
{
	[SerializeField]
	[Subcomponent(typeof(Teleport))]
	private Teleport _teleport;

	[SerializeField]
	private Transform _destinationTransform;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private float _destinationSettingDelay;

	[SerializeField]
	[MinMaxSlider(-10f, 10f)]
	private Vector2 _distance;

	[SerializeField]
	private bool _lastStandingCollider = true;

	[SerializeField]
	private LayerMask _groundMask = Layers.groundMask;

	public override IEnumerator CRun(AIController controller)
	{
		((MonoBehaviour)this).StartCoroutine(SetDestination(controller));
		yield return _teleport.CRun(controller);
	}

	private IEnumerator SetDestination(AIController controller)
	{
		Character target = controller.target;
		float amount = Random.Range(_distance.x, _distance.y);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)controller.character.chronometer.master, _destinationSettingDelay);
		float num = ((target.lookingDirection == Character.LookingDirection.Right) ? (amount * -1f) : amount);
		float num2 = ((Component)target).transform.position.x + num;
		Collider2D collider;
		Bounds val = (_lastStandingCollider ? target.movement.controller.collisionState.lastStandingCollider.bounds : ((!target.movement.TryGetClosestBelowCollider(out collider, _groundMask)) ? controller.character.movement.controller.collisionState.lastStandingCollider.bounds : collider.bounds));
		if (num2 <= ((Bounds)(ref val)).min.x + 0.5f && target.lookingDirection == Character.LookingDirection.Right)
		{
			num2 = ((Bounds)(ref val)).min.x + 0.5f;
		}
		else if (num2 >= ((Bounds)(ref val)).max.x - 0.5f && target.lookingDirection == Character.LookingDirection.Left)
		{
			num2 = ((Bounds)(ref val)).max.x - 0.5f;
		}
		_destinationTransform.position = new Vector3(num2, ((Bounds)(ref val)).max.y + controller.character.collider.size.y);
	}
}
