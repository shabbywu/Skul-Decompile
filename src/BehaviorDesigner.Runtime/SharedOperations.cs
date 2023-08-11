using System;
using Characters.Operations;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedOperations : SharedVariable<OperationInfos>
{
	public static explicit operator SharedOperations(OperationInfos value)
	{
		return new SharedOperations
		{
			mValue = value
		};
	}
}
