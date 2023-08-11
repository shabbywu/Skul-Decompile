using System;
using System.Collections.Generic;
using Characters;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedCharacterList : SharedVariable<List<Character>>
{
	public static implicit operator SharedCharacterList(List<Character> value)
	{
		return new SharedCharacterList
		{
			mValue = value
		};
	}
}
