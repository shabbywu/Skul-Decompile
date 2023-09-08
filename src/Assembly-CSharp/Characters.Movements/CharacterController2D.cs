using Level;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Movements;

public class CharacterController2D : MonoBehaviour
{
	public class CollisionState
	{
		internal readonly ManualCollisionDetector aboveCollisionDetector = new ManualCollisionDetector();

		internal readonly ManualCollisionDetector belowCollisionDetector = new ManualCollisionDetector();

		internal readonly ManualCollisionDetector leftCollisionDetector = new ManualCollisionDetector();

		internal readonly ManualCollisionDetector rightCollisionDetector = new ManualCollisionDetector();

		public bool above => aboveCollisionDetector.colliding;

		public bool below => belowCollisionDetector.colliding;

		public bool right => rightCollisionDetector.colliding;

		public bool left => leftCollisionDetector.colliding;

		public bool horizontal
		{
			get
			{
				if (!right)
				{
					return left;
				}
				return true;
			}
		}

		public bool vertical
		{
			get
			{
				if (!right)
				{
					return left;
				}
				return true;
			}
		}

		public bool any
		{
			get
			{
				if (!below && !right && !left)
				{
					return above;
				}
				return true;
			}
		}

		public Collider2D lastStandingCollider { get; internal set; }

		internal CollisionState()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown

	}

	[HideInInspector]
	public bool ignorePlatform;

	[HideInInspector]
	public bool ignoreAbovePlatform = true;

	private const float _extraRayLength = 0.001f;

	[Range(0.001f, 0.3f)]
	private float _skinWidth = 0.03f;

	[FormerlySerializedAs("platformMask")]
	public LayerMask terrainMask = LayerMask.op_Implicit(0);

	public LayerMask triggerMask = LayerMask.op_Implicit(0);

	public LayerMask oneWayPlatformMask = LayerMask.op_Implicit(0);

	public float jumpingThreshold = 0.07f;

	[Range(2f, 20f)]
	public int horizontalRays = 8;

	[Range(2f, 20f)]
	public int verticalRays = 4;

	[SerializeField]
	protected BoxCollider2D _boxCollider;

	[SerializeField]
	protected Rigidbody2D _rigidBody;

	public readonly CollisionState collisionState = new CollisionState();

	[HideInInspector]
	public Vector3 velocity;

	private BoxSequenceNonAllocCaster _boxCaster;

	private Bounds _bounds;

	public bool isGrounded
	{
		get
		{
			if (collisionState.below)
			{
				return velocity.y <= 0.001f;
			}
			return false;
		}
	}

