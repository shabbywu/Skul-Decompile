using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities;

public sealed class OperationByTriggersComponent : AbilityComponent<OperationByTriggers>
{
	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _operations;

	public override void Initialize()
	{
		_ability.operations = _operations.components;
		base.Initialize();
	}
}
