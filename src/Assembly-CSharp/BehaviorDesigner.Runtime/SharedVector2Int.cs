using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedVector2Int : SharedVariable<Vector2Int>
{
	public static implicit operator SharedVector2Int(Vector2Int value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new SharedVector2Int
		{
			mValue = value
		};
	}
}
