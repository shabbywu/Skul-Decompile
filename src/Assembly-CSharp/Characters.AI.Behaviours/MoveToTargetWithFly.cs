using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveToTargetWithFly : Move
{
	public enum RotateMethod
	{
		Constant,
		Lerp,
		Slerp
	}

	[SerializeField]
	private RotateMethod _rotateMethod;

	[SerializeField]
	private float _rotateSpeed = 2f;

	private Quaternion _rotation;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			if (Object.op_Implicit((Object)(object)controller.FindClosestPlayerBody(controller.stopTrigger)))
			{
				base.result = Result.Fail;
				break;
			}
			if ((Object)(object)controller.target == (Object)null)
			{
				base.result = Result.Fail;
				break;
			}
			yield return null;
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			bounds = ((Collider2D)character.collider).bounds;
			Vector3 val = center - ((Bounds)(ref bounds)).center;
			if (((Vector3)(ref val)).magnitude < 0.1f || LookAround(controller))
			{
				yield return idle.CRun(controller);
				base.result = Result.Success;
				break;
			}
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
			_ = ((Quaternion)(ref _rotation)).eulerAngles;
			Vector3 val2 = _rotation * Vector2.op_Implicit(Vector2.right);
			controller.character.movement.move = Vector2.op_Implicit(val2);
		}
	}
}
