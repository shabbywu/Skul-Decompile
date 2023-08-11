using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class TakeAimTargetPlatform : CharacterOperation
{
	[SerializeField]
	private Transform _centerAxisPosition;

	[SerializeField]
	private Transform _weaponAxisPosition;

	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private LayerMask _layerMask = LayerMask.op_Implicit(18);

	[SerializeField]
	private float _distance = 10f;

	private Vector3 _originalScale;

	private float _originalDirection;

	private void Awake()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_originalDirection = 0f;
		_originalScale = Vector3.one;
	}

	public override void Run(Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (_controller.target.movement.TryBelowRayCast(_layerMask, out var point, _distance))
		{
			Vector2 point2 = ((RaycastHit2D)(ref point)).point;
			point2.y += _controller.target.collider.size.y;
			((RaycastHit2D)(ref point)).point = point2;
			Vector2 val = ((RaycastHit2D)(ref point)).point - Vector2.op_Implicit(((Component)_weaponAxisPosition).transform.position);
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			_weaponAxisPosition.rotation = Quaternion.Euler(0f, 0f, _originalDirection + num);
		}
	}
}
