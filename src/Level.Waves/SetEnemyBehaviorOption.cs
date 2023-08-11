using System;
using Characters;
using UnityEngine;

namespace Level.Waves;

[Serializable]
public class SetEnemyBehaviorOption : IPinEnemyOption
{
	[SerializeField]
	private bool _setTargetToPlayer;

	[SerializeField]
	private bool _idleUntilFindTarget;

	[SerializeField]
	private bool _staticMovement;

	public SetEnemyBehaviorOption(bool setTargetToPlayer, bool idleUntilFindTarget, bool staticMovement)
	{
		_setTargetToPlayer = setTargetToPlayer;
		_idleUntilFindTarget = idleUntilFindTarget;
		_staticMovement = staticMovement;
	}

	public void ApplyTo(Character character)
	{
		EnemyCharacterBehaviorOption component = ((Component)character).GetComponent<EnemyCharacterBehaviorOption>();
		if (Object.op_Implicit((Object)(object)component))
		{
			component.SetBehaviorOption(_setTargetToPlayer, _idleUntilFindTarget, _staticMovement);
		}
	}
}
