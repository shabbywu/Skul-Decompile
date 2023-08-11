using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedVector3Int : SharedVariable<Vector3Int>
{
	public static implicit operator SharedVector3Int(Vector3Int value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new SharedVector3Int
		{
			mValue = value
		};
	}
}
