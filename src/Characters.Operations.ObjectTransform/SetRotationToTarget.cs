using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class SetRotationToTarget : CharacterOperation
{
	[SerializeField]
	private Transform _rotateTransform;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private CustomFloat _customOffsetAngle;

	[SerializeField]
	private bool _flipXByRotateTransformDirection = true;

	[SerializeField]
	private bool _flipYByRotateTransformDirection;

	public override void Run(Character owner)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_target == (Object)null) && !((Object)(object)_rotateTransform == (Object)null))
		{
			Vector3 val = _target.position - _rotateTransform.position;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			if (_flipXByRotateTransformDirection)
			{
				num += (float)((_rotateTransform.lossyScale.x < 0f) ? (-180) : 0);
			}
			if (_flipYByRotateTransformDirection)
			{
				num *= (float)((!(_rotateTransform.lossyScale.x < 0f)) ? 1 : (-1));
			}
			_rotateTransform.localRotation = Quaternion.Euler(0f, 0f, num + _customOffsetAngle.value);
		}
	}
}
