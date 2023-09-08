using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class MoveTransformFromPosition : CharacterOperation
{
	[SerializeField]
	private Transform _fromPosition;

	[SerializeField]
	private Transform _targetTransform;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_targetTransform.position = _fromPosition.position;
	}
}
