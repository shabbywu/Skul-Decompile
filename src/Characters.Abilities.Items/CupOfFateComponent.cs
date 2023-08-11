using UnityEngine;

namespace Characters.Abilities.Items;

public sealed class CupOfFateComponent : AbilityComponent<CupOfFate>, IStackable
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
