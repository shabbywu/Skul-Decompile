using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class TakeAim : CharacterOperation
{
	[SerializeField]
	private Transform _centerAxisPosition;

	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private bool _platformTarget;

	[SerializeField]
	private bool _lastStandingCollider = true;

	[SerializeField]
	private LayerMask _groundMask = Layers.groundMask;

	public override void Run(Character owner)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Character target = _controller.target;
		Bounds bounds;
		float num;
		if (_platformTarget)
		{
			Collider2D collider;
			if (_lastStandingCollider)
			{
				bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
				num = ((Bounds)(ref bounds)).max.y;
			}
			else if (target.movement.TryGetClosestBelowCollider(out collider, _groundMask))
			{
				bounds = collider.bounds;
				num = ((Bounds)(ref bounds)).max.y;
			}
			else
			{
				num = ((Component)target).transform.position.y;
			}
		}
		else
		{
			float y = ((Component)target).transform.position.y;
			bounds = ((Collider2D)target.collider).bounds;
			num = y + ((Bounds)(ref bounds)).extents.y;
		}
		Vector3 val = new Vector3(((Component)target).transform.position.x, num) - ((Component)_centerAxisPosition).transform.position;
		float num2 = Mathf.Atan2(val.y, val.x) * 57.29578f;
		_centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, num2);
	}
}
