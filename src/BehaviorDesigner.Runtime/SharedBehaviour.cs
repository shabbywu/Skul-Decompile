using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedBehaviour : SharedVariable<Behaviour>
{
	public static explicit operator SharedBehaviour(Behaviour value)
	{
		return new SharedBehaviour
		{
			mValue = value
		};
	}
}
