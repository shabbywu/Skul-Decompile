using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBDKeepDistance : Policy
{
	[SerializeField]
	private BehaviorTree _tree;

	[SerializeField]
	private string _ownerValueName = "Owner";

	[SerializeField]
	private string _targetValueName = "Target";

	[SerializeField]
	private CustomFloat _distance;

	private Vector2 _default => Vector2.op_Implicit(((Component)this).transform.position);

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		SharedCharacter obj = ((Behavior)_tree).GetVariable(_ownerValueName) as SharedCharacter;
		Character value = ((SharedVariable<Character>)(((Behavior)_tree).GetVariable(_targetValueName) as SharedCharacter)).Value;
		Character value2 = ((SharedVariable<Character>)obj).Value;
		BoxCollider2D collider = value2.collider;
		Collider2D collider2 = value2.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)collider2 == (Object)null)
		{
			value2.movement.TryGetClosestBelowCollider(out collider2, Layers.footholdMask);
			if ((Object)(object)collider2 == (Object)null)
			{
				return _default;
			}
		}
		Bounds bounds = collider2.bounds;
		Vector2 val = ((((Component)value).transform.position.x - ((Component)value2).transform.position.x > 0f) ? Vector2.left : Vector2.right);
		float value3 = _distance.value;
		Vector3 position = ((Component)value2).transform.position;
		Bounds bounds2;
		float num;
		if (!(val == Vector2.left))
		{
			float x = ((Bounds)(ref bounds)).max.x;
			bounds2 = ((Collider2D)collider).bounds;
			num = Mathf.Min(x - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)value2.collider).offset.x, position.x + value3);
		}
		else
		{
			float x2 = ((Bounds)(ref bounds)).min.x;
			bounds2 = ((Collider2D)collider).bounds;
			num = Mathf.Max(x2 + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)value2.collider).offset.x, position.x - value3);
		}
		float num2 = num;
		if (val == Vector2.right && ((Bounds)(ref bounds)).max.x < position.x + value3)
		{
			float x3 = ((Bounds)(ref bounds)).min.x;
			bounds2 = ((Collider2D)collider).bounds;
			num2 = Mathf.Max(x3 + ((Bounds)(ref bounds2)).extents.x + ((Collider2D)value2.collider).offset.x, position.x - value3);
		}
		else if (val == Vector2.left && ((Bounds)(ref bounds)).min.x > position.x - value3)
		{
			float x4 = ((Bounds)(ref bounds)).max.x;
			bounds2 = ((Collider2D)collider).bounds;
			num2 = Mathf.Min(x4 - ((Bounds)(ref bounds2)).extents.x + ((Collider2D)value2.collider).offset.x, position.x + value3);
		}
		return new Vector2(num2, ((Component)value2).transform.position.y);
	}
}
