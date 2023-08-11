using System;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class DarkAbilityConstructor
{
	[SerializeField]
	private DarkAbilityAttacher _attacher;

	public bool Provide(Character target)
	{
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"target is null");
			return false;
		}
		DarkAbilityAttacher darkAbilityAttacher = Object.Instantiate<DarkAbilityAttacher>(_attacher, target.attach.transform);
		darkAbilityAttacher.Initialize(target);
		DarkEnemy component = ((Component)target).GetComponent<DarkEnemy>();
		if ((Object)(object)component != (Object)null)
		{
			component.Initialize(darkAbilityAttacher);
		}
		target.type = Character.Type.Named;
		return true;
	}
}
