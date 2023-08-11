using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class SetFlyDirectionFromTarget : Action
{
	public enum RotateMethod
	{
		Constant,
		Lerp,
		Slerp
	}

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedVector2 _moveDirection;

	[SerializeField]
	private RotateMethod _rotateMethod;

	[SerializeField]
	private float _rotateSpeed = 2f;

	private Quaternion _rotation;

	public override TaskStatus OnUpdate()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		if (_owner != null && _target != null)
		{
			Character value = ((SharedVariable<Character>)_owner).Value;
			Bounds bounds = ((Collider2D)((SharedVariable<Character>)_target).Value.collider).bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			bounds = ((Collider2D)value.collider).bounds;
			Vector3 val = center - ((Bounds)(ref bounds)).center;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			switch (_rotateMethod)
			{
			case RotateMethod.Constant:
				_rotation = Quaternion.RotateTowards(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _rotateSpeed * 100f * Time.deltaTime);
				break;
			case RotateMethod.Lerp:
				_rotation = Quaternion.Lerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _rotateSpeed * Time.deltaTime);
				break;
			case RotateMethod.Slerp:
				_rotation = Quaternion.Slerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _rotateSpeed * Time.deltaTime);
				break;
			}
			Vector2 val2 = Vector2.op_Implicit(_rotation * Vector2.op_Implicit(Vector2.right));
			((SharedVariable)_moveDirection).SetValue((object)val2);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
