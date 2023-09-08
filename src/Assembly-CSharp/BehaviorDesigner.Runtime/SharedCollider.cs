using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedCollider : SharedVariable<Collider2D>
{
	public static implicit operator SharedCollider(Collider2D value)
	{
		return new SharedCollider
		{
			mValue = value
		};
	}
}
