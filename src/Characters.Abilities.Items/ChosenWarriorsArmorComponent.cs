using UnityEngine;

namespace Characters.Abilities.Items;

public sealed class ChosenWarriorsArmorComponent : AbilityComponent<ChosenWarriorsArmor>, IStackable
{
	public float stack
	{
		get
		{
			return base.baseAbility.amount;
		}
		set
		{
			Character componentInParent = ((Component)this).GetComponentInParent<Character>();
			if ((Object)(object)componentInParent == (Object)null)
			{
				base.baseAbility.amount = value;
			}
			else
			{
				base.baseAbility.Load(componentInParent, (int)value);
			}
		}
	}
}
