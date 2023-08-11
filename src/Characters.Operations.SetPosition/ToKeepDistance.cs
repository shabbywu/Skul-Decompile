using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToKeepDistance : Policy
{
	private enum BasedCharacter
	{
		Owner,
		Target
	}

	private enum Direction
	{
		Looking,
		ReverseLooking
	}

	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private string _targetValueName = "Target";

	[SerializeField]
	private BasedCharacter _basedCharacter;

	[SerializeField]
	private Direction _direction;

	[SerializeField]
	private CustomFloat _distance;

	[SerializeField]
	private bool _lastStandingCollider;

	private float _epsilon = 0.05f;

	private Vector2 _default => Vector2.op_Implicit(((Component)this).transform.position);

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetValueName)).Value;
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = value.movement.controller.collisionState.lastStandingCollider;
		}
		else
		{
			value.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
		}
		if ((Object)(object)collider == (Object)null)
		{
			return _default;
		}
		Bounds bounds = collider.bounds;
		float value2 = _distance.value;
		BoxCollider2D collider2 = _owner.collider;
		Vector2 val = ((_basedCharacter == BasedCharacter.Owner) ? ((_direction != 0) ? ((_owner.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left) : ((_owner.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right)) : ((_direction != 0) ? ((value.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left) : ((value.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right)));
		Bounds bounds2;
		Vector2 val2 = default(Vector2);
		if (val == Vector2.left)
		{
			float num = ((Component)value).transform.position.x - value2;
			bounds2 = ((Collider2D)collider2).bounds;
			if (num + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x > ((Bounds)(ref bounds)).min.x)
			{
				float num2 = ((Component)value).transform.position.x - value2;
				bounds2 = ((Collider2D)collider2).bounds;
				val2.x = num2 + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x + _epsilon;
			}
			else
			{
				float num3 = ((Component)value).transform.position.x + value2;
				bounds2 = ((Collider2D)collider2).bounds;
				if (num3 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x < ((Bounds)(ref bounds)).max.x)
				{
					float num4 = ((Component)value).transform.position.x + value2;
					bounds2 = ((Collider2D)collider2).bounds;
					val2.x = num4 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x - _epsilon;
				}
				else
				{
					float x = ((Bounds)(ref bounds)).min.x;
					bounds2 = ((Collider2D)collider2).bounds;
					val2.x = x + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x + _epsilon;
				}
			}
		}
		else
		{
			float num5 = ((Component)value).transform.position.x + value2;
			bounds2 = ((Collider2D)collider2).bounds;
			if (num5 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x < ((Bounds)(ref bounds)).max.x)
			{
				float num6 = ((Component)value).transform.position.x + value2;
				bounds2 = ((Collider2D)collider2).bounds;
				val2.x = num6 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x - _epsilon;
			}
			else
			{
				float num7 = ((Component)value).transform.position.x - value2;
				bounds2 = ((Collider2D)collider2).bounds;
				if (num7 + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x > ((Bounds)(ref bounds)).min.x)
				{
					float num8 = ((Component)value).transform.position.x - value2;
					bounds2 = ((Collider2D)collider2).bounds;
					val2.x = num8 + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x + _epsilon;
				}
				else
				{
					float x2 = ((Bounds)(ref bounds)).max.x;
					bounds2 = ((Collider2D)collider2).bounds;
					val2.x = x2 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x - _epsilon;
				}
			}
		}
		float x3 = val2.x;
		float x4 = ((Bounds)(ref bounds)).max.x;
		bounds2 = ((Collider2D)collider2).bounds;
		float max = x4 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x - _epsilon;
		float x5 = ((Bounds)(ref bounds)).min.x;
		bounds2 = ((Collider2D)collider2).bounds;
		val2.x = ClampX(x3, max, x5 + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)collider2).offset.x + _epsilon);
		if (_lastStandingCollider)
		{
			val2.y = ((Bounds)(ref bounds)).max.y;
		}
		else
		{
			val2.y = ((Component)_owner).transform.position.y;
		}
		return new Vector2(val2.x, val2.y);
	}

	private float ClampX(float x, float max, float min)
	{
		if (x > max)
		{
			return max;
		}
		if (x < min)
		{
			return min;
		}
		return x;
	}
}
