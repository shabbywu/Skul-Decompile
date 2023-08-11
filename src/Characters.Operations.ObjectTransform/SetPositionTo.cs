using Characters.Operations.SetPosition;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class SetPositionTo : CharacterOperation
{
	private enum Type
	{
		Object,
		Owner
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private TargetInfo _targetInfo;

	public override void Run(Character owner)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (_type == Type.Owner)
		{
			_object = ((Component)owner).transform;
		}
		Vector2 position = _targetInfo.GetPosition(owner);
		_object.position = Vector2.op_Implicit(position);
	}
}
