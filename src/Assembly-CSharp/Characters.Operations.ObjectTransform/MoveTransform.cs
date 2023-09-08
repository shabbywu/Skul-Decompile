using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class MoveTransform : CharacterOperation
{
	[SerializeField]
	[Range(-100f, 100f)]
	private float _xValue;

	[Range(-100f, 100f)]
	[SerializeField]
	private float _yValue;

	[SerializeField]
	private Transform _targetTransform;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		_targetTransform.position += new Vector3((owner.lookingDirection == Character.LookingDirection.Right) ? _xValue : (0f - _xValue), _yValue, 0f);
	}
}
