using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class SetRotationTo : CharacterOperation
{
	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private CustomFloat _rotation;

	public override void Run(Character owner)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_transform.rotation = Quaternion.Euler(0f, 0f, _rotation.value);
	}
}
