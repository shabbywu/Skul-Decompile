using System;
using Characters;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedCharacter : SharedVariable<Character>
{
	public static explicit operator SharedCharacter(Character value)
	{
		return new SharedCharacter
		{
			mValue = value
		};
	}
}
