using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("적이 같은 플랫폼에 올 때 까지 주변을 돌아다닌다.")]
[TaskIcon("{SkinColor}StackedActionIcon.png")]
public sealed class RangeWander : Action
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedVector2 _wanderDistanceRange;

	[SerializeField]
	private SharedVector2 _idleTimeRange;

	[SerializeField]
	private SharedCollider _stopTrigger;

	[SerializeField]
	private TargetLayer _targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private Character _characterValue;

	private Collider2D _stopTriggerValue;

	private Vector2 _idleTimeRangeValue;

	private Vector2 _wanderDistanceRangeValue;

	private Vector3 _center;

	private bool _rightDirection;

	private bool _needDirectionUpdate;

	private float _remainIdleTime;

	private float _destinationX;

	public override void OnAwake()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		_characterValue = ((SharedVariable<Character>)_character).Value;
		_stopTriggerValue = ((SharedVariable<Collider2D>)_stopTrigger).Value;
		_idleTimeRangeValue = ((SharedVariable<Vector2>)_idleTimeRange).Value;
		_wanderDistanceRangeValue = ((SharedVariable<Vector2>)_wanderDistanceRange).Value;
		_needDirectionUpdate = true;
	}

	public override void OnStart()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_center = ((Component)_characterValue).transform.position;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		if (!_characterValue.stunedOrFreezed)
		{
			if (CheckStopWander())
			{
				_needDirectionUpdate = true;
				return (TaskStatus)2;
			}
			if (!Precondition.CanMove(_characterValue))
			{
				_needDirectionUpdate = true;
				return (TaskStatus)3;
			}
			if (_needDirectionUpdate)
			{
				if (_remainIdleTime > 0f)
				{
					_remainIdleTime -= ((ChronometerBase)_characterValue.chronometer.master).deltaTime;
					return (TaskStatus)3;
				}
				Collider2D lastStandingCollider = _characterValue.movement.controller.collisionState.lastStandingCollider;
				if ((Object)(object)lastStandingCollider == (Object)null)
				{
					return (TaskStatus)1;
				}
				Bounds bounds = lastStandingCollider.bounds;
				float num = Random.Range(_wanderDistanceRangeValue.x, _wanderDistanceRangeValue.y);
				Bounds bounds2;
				if (_rightDirection)
				{
					float num2 = _center.x + num;
					float x = ((Bounds)(ref bounds)).max.x;
					bounds2 = ((Collider2D)_characterValue.collider).bounds;
					_destinationX = Mathf.Min(num2, x - ((Bounds)(ref bounds2)).size.x / 2f);
				}
				else
				{
					float num3 = _center.x - num;
					float x2 = ((Bounds)(ref bounds)).min.x;
					bounds2 = ((Collider2D)_characterValue.collider).bounds;
					_destinationX = Mathf.Max(num3, x2 + ((Bounds)(ref bounds2)).size.x / 2f);
				}
				_needDirectionUpdate = false;
				if (CheckReachedToDestination(1f))
				{
					Turn();
					return (TaskStatus)3;
				}
			}
			if (CheckReachedToDestination(0.5f))
			{
				Turn();
				return (TaskStatus)3;
			}
			_characterValue.movement.MoveTo(new Vector2(_destinationX, ((Component)_characterValue).transform.position.y));
			return (TaskStatus)3;
		}
		return (TaskStatus)3;
	}

	private bool CheckReachedToDestination(float distance)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return Mathf.Abs(((Component)_characterValue).transform.position.x - _destinationX) < distance;
	}

	private void Turn()
	{
		_remainIdleTime = Random.Range(_idleTimeRangeValue.x, _idleTimeRangeValue.y);
		_needDirectionUpdate = true;
		_rightDirection = !_rightDirection;
	}

	private bool CheckStopWander()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (Precondition.CanChase(_characterValue, ((SharedVariable<Character>)_target).Value))
		{
			return true;
		}
		if (Object.op_Implicit((Object)(object)TargetFinder.FindClosestTarget(_stopTriggerValue, (Collider2D)(object)_characterValue.collider, _targetLayer.Evaluate(((Component)_characterValue).gameObject))))
		{
			return true;
		}
		return false;
	}
}
