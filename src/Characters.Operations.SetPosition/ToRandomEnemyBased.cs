using System.Collections.Generic;
using Characters.Operations.FindOptions;
using Level;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public sealed class ToRandomEnemyBased : Policy
{
	[SerializeField]
	private Collider2D _characterCollider;

	[SerializeField]
	private float _belowRayDistance = 100f;

	[SerializeField]
	[Header("타겟과의 거리옵션")]
	private bool _behind;

	[SerializeField]
	private CustomFloat _distanceRange;

	[SerializeReference]
	[SubclassSelector]
	[Header("만족 조건")]
	private ICondition[] _condition;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_characterCollider == (Object)null)
		{
			_characterCollider = (Collider2D)(object)owner.collider;
		}
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
		allEnemies.PseudoShuffle();
		Character character = SelectTargetFromCondition(allEnemies);
		if ((Object)(object)character == (Object)null)
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		Vector2.op_Implicit(((Component)character).transform.position);
		return Clamp(_distanceRange.value, character);
	}

	private Character SelectTargetFromCondition(List<Character> allEnemies)
	{
		foreach (Character allEnemy in allEnemies)
		{
			bool flag = true;
			ICondition[] condition = _condition;
			for (int i = 0; i < condition.Length; i++)
			{
				if (!condition[i].Satisfied(allEnemy))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return allEnemy;
			}
		}
		return null;
	}

	private Vector2 Clamp(float amount, Character target)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		Collider2D collider = target.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)collider == (Object)null)
		{
			target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
		}
		Bounds bounds = collider.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = _characterCollider.bounds;
		float min = x + ((Bounds)(ref bounds)).size.x;
		bounds = collider.bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		bounds = _characterCollider.bounds;
		float max = x2 - ((Bounds)(ref bounds)).size.x;
		Vector3 position = ((Component)target).transform.position;
		position = ((target.lookingDirection != 0) ? Vector2.op_Implicit(ClampX(Vector2.op_Implicit(position), _behind ? (position.x + amount) : (position.x - amount), min, max)) : Vector2.op_Implicit(ClampX(Vector2.op_Implicit(position), _behind ? (position.x - amount) : (position.x + amount), min, max)));
		return Vector2.op_Implicit(position);
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
