using UnityEngine;

namespace Characters.Operations.Customs;

public class ChangeTransformParent : CharacterOperation
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _newParent;

	[SerializeField]
	private bool _resetPosition;

	public override void Run(Character owner)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_target.parent = _newParent;
		if (_resetPosition)
		{
			_target.localPosition = Vector3.zero;
		}
	}
}
