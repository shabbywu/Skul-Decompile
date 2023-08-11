using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBDTarget : Policy
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _targetName = "Target";

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _lastStandingCollider;

	[SerializeField]
	private Collider2D _interplateCollider;

	private Vector2 _default => Vector2.op_Implicit(((Component)this).transform.position);

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetName)).Value;
		if ((Object)(object)value == (Object)null)
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(((Component)value).transform.position);
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = value.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				value.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
				if ((Object)(object)collider == (Object)null)
				{
					return _default;
				}
			}
		}
		else
		{
			value.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
			if ((Object)(object)collider == (Object)null)
			{
				return _default;
			}
		}
		float num = 0f;
		Bounds bounds;
		if ((Object)(object)_interplateCollider != (Object)null)
		{
			bounds = collider.bounds;
			float x = ((Bounds)(ref bounds)).min.x;
			bounds = _interplateCollider.bounds;
			float min = x + ((Bounds)(ref bounds)).extents.x;
			bounds = collider.bounds;
			float x2 = ((Bounds)(ref bounds)).max.x;
			bounds = _interplateCollider.bounds;
			float max = x2 - ((Bounds)(ref bounds)).extents.x;
			num = ClampX(((Component)value).transform.position.x, min, max);
		}
		else
		{
			num = ((Component)value).transform.position.x;
		}
		bounds = collider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		return new Vector2(num, y);
	}

	private float ClampX(float x, float min, float max)
	{
		float num = 0.05f;
		if (x <= min)
		{
			return min + num;
		}
		if (x >= max)
		{
			return max - num;
		}
		return x;
	}
}