	public bool onTerrain
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (collisionState.below)
			{
				return terrainMask.Contains(((Component)collisionState.lastStandingCollider).gameObject.layer);
			}
			return false;
		}
	}

	public bool onPlatform
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (collisionState.below)
			{
				return oneWayPlatformMask.Contains(((Component)collisionState.lastStandingCollider).gameObject.layer);
			}
			return false;
		}
	}

	public LayerMask lastStandingMask { get; set; } = LayerMask.op_Implicit(0);


	private void Awake()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)_boxCollider).bounds;
		((Bounds)(ref bounds)).center = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
		_boxCaster = new BoxSequenceNonAllocCaster(1, horizontalRays, verticalRays);
		_boxCaster.SetOriginsFromBounds(bounds);
		lastStandingMask = LayerMask.op_Implicit(LayerMask.op_Implicit(terrainMask) | LayerMask.op_Implicit(oneWayPlatformMask));
	}

	public void ResetBounds()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((Bounds)(ref _bounds)).size = Vector2.op_Implicit(Vector2.zero);
		_boxCaster.SetOriginsFromBounds(_bounds);
	}

	public void UpdateBounds()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)_boxCollider).bounds;
		((Bounds)(ref bounds)).center = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
		if (!(_bounds == bounds) && _boxCaster != null)
		{
			_boxCaster.origin = ((Component)this).transform.position;
			UpdateTopCasterPosition(bounds);
			UpdateBottomCasterPosition(bounds);
			UpdateLeftCasterPosition(bounds);
			UpdateRightCasterPosition(bounds);
			_boxCaster.SetOriginsFromBounds(_bounds);
		}
	}

	private void UpdateTopCasterPosition(Bounds bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mostLeftTop = bounds.GetMostLeftTop();
		Vector2 mostRightTop = bounds.GetMostRightTop();
		LineSequenceNonAllocCaster topRaycaster = _boxCaster.topRaycaster;
		float num = mostLeftTop.y;
		if (topRaycaster.start.y < mostLeftTop.y - _skinWidth)
		{
			((ContactFilter2D)(ref topRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
			topRaycaster.CastToLine(mostLeftTop, mostRightTop);
			for (int i = 0; i < topRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = topRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					float num2 = num;
					RaycastHit2D val = results[0];
					num = math.min(num2, ((RaycastHit2D)(ref val)).point.y - _boxCaster.origin.y);
				}
			}
			num -= _skinWidth;
			if (num < ((Bounds)(ref _bounds)).max.y)
			{
				return;
			}
		}
		Vector3 max = ((Bounds)(ref _bounds)).max;
		max.y = num;
		((Bounds)(ref _bounds)).max = max;
	}

	private void UpdateBottomCasterPosition(Bounds bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mostLeftBottom = bounds.GetMostLeftBottom();
		Vector2 mostRightBottom = bounds.GetMostRightBottom();
		LineSequenceNonAllocCaster bottomRaycaster = _boxCaster.bottomRaycaster;
		float num = mostLeftBottom.y;
		if (bottomRaycaster.start.y > mostLeftBottom.y + _skinWidth)
		{
			((ContactFilter2D)(ref bottomRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
			bottomRaycaster.CastToLine(mostLeftBottom, mostRightBottom);
			for (int i = 0; i < bottomRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = bottomRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					float num2 = num;
					RaycastHit2D val = results[0];
					num = math.max(num2, ((RaycastHit2D)(ref val)).point.y - _boxCaster.origin.y);
				}
			}
			num += _skinWidth;
			if (num > ((Bounds)(ref _bounds)).min.y)
			{
				return;
			}
		}
		Vector3 min = ((Bounds)(ref _bounds)).min;
		min.y = num;
		((Bounds)(ref _bounds)).min = min;
	}

	private void UpdateLeftCasterPosition(Bounds bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mostLeftTop = bounds.GetMostLeftTop();
		Vector2 mostLeftBottom = bounds.GetMostLeftBottom();
		LineSequenceNonAllocCaster leftRaycaster = _boxCaster.leftRaycaster;
		float num = mostLeftTop.x;
		if (leftRaycaster.start.x > mostLeftTop.x + _skinWidth)
		{
			((ContactFilter2D)(ref leftRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
			leftRaycaster.CastToLine(mostLeftTop, mostLeftBottom);
			for (int i = 0; i < leftRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = leftRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					float num2 = num;
					RaycastHit2D val = results[0];
					num = math.max(num2, ((RaycastHit2D)(ref val)).point.x - _boxCaster.origin.x);
				}
			}
			num += _skinWidth;
			if (num > ((Bounds)(ref _bounds)).min.x)
			{
				return;
			}
		}
		Vector3 min = ((Bounds)(ref _bounds)).min;
		min.x = num;
		((Bounds)(ref _bounds)).min = min;
	}

	private void UpdateRightCasterPosition(Bounds bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mostRightTop = bounds.GetMostRightTop();
		Vector2 mostRightBottom = bounds.GetMostRightBottom();
		LineSequenceNonAllocCaster rightRaycaster = _boxCaster.rightRaycaster;
		float num = mostRightTop.x;
		if (rightRaycaster.start.x < mostRightTop.x - _skinWidth)
		{
			((ContactFilter2D)(ref rightRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
			rightRaycaster.CastToLine(mostRightTop, mostRightBottom);
			for (int i = 0; i < rightRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = rightRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					float num2 = num;
					RaycastHit2D val = results[0];
					num = math.min(num2, ((RaycastHit2D)(ref val)).point.x - _boxCaster.origin.x);
				}
			}
			num -= _skinWidth;
			if (num < ((Bounds)(ref _bounds)).max.x)
			{
				return;
			}
		}
		Vector3 max = ((Bounds)(ref _bounds)).max;
		max.x = num;
		((Bounds)(ref _bounds)).max = max;
	}

	public Vector2 Move(Vector2 deltaMovement)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 origin = ((Component)this).transform.position;
		Move(ref origin, ref deltaMovement);
		origin.x += deltaMovement.x;
		origin.y += deltaMovement.y;
		Map instance = Map.Instance;
		if ((Object)(object)instance != (Object)null && !instance.IsInMap(origin))
		{
			Debug.LogWarning((object)("The new position of character " + ((Object)this).name + " is out of the map. The move was ignored."));
			return Vector2.zero;
		}
		((Component)this).transform.position = origin;
		return deltaMovement;
	}

	private bool TeleportUponGround(Vector2 direction, float distance, bool recursive)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (recursive)
		{
			Vector2 val = Vector2.op_Implicit(((Component)this).transform.position);
			while (distance > 0f)
			{
				if (TeleportUponGround(val + direction * distance))
				{
					return true;
				}
				distance -= 1f;
			}
		}
		else
		{
			TeleportUponGround(Vector2.op_Implicit(((Component)this).transform.position) + direction * distance);
		}
		return false;
	}

	public bool TeleportUponGround(Vector2 destination, float distance = 4f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D val = Physics2D.Raycast(destination, Vector2.down, distance, LayerMask.op_Implicit(terrainMask) | LayerMask.op_Implicit(oneWayPlatformMask));
		if (RaycastHit2D.op_Implicit(val))
		{
			destination = ((RaycastHit2D)(ref val)).point;
			destination.y += _skinWidth * 2f;
			return Teleport(destination);
		}
		return false;
	}

	public bool Teleport(Vector2 destination, float maxRetryDistance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.Vector3ToVector2(((Component)this).transform.position) - destination;
		return Teleport(destination, ((Vector2)(ref val)).normalized, maxRetryDistance);
	}

	public bool Teleport(Vector2 destination, Vector2 direction, float maxRetryDistance)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; (float)i <= maxRetryDistance; i++)
		{
			if (Teleport(destination + direction * (float)i))
			{
				return true;
			}
		}
		return false;
	}

	public bool Teleport(Vector2 destination)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)_boxCollider).bounds;
		((Bounds)(ref bounds)).center = Vector2.op_Implicit(new Vector2(destination.x, destination.y + (((Bounds)(ref bounds)).center.y - ((Bounds)(ref bounds)).min.y)));
		((ContactFilter2D)(ref NonAllocOverlapper.shared.contactFilter)).SetLayerMask(terrainMask);
		if (NonAllocOverlapper.shared.OverlapBox(Vector2.op_Implicit(((Bounds)(ref bounds)).center), Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f).results.Count == 0)
		{
			((Component)this).transform.position = Vector2.op_Implicit(destination);
			return true;
		}
		return false;
	}

	public bool IsInTerrain()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref NonAllocOverlapper.shared.contactFilter)).SetLayerMask(terrainMask);
		return NonAllocOverlapper.shared.OverlapBox(Vector2.op_Implicit(((Component)this).transform.position + ((Bounds)(ref _bounds)).center), Vector2.op_Implicit(((Bounds)(ref _bounds)).size), 0f).results.Count > 0;
	}

	private void Move(ref Vector3 origin, ref Vector2 deltaMovement)
	{
		int num = 0;
		bool flag;
		do
		{
			flag = false;
			num++;
			if (!CastLeft(ref origin, ref deltaMovement))
			{
				origin.x += 0.1f * (float)num;
				flag = true;
			}
			if (!CastRight(ref origin, ref deltaMovement))
			{
				origin.x -= 0.1f * (float)num;
				flag = true;
			}
			if (!CastUp(ref origin, ref deltaMovement))
			{
				origin.y -= 0.1f * (float)num;
				flag = true;
			}
			if (!CastDown(ref origin, ref deltaMovement))
			{
				origin.y += 0.1f * (float)num;
				flag = true;
			}
		}
		while (flag && num < 30);
	}

	private bool CastRight(ref Vector3 origin, ref Vector2 deltaMovement)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		float num = _skinWidth + 0.001f;
		if (deltaMovement.x > 0f)
		{
			num += deltaMovement.x;
		}
		((ContactFilter2D)(ref _boxCaster.rightRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
		_boxCaster.rightRaycaster.caster.origin = Vector2.op_Implicit(origin);
		_boxCaster.rightRaycaster.caster.distance = num;
		_boxCaster.rightRaycaster.Cast();
		using (collisionState.rightCollisionDetector.scope)
		{
			for (int i = 0; i < _boxCaster.rightRaycaster.nonAllocCasters.Count; i++)
			{
				NonAllocCaster val = _boxCaster.rightRaycaster.nonAllocCasters[i];
				if (val.results.Count == 0)
				{
					continue;
				}
				RaycastHit2D val2 = val.results[0];
				if (RaycastHit2D.op_Implicit(val2))
				{
					collisionState.rightCollisionDetector.Add(val2);
					if (((RaycastHit2D)(ref val2)).distance == 0f)
					{
						return false;
					}
					deltaMovement.x = math.min(deltaMovement.x, ((RaycastHit2D)(ref val2)).distance - _skinWidth);
				}
			}
		}
		return true;
	}

	private bool CastLeft(ref Vector3 origin, ref Vector2 deltaMovement)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		float num = _skinWidth + 0.001f;
		if (deltaMovement.x < 0f)
		{
			num += 0f - deltaMovement.x;
		}
		((ContactFilter2D)(ref _boxCaster.leftRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
		_boxCaster.leftRaycaster.caster.origin = Vector2.op_Implicit(origin);
		_boxCaster.leftRaycaster.caster.distance = num;
		_boxCaster.leftRaycaster.Cast();
		using (collisionState.leftCollisionDetector.scope)
		{
			for (int i = 0; i < _boxCaster.leftRaycaster.nonAllocCasters.Count; i++)
			{
				NonAllocCaster val = _boxCaster.leftRaycaster.nonAllocCasters[i];
				if (val.results.Count == 0)
				{
					continue;
				}
				RaycastHit2D val2 = val.results[0];
				if (RaycastHit2D.op_Implicit(val2))
				{
					collisionState.leftCollisionDetector.Add(val2);
					if (((RaycastHit2D)(ref val2)).distance == 0f)
					{
						return false;
					}
					deltaMovement.x = math.max(deltaMovement.x, 0f - ((RaycastHit2D)(ref val2)).distance + _skinWidth);
				}
			}
		}
		return true;
	}

	private bool CastUp(ref Vector3 origin, ref Vector2 deltaMovement)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		float num = _skinWidth + 0.001f;
		if (deltaMovement.y > 0f)
		{
			num += deltaMovement.y;
		}
		if (ignoreAbovePlatform)
		{
			((ContactFilter2D)(ref _boxCaster.topRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
		}
		else
		{
			((ContactFilter2D)(ref _boxCaster.topRaycaster.caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(LayerMask.op_Implicit(terrainMask) | LayerMask.op_Implicit(oneWayPlatformMask)));
		}
		_boxCaster.topRaycaster.caster.origin = Vector2.op_Implicit(origin);
		_boxCaster.topRaycaster.caster.distance = num;
		_boxCaster.topRaycaster.Cast();
		using (collisionState.aboveCollisionDetector.scope)
		{
			for (int i = 0; i < _boxCaster.topRaycaster.nonAllocCasters.Count; i++)
			{
				NonAllocCaster val = _boxCaster.topRaycaster.nonAllocCasters[i];
				if (val.results.Count == 0)
				{
					continue;
				}
				RaycastHit2D val2 = val.results[0];
				if (RaycastHit2D.op_Implicit(val2))
				{
					collisionState.aboveCollisionDetector.Add(val2);
					if (((RaycastHit2D)(ref val2)).distance == 0f)
					{
						return false;
					}
					deltaMovement.y = math.min(deltaMovement.y, ((RaycastHit2D)(ref val2)).distance - _skinWidth);
				}
			}
		}
		return true;
	}

	private bool CastDown(ref Vector3 origin, ref Vector2 deltaMovement)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		float num = _skinWidth + 0.001f;
		if (deltaMovement.y < 0f)
		{
			num += 0f - deltaMovement.y;
		}
		if (ignorePlatform)
		{
			((ContactFilter2D)(ref _boxCaster.bottomRaycaster.caster.contactFilter)).SetLayerMask(terrainMask);
		}
		else
		{
			((ContactFilter2D)(ref _boxCaster.bottomRaycaster.caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(LayerMask.op_Implicit(terrainMask) | LayerMask.op_Implicit(oneWayPlatformMask)));
		}
		_boxCaster.bottomRaycaster.caster.origin = Vector2.op_Implicit(origin);
		_boxCaster.bottomRaycaster.caster.distance = num;
		_boxCaster.bottomRaycaster.Cast();
		using (collisionState.belowCollisionDetector.scope)
		{
			for (int i = 0; i < _boxCaster.bottomRaycaster.nonAllocCasters.Count; i++)
			{
				NonAllocCaster val = _boxCaster.bottomRaycaster.nonAllocCasters[i];
				if (val.results.Count == 0)
				{
					continue;
				}
				RaycastHit2D val2 = val.results[0];
				if (RaycastHit2D.op_Implicit(val2))
				{
					if (lastStandingMask.Contains(((Component)((RaycastHit2D)(ref val2)).collider).gameObject.layer))
					{
						collisionState.lastStandingCollider = ((RaycastHit2D)(ref val2)).collider;
					}
					collisionState.belowCollisionDetector.Add(val2);
					if (((RaycastHit2D)(ref val2)).distance == 0f)
					{
						return false;
					}
					if (deltaMovement.y < 0f)
					{
						deltaMovement.y = math.max(deltaMovement.y, 0f - ((RaycastHit2D)(ref val2)).distance + _skinWidth);
					}
				}
			}
		}
		return true;
	}
}
