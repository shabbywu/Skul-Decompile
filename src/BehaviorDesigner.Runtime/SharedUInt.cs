using System;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedUInt : SharedVariable<uint>
{
	public static implicit operator SharedUInt(uint value)
	{
		return new SharedUInt
		{
			mValue = value
		};
	}
}
