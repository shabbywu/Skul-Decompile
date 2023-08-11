using System;
using Characters.Abilities;

namespace BehaviorDesigner.Runtime;

[Serializable]
public class SharedAbilityComponent : SharedVariable<AbilityComponent>
{
	public static implicit operator SharedAbilityComponent(AbilityComponent value)
	{
		return new SharedAbilityComponent
		{
			Value = value
		};
	}
}
