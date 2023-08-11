using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public sealed class ToColliderBased : Policy
{
	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	private Collider2D _targetCollider;

	[SerializeField]
	private float _maxRetryDistance;

	[SerializeField]
	private Transform _defaultPosition;

	[SerializeField]
	[Range(1f, 10f)]
	private int _horizontalRayCount;

	[Range(1f, 10f)]
	[SerializeField]
	private int _verticalRayCount;

	private LineSequenceNonAllocCaster _topLineRaycaster;

	private LineSequenceNonAllocCaster _bottomLineRaycaster;

	private LineSequenceNonAllocCaster _leftLineRaycaster;

	private LineSequenceNonAllocCaster _rightLineRaycaster;

	private float _upMinDistance;

	private float _rightMinDistance;

	private float _horizontalMoveDelta;

	private float _verticalMoveDelta;

	private bool _hit;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	private void Awake()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002d: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005a: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0087: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00b4: Expected O, but got Unknown
		_topLineRaycaster = new LineSequenceNonAllocCaster(_verticalRayCount, _verticalRayCount)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.down
			}
		};
		_bottomLineRaycaster = new LineSequenceNonAllocCaster(_verticalRayCount, _verticalRayCount)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.up
			}
		};
		_leftLineRaycaster = new LineSequenceNonAllocCaster(_horizontalRayCount, _horizontalRayCount)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.right
			}
		};
		_rightLineRaycaster = new LineSequenceNonAllocCaster(_horizontalRayCount, _horizontalRayCount)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.left
			}
		};
	}

	public override Vector2 GetPosition()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		_horizontalMoveDelta = 0f;
		_verticalMoveDelta = 0f;
		_hit = false;
		SetBounds();
		CheckTop();
		CheckBottom();
		CheckRight();
		CheckLeft();
		if (!_hit)
		{
			return Vector2.op_Implicit(((Object)(object)_defaultPosition == (Object)null) ? ((Component)this).transform.position : ((Component)_defaultPosition).transform.position);
		}
		return new Vector2(((Component)_targetPoint).transform.position.x + _horizontalMoveDelta, ((Component)_targetPoint).transform.position.y + _verticalMoveDelta);
	}

	private void SetBounds()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _targetCollider.bounds;
		_topLineRaycaster.start = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.y);
		_topLineRaycaster.end = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).max.y);
		_bottomLineRaycaster.start = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).min.y);
		_bottomLineRaycaster.end = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).min.y);
		_leftLineRaycaster.start = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).min.y);
		_leftLineRaycaster.end = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.y);
		_rightLineRaycaster.start = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).min.y);
		_rightLineRaycaster.end = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).max.y);
	}

	private void CheckTop()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _targetCollider.bounds;
		LineSequenceNonAllocCaster topLineRaycaster = _topLineRaycaster;
		((ContactFilter2D)(ref topLineRaycaster.caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
		topLineRaycaster.caster.origin = Vector2.zero;
		topLineRaycaster.caster.distance = ((Bounds)(ref bounds)).size.y;
		topLineRaycaster.Cast();
		float num = ((Bounds)(ref bounds)).size.y;
		_upMinDistance = 0f;
		for (int i = 0; i < topLineRaycaster.nonAllocCasters.Count; i++)
		{
			ReadonlyBoundedList<RaycastHit2D> results = topLineRaycaster.nonAllocCasters[i].results;
			if (results.Count != 0)
			{
				RaycastHit2D val = results[0];
				if (!(((RaycastHit2D)(ref val)).distance >= ((Bounds)(ref bounds)).size.y) && !(((RaycastHit2D)(ref val)).distance <= float.Epsilon))
				{
					_hit = true;
					num = (_upMinDistance = Mathf.Min(num, ((RaycastHit2D)(ref val)).distance));
					_verticalMoveDelta = ((Bounds)(ref bounds)).size.y - num;
				}
			}
		}
	}

	private void CheckBottom()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _targetCollider.bounds;
		LineSequenceNonAllocCaster bottomLineRaycaster = _bottomLineRaycaster;
		((ContactFilter2D)(ref bottomLineRaycaster.caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
		bottomLineRaycaster.caster.origin = Vector2.zero;
		bottomLineRaycaster.caster.distance = ((Bounds)(ref bounds)).size.y;
		bottomLineRaycaster.Cast();
		float num = ((Bounds)(ref bounds)).size.y;
		for (int i = 0; i < bottomLineRaycaster.nonAllocCasters.Count; i++)
		{
			ReadonlyBoundedList<RaycastHit2D> results = bottomLineRaycaster.nonAllocCasters[i].results;
			if (results.Count == 0)
			{
				continue;
			}
			RaycastHit2D val = results[0];
			if (!(((RaycastHit2D)(ref val)).distance >= ((Bounds)(ref bounds)).size.y) && !(((RaycastHit2D)(ref val)).distance <= float.Epsilon))
			{
				_hit = true;
				num = Mathf.Min(num, ((RaycastHit2D)(ref val)).distance);
				if (num > _upMinDistance)
				{
					_verticalMoveDelta = -1f * (((Bounds)(ref bounds)).size.y - num);
				}
			}
		}
	}

	private void CheckRight()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _targetCollider.bounds;
		LineSequenceNonAllocCaster rightLineRaycaster = _rightLineRaycaster;
		((ContactFilter2D)(ref rightLineRaycaster.caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
		rightLineRaycaster.caster.origin = Vector2.zero;
		rightLineRaycaster.caster.distance = ((Bounds)(ref bounds)).size.x;
		rightLineRaycaster.Cast();
		float num = ((Bounds)(ref bounds)).size.x;
		_rightMinDistance = 0f;
		for (int i = 0; i < rightLineRaycaster.nonAllocCasters.Count; i++)
		{
			ReadonlyBoundedList<RaycastHit2D> results = rightLineRaycaster.nonAllocCasters[i].results;
			if (results.Count != 0)
			{
				RaycastHit2D val = results[0];
				if (!(((RaycastHit2D)(ref val)).distance >= ((Bounds)(ref bounds)).size.x) && !(((RaycastHit2D)(ref val)).distance <= float.Epsilon))
				{
					_hit = true;
					num = (_rightMinDistance = Mathf.Min(num, ((RaycastHit2D)(ref val)).distance));
					_horizontalMoveDelta = ((Bounds)(ref bounds)).size.x - num;
				}
			}
		}
	}

	private void CheckLeft()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _targetCollider.bounds;
		LineSequenceNonAllocCaster leftLineRaycaster = _leftLineRaycaster;
		((ContactFilter2D)(ref leftLineRaycaster.caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
		leftLineRaycaster.caster.origin = Vector2.zero;
		leftLineRaycaster.caster.distance = ((Bounds)(ref bounds)).size.x;
		leftLineRaycaster.Cast();
		float num = ((Bounds)(ref bounds)).size.x;
		for (int i = 0; i < leftLineRaycaster.nonAllocCasters.Count; i++)
		{
			ReadonlyBoundedList<RaycastHit2D> results = leftLineRaycaster.nonAllocCasters[i].results;
			if (results.Count == 0)
			{
				continue;
			}
			RaycastHit2D val = results[0];
			if (!(((RaycastHit2D)(ref val)).distance >= ((Bounds)(ref bounds)).size.x) && ((RaycastHit2D)(ref val)).distance != 0f)
			{
				_hit = true;
				num = Mathf.Min(num, ((RaycastHit2D)(ref val)).distance);
				if (num > _rightMinDistance)
				{
					_horizontalMoveDelta = -1f * (((Bounds)(ref bounds)).size.x - num);
				}
			}
		}
	}
}
