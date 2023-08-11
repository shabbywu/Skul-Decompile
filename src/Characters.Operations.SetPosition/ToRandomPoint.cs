using System.Collections.Generic;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToRandomPoint : Policy
{
	[SerializeField]
	private Transform[] _transforms;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.op_Implicit(ExtensionMethods.Random<Transform>((IEnumerable<Transform>)_transforms).position);
	}
}
