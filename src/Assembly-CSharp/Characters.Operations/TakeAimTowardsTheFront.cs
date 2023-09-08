using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class TakeAimTowardsTheFront : CharacterOperation
{
	[SerializeField]
	private Transform _centerAxisPosition;

	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private bool _platformTarget;

	private float _originalDirection;

	private void Awake()
	{
		_originalDirection = 0f;
	}

	public override void Run(Character owner)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		Character target = _controller.target;
		Bounds bounds;
		float num;
		if (_platformTarget)
		{
			bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			num = ((Bounds)(ref bounds)).max.y;
		}
		else
		{
			float y = ((Component)target).transform.position.y;
			bounds = ((Collider2D)target.collider).bounds;
			num = y + ((Bounds)(ref bounds)).extents.y;
		}
		Vector3 val = new Vector3(((Component)target).transform.position.x, num) - ((Component)_centerAxisPosition).transform.position;
		float num2 = (num2 = Mathf.Atan2(val.y, val.x) * 57.29578f);
		if (owner.lookingDirection == Character.LookingDirection.Left)
		{
			num2 = 180f - num2;
		}
		_centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, _originalDirection + num2);
	}
}
