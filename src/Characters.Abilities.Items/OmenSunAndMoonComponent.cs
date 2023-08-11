using UnityEngine;

namespace Characters.Abilities.Items;

public sealed class OmenSunAndMoonComponent : AbilityComponent<OmenSunAndMoon>, IStackable
{
	public float stack
	{
		get
		{
			return base.baseAbility.stack;
		}
		set
		{
			Debug.Log((object)$"Load {value}");
			base.baseAbility.stack = (int)value;
			base.baseAbility.LoadStack();
		}
	}
}
