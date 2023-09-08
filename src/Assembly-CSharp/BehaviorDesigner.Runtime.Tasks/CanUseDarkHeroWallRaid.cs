using Characters;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Player의 현재 높이가 발판에서 얼만큼 떨어져 있는가")]
public sealed class CanUseDarkHeroWallRaid : Conditional
{
	private enum Compare
	{
		GreatherThan,
		LessThan
	}

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedTransform _originTransform;

	[SerializeField]
	private SharedTransform _destinationTransform;

	[SerializeField]
	private SharedFloat _height;

	[SerializeField]
	private Compare _comparer;

	private NonAllocCaster _nonAllocCaster;

	public override void OnAwake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		_nonAllocCaster = new NonAllocCaster(1);
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Bounds bounds = ((Collider2D)player.collider).bounds;
		Vector2 val = Vector2.op_Implicit(((Bounds)(ref bounds)).center - ((SharedVariable<Transform>)_originTransform).Value.position);
		float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		Character value = ((SharedVariable<Character>)_owner).Value;
		if (num < 0f)
		{
			num += 360f;
		}
		if (value.lookingDirection == Character.LookingDirection.Right)
		{
			if (num > 90f && num < 270f)
			{
				val = Vector2.down;
			}
			else if (num > 0f && num < 90f)
			{
				val = Vector2.right;
			}
		}
		else if (value.lookingDirection == Character.LookingDirection.Left)
		{
			if (num < 90f || num > 270f)
			{
				val = Vector2.down;
			}
			else if (num > 90f && num < 180f)
			{
				val = Vector2.left;
			}
		}
		_nonAllocCaster.RayCast(Vector2.op_Implicit(((Component)((SharedVariable<Transform>)_originTransform).Value).transform.position), val, 30f);
		Transform value2 = ((SharedVariable<Transform>)_destinationTransform).Value;
		Bounds bounds2 = player.movement.controller.collisionState.lastStandingCollider.bounds;
		if (_nonAllocCaster.results.Count == 0)
		{
			value2.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds2)).center.x, ((Bounds)(ref bounds2)).max.y));
			return (TaskStatus)1;
		}
		RaycastHit2D val2 = _nonAllocCaster.results[0];
		Vector2 point = ((RaycastHit2D)(ref val2)).point;
		if (point.x >= ((Component)value).transform.position.x)
		{
			float x = point.x;
			bounds = ((Collider2D)value.collider).bounds;
			((Vector2)(ref point))._002Ector(x - ((Bounds)(ref bounds)).extents.x, point.y);
		}
		else
		{
			float x2 = point.x;
			bounds = ((Collider2D)value.collider).bounds;
			((Vector2)(ref point))._002Ector(x2 + ((Bounds)(ref bounds)).extents.x, point.y);
		}
		value2.position = Vector2.op_Implicit(point);
		float num2 = point.y - ((Bounds)(ref bounds2)).max.y;
		switch (_comparer)
		{
		case Compare.GreatherThan:
			if (!(num2 > ((SharedVariable<float>)_height).Value))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		case Compare.LessThan:
			if (!(num2 < ((SharedVariable<float>)_height).Value))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		default:
			return (TaskStatus)1;
		}
	}
}
