using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class RotateToTarget : CharacterOperation
{
	[SerializeField]
	private Transform _object;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private float _defaultRotation;

	private static readonly NonAllocOverlapper _overlapper;

	static RotateToTarget()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(15);
	}

	public override void Run(Character owner)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Transform closestTargetTransform = GetClosestTargetTransform(owner);
		if ((Object)(object)closestTargetTransform == (Object)null)
		{
			float num = ((owner.lookingDirection == Character.LookingDirection.Right) ? _defaultRotation : (_defaultRotation + 180f));
			_object.rotation = Quaternion.Euler(0f, 0f, num);
		}
		else
		{
			Vector3 val = closestTargetTransform.position - _object.position;
			float num2 = Mathf.Atan2(val.y, val.x) * 57.29578f;
			_object.rotation = Quaternion.Euler(0f, 0f, num2);
		}
	}

	private Transform GetClosestTargetTransform(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		LayerMask layerMask = _targetLayer.Evaluate(((Component)owner).gameObject);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(layerMask);
		List<Target> components = _overlapper.OverlapCollider(_range).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			return null;
		}
		if (components.Count == 1)
		{
			return ((Component)components[0]).transform;
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < components.Count; i++)
		{
			if (!((Object)(object)components[i].character == (Object)null))
			{
				ColliderDistance2D val = Physics2D.Distance((Collider2D)(object)components[i].character.collider, (Collider2D)(object)owner.collider);
				float distance = ((ColliderDistance2D)(ref val)).distance;
				if (num > distance)
				{
					index = i;
					num = distance;
				}
			}
		}
		return ((Component)components[index]).transform;
	}
}
