using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public class TakeAimObject : CharacterOperation
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _centerAxisPosition;

	[SerializeField]
	private bool _platformTarget;

	public override void Run(Character owner)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_target == (Object)null)
		{
			_target = ((Component)Singleton<Service>.Instance.levelManager.player).transform;
		}
		float y;
		if (_platformTarget)
		{
			Collider2D val = FindTargetPlatform();
			if ((Object)(object)val == (Object)null)
			{
				return;
			}
			Bounds bounds = val.bounds;
			y = ((Bounds)(ref bounds)).max.y;
		}
		else
		{
			y = ((Component)_target).transform.position.y;
		}
		Vector3 val2 = new Vector3(((Component)_target).transform.position.x, y) - ((Component)_centerAxisPosition).transform.position;
		float num = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
		_centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, num);
	}

	private Collider2D FindTargetPlatform()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(_target.position), Vector2.down, float.PositiveInfinity, LayerMask.op_Implicit(Layers.groundMask));
		if (RaycastHit2D.op_Implicit(val))
		{
			return ((RaycastHit2D)(ref val)).collider;
		}
		return null;
	}
}
