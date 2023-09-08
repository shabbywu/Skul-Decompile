using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class ScaleByPlatformSize : CharacterOperation
{
	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private BoxCollider2D _targetCollider;

	[SerializeField]
	private Transform _minScale;

	[SerializeField]
	private Transform _maxScale;

	[SerializeField]
	private float _multiplier;

	[SerializeField]
	private LayerMask _groundMask = Layers.groundMask;

	private static NonAllocCaster _belowCaster;

	static ScaleByPlatformSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_belowCaster = new NonAllocCaster(1);
	}

	public override void Run(Character owner)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		Bounds? boundsBelowPlatform = GetBoundsBelowPlatform();
		if (!boundsBelowPlatform.HasValue)
		{
			return;
		}
		Bounds value = boundsBelowPlatform.Value;
		if (((Bounds)(ref value)).size.x / _multiplier > _maxScale.localScale.x)
		{
			_target.localScale = Vector2.op_Implicit(new Vector2(_maxScale.localScale.x, _target.localScale.y));
			return;
		}
		value = boundsBelowPlatform.Value;
		if (((Bounds)(ref value)).size.x / _multiplier < _minScale.localScale.x)
		{
			_target.localScale = Vector2.op_Implicit(new Vector2(_minScale.localScale.x, _target.localScale.y));
			return;
		}
		Transform target = _target;
		value = boundsBelowPlatform.Value;
		target.localScale = Vector2.op_Implicit(new Vector2(((Bounds)(ref value)).size.x / _multiplier, _target.localScale.y));
	}

	private Bounds? GetBoundsBelowPlatform(float distance = 1f)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _belowCaster.contactFilter)).SetLayerMask(_groundMask);
		_belowCaster.BoxCast(Vector2.op_Implicit(((Component)_origin).transform.position), _targetCollider.size, 0f, Vector2.down, distance);
		ReadonlyBoundedList<RaycastHit2D> results = _belowCaster.results;
		if (results.Count <= 0)
		{
			return null;
		}
		int num = -1;
		float num2 = float.MaxValue;
		RaycastHit2D val;
		for (int i = 0; i < results.Count; i++)
		{
			val = results[i];
			float distance2 = ((RaycastHit2D)(ref val)).distance;
			if (distance2 < num2)
			{
				num2 = distance2;
				num = i;
			}
		}
		if (num == -1)
		{
			return null;
		}
		val = results[num];
		return ((RaycastHit2D)(ref val)).collider.bounds;
	}
}
