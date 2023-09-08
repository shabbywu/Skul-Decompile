using System;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Runnables;

[Serializable]
public class SetBDTargetToAttacker : IHitEvent
{
	[SerializeField]
	private BehaviorDesignerCommunicator _behaviorDesignerCommunicator;

	[SerializeField]
	private LayerMask _layermask;

	[SerializeField]
	private string _variableName;

	public void OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Character character = originalDamage.attacker.character;
		if (!((Object)(object)character == (Object)null) && !((Object)(object)character.collider == (Object)null) && !((Object)(object)originalDamage.attacker.character.health == (Object)null) && LayerMask.op_Implicit(_layermask) == 1 << ((Component)character).gameObject.layer)
		{
			_behaviorDesignerCommunicator.SetVariable<SharedCharacter>(_variableName, (object)character);
		}
	}

	void IHitEvent.OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		OnHit(in originalDamage, in tookDamage, damageDealt);
	}
}
