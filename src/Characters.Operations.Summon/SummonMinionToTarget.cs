using BT;
using BT.SharedValues;
using Characters.Minions;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonMinionToTarget : CharacterOperation
{
	[SerializeField]
	private Minion _minion;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private MinionSetting _overrideSetting;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private int _preloadCount = 1;

	[SerializeField]
	[Space]
	private bool _snapToGround;

	[SerializeField]
	[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
	private float _groundFindingDistance = 7f;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private bool _lastStandingCollider;

	[SerializeField]
	private bool _behind;

	[SerializeField]
	private Collider2D _findRange;

	[SerializeField]
	private Collider2D _minionCollider;

	[SerializeField]
	private CustomFloat _amount;

	private Vector2 _default => Vector2.op_Implicit(((Component)this).transform.position);

	public override void Run(Character owner)
	{
		if (owner.playerComponents != null)
		{
			Spawn(owner);
		}
	}

	private void Spawn(Character owner)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		Character character = TargetFinder.FindClosestTarget(_findRange, _minionCollider, _targetLayer.Evaluate(((Component)_minionCollider).gameObject));
		Context context = Context.Create();
		Vector2 val = GetPosition(character);
		if (_snapToGround)
		{
			RaycastHit2D val2 = Physics2D.Raycast(val, Vector2.down, _groundFindingDistance, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val2))
			{
				val = ((RaycastHit2D)(ref val2)).point;
			}
		}
		Minion minion = owner.playerComponents.minionLeader.Summon(_minion, Vector2.op_Implicit(val), _overrideSetting);
		context.Add(BT.Key.Target, new SharedValue<Character>(character));
		context.Add(BT.Key.OwnerTransform, new SharedValue<Transform>(((Component)minion).transform));
		context.Add(BT.Key.OwnerCharacter, new SharedValue<Character>(minion.character));
		if ((Object)(object)character != (Object)null)
		{
			minion.character.ForceToLookAt(((Component)character).transform.position.x);
			((Component)minion).GetComponentInChildren<BehaviourTreeRunner>().Run(context);
		}
		else
		{
			minion.character.ForceToLookAt(owner.lookingDirection);
			((Component)minion).GetComponentInChildren<BehaviourTreeRunner>().Run();
		}
	}

	private Vector2 GetPosition(Character target)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target == (Object)null)
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		Vector2 result = Vector2.op_Implicit(((Component)target).transform.position);
		Clamp(ref result, _amount.value, target);
		if (!_snapToGround)
		{
			return result;
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = target.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
				if ((Object)(object)collider == (Object)null)
				{
					return _default;
				}
			}
		}
		else
		{
			target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
			if ((Object)(object)collider == (Object)null)
			{
				return _default;
			}
		}
		float x = ((Component)target).transform.position.x;
		Bounds bounds = collider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		return new Vector2(x, y);
	}

	private void Clamp(ref Vector2 result, float amount, Character target)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = target.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _groundFindingDistance);
			}
		}
		else
		{
			target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _groundFindingDistance);
		}
		Bounds bounds = collider.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = _minionCollider.bounds;
		float min = x + ((Bounds)(ref bounds)).size.x;
		bounds = collider.bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		bounds = _minionCollider.bounds;
		float max = x2 - ((Bounds)(ref bounds)).size.x;
		if (target.lookingDirection == Character.LookingDirection.Right)
		{
			result = ClampX(result, _behind ? (result.x - amount) : (result.x + amount), min, max);
		}
		else
		{
			result = ClampX(result, _behind ? (result.x + amount) : (result.x - amount), min, max);
		}
	}

	private Vector2 ClampX(Vector2 result, float x, float min, float max)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.05f;
		if (x <= min)
		{
			((Vector2)(ref result))._002Ector(min + num, result.y);
		}
		else if (x >= max)
		{
			((Vector2)(ref result))._002Ector(max - num, result.y);
		}
		else
		{
			((Vector2)(ref result))._002Ector(x, result.y);
		}
		return result;
	}
}
