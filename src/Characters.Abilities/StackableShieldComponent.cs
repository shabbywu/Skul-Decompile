using UnityEngine;

namespace Characters.Abilities;

public sealed class StackableShieldComponent : AbilityComponent<StackableShield>, IStackable
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
