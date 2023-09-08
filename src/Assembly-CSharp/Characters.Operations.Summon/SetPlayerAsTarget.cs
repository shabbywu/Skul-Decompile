using System;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class SetPlayerAsTarget : IBDCharacterSetting
{
	public void ApplyTo(Character character)
	{
		EnemyCharacterBehaviorOption component = ((Component)character).GetComponent<EnemyCharacterBehaviorOption>();
		if (Object.op_Implicit((Object)(object)component))
		{
			component.SetTargetToPlayer();
		}
	}
}
