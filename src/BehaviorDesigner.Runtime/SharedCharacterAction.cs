using System;
using Characters.Actions;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedCharacterAction : SharedVariable<Characters.Actions.Action>
{
	public static explicit operator SharedCharacterAction(Characters.Actions.Action value)
	{
		return new SharedCharacterAction
		{
			mValue = value
		};
	}
}
