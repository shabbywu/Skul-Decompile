using System;
using Utils;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedGrabBoard : SharedVariable<GrabBoard>
{
	public static implicit operator SharedGrabBoard(GrabBoard value)
	{
		return new SharedGrabBoard
		{
			mValue = value
		};
	}
}
