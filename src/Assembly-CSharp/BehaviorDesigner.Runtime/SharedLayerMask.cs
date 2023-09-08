using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedLayerMask : SharedVariable<LayerMask>
{
	public static implicit operator SharedLayerMask(LayerMask value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return new SharedLayerMask
		{
			Value = value
		};
	}
}
