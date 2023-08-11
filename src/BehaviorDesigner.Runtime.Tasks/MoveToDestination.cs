using Characters;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class MoveToDestination : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedTransform _destinationTransform;

	[SerializeField]
	private float minimumDistanceValue = 0.1f;

	[SerializeField]
	private bool _stopOnPlatformEdge;

	private Character _ownerValue;

	private Transform _destinationTransformValue;

	private static NonAllocCaster _belowCaster;

	private LayerMask _groundMask = Layers.groundMask;

	private Collider2D _destinationCollider;

	static MoveToDestination()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_belowCaster = new NonAllocCaster(1);
	}

	public override void OnStart()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_destinationTransformValue = ((SharedVariable<Transform>)_destinationTransform).Value;
		_destinationCollider = PlatformUtils.GetClosestPlatform(Vector2.op_Implicit(_destinationTransformValue.position), Vector2.down, _belowCaster, _groundMask);
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_ownerValue == (Object)null) && !((Object)(object)_destinationTransformValue == (Object)null))
		{
			if (!((Object)(object)_ownerValue.movement.controller.collisionState.lastStandingCollider == (Object)null))
			{
				if (!((Object)(object)_destinationCollider != (Object)(object)_ownerValue.movement.controller.collisionState.lastStandingCollider))
				{
					Vector3 val = _destinationTransformValue.position - ((Component)_ownerValue).transform.position;
					Vector2 val2 = Vector2.op_Implicit(((Vector3)(ref val)).normalized);
					float num = _ownerValue.stat.GetInterpolatedMovementSpeed() * ((ChronometerBase)_ownerValue.chronometer.master).deltaTime;
					Vector2 val3 = val2 * num;
					float num2 = Vector2.Distance(Vector2.op_Implicit(_destinationTransformValue.position), Vector2.op_Implicit(((Component)_ownerValue).transform.position));
					float num3 = Vector2.Distance(Vector2.op_Implicit(_destinationTransformValue.position), Vector2.op_Implicit(((Component)_ownerValue).transform.position) + val3);
					if (!(num2 <= num3))
					{
						if (!(num2 <= minimumDistanceValue))
						{
							if (_stopOnPlatformEdge)
							{
								Bounds bounds = ((Collider2D)_ownerValue.collider).bounds;
								float num4 = ((Bounds)(ref bounds)).max.x + num;
								bounds = _ownerValue.movement.controller.collisionState.lastStandingCollider.bounds;
								if (num4 >= ((Bounds)(ref bounds)).max.x && _ownerValue.lookingDirection == Character.LookingDirection.Right)
								{
									return (TaskStatus)1;
								}
								bounds = ((Collider2D)_ownerValue.collider).bounds;
								float num5 = ((Bounds)(ref bounds)).min.x + num;
								bounds = _ownerValue.movement.controller.collisionState.lastStandingCollider.bounds;
								if (num5 <= ((Bounds)(ref bounds)).min.x && _ownerValue.lookingDirection == Character.LookingDirection.Left)
								{
									return (TaskStatus)1;
								}
							}
							_ownerValue.movement.Move(val2);
							return (TaskStatus)3;
						}
						return (TaskStatus)2;
					}
					return (TaskStatus)2;
				}
				return (TaskStatus)1;
			}
			return (TaskStatus)3;
		}
		return (TaskStatus)1;
	}
}
