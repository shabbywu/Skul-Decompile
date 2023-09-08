using System.Collections.Generic;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Movement;

public class Teleport : CharacterOperation
{
	private enum Type
	{
		TeleportUponGround,
		Teleport
	}

	private enum DistanceType
	{
		Constant,
		TargetDistance,
		Range
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private DistanceType _distanceType;

	[SerializeField]
	private Collider2D _distanceArea;

	[SerializeField]
	private float _minDistance;

	[SerializeField]
	private float _maxDistance;

	[SerializeField]
	private Transform _targetPosition;

	private static readonly NonAllocOverlapper _nonAllocOverlapper;

	static Teleport()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_nonAllocOverlapper = new NonAllocOverlapper(15);
	}

	private void Awake()
	{
		if (_maxDistance <= 0f)
		{
			_maxDistance = float.PositiveInfinity;
		}
	}

	public override void Run(Character owner)
	{
		if (_distanceType == DistanceType.Range)
		{
			TeleportByRange(owner);
		}
		else if (_distanceType == DistanceType.Constant)
		{
			TeleportByDistanceInPlatform(owner);
		}
		else if (_distanceType == DistanceType.TargetDistance)
		{
			TeleportByTarget(owner);
		}
	}

	private void TeleportByRange(Character owner)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds;
		if (_type == Type.Teleport)
		{
			Transform targetPosition = _targetPosition;
			bounds = _distanceArea.bounds;
			Vector3 min = ((Bounds)(ref bounds)).min;
			bounds = _distanceArea.bounds;
			targetPosition.position = MMMaths.RandomVector3(min, ((Bounds)(ref bounds)).max);
			owner.movement.controller.Teleport(Vector2.op_Implicit(_targetPosition.position));
			return;
		}
		((ContactFilter2D)(ref _nonAllocOverlapper.contactFilter)).SetLayerMask(Layers.groundMask);
		ReadonlyBoundedList<Collider2D> results = _nonAllocOverlapper.OverlapCollider(_distanceArea).results;
		if (results.Count == 0)
		{
			Debug.LogError((object)"Failed to teleport, you can widen distanceArea");
			return;
		}
		List<Collider2D> list = new List<Collider2D>(results.Count);
		Collider2D lastStandingCollider = owner.movement.controller.collisionState.lastStandingCollider;
		foreach (Collider2D item in results)
		{
			bounds = Map.Instance.bounds;
			float x = ((Bounds)(ref bounds)).min.x;
			bounds = item.bounds;
			if (x > ((Bounds)(ref bounds)).min.x)
			{
				continue;
			}
			bounds = Map.Instance.bounds;
			float x2 = ((Bounds)(ref bounds)).max.x;
			bounds = item.bounds;
			if (x2 < ((Bounds)(ref bounds)).max.x)
			{
				continue;
			}
			ColliderDistance2D val = Physics2D.Distance(item, (Collider2D)(object)owner.collider);
			if ((Object)(object)lastStandingCollider == (Object)(object)item)
			{
				float num = ((Component)owner).transform.position.x + _minDistance;
				bounds = lastStandingCollider.bounds;
				if (!(num < ((Bounds)(ref bounds)).max.x))
				{
					float num2 = ((Component)owner).transform.position.x - _minDistance;
					bounds = lastStandingCollider.bounds;
					if (!(num2 > ((Bounds)(ref bounds)).min.x))
					{
						continue;
					}
				}
				list.Add(item);
			}
			else if (((ColliderDistance2D)(ref val)).distance >= _minDistance)
			{
				list.Add(item);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError((object)"Failed to teleport, you can widen distanceArea");
			return;
		}
		int index = Random.Range(0, list.Count);
		Bounds bounds2 = list[index].bounds;
		if (bounds2 == lastStandingCollider.bounds)
		{
			float num3 = ((Component)owner).transform.position.x + _minDistance;
			bounds = lastStandingCollider.bounds;
			bool num4 = num3 < ((Bounds)(ref bounds)).max.x;
			float num5 = ((Component)owner).transform.position.x - _minDistance;
			bounds = lastStandingCollider.bounds;
			bool flag = num5 > ((Bounds)(ref bounds)).min.x;
			float num6;
			if (num4 && (!flag || MMMaths.RandomBool()))
			{
				bounds = lastStandingCollider.bounds;
				num6 = Random.Range(Mathf.Max(((Bounds)(ref bounds)).min.x, ((Component)owner).transform.position.x - _maxDistance), ((Component)owner).transform.position.x - _minDistance);
			}
			else
			{
				float num7 = ((Component)owner).transform.position.x + _minDistance;
				bounds = lastStandingCollider.bounds;
				num6 = Random.Range(num7, Mathf.Min(((Bounds)(ref bounds)).max.x, ((Component)owner).transform.position.x + _maxDistance));
			}
			Transform targetPosition2 = _targetPosition;
			float num8 = num6;
			bounds = lastStandingCollider.bounds;
			targetPosition2.position = Vector2.op_Implicit(new Vector2(num8, ((Bounds)(ref bounds)).max.y));
		}
		else
		{
			_targetPosition.position = Vector2.op_Implicit(new Vector2(Random.Range(((Bounds)(ref bounds2)).min.x, ((Bounds)(ref bounds2)).max.x), ((Bounds)(ref bounds2)).max.y));
		}
		owner.movement.controller.TeleportUponGround(Vector2.op_Implicit(_targetPosition.position));
	}

	private void TeleportByTarget(Character owner)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (_type == Type.Teleport)
		{
			owner.movement.controller.Teleport(Vector2.op_Implicit(_targetPosition.position));
		}
		else
		{
			owner.movement.controller.TeleportUponGround(FilterDest(owner, Vector2.op_Implicit(_targetPosition.position)));
		}
	}

	private float FilterDestX(float extends, float position)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = Map.Instance.bounds;
		if (position + extends >= ((Bounds)(ref bounds)).max.x)
		{
			return position - extends;
		}
		if (position - extends <= ((Bounds)(ref bounds)).min.x)
		{
			return position + extends;
		}
		return position;
	}

	private Vector2 FilterDest(Character owner, Vector2 position)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)owner.collider).bounds;
		return new Vector2(FilterDestX(((Bounds)(ref bounds)).extents.x, position.x), position.y);
	}

	private void TeleportByDistanceInPlatform(Character owner)
	{
	}
}
