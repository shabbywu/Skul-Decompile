using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class RotateAngle : CharacterOperation
{
	[SerializeField]
	private Transform _centerAxisPosition;

	[SerializeField]
	[Range(0f, 90f)]
	private float _angle = 5f;

	[SerializeField]
	private bool _isAdded;

	public override void Run(Character owner)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Quaternion rotation = _centerAxisPosition.rotation;
		Vector3 eulerAngles = ((Quaternion)(ref rotation)).eulerAngles;
		_centerAxisPosition.rotation = Quaternion.Euler(eulerAngles + new Vector3(0f, 0f, (!_isAdded) ? _angle : (0f - _angle)));
	}
}
