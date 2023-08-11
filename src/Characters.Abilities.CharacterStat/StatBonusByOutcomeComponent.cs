using UnityEngine;

namespace Characters.Abilities.CharacterStat;

public sealed class StatBonusByOutcomeComponent : AbilityComponent<StatBonusByOutcome>, IStackable
{
	public float stack
	{
		get
		{
			return base.baseAbility.stack;
		}
		set
		{
			Character componentInParent = ((Component)this).GetComponentInParent<Character>();
			if ((Object)(object)componentInParent == (Object)null)
			{
				base.baseAbility.stack = (int)value;
			}
			else
			{
				base.baseAbility.Load(componentInParent, (int)value);
			}
		}
	}
}
